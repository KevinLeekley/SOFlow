// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace PurpleMuffin.Internal.SceneManagement
{
    [CustomEditor(typeof(SceneSet))]
    [CanEditMultipleObjects]
    public class SceneSetEditor : PMCustomEditor
    {
        /// <summary>
        ///     Field used to capture the input scene into the scene set.
        /// </summary>
        private Object _captureScene;

        /// <summary>
        ///     The scene set target.
        /// </summary>
        private SceneSet _target;

        private void OnEnable()
        {
            _target = (SceneSet)target;
        }

        /// <inheritdoc />
        protected override void DrawCustomInspector()
        {
            base.DrawCustomInspector();

            PMEditorUtilities.DrawPrimaryLayer(() =>
                                               {
                                                   DrawSceneSet();
                                                   GUILayout.Space(25f);

                                                   if(_target.SetScenes.Count > 0)
                                                   {
                                                       GUILayout.Space(10f);
                                                       DrawLoadSceneSetButton();
                                                   }
                                               });

            if(GUI.changed)
            {
                EditorUtility.SetDirty(this);
                AssetDatabase.SaveAssets();
            }
        }

        /// <summary>
        ///     Draws the scene set.
        /// </summary>
        private void DrawSceneSet()
        {
            PMEditorUtilities.DrawListComponentProperty(serializedObject, serializedObject.FindProperty("SetScenes"),
                                                        PMEditorSettings.SecondaryLayerColour);
        }

        /// <summary>
        ///     Draws the load scene set button.
        /// </summary>
        private void DrawLoadSceneSetButton()
        {
            if(PMEditorUtilities.DrawColourButton("Load Scene Set"))
                if(EditorUtility.DisplayDialog("Load Scene Set",
                                               "Replacing currently opens scenes with Scene Set.\nAre you sure?",
                                               "Load", "Cancel"))
                    _target.LoadSceneSet(false);

            if(PMEditorUtilities.DrawColourButton("Load Scene Set (Keep Current Scenes)")) _target.LoadSceneSet(true);

            if(PMEditorUtilities.DrawColourButton("Unload Scene Set")) _target.UnloadSceneSet();
        }
    }
}
#endif