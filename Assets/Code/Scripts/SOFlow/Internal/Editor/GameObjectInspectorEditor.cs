// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

#if UNITY_EDITOR
using System;
using System.Reflection;
using SOFlow.Data.Events;
using UnityEditor;
using UnityEngine;

namespace SOFlow.Internal
{
    [CustomEditor(typeof(GameObject), true), CanEditMultipleObjects]
    public class GameObjectInspectorEditor : Editor
    {
        /// <summary>
        /// The Game Object Inspector type.
        /// </summary>
        private readonly Type _gameObjectInspector;

        /// <summary>
        /// The Game Object Inspector OnHeaderGUI method reference.
        /// </summary>
        private readonly MethodInfo _gameObjectInspectorOnHeaderGUI;
        
        /// <summary>
        /// The event button content.
        /// </summary>
        private GUIContent _eventButtonContent = new GUIContent();
        
        /// <summary>
        /// The logo content.
        /// </summary>
        private GUIContent _logoContent = new GUIContent();

        public GameObjectInspectorEditor()
        {
            _gameObjectInspector = Assembly.GetAssembly(typeof(Editor)).GetType("UnityEditor.GameObjectInspector");

            _gameObjectInspectorOnHeaderGUI =
                _gameObjectInspector.GetMethod("OnHeaderGUI",
                                               BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        }

        private void Awake()
        {
            _eventButtonContent.image = Resources.Load<Texture2D>("Game Event Icon");
            _logoContent.image        = Resources.Load<Texture2D>("SOFlow Logo");
        }

        protected override void OnHeaderGUI()
        {
            Editor gameObjectInspectorEditor = CreateEditor(target, _gameObjectInspector);
            _gameObjectInspectorOnHeaderGUI.Invoke(gameObjectInspectorEditor, null);

            SOFlowEditorUtilities.DrawHeaderColourLayer(SOFlowEditorSettings.PrimaryLayerColour, DrawHeaderItems);
        }

        /// <summary>
        /// Draws custom header items to the top of all Game Object inspectors.
        /// </summary>
        private void DrawHeaderItems()
        {
            EditorGUILayout.LabelField(_logoContent);
            
            GUILayout.FlexibleSpace();

            if(SOFlowEditorUtilities.DrawColourButton(_eventButtonContent, SOFlowEditorSettings.AcceptContextColour, SOFlowStyles.HeaderButton,
                                                      GUILayout.MaxWidth(20f), GUILayout.MaxHeight(16f)))
            {
                if(Selection.activeGameObject != null)
                {
                    Undo.AddComponent<SimpleGameEventListener>(Selection.activeGameObject);
                }
            }
        }

        public override void OnInspectorGUI()
        {
        }
    }
}
#endif