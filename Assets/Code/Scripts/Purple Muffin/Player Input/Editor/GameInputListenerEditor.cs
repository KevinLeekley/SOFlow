// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

#if UNITY_EDITOR
using PurpleMuffin.Internal;
using UnityEditor;

namespace PurpleMuffin.PlayerInput
{
    [CustomEditor(typeof(GameInputListener))]
    public class GameInputListenerEditor : PMCustomEditor
    {
        protected override void DrawCustomInspector()
        {
            base.DrawCustomInspector();

            PMEditorUtilities.DrawPrimaryLayer(() =>
                                               {
                                                   serializedObject.DrawProperty("AutoCheckGameInput");

                                                   PMEditorUtilities
                                                      .DrawListComponentProperty(serializedObject,
                                                                                 serializedObject.FindProperty("Input"),
                                                                                 PMEditorSettings.SecondaryLayerColour);
                                               });
        }
    }
}
#endif