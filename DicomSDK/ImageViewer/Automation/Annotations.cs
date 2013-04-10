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

using System.Collections.Generic;
using System.Drawing;
using ClearCanvas.ImageViewer.Graphics;

namespace ClearCanvas.ImageViewer.Automation
{
    public interface IDrawEllipse
    {
        void Draw(CoordinateSystem coordinateSystem, string name, PointF topLeft, PointF bottomRight);
    }

    public interface IDrawPolygon
    {
        void Draw(CoordinateSystem coordinateSystem, string name, IList<PointF> vertices);
    }

    public interface IDrawProtractor
    {
        void Draw(CoordinateSystem coordinateSystem, string name, PointF point1, PointF vertex, PointF point2);
    }

    public interface IDrawRectangle
    {
        void Draw(CoordinateSystem coordinateSystem, string name, PointF topLeft, PointF bottomRight);
    }

    public interface IDrawRuler
    {
        void Draw(CoordinateSystem coordinateSystem, string name, PointF point1, PointF point2);
    }

    public interface IDrawTextArea
    {
        void Draw(CoordinateSystem coordinateSystem, string name, string text, PointF textLocation);
    }

    public interface IDrawTextCallout
    {
        void Draw(CoordinateSystem coordinateSystem, string name, PointF anchorPoint, string text, PointF textLocation);
    }
}