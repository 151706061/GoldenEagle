﻿#region License

// Copyright (c) 2013, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This file is part of the ClearCanvas RIS/PACS open source project.
//
// The ClearCanvas RIS/PACS open source project is free software: you can
// redistribute it and/or modify it under the terms of the GNU General Public
// License as published by the Free Software Foundation, either version 3 of the
// License, or (at your option) any later version.
//
// The ClearCanvas RIS/PACS open source project is distributed in the hope that it
// will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General
// Public License for more details.
//
// You should have received a copy of the GNU General Public License along with
// the ClearCanvas RIS/PACS open source project.  If not, see
// <http://www.gnu.org/licenses/>.

#endregion

using System;
using System.Drawing;
using ClearCanvas.Common.Utilities;
using ClearCanvas.Dicom.Iod;
using ClearCanvas.ImageViewer.Mathematics;
using Matrix=System.Drawing.Drawing2D.Matrix;

namespace ClearCanvas.ImageViewer.Graphics
{
	/// <summary>
	/// A custom <see cref="SpatialTransform"/> for mammography images.
	/// </summary>
	[Cloneable]
	internal class MammographyImageSpatialTransform : ImageSpatialTransform
	{
		private const char _lateralityLeft = 'L';
		private const char _lateralityRight = 'R';
		private const char _orientationLeft = 'L';
		private const char _orientationRight = 'R';
		private const char _orientationHead = 'H';
		private const char _orientationFoot = 'F';
		private const char _orientationPosterior = 'P';
		private const char _orientationAnterior = 'A';

		[CloneIgnore]
		private readonly Vector3D _imagePosterior;

		/// <summary>
		/// Initializes a new instance of <see cref="MammographyImageSpatialTransform"/> with the specified image plane details.
		/// </summary>
		public MammographyImageSpatialTransform(IGraphic ownerGraphic, int rows, int columns, double pixelSpacingX, double pixelSpacingY, double pixelAspectRatioX, double pixelAspectRatioY, PatientOrientation patientOrientation, string laterality)
			: base(ownerGraphic, rows, columns, pixelSpacingX, pixelSpacingY, pixelAspectRatioX, pixelAspectRatioY)
		{
			// image coordinates are defined with X and Y being equivalent to the screen axes (right and down) and with Z = X cross Y (into the screen)

			Vector3D imagePosterior, imageHead, imageLeft; // patient orientation vectors in image space
			GetPatientOrientationVectors(patientOrientation, out imageHead, out imageLeft, out imagePosterior);

			// save the posterior vector
			_imagePosterior = imagePosterior;

			if (imagePosterior != null)
			{
				Vector3D normativePosterior, normativeHead, normativeLeft; // normative patient orientation vectors in image space
				GetNormativeOrientationVectors(laterality, out normativeHead, out normativeLeft, out normativePosterior);

				// only do any adjustments if laterality implies a normative orientation for the posterior direction
				if (normativePosterior != null)
				{
					// check if the order of the patient vectors are flipped according to the normative vectors
					// we know we need to flip if the direction vector cross products have different signs
					if (imageHead != null)
						FlipX = Math.Sign(imagePosterior.Cross(imageHead).Z) != Math.Sign(normativePosterior.Cross(normativeHead).Z);
					else if (imageLeft != null)
						FlipX = Math.Sign(imagePosterior.Cross(imageLeft).Z) != Math.Sign(normativePosterior.Cross(normativeLeft).Z);

					// with flip normalized, just rotate to align the current posterior direction with the normative posterior
					var currentPosterior = ScreenPosterior;
					var posteriorAngle = Math.Atan2(currentPosterior.Y, currentPosterior.X);
					var normativeAngle = Math.Atan2(normativePosterior.Y, normativePosterior.X);

					// compute required rotation, rounded to multiples of 90 degrees (PI/2 radians)
					RotationXY = 90*((int) Math.Round((normativeAngle - posteriorAngle)*2/Math.PI));
				}
			}
		}

		/// <summary>
		/// Cloning constructor.
		/// </summary>
		/// <param name="source">The source object from which to clone.</param>
		/// <param name="context">The cloning context object.</param>
		protected MammographyImageSpatialTransform(MammographyImageSpatialTransform source, ICloningContext context)
			: base(source, context)
		{
			context.CloneFields(source, this);

			_imagePosterior = new Vector3D(source._imagePosterior);
		}

		/// <summary>
		/// Gets the patient posterior orientation vector in the screen coordinate space.
		/// </summary>
		private Vector3D ScreenPosterior
		{
			get
			{
				if (_imagePosterior == null)
					return null;

				// figure out where the posterior direction went
				using (var transform = new Matrix())
				{
					var points = new[] {new PointF(SourceWidth*_imagePosterior.X, AdjustedSourceHeight*_imagePosterior.Y)};
					transform.Rotate(RotationXY);
					transform.Scale(ScaleX*(FlipY ? -1 : 1), ScaleY*(FlipX ? -1 : 1));
					transform.TransformPoints(points);
					return new Vector3D(points[0].X, points[0].Y, 0);
				}
			}
		}

		protected override void CalculatePreTransform(Matrix cumulativeTransform)
		{
			var destPosteriorVector = ScreenPosterior;
			if (destPosteriorVector != null)
			{
				// when the posterior edge of the image can be determined,
				// apply an offset in addition to user-applied translations
				// this allows the image to appear initially at (and reset to)
				// a position where the posterior ("chest wall") is aligned
				// against an edge of the client rectangle.

				// check if posterior direction is along client X axis
				if (Math.Abs(destPosteriorVector.X) > Math.Abs(destPosteriorVector.Y))
				{
					// compute additional horizontal translation to align posterior edge of image with client bounds
					cumulativeTransform.Translate(Math.Sign(destPosteriorVector.X)*((ClientRectangle.Width - Math.Abs(destPosteriorVector.X))/2f), 0);
				}
				else
				{
					// compute additional vertical translation to align posterior edge of image with client bounds
					cumulativeTransform.Translate(0, Math.Sign(destPosteriorVector.Y)*((ClientRectangle.Height - Math.Abs(destPosteriorVector.Y))/2f));
				}
			}
			base.CalculatePreTransform(cumulativeTransform);
		}

		/// <summary>
		/// Gets the effective posterior (or anterior) patient orientation after transforms have been applied.
		/// </summary>
		internal void GetEffectivePosteriorPatientOrientation(out string row, out string column)
		{
			row = column = string.Empty;

			var screenPosterior = ScreenPosterior;
			if (screenPosterior == null)
				return;

			// since we only deal with orthogonal rotations, getting the unit vector and truncating values will effectively remove any floating point error
			screenPosterior = screenPosterior.Normalize();
			screenPosterior = new Vector3D((int) screenPosterior.X, (int) screenPosterior.Y, 0);

			if (screenPosterior.Y > 0)
				column = _orientationPosterior.ToString();
			else if (screenPosterior.Y < 0)
				column = _orientationAnterior.ToString();

			if (screenPosterior.X > 0)
				row = _orientationPosterior.ToString();
			else if (screenPosterior.X < 0)
				row = _orientationAnterior.ToString();
		}

		private static void GetNormativeOrientationVectors(string laterality, out Vector3D headVector, out Vector3D leftVector, out Vector3D posteriorVector)
		{
			headVector = leftVector = posteriorVector = null;
			if (string.IsNullOrEmpty(laterality))
				return;

			// head should always be up where possible
			headVector = new Vector3D(0, -1, 0);

			if (char.ToUpperInvariant(laterality[0]) == _lateralityLeft)
			{
				// when a left breast is shown, the chest should be oriented to the left on the screen and the patient's left side should be oriented up on the screen
				leftVector = new Vector3D(0, -1, 0);
				posteriorVector = new Vector3D(-1, 0, 0);
			}
			else if (char.ToUpperInvariant(laterality[0]) == _lateralityRight)
			{
				// when a right breast is shown, the chest should be oriented to the right on the screen and the patient's left side should be oriented down on the screen
				leftVector = new Vector3D(0, +1, 0);
				posteriorVector = new Vector3D(+1, 0, 0);
			}
		}

		private static void GetPatientOrientationVectors(PatientOrientation patientOrientation, out Vector3D headVector, out Vector3D leftVector, out Vector3D posteriorVector)
		{
			headVector = leftVector = posteriorVector = null;
			if (patientOrientation == null)
				return;

			if (!string.IsNullOrEmpty(patientOrientation.Row))
			{
				switch (char.ToUpperInvariant(patientOrientation.Row.Code[0]))
				{
					case _orientationLeft:
						leftVector = new Vector3D(+1, 0, 0);
						break;
					case _orientationRight:
						leftVector = new Vector3D(-1, 0, 0);
						break;
					case _orientationPosterior:
						posteriorVector = new Vector3D(+1, 0, 0);
						break;
					case _orientationAnterior:
						posteriorVector = new Vector3D(-1, 0, 0);
						break;
					case _orientationHead:
						headVector = new Vector3D(+1, 0, 0);
						break;
					case _orientationFoot:
						headVector = new Vector3D(-1, 0, 0);
						break;
				}
			}

			if (!string.IsNullOrEmpty(patientOrientation.Column))
			{
				switch (char.ToUpperInvariant(patientOrientation.Column.Code[0]))
				{
					case _orientationLeft:
						leftVector = new Vector3D(0, +1, 0);
						break;
					case _orientationRight:
						leftVector = new Vector3D(0, -1, 0);
						break;
					case _orientationPosterior:
						posteriorVector = new Vector3D(0, +1, 0);
						break;
					case _orientationAnterior:
						posteriorVector = new Vector3D(0, -1, 0);
						break;
					case _orientationHead:
						headVector = new Vector3D(0, +1, 0);
						break;
					case _orientationFoot:
						headVector = new Vector3D(0, -1, 0);
						break;
				}
			}
		}
	}
}