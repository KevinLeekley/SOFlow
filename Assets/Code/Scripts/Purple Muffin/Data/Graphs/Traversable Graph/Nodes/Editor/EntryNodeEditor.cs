// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

using PurpleMuffin.Internal;
#if UNITY_EDITOR
using UnityEngine;
using XNodeEditor;

namespace PurpleMuffin.Data.Graphs
{
    /// <inheritdoc />
    [CustomNodeEditor(typeof(EntryNode))]
    public class EntryNodeEditor : NodeEditor
    {
        public override void OnBodyGUI()
        {
            PMEditorUtilities.AdjustTextContrast(PMEditorSettings.AcceptContextColour);

            base.OnBodyGUI();

            PMEditorUtilities.RestoreTextContrast();
        }

        /// <inheritdoc />
        public override Color GetTint()
        {
            return PMEditorSettings.AcceptContextColour;
        }
    }
}
#endif