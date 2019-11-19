// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace SOFlow.Utilities
{
    [CustomPropertyDrawer(typeof(SOFlowDynamic))]
    public class SOFlowDynamicDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
        {
            EditorGUI.BeginProperty(_position, GUIContent.none, _property);

            SerializedProperty dynamicValue = _property.FindPropertyRelative("_serializedValue");

            _position = EditorGUI.PrefixLabel(_position, GUIUtility.GetControlID(FocusType.Passive), _label);

            if(dynamicValue != null)
                dynamicValue.objectReferenceValue =
                    EditorGUI.ObjectField(_position, dynamicValue.objectReferenceValue,
                                          dynamicValue.objectReferenceValue == null
                                              ? typeof(Object)
                                              : dynamicValue.objectReferenceValue.GetType(), false);

            EditorGUI.EndProperty();
        }
    }
}
#endif