// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace PurpleMuffin.Internal
{
    [CustomEditor(typeof(ScriptableObject), true)]
    [CanEditMultipleObjects]
    public class ScriptableObjectPMEditor : PMCustomEditor
    {
        protected override void DrawCustomInspector()
        {
            base.DrawCustomInspector();

            PMEditorUtilities.DrawLayeredProperties(serializedObject);

            PMEditorUtilities.DrawTertiaryLayer(() =>
                                                {
                                                    PMEditorUtilities.DrawNonSerializableFields(target);
                                                });
        }
    }
}
#endif