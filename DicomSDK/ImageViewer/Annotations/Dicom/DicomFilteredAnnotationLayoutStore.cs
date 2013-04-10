#region License

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
using System.Collections.Generic;
using System.IO;
using System.Xml;
using ClearCanvas.Common;
using ClearCanvas.ImageViewer.StudyManagement;
using ClearCanvas.Dicom.Iod;

namespace ClearCanvas.ImageViewer.Annotations.Dicom
{
	internal sealed class DicomFilteredAnnotationLayoutStore
	{
		private static readonly DicomFilteredAnnotationLayoutStore _instance = new DicomFilteredAnnotationLayoutStore();

		private readonly object _syncLock = new object();
		private XmlDocument _document;

		private DicomFilteredAnnotationLayoutStore()
		{
			DicomFilteredAnnotationLayoutStoreSettings.Default.PropertyChanged +=
				delegate
				{
					this.Initialize(true);
				};
		}

		public static DicomFilteredAnnotationLayoutStore Instance
		{
			get { return _instance; }
		}

		public IList<DicomFilteredAnnotationLayout> FilteredLayouts
		{
			get
			{
				List<DicomFilteredAnnotationLayout> allFilteredLayouts = new List<DicomFilteredAnnotationLayout>();

				string xPath = "dicom-filtered-annotation-layout-configuration/dicom-filtered-annotation-layouts/dicom-filtered-annotation-layout";
				
				lock (_syncLock)
				{
					XmlNodeList filteredLayoutNodes = _document.SelectNodes(xPath);
					foreach (XmlElement filteredLayoutNode in filteredLayoutNodes)
						allFilteredLayouts.Add(DeserializeFilteredLayout(filteredLayoutNode));
				}

				return allFilteredLayouts;
			}
		}

		private void Initialize(bool reloadSettings)
		{
			lock (_syncLock)
			{
				if (_document != null && !reloadSettings)
					return;

				try
				{
					_document = new XmlDocument();

					if (!String.IsNullOrEmpty(DicomFilteredAnnotationLayoutStoreSettings.Default.FilteredLayoutSettingsXml))
					{
						_document.LoadXml(DicomFilteredAnnotationLayoutStoreSettings.Default.FilteredLayoutSettingsXml);
					}
					else
					{
						XmlElement root = _document.CreateElement("dicom-filtered-annotation-layout-configuration");
						_document.AppendChild(root);
						root.AppendChild(_document.CreateElement("dicom-filtered-annotation-layouts"));

						SaveSettings(_document.OuterXml);
					}
				}
				catch
				{
					_document = null;
					throw;
				}
			}
		}

		private static void SaveSettings(string settingsXml)
		{
			DicomFilteredAnnotationLayoutStoreSettings.Default.FilteredLayoutSettingsXml = settingsXml;
			DicomFilteredAnnotationLayoutStoreSettings.Default.Save();
		}

		private static DicomFilteredAnnotationLayout DeserializeFilteredLayout(XmlElement dicomFilteredLayoutNode)
		{
			string matchingLayoutId = dicomFilteredLayoutNode.GetAttribute("matching-layout-id");
			string filteredLayoutId = dicomFilteredLayoutNode.GetAttribute("id");

			DicomFilteredAnnotationLayout filteredLayout = new DicomFilteredAnnotationLayout(filteredLayoutId, matchingLayoutId);

			foreach (XmlElement filterNode in dicomFilteredLayoutNode.SelectNodes("filters/filter"))
			{
				string key = filterNode.GetAttribute("key");
				if (String.IsNullOrEmpty(key))
					continue;

				string filterValue = filterNode.GetAttribute("value");
				if (String.IsNullOrEmpty(filterValue))
					continue;

				filteredLayout.Filters.Add(new KeyValuePair<string, string>(key, filterValue));
			}

			return filteredLayout;
		}

		private static void SerializeFilteredLayout(XmlDocument document, DicomFilteredAnnotationLayout dicomFilteredAnnotationLayout)
		{
			string xPath = "dicom-filtered-annotation-layout-configuration/dicom-filtered-annotation-layouts";
			XmlElement filteredLayoutsNode = (XmlElement)document.SelectSingleNode(xPath);
			if (filteredLayoutsNode == null)
				throw new InvalidDataException(String.Format(SR.ExceptionInvalidFilteredAnnotationLayoutXml, "'dicom-filtered-annotation-layouts' node does not exist"));

			XmlElement newFilteredLayoutNode = document.CreateElement("dicom-filtered-annotation-layout");
			newFilteredLayoutNode.SetAttribute("id", dicomFilteredAnnotationLayout.Identifier);
			newFilteredLayoutNode.SetAttribute("matching-layout-id", dicomFilteredAnnotationLayout.MatchingLayoutIdentifier);

			XmlElement filtersNode = document.CreateElement("filters");
			newFilteredLayoutNode.AppendChild(filtersNode);

			foreach (KeyValuePair<string, string> keyValuePair in dicomFilteredAnnotationLayout.Filters)
			{
				XmlElement newFilterNode = document.CreateElement("filter");
				newFilterNode.SetAttribute("key", keyValuePair.Key);
				newFilterNode.SetAttribute("value", keyValuePair.Value);
				filtersNode.AppendChild(newFilterNode);
			}

			xPath = String.Format("dicom-filtered-annotation-layout[@id='{0}']", dicomFilteredAnnotationLayout.Identifier);
			XmlElement existingNode = (XmlElement)filteredLayoutsNode.SelectSingleNode(xPath);
			if (existingNode != null)
				filteredLayoutsNode.ReplaceChild(newFilteredLayoutNode, existingNode);
			else
				filteredLayoutsNode.AppendChild(newFilteredLayoutNode);
		}

		public void Clear()
		{
			SaveSettings("");
			Initialize(true);
		}

		public DicomFilteredAnnotationLayout GetFilteredLayout(string filteredLayoutId)
		{
			lock (_syncLock)
			{
				string xPath = String.Format("dicom-filtered-annotation-layout-configuration/dicom-filtered-annotation-layouts/dicom-filtered-annotation-layout[@id='{0}']", filteredLayoutId);
				XmlElement filteredLayoutNode = (XmlElement)_document.SelectSingleNode(xPath);
				if (filteredLayoutNode == null)
					return null;

				return DeserializeFilteredLayout(filteredLayoutNode);
			}
		}
		
		public void RemoveFilteredLayout(string filteredLayoutId)
		{
			lock (_syncLock)
			{
				string xPath = "dicom-filtered-annotation-layout-configuration/dicom-filtered-annotation-layouts";
				XmlElement filteredLayoutsNode = (XmlElement)_document.SelectSingleNode(xPath);
				if (filteredLayoutsNode == null)
					throw new InvalidDataException(String.Format(SR.ExceptionInvalidFilteredAnnotationLayoutXml, "'dicom-filtered-annotation-layouts' node does not exist"));

				xPath = String.Format("dicom-filtered-annotation-layout[@id='{0}']", filteredLayoutId);
				XmlElement filteredLayoutNode = (XmlElement)filteredLayoutsNode.SelectSingleNode(xPath);
				if (filteredLayoutNode != null)
					filteredLayoutsNode.RemoveChild(filteredLayoutNode);
			}
		}

		public void Update(IEnumerable<DicomFilteredAnnotationLayout> filteredLayouts)
		{
			lock (_syncLock)
			{
				Initialize(false);

				try
				{
					foreach (DicomFilteredAnnotationLayout filteredLayout in filteredLayouts)
					{
						Platform.CheckForNullReference(filteredLayout, "filteredLayout");
						Platform.CheckForEmptyString(filteredLayout.MatchingLayoutIdentifier, "filteredLayout.MatchingLayoutIdentifier");

						SerializeFilteredLayout(_document, filteredLayout);
					}

					SaveSettings(_document.OuterXml);
				}
				catch
				{
					Initialize(true);
					throw;
				}
			}
		}

		public void Update(DicomFilteredAnnotationLayout filteredLayout)
		{ 
			Platform.CheckForNullReference(filteredLayout, "filteredLayout");
			Platform.CheckForEmptyString(filteredLayout.MatchingLayoutIdentifier, "filteredLayout.MatchingLayoutIdentifier");

			lock (_syncLock)
			{
				Initialize(false);

				try
				{
					SerializeFilteredLayout(_document, filteredLayout);
					SaveSettings(_document.OuterXml);
				}
				catch
				{
					Initialize(true);
					throw;
				}
			}
		}

		public string GetMatchingStoredLayoutId(IImageSopProvider dicomImage)
		{
			if (dicomImage == null)
				return null;

			var filterCandidates = new List<KeyValuePair<string, string>>
            {new KeyValuePair<string, string>("Modality", dicomImage.ImageSop.Modality)};

			// these are hard-coded as the only filter candidates for now, until more general use cases are identified.
		    var patientOrientation = dicomImage.Frame.PatientOrientation;
            filterCandidates.Add(new KeyValuePair<string, string>("PatientOrientation_Row", patientOrientation.PrimaryRow));
		    filterCandidates.Add(new KeyValuePair<string, string>("PatientOrientation_Col", patientOrientation.PrimaryColumn));

			return GetMatchingStoredLayoutId(filterCandidates);
		}

		public string GetMatchingStoredLayoutId(List<KeyValuePair<string, string>> filterCandidates)
		{
			lock (_syncLock)
			{
				Initialize(false);

				string xPath = "dicom-filtered-annotation-layout-configuration/dicom-filtered-annotation-layouts/dicom-filtered-annotation-layout";
				XmlNodeList filteredLayoutNodes = _document.SelectNodes(xPath);
				foreach (XmlElement filteredLayoutNode in filteredLayoutNodes)
				{
					DicomFilteredAnnotationLayout filteredAnnotationLayout = DeserializeFilteredLayout(filteredLayoutNode);
					if (filteredAnnotationLayout.IsMatch(filterCandidates))
					{
						return filteredAnnotationLayout.MatchingLayoutIdentifier;
					}
				}
			}

			return "";
		}
	}
}
