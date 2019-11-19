// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

using System.IO;
using System.Text;
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace PurpleMuffin.Internal
{
    public class PMStylesEditor : EditorWindow
    {
        /// <summary>
        ///     The custom style label width.
        /// </summary>
        private readonly float _labelWidth = 160f;

        /// <summary>
        ///     The custom GUI skin.
        /// </summary>
        private GUISkin _editorSkin;

        /// <summary>
        ///     The custom styles scroll position.
        /// </summary>
        private Vector2 _scrollPosition;

        /// <summary>
        ///     The data path.
        /// </summary>
        private string _dataPath => Path.Combine("Assets", "Code", "Scripts", "Purple Muffin", "Internal", "Editor");

        [MenuItem("PurpleMuffin/Editor Styles")]
        public static void ShowWindow()
        {
            GetWindow<PMStylesEditor>("PM-Editor Styles");
        }

        private void OnEnable()
        {
            string guiSkinPath = Path.Combine(_dataPath, "PM GUI Skin.guiskin");

            if(!File.Exists(guiSkinPath))
            {
                _editorSkin = CreateInstance<GUISkin>();

                AssetDatabase.CreateAsset(_editorSkin, guiSkinPath);
                AssetDatabase.SaveAssets();
            }
            else
            {
                _editorSkin = AssetDatabase.LoadAssetAtPath<GUISkin>(guiSkinPath);
            }
        }

        private void OnGUI()
        {
            DrawWindowGUI();
        }

        /// <summary>
        ///     Draws the window GUI.
        /// </summary>
        private void DrawWindowGUI()
        {
            PMEditorUtilities.DrawPrimaryLayer(() =>
                                               {
                                                   _editorSkin =
                                                       (GUISkin)EditorGUILayout.ObjectField("GUI Styles Reference",
                                                                                            _editorSkin,
                                                                                            typeof(GUISkin), true);

                                                   DrawCustomStyles();

                                                   GUILayout.FlexibleSpace();

                                                   DrawStylesManagementButtons();
                                               });
        }

        /// <summary>
        ///     Draws the GUI skin custom styles list.
        /// </summary>
        private void DrawCustomStyles()
        {
            EditorGUILayout.BeginScrollView(_scrollPosition, false, false, GUIStyle.none, GUIStyle.none,
                                            PMStyles.HelpBox);

            DrawGUIStyleHeaders();

            EditorGUILayout.EndScrollView();

            PMEditorUtilities.DrawScrollViewColourLayer(PMEditorSettings.SecondaryLayerColour, ref _scrollPosition,
                                                        () =>
                                                        {
                                                            foreach(GUIStyle style in _editorSkin.customStyles)
                                                                DrawGUIStyle(style);
                                                        });
        }

        /// <summary>
        ///     Draws the given GUI style headers.
        /// </summary>
        private void DrawGUIStyleHeaders()
        {
            PMEditorUtilities.DrawHorizontalColourLayer(PMEditorSettings.TertiaryLayerColour,
                                                        () =>
                                                        {
                                                            EditorGUILayout.LabelField("Style Name",
                                                                                       PMStyles.BoldCenterLabel,
                                                                                       GUILayout.Width(_labelWidth));

                                                            EditorGUILayout.LabelField("Font Name",
                                                                                       PMStyles.BoldCenterLabel,
                                                                                       GUILayout.Width(_labelWidth));

                                                            EditorGUILayout.LabelField("Font Size",
                                                                                       PMStyles.BoldCenterLabel,
                                                                                       GUILayout.Width(_labelWidth));

                                                            EditorGUILayout.LabelField("Font Style",
                                                                                       PMStyles.BoldCenterLabel,
                                                                                       GUILayout.Width(_labelWidth));

                                                            EditorGUILayout.LabelField("Alignment",
                                                                                       PMStyles.BoldCenterLabel,
                                                                                       GUILayout.Width(_labelWidth));

                                                            EditorGUILayout.LabelField("Word Wrapped",
                                                                                       PMStyles.BoldCenterLabel,
                                                                                       GUILayout.Width(_labelWidth));

                                                            EditorGUILayout.LabelField("Text Clipping",
                                                                                       PMStyles.BoldCenterLabel,
                                                                                       GUILayout.Width(_labelWidth));

                                                            EditorGUILayout.LabelField("Fixed Width",
                                                                                       PMStyles.BoldCenterLabel,
                                                                                       GUILayout.Width(_labelWidth));

                                                            EditorGUILayout.LabelField("Fixed Height",
                                                                                       PMStyles.BoldCenterLabel,
                                                                                       GUILayout.Width(_labelWidth));

                                                            EditorGUILayout.LabelField("Width Stretched",
                                                                                       PMStyles.BoldCenterLabel,
                                                                                       GUILayout.Width(_labelWidth));

                                                            EditorGUILayout.LabelField("Height Stretched",
                                                                                       PMStyles.BoldCenterLabel,
                                                                                       GUILayout.Width(_labelWidth));
                                                        });
        }

        /// <summary>
        ///     Draws the given GUI style.
        /// </summary>
        /// <param name="style"></param>
        private void DrawGUIStyle(GUIStyle style)
        {
            PMEditorUtilities.DrawHorizontalColourLayer(PMEditorSettings.SecondaryLayerColour,
                                                        () =>
                                                        {
                                                            EditorGUILayout
                                                               .LabelField(string.IsNullOrEmpty(style.name) ? "Untitled" : style.name,
                                                                           PMStyles.CenteredLabel,
                                                                           GUILayout.Width(_labelWidth));

                                                            EditorGUILayout.LabelField(style.font == null
                                                                                           ? "No Font"
                                                                                           : style.font.name,
                                                                                       PMStyles.CenteredLabel,
                                                                                       GUILayout.Width(_labelWidth));

                                                            EditorGUILayout.LabelField(style.fontSize.ToString(),
                                                                                       PMStyles.CenteredLabel,
                                                                                       GUILayout.Width(_labelWidth));

                                                            EditorGUILayout.LabelField(style.fontStyle.ToString(),
                                                                                       PMStyles.CenteredLabel,
                                                                                       GUILayout.Width(_labelWidth));

                                                            EditorGUILayout.LabelField(style.alignment.ToString(),
                                                                                       PMStyles.CenteredLabel,
                                                                                       GUILayout.Width(_labelWidth));

                                                            EditorGUILayout.LabelField(style.wordWrap
                                                                                           ? "Word Wrap"
                                                                                           : "No Word Wrap",
                                                                                       PMStyles.CenteredLabel,
                                                                                       GUILayout.Width(_labelWidth));

                                                            EditorGUILayout.LabelField(style.clipping.ToString(),
                                                                                       PMStyles.CenteredLabel,
                                                                                       GUILayout.Width(_labelWidth));

                                                            EditorGUILayout.LabelField(style.fixedWidth.ToString(),
                                                                                       PMStyles.CenteredLabel,
                                                                                       GUILayout.Width(_labelWidth));

                                                            EditorGUILayout.LabelField(style.fixedHeight.ToString(),
                                                                                       PMStyles.CenteredLabel,
                                                                                       GUILayout.Width(_labelWidth));

                                                            EditorGUILayout.LabelField(style.stretchWidth
                                                                                           ? "Width Stretch"
                                                                                           : "No Width Stretch",
                                                                                       PMStyles.CenteredLabel,
                                                                                       GUILayout.Width(_labelWidth));

                                                            EditorGUILayout.LabelField(style.stretchHeight
                                                                                           ? "Height Stretch"
                                                                                           : "No Height Stretch",
                                                                                       PMStyles.CenteredLabel,
                                                                                       GUILayout.Width(_labelWidth));
                                                        });
        }

        /// <summary>
        ///     Draws the styles management buttons.
        /// </summary>
        private void DrawStylesManagementButtons()
        {
            PMEditorUtilities.DrawHorizontalColourLayer(PMEditorSettings.SecondaryLayerColour,
                                                        () =>
                                                        {
                                                            if(PMEditorUtilities.DrawColourButton("Import",
                                                                                                  PMEditorSettings
                                                                                                     .TertiaryLayerColour)
                                                            )
                                                            {
                                                            }

                                                            GUILayout.FlexibleSpace();

                                                            if(PMEditorUtilities.DrawColourButton("Save",
                                                                                                  PMEditorSettings
                                                                                                     .AcceptContextColour)
                                                            )
                                                                SavePMStyles();
                                                        });
        }

        /// <summary>
        ///     Saves the PM Styles to script.
        /// </summary>
        private void SavePMStyles()
        {
            string scriptTemplatePath = Path.Combine(_dataPath, "PMStylesTemplate.txt");

            if(File.Exists(scriptTemplatePath))
            {
                StringBuilder scriptText     = new StringBuilder();
                string        styleTemplate  = "        public static GUIStyle #VARIABLE# => GetStyle(#KEY#);\r\n";
                string        scriptTemplate = File.ReadAllText(scriptTemplatePath);

                foreach(GUIStyle style in _editorSkin.customStyles)
                    if(!string.IsNullOrEmpty(style.name))
                        scriptText.Append(styleTemplate.Replace("#VARIABLE#", style.name.Replace(" ", ""))
                                                       .Replace("#KEY#", $"\"{style.name}\""));

                scriptTemplate = scriptTemplate.Replace("#STYLES#", scriptText.ToString());

                File.WriteAllText(Path.Combine(_dataPath, "PMStyles.cs"), scriptTemplate);

                AssetDatabase.Refresh();
            }
            else
            {
                EditorUtility.DisplayDialog("Error",
                                            "Failed to save PM Styles.\n\nPM Styles script template unavailable", "OK");
            }
        }
    }
}
#endif