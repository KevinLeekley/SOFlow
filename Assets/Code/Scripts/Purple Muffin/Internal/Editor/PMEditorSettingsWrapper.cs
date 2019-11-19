// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

using System;
using UnityEngine;

namespace PurpleMuffin.Internal
{
    /// <summary>
    ///     A simple wrapper for all editor settings for exporting and importing purposes.
    /// </summary>
    [Serializable]
    public class PMEditorSettingsWrapper
    {
        public Color AcceptContextColour;
        public Color DeclineContextColour;
        public bool  DrawDefaultInspectors;
        public bool  DrawDefaultProperties;
        public Color PrimaryLayerColour;
        public Color SecondaryLayerColour;
        public Color StandardNodeColour;
        public Color TertiaryLayerColour;
        public Color TextColour;
        public Color TriggeredNodeColour;
    }
}