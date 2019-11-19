// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

using UnityEditor;
#if UNITY_EDITOR
using System;
using UnityEngine;

namespace PurpleMuffin.Internal
{
    public static partial class PMEditorUtilities
    {
        /// <summary>
        ///     Draws the primary colour layer.
        ///     <param name="action"></param>
        ///     <param name="options"></param>
        /// </summary>
        public static void DrawPrimaryLayer(Action                   action,
                                            params GUILayoutOption[] options)
        {
            DrawColourLayer(PMEditorSettings.PrimaryLayerColour, action, options);
        }

        /// <summary>
        ///     Draws the secondary colour layer.
        ///     <param name="action"></param>
        ///     <param name="options"></param>
        /// </summary>
        public static void DrawSecondaryLayer(Action                   action,
                                              params GUILayoutOption[] options)
        {
            DrawColourLayer(PMEditorSettings.SecondaryLayerColour, action, options);
        }

        /// <summary>
        ///     Draws the tertiary colour layer.
        ///     <param name="action"></param>
        ///     <param name="options"></param>
        /// </summary>
        public static void DrawTertiaryLayer(Action                   action,
                                             params GUILayoutOption[] options)
        {
            DrawColourLayer(PMEditorSettings.TertiaryLayerColour, action, options);
        }

        /// <summary>
        ///     Draws a custom colour layer.
        /// </summary>
        /// <param name="colour"></param>
        /// <param name="action"></param>
        /// <param name="options"></param>
        public static void DrawColourLayer(Color                    colour, Action action,
                                           params GUILayoutOption[] options)
        {
            Color originalGUIColor = GUI.backgroundColor;
            GUI.backgroundColor = colour;

            EditorGUILayout.BeginVertical(PMStyles.HelpBox, options);
            GUI.backgroundColor = originalGUIColor;

            action?.Invoke();
            EditorGUILayout.EndVertical();
        }

        /// <summary>
        ///     Draws a custom horizontal colour layer.
        /// </summary>
        /// <param name="colour"></param>
        /// <param name="action"></param>
        /// <param name="options"></param>
        public static void DrawHorizontalColourLayer(Color                    colour, Action action,
                                                     params GUILayoutOption[] options)
        {
            Color originalGUIColor = GUI.backgroundColor;
            GUI.backgroundColor = colour;

            EditorGUILayout.BeginHorizontal(PMStyles.HelpBox, options);
            GUI.backgroundColor = originalGUIColor;

            action?.Invoke();
            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        ///     Draws a custom scroll view colour layer.
        /// </summary>
        /// <param name="colour"></param>
        /// <param name="scrollPosition"></param>
        /// <param name="action"></param>
        /// <param name="options"></param>
        public static void DrawScrollViewColourLayer(Color                    colour, ref Vector2 scrollPosition,
                                                     Action                   action,
                                                     params GUILayoutOption[] options)
        {
            Color originalGUIColor = GUI.backgroundColor;
            GUI.backgroundColor = colour;

            scrollPosition      = EditorGUILayout.BeginScrollView(scrollPosition, PMStyles.HelpBox, options);
            GUI.backgroundColor = originalGUIColor;

            action?.Invoke();
            EditorGUILayout.EndScrollView();
        }
    }
}
#endif