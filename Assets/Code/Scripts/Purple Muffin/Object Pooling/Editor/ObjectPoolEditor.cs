// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

using System.Collections;
using System.Reflection;
#if UNITY_EDITOR
using PurpleMuffin.Internal;
using UnityEditor;

namespace PurpleMuffin.ObjectPooling
{
    [CustomEditor(typeof(ObjectPoolBase), true)]
    public class ObjectPoolEditor : PMCustomEditor
    {
        /// <summary>
        ///     The ObjectPool target.
        /// </summary>
        private ObjectPoolBase _target;

        private void OnEnable()
        {
            _target = (ObjectPoolBase)target;
        }

        protected override void DrawCustomInspector()
        {
            base.DrawCustomInspector();

            PMEditorUtilities.DrawLayeredProperties(serializedObject);

            PMEditorUtilities.DrawTertiaryLayer(() =>
                                                {
                                                    PMEditorUtilities.DrawNonSerializableFields(target);
                                                });

            PMEditorUtilities.DrawSecondaryLayer(() =>
                                                 {
                                                     IEnumerable pool = _target.GetPool();

                                                     foreach(object objectSet in pool)
                                                     {
                                                         EditorGUILayout.BeginHorizontal();

                                                         EditorGUILayout.LabelField("ID", PMStyles.BoldCenterLabel);

                                                         EditorGUILayout.LabelField("Pool Count",
                                                                                    PMStyles.BoldCenterLabel);

                                                         EditorGUILayout.EndHorizontal();

                                                         PropertyInfo key   = objectSet.GetType().GetProperty("Key");
                                                         PropertyInfo value = objectSet.GetType().GetProperty("Value");

                                                         PropertyInfo valueCount = value
                                                                                  .GetValue(objectSet).GetType()
                                                                                  .GetProperty("Count");

                                                         PMEditorUtilities
                                                            .DrawHorizontalColourLayer(PMEditorSettings.TertiaryLayerColour,
                                                                                       () =>
                                                                                       {
                                                                                           EditorGUILayout
                                                                                              .LabelField(key.GetValue(objectSet).ToString(),
                                                                                                          PMStyles
                                                                                                             .CenteredLabel);

                                                                                           EditorGUILayout
                                                                                              .LabelField(valueCount.GetValue(value.GetValue(objectSet)).ToString(),
                                                                                                          PMStyles
                                                                                                             .CenteredLabel);
                                                                                       });
                                                     }
                                                 });
        }
    }
}
#endif