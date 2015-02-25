﻿using System;
using System.Collections.Generic;
using System.Drawing;

using Niche.Graphs;
using Niche.Shared;

namespace Niche.NAntGraph
{
    /// <summary>
    /// Generator class to create a graph representing the NAnt file
    /// </summary>
    public class NodeGenerator : INAntVisitor
    {
        /// <summary>
        /// Gets the nodes generated by this visitor
        /// </summary>
        public IEnumerable<Node> Nodes
        {
            get
            {
                return mNodes;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to include descriptions
        /// </summary>
        public bool IncludeDescriptions
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the label font to use 
        /// </summary>
        public string LabelFont
        {
            get
            {
                return mLabelFont;
            }
            set
            {
                mLabelFont = value;
                UpdateStyles();
            }
        }

        public int LabelFontSize
        {
            get
            {
                return mLabelFontSize;
            }
            set
            {
                mLabelFontSize = value;
                UpdateStyles();
            }
        }

        private void UpdateStyles()
        {
            mProjectStyle.Font = LabelFont;
            mProjectStyle.FontSize = LabelFontSize;

            mExternalTargetStyle.Font = LabelFont;
            mExternalTargetStyle.FontSize = LabelFontSize;

            mInternalTargetStyle.Font = LabelFont;
            mInternalTargetStyle.FontSize = LabelFontSize;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeGenerator"/> class.
        /// </summary>
        public NodeGenerator()
        {
            mProjectStyle
                = new NodeStyle
                      {
                          FillColor = Color.WhiteSmoke,
                          Shape = NodeShape.Box
                      };

            mExternalTargetStyle
                = new NodeStyle
                      {
                          FillColor = Color.LemonChiffon,
                          Shape = NodeShape.Record
                      };

            mInternalTargetStyle
                = new NodeStyle
                      {
                          FillColor = Color.Khaki,
                          Shape = NodeShape.Box
                      };
        }

        /// <summary>
        /// Visit the specified project
        /// </summary>
        /// <param name="project">Project to visit.</param>
        public void VisitProject(NAntProject project)
        {
            Require.NotNull("project", project);
        }

        /// <summary>
        /// Visit the specified target
        /// </summary>
        /// <param name="target">Target to visit.</param>
        public void VisitTarget(NAntTarget target)
        {
            Require.NotNull("target", target);
            
            Node node;
            if (string.IsNullOrEmpty(target.Description))
            {
                // Internal Node
                node = mInternalTargetStyle.CreateNode(target.Name, target.Name);
            }
            else if (IncludeDescriptions)
            {
                // External Node with description

                var description
                    = target.Description.Wrap(30, "\\l")
                        .Replace("<", "\\<")
                        .Replace(">", "\\>")
                        .Replace("{", "\\{")
                        .Replace("}", "\\}");
                string label
                    = string.Format(
                        "{{{0}|{1}}}",
                        target.Name,
                        description);
                node = mExternalTargetStyle.CreateNode(target.Name, label);
            }
            else
            {
                string label
                   = string.Format(
                       "{{{0}}}",
                       target.Name);
                node = mExternalTargetStyle.CreateNode(target.Name, label);
            }

            mNodes.Add(node);
        }

        /// <summary>
        /// Storage for the node style to use for Projects
        /// </summary>
        private readonly NodeStyle mProjectStyle;

        /// <summary>
        /// Storage for the node style to use for external targets
        /// </summary>
        private readonly NodeStyle mExternalTargetStyle;

        /// <summary>
        /// Storage for the node style to use for internal targets
        /// </summary>
        private readonly NodeStyle mInternalTargetStyle;

        /// <summary>
        /// Storage for the list of nodes generated by this visitor
        /// </summary>
        private readonly List<Node> mNodes = new List<Node>();

        /// <summary>
        /// Storage for the LabelFont property
        /// </summary>
        private string mLabelFont;

        /// <summary>
        /// Storage for the LabelFontSize property
        /// </summary>
        private int mLabelFontSize;
    }
}