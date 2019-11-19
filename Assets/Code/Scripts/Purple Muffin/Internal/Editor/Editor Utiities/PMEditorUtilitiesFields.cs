// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

using System;
using System.Reflection;
using PurpleMuffin.Data.Events;
using PurpleMuffin.Data.Primitives;
using Object = UnityEngine.Object;
#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;

namespace PurpleMuffin.Internal
{
    public static partial class PMEditorUtilities
    {
        /// <summary>
        ///     The list of expanded field flags.
        /// </summary>
        private static readonly Dictionary<int, bool> _expandedFlags = new Dictionary<int, bool>();

        /// <summary>
        ///     Draws the item field.
        /// </summary>
        /// <param name="itemLabel"></param>
        /// <param name="item"></param>
        public static void DrawItemField(string itemLabel, object item)
        {
            if(item == null)
            {
                EditorGUILayout.LabelField("Null");

                return;
            }

            int itemHashCode = item.GetHashCode();

            if(!_expandedFlags.ContainsKey(itemHashCode)) _expandedFlags.Add(itemHashCode, false);

            EditorGUILayout.BeginHorizontal();

            if(DrawColourButton(_expandedFlags[itemHashCode] ? "↑" : "↓",
                                PMEditorSettings.AcceptContextColour))
                _expandedFlags[itemHashCode] = !_expandedFlags[itemHashCode];

            EditorGUILayout.LabelField(itemLabel);

            EditorGUILayout.LabelField(item.GetType().Name);
            EditorGUILayout.EndHorizontal();

            if(_expandedFlags[itemHashCode]) DrawTertiaryLayer(() => DrawExpandedField(item));
        }

        /// <summary>
        ///     Draws the expanded field values.
        /// </summary>
        /// <param name="item"></param>
        private static void DrawExpandedField(object item)
        {
            FieldInfo[] fields = item.GetType().GetFields();

            foreach(FieldInfo field in fields)
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(field.Name);

                object fieldValue = field.GetValue(item);

                EditorGUILayout.LabelField(fieldValue == null ? "Null" : fieldValue.ToString());

                EditorGUILayout.EndHorizontal();
            }
        }

        /// <summary>
        ///     Draws all non-serializable fields for the provided object.
        /// </summary>
        public static void DrawNonSerializableFields(object target)
        {
            FieldInfo[] fields = target.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);

            foreach(FieldInfo field in fields)
            {
                object value = field.GetValue(target);

                if(value != null)
                {
                    Type valueType = value.GetType();

                    if(!(value is Object)       && !(value is string)       &&
                       !(value is DataField)    && !(value is DynamicEvent) &&
                       !valueType.IsGenericType &&
                       valueType.IsClass        && !valueType.IsArray)
                        DrawItemField(valueType.Name, value);
                }
            }
        }
    }
}
#endif