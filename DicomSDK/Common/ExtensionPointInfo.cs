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
using ClearCanvas.Common.Utilities;

namespace ClearCanvas.Common
{
    /// <summary>
    /// Describes an extension point.  
    /// </summary>
    /// <remarks>
    /// Instances of this class are constructed by the framework when it processes
    /// plugins looking for extension points.
    /// </remarks>
    public class ExtensionPointInfo : IBrowsable
    {
        private Type _extensionPointClass;
        private Type _extensionInterface;
        private string _name;
        private string _description;

        /// <summary>
        /// Internal constructor.
        /// </summary>
        internal ExtensionPointInfo(Type extensionPointClass, Type extensionInterface, string name, string description)
        {
            _extensionPointClass = extensionPointClass;
            _extensionInterface = extensionInterface;
            _name = name;
            _description = description;
        }

        /// <summary>
        /// Gets the class that defines the extension point.
        /// </summary>
        public Type ExtensionPointClass
        {
            get { return _extensionPointClass; }
        }

        /// <summary>
        /// Gets the interface that an extension must implement.
        /// </summary>
        public Type ExtensionInteface
        {
            get { return _extensionInterface; }
        }

        /// <summary>
        /// Computes and returns a list of the installed extensions to this point,
        /// including disabled extensions.
        /// </summary>
        /// <returns></returns>
        public IList<ExtensionInfo> ListExtensions()
        {
            return CollectionUtils.Select(Platform.PluginManager.Extensions,
                delegate(ExtensionInfo ext) { return ext.PointExtended == _extensionPointClass; });
        }

        #region IBrowsable Members

        /// <summary>
        /// Friendly name of the extension point, if one exists, otherwise null.
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// A friendly description of the extension point, if one exists, otherwise null.
        /// </summary>
        public string Description
        {
            get { return _description; }
        }

        /// <summary>
        /// Formal name of the extension, which is the fully qualified name of the extension point class.
        /// </summary>
        public string FormalName
        {
            get { return _extensionPointClass.FullName; }
        }

        #endregion
    }
}
