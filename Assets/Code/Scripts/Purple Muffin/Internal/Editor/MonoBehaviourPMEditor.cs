// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace PurpleMuffin.Internal
{
    [CustomEditor(typeof(MonoBehaviour), true)]
    [CanEditMultipleObjects]
    public class MonoBehaviourPMEditor : PMCustomEditor
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