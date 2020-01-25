// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

#if UNITY_EDITOR
using System.Collections.Generic;
using Object = UnityEngine.Object;
using System;
using System.IO;
using SOFlow.Audio;
using SOFlow.CameraUtilities;
using SOFlow.Data.Collections;
using SOFlow.Data.Evaluations;
using SOFlow.Data.Events;
using SOFlow.Data.Primitives;
using SOFlow.Extensions;
using SOFlow.Fading;
using SOFlow.Internal.SceneManagement;
using SOFlow.ManagedComponents.Components;
using SOFlow.ManagedComponents.Managers;
using SOFlow.Motion;
using SOFlow.ObjectPooling;
using SOFlow.PlayerInput;
using SOFlow.Sprites;
using UnityEditor;
using UnityEngine;

namespace SOFlow.Internal
{
    public class SOFlowComponentShortcutWindow : EditorWindow
    {
        /// <summary>
        /// The last selected folder path.
        /// </summary>
        private static string _lastFolderPath = "Assets";

        /// <summary>
        /// The SOFlowComponentShortcutWindow instance.
        /// </summary>
        private static SOFlowComponentShortcutWindow _window;

        /// <summary>
        /// The current asset search filter.
        /// </summary>
        private static string _assetSearchFilter = "";

        /// <summary>
        /// The current scene search filter.
        /// </summary>
        private static string _sceneSearchFilter = "";

        /// <summary>
        /// The category height.
        /// </summary>
        private static float _categoryHeight = 220f;

        /// <summary>
        /// The section width.
        /// </summary>
        private static float _sectionWidth = 200f;

        /// <summary>
        /// The search box width.
        /// </summary>
        private static float _searchBoxWidth = 200f;

        /// <summary>
        /// The asset components scroll position.
        /// </summary>
        private static Vector2 _assetComponentsScrollPosition;

        /// <summary>
        /// The scene components scroll position.
        /// </summary>
        private static Vector2 _sceneComponentsScrollPosition;

        /// <summary>
        /// The asset sections.
        /// </summary>
        private static List<SectionEntry> _assetSections = new List<SectionEntry>
                                                           {
                                                               new SectionEntry("Events",
                                                                                new[]
                                                                                {
                                                                                    typeof(GameEvent)
                                                                                }),
                                                               new SectionEntry("Primitive Data",
                                                                                new[]
                                                                                {
                                                                                    typeof(BoolData),
                                                                                    typeof(IntData),
                                                                                    typeof(FloatData),
                                                                                    typeof(StringData),
                                                                                    typeof(Vector2Data),
                                                                                    typeof(Vector3Data),
                                                                                    typeof(Vector2IntData),
                                                                                    typeof(Vector3IntData),
                                                                                    typeof(ColorData),
                                                                                    typeof(PrimitiveData)
                                                                                }),
                                                               new SectionEntry("Collection Data",
                                                                                new[]
                                                                                {
                                                                                    typeof(UnityRuntimeSet),
                                                                                    typeof(DataRuntimeSet)
                                                                                }),
                                                               new SectionEntry("Scene Management",
                                                                                new[]
                                                                                {
                                                                                    typeof(SceneSet)
                                                                                }),
                                                               new SectionEntry("Managed Components",
                                                                                new[]
                                                                                {
                                                                                    typeof(BehaviourComponentManager)
                                                                                }),
                                                               new SectionEntry("Object Pooling",
                                                                                new[]
                                                                                {
                                                                                    typeof(PoolObjectListReference)
                                                                                }),
                                                               new SectionEntry("Audio",
                                                                                new[]
                                                                                {
                                                                                    typeof(AudioLink)
                                                                                }),
                                                               new SectionEntry("Utilities",
                                                                                new[]
                                                                                {
                                                                                    typeof(CameraReference),
                                                                                    typeof(ResolutionState)
                                                                                })
                                                           };

        /// <summary>
        /// The scene sections.
        /// </summary>
        private static List<SectionEntry> _sceneSections = new List<SectionEntry>
                                                           {
                                                               new SectionEntry("Events",
                                                                                new[]
                                                                                {
                                                                                    typeof(GameEventListener),
                                                                                    typeof(SimpleGameEventListener),
                                                                                    typeof(GameEventRaiser),
                                                                                    typeof(GameEventReactorSet),
                                                                                    typeof(PhysicsGameEventReactor)
                                                                                }),
                                                               new SectionEntry("Evaluations",
                                                                                new[]
                                                                                {
                                                                                    typeof(Comparison),
                                                                                    typeof(Evaluator),
                                                                                    typeof(GrowthCurve)
                                                                                }),
                                                               new SectionEntry("Fadables",
                                                                                new[]
                                                                                {
                                                                                    typeof(FadableUIElement),
                                                                                    typeof(TextMeshProFadable),
                                                                                    typeof(SpriteRendererFadable),
                                                                                    typeof(LightFadable),
                                                                                    typeof(FloatDataFadable),
                                                                                    typeof(FadableGroup)
                                                                                }),
                                                               new SectionEntry("Faders",
                                                                                new[]
                                                                                {
                                                                                    typeof(GenericFader),
                                                                                    typeof(SimpleUIFader)
                                                                                }),
                                                               new SectionEntry("Input",
                                                                                new[]
                                                                                {
                                                                                    typeof(GameInputListener)
                                                                                }),
                                                               new SectionEntry("Audio",
                                                                                new[]
                                                                                {
                                                                                    typeof(AudioBridge),
                                                                                    typeof(AudioController)
                                                                                }),
                                                               new SectionEntry("Motion",
                                                                                new[]
                                                                                {
                                                                                    typeof(BasicMotion)
                                                                                }),
                                                               new SectionEntry("Camera Utilities",
                                                                                new[]
                                                                                {
                                                                                    typeof(GameCamera),
                                                                                    typeof(ScreenSizeMonitor)
                                                                                }),
                                                               new SectionEntry("Sprites",
                                                                                new[]
                                                                                {
                                                                                    typeof(SpriteScaleHelper)
                                                                                }),
                                                               new SectionEntry("TextMesh Pro",
                                                                                new[]
                                                                                {
                                                                                    typeof(DataTextSetter)
                                                                                }),
                                                               new SectionEntry("Managed Components",
                                                                                new[]
                                                                                {
                                                                                    typeof(GameManager),
                                                                                    typeof(SimpleMotionComponent)
                                                                                })
                                                           };

        [MenuItem("SOFlow/Component Shortcut Window")]
        public static void ShowWindow()
        {
            _window = GetWindow<SOFlowComponentShortcutWindow>("SOFlow-Component Shortcuts");
        }

        private void OnEnable()
        {
            Selection.selectionChanged -= UpdateLastFolderPath;
            Selection.selectionChanged += UpdateLastFolderPath;

            if(_window == null)
            {
                _window = this;
            }
        }

        private void OnDisable()
        {
            Selection.selectionChanged -= UpdateLastFolderPath;
        }

        private void OnGUI()
        {
            SOFlowEditorUtilities.DrawPrimaryLayer(DrawComponentShortcuts);
        }

        /// <summary>
        /// Updates the last selected folder path.
        /// </summary>
        private static void UpdateLastFolderPath()
        {
            foreach(Object _object in Selection.objects)
            {
                string path = AssetDatabase.GetAssetPath(_object);

                if(File.Exists(path))
                {
                    _lastFolderPath = Path.GetDirectoryName(path);
                    _window.Repaint();
                }
                else if(Directory.Exists(path))
                {
                    _lastFolderPath = path;
                    _window.Repaint();
                }

                break;
            }
        }

        /// <summary>
        /// Draws the component shortcuts.
        /// </summary>
        private static void DrawComponentShortcuts()
        {
            SOFlowEditorUtilities.DrawTertiaryLayer(() =>
                                                    {
                                                        EditorGUILayout
                                                           .LabelField($"Current asset path: {_lastFolderPath}",
                                                                       SOFlowStyles.CenteredLabel);

                                                        EditorGUILayout
                                                           .LabelField("Select an asset within the Project Window to update path.",
                                                                       SOFlowStyles.CenterTextHelpBox);
                                                    });

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Scriptable Objects", SOFlowStyles.BoldCenterTextHelpBox,
                                       GUILayout.Height(EditorGUIUtility.singleLineHeight * 2.5f));

            EditorGUILayout.BeginVertical(SOFlowStyles.CenterTextHelpBox, GUILayout.MaxWidth(_searchBoxWidth));

            EditorGUILayout.LabelField("Search", SOFlowStyles.CenteredLabel, GUILayout.MaxWidth(_searchBoxWidth));

            Color originalColor = GUI.backgroundColor;
            GUI.backgroundColor = SOFlowEditorSettings.SecondaryLayerColour;

            _assetSearchFilter =
                EditorGUILayout.TextField(_assetSearchFilter, GUILayout.MaxWidth(_searchBoxWidth)).Trim().ToLower();

            GUI.backgroundColor = originalColor;

            EditorGUILayout.EndVertical();

            EditorGUILayout.EndHorizontal();

            SOFlowEditorUtilities.DrawScrollViewColourLayer(SOFlowEditorSettings.SecondaryLayerColour,
                                                            ref _assetComponentsScrollPosition,
                                                            DrawAssetComponentShortcuts,
                                                            GUILayout.MaxHeight(_categoryHeight));

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Components", SOFlowStyles.BoldCenterTextHelpBox,
                                       GUILayout.Height(EditorGUIUtility.singleLineHeight * 2.5f));

            EditorGUILayout.BeginVertical(SOFlowStyles.CenterTextHelpBox, GUILayout.MaxWidth(_searchBoxWidth));

            EditorGUILayout.LabelField("Search", SOFlowStyles.CenteredLabel, GUILayout.MaxWidth(_searchBoxWidth));

            originalColor       = GUI.backgroundColor;
            GUI.backgroundColor = SOFlowEditorSettings.SecondaryLayerColour;

            _sceneSearchFilter =
                EditorGUILayout.TextField(_sceneSearchFilter, GUILayout.MaxWidth(_searchBoxWidth)).Trim().ToLower();

            GUI.backgroundColor = originalColor;

            EditorGUILayout.EndVertical();

            EditorGUILayout.EndHorizontal();

            SOFlowEditorUtilities.DrawScrollViewColourLayer(SOFlowEditorSettings.SecondaryLayerColour,
                                                            ref _sceneComponentsScrollPosition,
                                                            DrawSceneComponentShortcuts,
                                                            GUILayout.MaxHeight(_categoryHeight));
        }

        /// <summary>
        /// Draws the asset component shortcuts.
        /// </summary>
        private static void DrawAssetComponentShortcuts()
        {
            EditorGUILayout.BeginHorizontal();

            foreach(SectionEntry sectionEntry in _assetSections)
            {
                bool partOfSearch = false;

                if(!string.IsNullOrEmpty(_assetSearchFilter))
                {
                    foreach(Type type in sectionEntry.Types)
                    {
                        string formattedName = FormatTypeName(type);

                        if(formattedName.ToLower().Contains(_assetSearchFilter))
                        {
                            partOfSearch = true;

                            break;
                        }
                    }
                }
                else
                {
                    partOfSearch = true;
                }

                if(partOfSearch)
                {
                    SOFlowEditorUtilities.DrawSecondaryLayer(() =>
                                                             {
                                                                 DrawAssetComponentsSection(sectionEntry.TypeHeader,
                                                                                            sectionEntry.Types);
                                                             }, GUILayout.Width(_sectionWidth));
                }
            }

            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// Draws a asset section with the given header and component types.
        /// </summary>
        /// <param name="header"></param>
        /// <param name="types"></param>
        private static void DrawAssetComponentsSection(string header, IEnumerable<Type> types)
        {
            EditorGUILayout.LabelField(header, SOFlowStyles.BoldCenterLabel);

            foreach(Type type in types)
            {
                string formattedName = FormatTypeName(type);

                if(!formattedName.ToLower().Contains(_assetSearchFilter))
                {
                    continue;
                }

                if(SOFlowEditorUtilities.DrawColourButton(formattedName, SOFlowEditorSettings.AcceptContextColour))
                {
                    Object newObject = CreateInstance(type);

                    AssetDatabase.CreateAsset(newObject,
                                              Path.Combine(_lastFolderPath, $"New {formattedName}.asset"));

                    Selection.activeObject = newObject;

                    FocusWindowIfItsOpen(TypeExtensions.GetInstanceType("ProjectBrowser"));
                }
            }
        }

        /// <summary>
        /// Draws the scene component shortcuts.
        /// </summary>
        private static void DrawSceneComponentShortcuts()
        {
            EditorGUILayout.BeginHorizontal();

            foreach(SectionEntry sectionEntry in _sceneSections)
            {
                bool partOfSearch = false;

                if(!string.IsNullOrEmpty(_sceneSearchFilter))
                {
                    foreach(Type type in sectionEntry.Types)
                    {
                        string formattedName = FormatTypeName(type);

                        if(formattedName.ToLower().Contains(_sceneSearchFilter))
                        {
                            partOfSearch = true;

                            break;
                        }
                    }
                }
                else
                {
                    partOfSearch = true;
                }

                if(partOfSearch)
                {
                    SOFlowEditorUtilities.DrawSecondaryLayer(() =>
                                                             {
                                                                 DrawSceneComponentsSection(sectionEntry.TypeHeader,
                                                                                            sectionEntry.Types);
                                                             }, GUILayout.Width(_sectionWidth));
                }
            }

            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// Draws a scene section with the given header and component types.
        /// </summary>
        /// <param name="header"></param>
        /// <param name="types"></param>
        private static void DrawSceneComponentsSection(string header, IEnumerable<Type> types)
        {
            EditorGUILayout.LabelField(header, SOFlowStyles.BoldCenterLabel);

            foreach(Type type in types)
            {
                string formattedName = FormatTypeName(type);

                if(!formattedName.ToLower().Contains(_sceneSearchFilter))
                {
                    continue;
                }

                if(SOFlowEditorUtilities.DrawColourButton(formattedName, SOFlowEditorSettings.AcceptContextColour))
                {
                    if(Selection.activeGameObject != null)
                    {
                        Selection.activeGameObject.AddComponent(type);
                    }
                    else
                    {
                        GameObject newObject = new GameObject($"New {formattedName}", type);

                        Selection.activeObject = newObject;

                        FocusWindowIfItsOpen(TypeExtensions.GetInstanceType("SceneHierarchyWindow"));
                    }
                }
            }
        }

        /// <summary>
        /// Formats the name for the given type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static string FormatTypeName(Type type)
        {
            string formattedName = type.Name;

            for(int i = formattedName.Length - 1, lastFormatIndex = -1; i > 0; i--)
            {
                if(char.IsUpper(formattedName[i]) && i != lastFormatIndex - 1)
                {
                    formattedName   = formattedName.Insert(i, " ");
                    lastFormatIndex = i;
                }
            }

            return formattedName;
        }
    }

    /// <summary>
    /// A utility class for containing the acceptable parameter for component sections.
    /// </summary>
    public class SectionEntry
    {
        /// <summary>
        /// The type header.
        /// </summary>
        public string TypeHeader;

        /// <summary>
        /// The list of types.
        /// </summary>
        public Type[] Types;

        public SectionEntry(string typeHeader, Type[] types)
        {
            TypeHeader = typeHeader;
            Types      = types;
        }
    }
}
#endif