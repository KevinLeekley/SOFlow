// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

#if UNITY_EDITOR
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
        private float _lineHeight;
        private bool _updateDragReferences;
        private bool _updateDragReferencePersistenceCheck;
        private Object _dragReference;

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
            if(SOFlowEditorSettings.DrawDefaultProperties)
            {
                EditorGUI.PropertyField(position, property, label);
            }
            else
            {
                _useConstant = property.FindPropertyRelative("UseConstant");
                Event currentEvent = Event.current;

                if(position.Contains(currentEvent.mousePosition))
                {
                    if(currentEvent.type == EventType.DragUpdated)
                    {
                        DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                        currentEvent.Use();
                    }
                    else if(currentEvent.type == EventType.DragExited && DragAndDrop.objectReferences.Length > 0)
                    {
                        Object             draggedObject     = DragAndDrop.objectReferences[0];
                        SerializedProperty referenceProperty = property.FindPropertyRelative("VariableType");
                        Type               variableType      = TypeExtensions.GetInstanceType(referenceProperty.type);

                        if(draggedObject.GetType().IsAssignableFrom(variableType))
                        {
                            // For some reason, Unity is not saving the newly assigned property values if we save those values
                            // here. The values stay alive for a few frames, then return to their previous values before the
                            // change. I have been unable to figure out why this is the case. This is a temporary workaround
                            // until a proper solution is made.
                            _dragReference = draggedObject;
                            _updateDragReferences = true;
                        }

                        currentEvent.Use();

                        return;
                    }
                }
                else if(_updateDragReferences)
                {
                    SerializedProperty referenceProperty = property.FindPropertyRelative("VariableType");
                    referenceProperty.objectReferenceValue = _dragReference;
                    _useConstant.boolValue = false;
                    property.serializedObject.ApplyModifiedProperties();

                    _updateDragReferences = false;
                    _updateDragReferencePersistenceCheck = true;
                }
                else if(_updateDragReferencePersistenceCheck)
                {
                    SerializedProperty referenceProperty = property.FindPropertyRelative("VariableType");

                    if(referenceProperty.objectReferenceValue != _dragReference || _useConstant.boolValue)
                    {
                        referenceProperty.objectReferenceValue = _dragReference;
                        _useConstant.boolValue                 = false;
                        property.serializedObject.ApplyModifiedProperties();
                        _updateDragReferencePersistenceCheck = false;
                    }
                }

                _isConstant             = _useConstant.boolValue;
                label                   = EditorGUI.BeginProperty(position, label, property);
                _currentPosition        = EditorGUI.PrefixLabel(position, label);
                _positionWidth          = _currentPosition.width;
                _currentPosition.width  = _positionWidth * 0.2f;
                _currentPosition.height = EditorGUIUtility.singleLineHeight;

                EditorGUI.LabelField(position, label);

                Rect buttonsRect = new Rect(_currentPosition)
                                   {
                                       width = 25f
                                   };

                if(GUI.Button(buttonsRect, "R", _isConstant ? SOFlowStyles.Button : SOFlowStyles.PressedButton))
                {
                    _isConstant = false;
                }

                buttonsRect.x += 25f;

                if(GUI.Button(buttonsRect, "V", _isConstant ? SOFlowStyles.PressedButton : SOFlowStyles.Button))
                {
                    _isConstant = true;
                }

                _currentPosition.x     += _positionWidth * 0.22f;
                _currentPosition.width =  _positionWidth * 0.78f;

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

                EditorGUI.EndProperty();

                if(GUI.changed)
                {
                    _useConstant.boolValue = _isConstant;
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