// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

using System;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace PurpleMuffin.Internal
{
    public class PMEditorSettings : EditorWindow
    {
        public static bool DrawDefaultInspectors
        {
            get => EditorPrefs.GetBool("PM-DrawDefaultInspectors", false);
            set => EditorPrefs.SetBool("PM-DrawDefaultInspectors", value);
        }

        public static bool DrawDefaultProperties
        {
            get => EditorPrefs.GetBool("PM-DrawDefaultProperties", false);
            set => EditorPrefs.SetBool("PM-DrawDefaultProperties", value);
        }

        private void OnEnable()
        {
            if(PrimaryLayerColour == Color.clear) PrimaryLayerColour = _primaryLayerColour;

            if(SecondaryLayerColour == Color.clear) SecondaryLayerColour = _secondaryLayerColour;

            if(TertiaryLayerColour == Color.clear) TertiaryLayerColour = _tertiaryLayerColour;

            if(TextColour == Color.clear) TextColour = _textColour;

            if(StandardNodeColour == Color.clear) StandardNodeColour = _standardNodeColour;

            if(TriggeredNodeColour == Color.clear) TriggeredNodeColour = _triggeredNodeColour;
        }

        [MenuItem("PurpleMuffin/Editor Settings")]
        public static void ShowWindow()
        {
            GetWindow<PMEditorSettings>("PM-Editor Settings");
        }

        private void OnGUI()
        {
            PMEditorUtilities.DrawPrimaryLayer(() =>
                                               {
                                                   EditorGUI.BeginChangeCheck();

                                                   PrimaryLayerColour =
                                                       EditorGUILayout.ColorField("Primary Layer Colour",
                                                                                  PrimaryLayerColour);

                                                   SecondaryLayerColour =
                                                       EditorGUILayout.ColorField("Secondary Layer Colour",
                                                                                  SecondaryLayerColour);

                                                   TertiaryLayerColour =
                                                       EditorGUILayout.ColorField("Tertiary Layer Colour",
                                                                                  TertiaryLayerColour);

                                                   AcceptContextColour =
                                                       EditorGUILayout.ColorField("Accept Context Colour",
                                                                                  AcceptContextColour);

                                                   DeclineContextColour =
                                                       EditorGUILayout.ColorField("Decline Context Colour",
                                                                                  DeclineContextColour);

                                                   TextColour = EditorGUILayout.ColorField("Text Colour", TextColour);

                                                   StandardNodeColour =
                                                       EditorGUILayout.ColorField("Standard Node Colour",
                                                                                  StandardNodeColour);

                                                   TriggeredNodeColour =
                                                       EditorGUILayout.ColorField("Triggered Node Colour",
                                                                                  TriggeredNodeColour);

                                                   DrawDefaultInspectors =
                                                       EditorGUILayout.Toggle("Draw Default Inspectors",
                                                                              DrawDefaultInspectors);

                                                   DrawDefaultProperties =
                                                       EditorGUILayout.Toggle("Draw Default Properties",
                                                                              DrawDefaultProperties);

                                                   PMEditorUtilities.DrawHorizontalColourLayer(Color.yellow, () =>
                                                                                                             {
                                                                                                                 if(
                                                                                                                     GUILayout
                                                                                                                        .Button("Export Settings\n↑")
                                                                                                                 )
                                                                                                                     ExportSettings();

                                                                                                                 if(
                                                                                                                     GUILayout
                                                                                                                        .Button("Import Settings\n↓")
                                                                                                                 )
                                                                                                                     ImportSettings();
                                                                                                             });

                                                   if(EditorGUI.EndChangeCheck()) RefreshCustomEditors();
                                               });
        }

        /// <summary>
        ///     Refreshes all custom PM editors.
        /// </summary>
        private void RefreshCustomEditors()
        {
            Editor[] inspectors = Resources.FindObjectsOfTypeAll<Editor>();

            foreach(Editor inspector in inspectors)
                if(inspector is ICustomEditor)
                    inspector.Repaint();
        }

        #region Colour Accessors

        public static Color PrimaryLayerColour
        {
            get
            {
                Color loadedColour = PMEditorUtilities.LoadColour("PM-PrimaryLayerColour");

                return loadedColour == Color.clear ? _primaryLayerColour : loadedColour;
            }
            set => PMEditorUtilities.SaveColour("PM-PrimaryLayerColour", value);
        }

        public static Color SecondaryLayerColour
        {
            get
            {
                Color loadedColour = PMEditorUtilities.LoadColour("PM-SecondaryLayerColour");

                return loadedColour == Color.clear ? _secondaryLayerColour : loadedColour;
            }
            set => PMEditorUtilities.SaveColour("PM-SecondaryLayerColour", value);
        }

        public static Color TertiaryLayerColour
        {
            get
            {
                Color loadedColour = PMEditorUtilities.LoadColour("PM-TertiaryLayerColour");

                return loadedColour == Color.clear ? _tertiaryLayerColour : loadedColour;
            }
            set => PMEditorUtilities.SaveColour("PM-TertiaryLayerColour", value);
        }

        public static Color AcceptContextColour
        {
            get
            {
                Color loadedColour = PMEditorUtilities.LoadColour("PM-AcceptContextColour");

                return loadedColour == Color.clear ? _acceptContextColour : loadedColour;
            }
            set => PMEditorUtilities.SaveColour("PM-AcceptContextColour", value);
        }

        public static Color DeclineContextColour
        {
            get
            {
                Color loadedColour = PMEditorUtilities.LoadColour("PM-DeclineContextColour");

                return loadedColour == Color.clear ? _declineContextColour : loadedColour;
            }
            set => PMEditorUtilities.SaveColour("PM-DeclineContextColour", value);
        }

        public static Color TextColour
        {
            get
            {
                Color loadedColour = PMEditorUtilities.LoadColour("PM-TextColour");

                return loadedColour == Color.clear ? _textColour : loadedColour;
            }
            set => PMEditorUtilities.SaveColour("PM-TextColour", value);
        }

        public static Color StandardNodeColour
        {
            get
            {
                Color loadedColour = PMEditorUtilities.LoadColour("PM-StandardNodeColour");

                return loadedColour == Color.clear ? _standardNodeColour : loadedColour;
            }
            set => PMEditorUtilities.SaveColour("PM-StandardNodeColour", value);
        }

        public static Color TriggeredNodeColour
        {
            get
            {
                Color loadedColour = PMEditorUtilities.LoadColour("PM-TriggeredNodeColour");

                return loadedColour == Color.clear ? _triggeredNodeColour : loadedColour;
            }
            set => PMEditorUtilities.SaveColour("PM-TriggeredNodeColour", value);
        }

        #endregion

        #region Default Colours

        private static readonly Color _primaryLayerColour   = new Color(0.47f, 0f, 1f);
        private static readonly Color _secondaryLayerColour = Color.cyan;
        private static readonly Color _tertiaryLayerColour  = Color.yellow;
        private static readonly Color _acceptContextColour  = Color.green;
        private static readonly Color _declineContextColour = Color.red;
        private static readonly Color _textColour           = Color.white;
        private static readonly Color _standardNodeColour   = new Color(0.26f, 0.68f, 0.22f);
        private static readonly Color _triggeredNodeColour  = new Color(1f,    0.59f, 0.24f);

        #endregion

        #region File Handling

        /// <summary>
        ///     Exports all PM editor settings to file.
        /// </summary>
        private void ExportSettings()
        {
            string exportPath = EditorUtility.SaveFilePanel("Export Settings",
                                                            Environment.GetFolderPath(Environment.SpecialFolder
                                                                                                 .DesktopDirectory),
                                                            "PM-EditorSettings", "json");

            if(!string.IsNullOrEmpty(exportPath))
            {
                PMEditorSettingsWrapper settings = new PMEditorSettingsWrapper
                                                   {
                                                       PrimaryLayerColour    = PrimaryLayerColour,
                                                       SecondaryLayerColour  = SecondaryLayerColour,
                                                       TertiaryLayerColour   = TertiaryLayerColour,
                                                       AcceptContextColour   = AcceptContextColour,
                                                       DeclineContextColour  = DeclineContextColour,
                                                       TextColour            = TextColour,
                                                       StandardNodeColour    = StandardNodeColour,
                                                       TriggeredNodeColour   = TriggeredNodeColour,
                                                       DrawDefaultInspectors = DrawDefaultInspectors,
                                                       DrawDefaultProperties = DrawDefaultProperties
                                                   };

                string jsonSettings = EditorJsonUtility.ToJson(settings);

                using(StreamWriter settingsFile = new StreamWriter(exportPath, false))
                {
                    try
                    {
                        settingsFile.Write(jsonSettings);
                        settingsFile.Close();
                    }
                    catch(IOException e)
                    {
                        EditorUtility.DisplayDialog("Export Settings", $"Settings failed to export.\n\n{e}", "OK");

                        throw;
                    }
                    finally
                    {
                        EditorUtility.DisplayDialog("Export Settings", "Settings successfully exported.", "OK");
                    }
                }
            }
        }

        /// <summary>
        ///     Imports all PM editor settings from file.
        /// </summary>
        private void ImportSettings()
        {
            string importPath = EditorUtility.OpenFilePanel("Import Settings",
                                                            Environment.GetFolderPath(Environment.SpecialFolder
                                                                                                 .DesktopDirectory),
                                                            "json");

            if(!string.IsNullOrEmpty(importPath))
                using(StreamReader settingsFile = new StreamReader(File.OpenRead(importPath)))
                {
                    try
                    {
                        PMEditorSettingsWrapper settingsWrapper = new PMEditorSettingsWrapper();
                        EditorJsonUtility.FromJsonOverwrite(settingsFile.ReadToEnd(), settingsWrapper);

                        PrimaryLayerColour    = settingsWrapper.PrimaryLayerColour;
                        SecondaryLayerColour  = settingsWrapper.SecondaryLayerColour;
                        TertiaryLayerColour   = settingsWrapper.TertiaryLayerColour;
                        AcceptContextColour   = settingsWrapper.AcceptContextColour;
                        DeclineContextColour  = settingsWrapper.DeclineContextColour;
                        TextColour            = settingsWrapper.TextColour;
                        StandardNodeColour    = settingsWrapper.StandardNodeColour;
                        TriggeredNodeColour   = settingsWrapper.TriggeredNodeColour;
                        DrawDefaultInspectors = settingsWrapper.DrawDefaultInspectors;
                        DrawDefaultProperties = settingsWrapper.DrawDefaultProperties;
                    }
                    catch(FileLoadException e)
                    {
                        EditorUtility.DisplayDialog("Import Settings", $"Settings failed to import.\n\n{e}", "OK");

                        throw;
                    }
                    finally
                    {
                        EditorUtility.DisplayDialog("Import Settings", "Settings imported successfully.", "OK");
                        RefreshCustomEditors();
                    }
                }
        }

        #endregion
    }
}
#endif