// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

#if UNITY_EDITOR
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace SOFlow.Internal
{
    public static partial class SOFlowEditorUtilities
    {
        /// <summary>
        ///     The original GUI colour.
        /// </summary>
        private static Color _originalGUIColour;

        /// <summary>
        ///     Saves the given colour to the given key in the EditorPrefs.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="colour"></param>
        public static void SaveColour(string key, Color colour)
        {
            EditorPrefs.SetFloat(key + "-r", colour.r);
            EditorPrefs.SetFloat(key + "-g", colour.g);
            EditorPrefs.SetFloat(key + "-b", colour.b);
            EditorPrefs.SetFloat(key + "-a", colour.a);
        }

        /// <summary>
        ///     Loads the saved colour with the given key from the EditorPrefs.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Color LoadColour(string key)
        {
            Color result = Color.clear;

            result.r = EditorPrefs.GetFloat(key + "-r");
            result.g = EditorPrefs.GetFloat(key + "-g");
            result.b = EditorPrefs.GetFloat(key + "-b");
            result.a = EditorPrefs.GetFloat(key + "-a");

            return result;
        }

        /// <summary>
        ///     Adjusts the text colouring to display clearly for any contrast.
        /// </summary>
        /// <param name="colour"></param>
        public static void AdjustTextContrast(Color colour)
        {
            _originalGUIColour = GUI.contentColor;

            float contrast = 0.2126f * colour.r +
                             0.7152f * colour.g +
                             0.0722f * colour.b;

            GUI.contentColor = contrast < 0.5f ? Color.white : Color.black;
        }

        /// <summary>
        ///     Reverts any contrast adjustments that have been made on text colouring.
        /// </summary>
        public static void RestoreTextContrast()
        {
            GUI.contentColor = _originalGUIColour;
        }

        /// <summary>
        ///     Draws a custom coloured button.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="colour"></param>
        /// <param name="style"></param>
        /// <param name="layoutOptions"></param>
        /// <returns></returns>
        public static bool DrawColourButton(string                   text, Color? colour = null, GUIStyle style = null,
                                            params GUILayoutOption[] layoutOptions)
        {
            Color originalGUIColor = GUI.backgroundColor;
            GUI.backgroundColor = colour ?? Color.white;

            bool result = style == null
                              ? GUILayout.Button(text, SOFlowStyles.Button, layoutOptions)
                              : GUILayout.Button(text, style,           layoutOptions);

            GUI.backgroundColor = originalGUIColor;

            return result;
        }

        /// <summary>
        /// Attempts to get the local object ID within the file the object is contained in.
        /// Returns -1 if no ID is found.
        /// </summary>
        /// <param name="targetObject"></param>
        /// <returns></returns>
        public static long GetObjectLocalIDInFile(Object targetObject)
        {
            long id = -1;
            SerializedObject serializedObject = new SerializedObject(targetObject);

            PropertyInfo inspectorModeInfo = typeof(SerializedObject).GetProperty("inspectorMode", BindingFlags.NonPublic | BindingFlags.Instance);

            if(inspectorModeInfo != null)
            {
                inspectorModeInfo.SetValue(serializedObject, InspectorMode.Debug, null);
            }

            SerializedProperty localIDProperty = serializedObject.FindProperty("m_LocalIdentfierInFile");

            if(localIDProperty != null)
            {
                id = localIDProperty.longValue;
            }

            return id;
        }
    }
}
#endif