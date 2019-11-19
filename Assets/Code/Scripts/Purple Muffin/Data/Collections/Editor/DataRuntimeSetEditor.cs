// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

using System.Reflection;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
using System.Collections.Generic;
using PurpleMuffin.Internal;

namespace PurpleMuffin.Data.Collections
{
    [CustomEditor(typeof(DataRuntimeSet))]
    public class DataRuntimeSetEditor : PMCustomEditor
    {
        /// <summary>
        ///     The list of items from the DataRuntimeSet target.
        /// </summary>
        private List<object> _items = new List<object>();

        /// <summary>
        ///     The DataRuntimeSet target.
        /// </summary>
        private DataRuntimeSet _target;

        private void OnEnable()
        {
            _target = (DataRuntimeSet)target;

            try
            {
                _items = _target.GetType().GetField("_items", BindingFlags.NonPublic | BindingFlags.Instance)
                                .GetValue(_target) as List<object>;

                if(_items == null) _items = new List<object>();
            }
            catch
            {
                _items = new List<object>();
            }
        }

        /// <inheritdoc />
        protected override void DrawCustomInspector()
        {
            base.DrawCustomInspector();

            PMEditorUtilities.DrawPrimaryLayer(() =>
                                               {
                                                   DrawDefaultInspector();
                                                   PMEditorUtilities.DrawSecondaryLayer(DrawListItems);
                                               });
        }

        /// <summary>
        ///     Draws the list items.
        /// </summary>
        private void DrawListItems()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            EditorGUILayout.LabelField("Items", PMStyles.BoldCenterLabel);

            GUILayout.FlexibleSpace();

            EditorGUILayout.LabelField($"Size: {_items.Count}", PMStyles.WordWrappedMiniLabel);
            EditorGUILayout.EndHorizontal();

            for(int i = 0; i < _items.Count; i++)
                PMEditorUtilities.DrawTertiaryLayer(() =>
                                                    {
                                                        PMEditorUtilities.DrawItemField($"Entry {i + 1}", _items[i]);
                                                    });
        }
    }
}
#endif