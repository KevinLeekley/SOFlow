// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

#if UNITY_EDITOR
using System.Collections.Generic;
using SOFlow.Extensions;
using Object = UnityEngine.Object;
using UnityEngine;
using UnityEditor;

namespace SOFlow.Internal
{
    public class ObjectPickerHistoryWindow : EditorWindow
    {
        /// <summary>
        /// THe list of objects that have been selected.
        /// </summary>
        public static readonly List<Object> ObjectPickerHistoryObjects = new List<Object>();
        
        /// <summary>
        /// Indicates whether a property is currently being edited.
        /// </summary>
        public static SerializedProperty EditingProperty = null;

        /// <summary>
        /// Indicates whether the property is available or has been disposed.
        /// </summary>
        public static bool PropertyAvailable
        {
            get
            {
                try
                {
                    return EditingProperty?.propertyPath != null;
                }
                catch
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Indicates whether the window is currently open.
        /// </summary>
        public static bool IsOpen
        {
            get;
            private set;
        }

        /// <summary>
        /// The object picker history window.
        /// </summary>
        public static ObjectPickerHistoryWindow Window
        {
            get
            {
                if(_window == null)
                {
                    _window = GetWindow<ObjectPickerHistoryWindow>("SOFlow-Object Picker History");
                }

                return _window;
            }
        }

        /// <summary>
        /// The object picker history window.
        /// </summary>
        private static ObjectPickerHistoryWindow _window;

        /// <summary>
        /// The scroll position.
        /// </summary>
        private Vector2 _scrollPosition = Vector2.zero;

        /// <summary>
        /// Indicates whether a selected object is currently being monitored.
        /// </summary>
        private static bool _monitoringSelectedObject = false;

        /// <summary>
        /// The monitored selected object.
        /// </summary>
        private static Object _monitoredObject = null;

        public void OnGUI()
        {
            DrawWindowContents();
        }

        private void OnEnable()
        {
            IsOpen = true;
        }

        private void OnDisable()
        {
            IsOpen = false;
        }

        /// <summary>
        /// Draws the contents of the window.
        /// </summary>
        private void DrawWindowContents()
        {
            SOFlowEditorUtilities.DrawPrimaryLayer(() =>
                                                   {
                                                       SOFlowEditorUtilities.DrawTertiaryLayer(() =>
                                                                                               {
                                                                                                   EditorGUILayout
                                                                                                      .LabelField("Previously Selected Objects",
                                                                                                                  SOFlowStyles
                                                                                                                     .BoldCenterTextHelpBox);
                                                                                               });
                                                       
                                                       SOFlowEditorUtilities
                                                          .DrawScrollViewColourLayer(SOFlowEditorSettings.SecondaryLayerColour,
                                                                                     ref _scrollPosition,
                                                                                     DrawObjectList);
                                                   });
        }

        /// <summary>
        /// Draws the object list.
        /// </summary>
        private void DrawObjectList()
        {
            foreach(Object historyObject in ObjectPickerHistoryObjects)
            {
                if(historyObject != null)
                {
                    Texture2D buttonIcon =
                        (Texture2D)AssetDatabase.GetCachedIcon(AssetDatabase
                                                                  .GetAssetPath(historyObject));

                    if(buttonIcon == null)
                    {
                        buttonIcon = (Texture2D)EditorGUIUtility.ObjectContent(null, historyObject.GetType()).image;
                    }

                    if(buttonIcon != null)
                    {
                        buttonIcon = TextureExtensions.ResizeTexture(buttonIcon, 12, 12);
                    }

                    if(GUILayout.Button(new GUIContent(historyObject.name,
                                                       buttonIcon),
                                        EditorStyles.objectField))
                    {
                        if(EditingProperty != null)
                        {
                            EditingProperty.objectReferenceValue =
                                historyObject;

                            EditingProperty
                               .serializedObject.ApplyModifiedProperties();

                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Registers the object picker monitor method to the editor update event.
        /// </summary>
        [InitializeOnLoadMethod]
        private static void RegisterObjectPickerMonitor()
        {
            EditorApplication.update -= MonitorObjectPicker;
            EditorApplication.update += MonitorObjectPicker;
        }

        /// <summary>
        /// Monitors the object picker for any selected object.
        /// </summary>
        private static void MonitorObjectPicker()
        {
            if(focusedWindow?.ToString() == " (UnityEditor.ObjectSelector)")
            {
                _monitoringSelectedObject = true;
                _monitoredObject          = EditorGUIUtility.GetObjectPickerObject();
            }
            else if(_monitoringSelectedObject)
            {
                if(!ObjectPickerHistoryObjects.Contains(_monitoredObject))
                {
                    ObjectPickerHistoryObjects.Insert(0, _monitoredObject);
                }
                else
                {
                    ObjectPickerHistoryObjects.Remove(_monitoredObject);
                    ObjectPickerHistoryObjects.Insert(0, _monitoredObject);
                }

                if(IsOpen)
                {
                    Window.Repaint();
                }

                _monitoredObject          = null;
                _monitoringSelectedObject = false;
            }
        }
    }
}
#endif