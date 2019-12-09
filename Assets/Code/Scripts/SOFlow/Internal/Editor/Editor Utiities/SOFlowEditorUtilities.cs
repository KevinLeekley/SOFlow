// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

#if UNITY_EDITOR
using Object = UnityEngine.Object;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
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
        /// Saves the SOFlow numeric slider data.
        /// </summary>
        public static void SaveNumericSliderData()
        {
            StringBuilder numericSliderData = new StringBuilder();

            foreach(KeyValuePair<int, NumericSliderData> data in _numericSliders)
            {
                numericSliderData.Append($"{data.Key},{data.Value.SliderActive},{data.Value.SliderMinValue},{data.Value.SliderMaxValue}\n");
            }

            try
            {
                File.WriteAllText(Path.Combine(Application.persistentDataPath, _numericSlidersFile), numericSliderData.ToString());
            }
            catch(Exception e)
            {
                Debug.LogError($"Failed to save numeric slider data.\n\n{e.Message}");
            }
        }

        /// <summary>
        /// Loads the SOFlow numeric slider data.
        /// </summary>
        [UnityEditor.Callbacks.DidReloadScripts]
        [InitializeOnLoadMethod]
        public static void LoadNumericSliderData()
        {
            try
            {
                string filePath = Path.Combine(Application.persistentDataPath, _numericSlidersFile);

                if(File.Exists(filePath))
                {
                    string[] numericSliderData = File.ReadAllLines(filePath);

                    foreach(string data in numericSliderData)
                    {
                        string[] splitData = data.Split(',');

                        int sliderID = int.Parse(splitData[0]);

                        if(!_numericSliders.ContainsKey(sliderID))
                        {
                            _numericSliders.Add(sliderID, new NumericSliderData
                                                          {
                                                              SliderActive   = bool.Parse(splitData[1]),
                                                              SliderMinValue = float.Parse(splitData[2]),
                                                              SliderMaxValue = float.Parse(splitData[3])
                                                          });
                        }
                        else
                        {
                            _numericSliders[sliderID].SliderActive   = bool.Parse(splitData[1]);
                            _numericSliders[sliderID].SliderMinValue = float.Parse(splitData[2]);
                            _numericSliders[sliderID].SliderMaxValue = float.Parse(splitData[3]);
                        }
                    }
                }
            }
            catch(Exception e)
            {
                Debug.LogError($"Failed to load numeric slider data.\n\n{e.Message}");
            }
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