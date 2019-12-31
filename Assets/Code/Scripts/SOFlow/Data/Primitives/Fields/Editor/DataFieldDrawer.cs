// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

#if UNITY_EDITOR
using UnityEditorInternal;
using Object = UnityEngine.Object;
using System;
using System.Reflection;
using Pather.CSharp;
using SOFlow.Extensions;
using SOFlow.Internal;
using UnityEditor;
using UnityEngine;

namespace SOFlow.Data.Primitives.Editor
{
    [CustomPropertyDrawer(typeof(DataField), true)]
    public class DataFieldDrawer : PropertyDrawer
    {
        private Rect  _currentPosition;
        private bool  _isConstant;
        private bool  _displayValueChangedEvent;
        private float _lineHeight;

        private float _positionWidth;
        private float _buttonWidth = 18f;

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
            if(SOFlowEditorSettings.DrawDefaultProperties)
            {
                EditorGUI.PropertyField(position, property, label);
            }
            else
            {
                SerializedProperty useConstant  = property.FindPropertyRelative("UseConstant");
                Event              currentEvent = Event.current;

                if(position.Contains(currentEvent.mousePosition))
                {
                    if(currentEvent.type == EventType.DragUpdated)
                    {
                        DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                        currentEvent.Use();
                    }
                    else if(currentEvent.type == EventType.DragPerform && DragAndDrop.objectReferences.Length > 0)
                    {
                        Object             draggedObject     = DragAndDrop.objectReferences[0];
                        SerializedProperty referenceProperty = property.FindPropertyRelative("VariableType");
                        Type               variableType      = TypeExtensions.GetInstanceType(referenceProperty.type);

                        if(draggedObject.GetType().IsAssignableFrom(variableType))
                        {
                            referenceProperty.objectReferenceValue = draggedObject;
                            useConstant.boolValue                  = false;
                            property.serializedObject.ApplyModifiedProperties();
                        }

                        currentEvent.Use();

                        return;
                    }
                }

                SerializedProperty displayValueChangedProperty =
                    property.FindPropertyRelative("_displayValueChangedEvent");

                _displayValueChangedEvent = displayValueChangedProperty.boolValue;
                _isConstant               = useConstant.boolValue;
                label                     = EditorGUI.BeginProperty(position, label, property);
                _currentPosition          = EditorGUI.PrefixLabel(position, label);
                _positionWidth            = _currentPosition.width;
                _currentPosition.width    = _positionWidth * 0.2f;
                _currentPosition.height   = EditorGUIUtility.singleLineHeight;

                EditorGUI.LabelField(position, label);

                Rect buttonsRect = new Rect(_currentPosition)
                                   {
                                       width = _buttonWidth
                                   };

                if(GUI.Button(buttonsRect, "R", _isConstant ? SOFlowStyles.Button : SOFlowStyles.PressedButton))
                {
                    _isConstant = false;
                }

                buttonsRect.x += _buttonWidth;

                if(GUI.Button(buttonsRect, "V", _isConstant ? SOFlowStyles.PressedButton : SOFlowStyles.Button))
                {
                    _isConstant = true;
                }

                if(_isConstant)
                {
                    buttonsRect.x     += _buttonWidth;
                    buttonsRect.width += 6f;

                    SerializedProperty persistentCalls =
                        property.FindPropertyRelative("OnConstantValueChanged._PersistentCalls");

                    Color originalColour = GUI.backgroundColor;
                    GUI.backgroundColor = persistentCalls.arraySize > 0 ? SOFlowEditorSettings.AcceptContextColour : SOFlowEditorSettings.DeclineContextColour;

                    if(GUI.Button(buttonsRect, $"E{persistentCalls.arraySize}", SOFlowStyles.ButtonSmallText))
                    {
                        _displayValueChangedEvent = !_displayValueChangedEvent;
                    }

                    GUI.backgroundColor = originalColour;
                }

                _currentPosition.x     += _positionWidth * 0.28f;
                _currentPosition.width =  _positionWidth * 0.72f;

                bool propertyIsArray = property.propertyPath.Contains("Array");

                if(_isConstant)
                {
                    EditorGUI.PropertyField(_currentPosition, property.FindPropertyRelative("ConstantValueType"),
                                            GUIContent.none);

                    _lineHeight = propertyIsArray ? _lineHeight : 0f;
                }
                else
                {
                    SerializedProperty referenceProperty = property.FindPropertyRelative("VariableType");

                    bool  nullDetected  = false;
                    Color currentColour = Color.white;

                    if(referenceProperty.propertyType == SerializedPropertyType.ObjectReference)
                    {
                        nullDetected = referenceProperty.objectReferenceValue == null;

                        if(nullDetected)
                        {
                            currentColour = GUI.color;
                            GUI.color     = SOFlowEditorSettings.DeclineContextColour;
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
                }

                if(_displayValueChangedEvent)
                {
                    _currentPosition.x     =  position.x;
                    _currentPosition.y     += EditorGUIUtility.singleLineHeight;
                    _currentPosition.width =  position.width;
                    EditorGUI.PropertyField(_currentPosition, property.FindPropertyRelative("OnConstantValueChanged"));
                }

                EditorGUI.EndProperty();

                if(GUI.changed)
                {
                    useConstant.boolValue                 = _isConstant;
                    displayValueChangedProperty.boolValue = _displayValueChangedEvent;
                    property.serializedObject.ApplyModifiedProperties();
                }
            }
        }

        /// <inheritdoc />
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float eventLineHeight = 0f;

            if(_displayValueChangedEvent)
            {
                int persistentArgumentsCount = 0;

                SerializedProperty persistentCalls = property.FindPropertyRelative("OnConstantValueChanged._PersistentCalls");

                for(int i = 0, condition = persistentCalls.arraySize; i < condition; i++)
                {
                    SerializedProperty persistentCall = persistentCalls.GetArrayElementAtIndex(i);

                    SerializedProperty persistentArguments =
                        persistentCall.FindPropertyRelative("_PersistentArguments");

                    persistentArgumentsCount += persistentArguments.arraySize;
                }

                ReorderableList eventDrawer = new ReorderableList(property.serializedObject, persistentCalls)
                                              {
                                                  elementHeight =
                                                      EditorGUIUtility.singleLineHeight * 1.45f,
                                                  headerHeight = 0f
                                              };

                SerializedProperty events = property.FindPropertyRelative("OnConstantValueChanged");

                eventLineHeight = eventDrawer.GetHeight() - 
                                  (events.isExpanded
                                       ? 0f
                                       : EditorGUIUtility.singleLineHeight * Mathf.Max(1, eventDrawer.count) *
                                         1.45f
                                  ) +
                                  (events.isExpanded
                                       ? EditorGUIUtility.singleLineHeight * 1.125f * persistentArgumentsCount
                                       : 0f);
            }

            return base.GetPropertyHeight(property, label) + _lineHeight + eventLineHeight;
        }
    }
}
#endif