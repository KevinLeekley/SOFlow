// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

using System;
using System.Reflection;
using Pather.CSharp;
using PurpleMuffin.Internal;
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace PurpleMuffin.Data.Primitives.Editor
{
    [CustomPropertyDrawer(typeof(DataField), true)]
    public class DataFieldDrawer : PropertyDrawer
    {
        private Rect  _currentPosition;
        private bool  _isConstant;
        private float _lineHeight;

        private GUIStyle _optionsStyle;
        private int      _popupSelection;

        private float _positionWidth;

        // Property values.
        private SerializedProperty _useConstant;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            DrawDataField(position, property, label);
        }

        /// <summary>
        ///     Draws the data field.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="property"></param>
        /// <param name="label"></param>
        protected void DrawDataField(Rect position, SerializedProperty property, GUIContent label)
        {
            if(PMEditorSettings.DrawDefaultProperties)
            {
                EditorGUI.PropertyField(position, property, label);
            }
            else
            {
                _useConstant = property.FindPropertyRelative("UseConstant");
                _isConstant  = _useConstant.boolValue;

                label                   = EditorGUI.BeginProperty(position, label, property);
                _currentPosition        = EditorGUI.PrefixLabel(position, label);
                _positionWidth          = _currentPosition.width;
                _currentPosition.width  = _positionWidth * 0.2f;
                _currentPosition.height = EditorGUIUtility.singleLineHeight;

                EditorGUI.LabelField(position, label);

                if(_optionsStyle == null)
                {
                    _optionsStyle = new GUIStyle(EditorStyles.label)
                                    {
                                        normal =
                                        {
                                            background =
                                                (Texture2D)
                                                AssetDatabase
                                                   .LoadAssetAtPath("Assets/Code/Sprites/Editor/options-icon.png",
                                                                    typeof(Texture2D))
                                        }
                                    };

                    _optionsStyle.active.background  = _optionsStyle.normal.background;
                    _optionsStyle.focused.background = _optionsStyle.normal.background;
                    _optionsStyle.hover.background   = _optionsStyle.normal.background;
                    _optionsStyle.fixedWidth         = 40f;
                }

                Color originalColour = GUI.contentColor;
                GUI.contentColor = Color.clear;

                _popupSelection = EditorGUI.Popup(_currentPosition,
                                                  _isConstant ? 0 : 1,
                                                  new[]
                                                  {
                                                      "Constant", "Reference"
                                                  },
                                                  _optionsStyle);

                GUI.contentColor = originalColour;

                _currentPosition.x     += _positionWidth * 0.22f;
                _currentPosition.width =  _positionWidth * 0.78f;

                bool propertyIsArray = property.propertyPath.Contains("Array");

                switch(_popupSelection)
                {
                    case 0:

                        EditorGUI.PropertyField(_currentPosition, property.FindPropertyRelative("ConstantValueType"),
                                                GUIContent.none);

                        _lineHeight = propertyIsArray ? _lineHeight : 0f;

                        break;
                    case 1:

                        SerializedProperty referenceProperty = property.FindPropertyRelative("VariableType");

                        bool  nullDetected  = false;
                        Color currentColour = Color.white;

                        if(referenceProperty.propertyType == SerializedPropertyType.ObjectReference)
                        {
                            nullDetected = referenceProperty.objectReferenceValue == null;

                            if(nullDetected)
                            {
                                currentColour = GUI.color;
                                GUI.color     = PMEditorSettings.DeclineContextColour;
                            }
                        }

                        EditorGUI.PropertyField(_currentPosition, referenceProperty,
                                                GUIContent.none);

                        if(nullDetected) GUI.color = currentColour;

                        DataField dataValue = null;

                        if(!propertyIsArray)
                        {
                            Type      dataType  = property.serializedObject.targetObject.GetType();
                            FieldInfo fieldData = dataType.GetField(property.propertyPath);

                            if(fieldData != null)
                                dataValue = (DataField)fieldData.GetValue(property.serializedObject.targetObject);
                        }
                        else
                        {
                            Resolver resolver = new Resolver();

                            dataValue = (DataField)resolver.Resolve(property.serializedObject.targetObject,
                                                                    property.propertyPath.Replace(".Array.data", ""));
                        }

                        if(dataValue != null && dataValue.GetVariable()?.GetValueData() != null)
                        {
                            _currentPosition.y += EditorGUIUtility.singleLineHeight;
                            _lineHeight        =  EditorGUIUtility.singleLineHeight;

                            EditorGUI.LabelField(_currentPosition, dataValue.GetVariable().GetValueData().ToString(),
                                                 EditorStyles.toolbarButton);
                        }
                        else
                        {
                            _lineHeight = propertyIsArray ? _lineHeight : 0f;
                        }

                        break;
                }

                EditorGUI.EndProperty();

                if(GUI.changed)
                {
                    _useConstant.boolValue = _popupSelection == 0;
                    property.serializedObject.ApplyModifiedProperties();
                }
            }
        }

        /// <inheritdoc />
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label) + _lineHeight;
        }
    }
}
#endif