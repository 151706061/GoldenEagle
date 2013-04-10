using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClearCanvas.ImageViewer
{
    /// <summary>
    /// Specifies window launch options for the <see cref="ImageViewerComponent"/>.
    /// </summary>
    public enum WindowBehaviour
    {
        /// <summary>
        /// Same as <see cref="Single"/> currently.
        /// </summary>
        Auto,

        /// <summary>
        /// Specifies that the <see cref="ImageViewerComponent"/> should be launched
        /// in a single (e.g. active) desktop window.
        /// </summary>
        Single,

        /// <summary>
        /// Specifies that the <see cref="ImageViewerComponent"/> should be launched
        /// in a separate desktop window.
        /// </summary>
        Separate
    }
}
