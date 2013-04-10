// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LayoutInitializer.cs" company="GoldenEagle.Desktop.Framwork development team">
//   Copyright (c) 2008 - 2012 GoldenEagle.Desktop.Framwork development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Linq;
using AvalonDock.Layout;

namespace GoldenEagle.Desktop.Framwork
{
    /// <summary>
    /// Layout initializer for GoldenEagle.Desktop.Framwork.
    /// </summary>
    public class LayoutInitializer : ILayoutUpdateStrategy
    {
        #region ILayoutUpdateStrategy Members

        /// <summary>
        /// Befores the insert anchorable.
        /// </summary>
        /// <param name="layout">
        /// The layout.
        /// </param>
        /// <param name="anchorableToShow">
        /// The anchorable to show.
        /// </param>
        /// <param name="destinationContainer">
        /// The destination container.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool BeforeInsertAnchorable(LayoutRoot layout, LayoutAnchorable anchorableToShow, ILayoutContainer destinationContainer)
        {
            // AD wants to add the anchorable into destinationContainer
            // just for test provide a new anchorablepane 
            // if the pane is floating let the manager go ahead
            // var destPane = destinationContainer as LayoutAnchorablePane;
            if (destinationContainer != null && destinationContainer.FindParent<LayoutFloatingWindow>() != null)
            {
                return false;
            }

            var toolsPane = layout.Descendents().OfType<LayoutAnchorablePane>().FirstOrDefault(d => d.Name == "ToolsPane");
            if (toolsPane != null)
            {
                toolsPane.Children.Add(anchorableToShow);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Called after an anchorable item is inserted.
        /// </summary>
        /// <param name="layout">
        /// The layout.
        /// </param>
        /// <param name="anchorableShown">
        /// The anchorable shown.
        /// </param>
        public void AfterInsertAnchorable(LayoutRoot layout, LayoutAnchorable anchorableShown)
        {
        }

        /// <summary>
        /// The before insert document.
        /// </summary>
        /// <param name="layout">
        /// The layout.
        /// </param>
        /// <param name="anchorableToShow">
        /// The anchorable to show.
        /// </param>
        /// <param name="destinationContainer">
        /// The destination container.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool BeforeInsertDocument(LayoutRoot layout, LayoutDocument anchorableToShow, ILayoutContainer destinationContainer)
        {
            return false;
        }

        /// <summary>
        /// The after insert document.
        /// </summary>
        /// <param name="layout">
        /// The layout.
        /// </param>
        /// <param name="anchorableShown">
        /// The anchorable shown.
        /// </param>
        public void AfterInsertDocument(LayoutRoot layout, LayoutDocument anchorableShown)
        {
        }

        #endregion

        /// <summary>
        /// Inserts the anchorable.
        /// </summary>
        /// <param name="layout">
        /// The layout.
        /// </param>
        /// <param name="anchorableToShow">
        /// The anchorable to show.
        /// </param>
        /// <param name="destinationContainer">
        /// The destination container.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool InsertAnchorable(LayoutRoot layout, LayoutAnchorable anchorableToShow, ILayoutContainer destinationContainer)
        {
            return false;
        }
    }
}