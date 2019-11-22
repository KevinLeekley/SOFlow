// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

using System;
using SOFlow.Extensions;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;
#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditorInternal;

namespace SOFlow.Internal
{
    public static partial class SOFlowEditorUtilities
    {
        /// <summary>
        ///     The list of cached reorderable lists.
        /// </summary>
        private static readonly Dictionary<string, ReorderableList> _cachedReorderableLists =
            new Dictionary<string, ReorderableList>();

        /// <summary>
        ///     Draws the given property for the serialized object if the property is available.
        /// </summary>
        /// <param name="serializedObject"></param>
        /// <param name="property"></param>
        /// <param name="includeChildren"></param>
        public static void DrawProperty(this SerializedObject serializedObject, string property,
                                        bool                  includeChildren = true)
        {
            SerializedProperty serializedProperty = serializedObject.FindProperty(property);

            if(serializedProperty != null) EditorGUILayout.PropertyField(serializedProperty, includeChildren);
        }

        /// <summary>
        ///     Draws a list for the component property.
        /// </summary>
        /// <param name="property"></param>
        /// <param name="layerColour"></param>
        /// <param name="serializedObject"></param>
        public static void DrawListComponentProperty(SerializedObject serializedObject, SerializedProperty property,
                                                     Color            layerColour)
        {
            EditorGUILayout.BeginHorizontal();

            if(DrawColourButton(property.isExpanded ? "↑" : "↓", SOFlowEditorSettings.AcceptContextColour, null,
                                GUILayout.MaxWidth(25f)))
                property.isExpanded = !property.isExpanded;

            GUILayout.FlexibleSpace();
            EditorGUILayout.LabelField(property.displayName, SOFlowStyles.BoldCenterLabel);
            GUILayout.FlexibleSpace();

            EditorGUILayout.LabelField($"Size: {property.arraySize}", SOFlowStyles.WordWrappedMiniLabel);

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            GUIContent addButtonContent = new GUIContent($"Add {property.displayName} Entry");

            if(DrawColourButton(addButtonContent.text, SOFlowEditorSettings.AcceptContextColour, null,
                                GUILayout.MaxWidth(EditorStyles.label.CalcSize(addButtonContent).x + 24)))
                property.InsertArrayElementAtIndex(property.arraySize);

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            if(property.isExpanded)
                DrawColourLayer(layerColour, () =>
                                             {
                                                 ReorderableList reorderableList =
                                                     GetCachedReorderableList(serializedObject, property);

                                                 if(reorderableList == null)
                                                 {
                                                     reorderableList =
                                                         new ReorderableList(serializedObject, property, true, false,
                                                                             true, true)
                                                         {
                                                             showDefaultBackground = false, headerHeight = 0f
                                                         };

                                                     reorderableList.onCanRemoveCallback += list => list.count > 0;

                                                     reorderableList.drawFooterCallback += rect =>
                                                                                           {
                                                                                               Color originalGUIColor =
                                                                                                   GUI.backgroundColor;

                                                                                               GUI.backgroundColor =
                                                                                                   SOFlowEditorSettings
                                                                                                      .AcceptContextColour;

                                                                                               ReorderableList
                                                                                                  .defaultBehaviours
                                                                                                  .DrawFooter(rect,
                                                                                                              reorderableList);

                                                                                               GUI.backgroundColor =
                                                                                                   originalGUIColor;
                                                                                           };

                                                     reorderableList.drawElementCallback +=
                                                         (rect, index, active, focused) =>
                                                         {
                                                             if(reorderableList.serializedProperty.arraySize <= index)
                                                                 return;

                                                             rect.x     -= 20f;
                                                             rect.width += 24f;

                                                             EditorGUI.HelpBox(rect, "", MessageType.None);

                                                             rect.x     += 20f;
                                                             rect.width -= 24f;

                                                             SerializedProperty indexProperty =
                                                                 reorderableList
                                                                    .serializedProperty.GetArrayElementAtIndex(index);

                                                             string             entryTitle     = null;
                                                             SerializedProperty copiedProperty = indexProperty.Copy();

                                                             if(copiedProperty.Next(true))
                                                                 entryTitle =
                                                                     copiedProperty.propertyType ==
                                                                     SerializedPropertyType.String
                                                                         ? copiedProperty.stringValue
                                                                         : null;

                                                             rect.height =
                                                                 EditorGUI.GetPropertyHeight(indexProperty,
                                                                                             GUIContent.none, true);

                                                             rect.y++;

                                                             if(indexProperty.hasVisibleChildren)
                                                             {
                                                                 rect.x     += 12f;
                                                                 rect.width -= 12f;
                                                             }

                                                             rect.width -= 25f;

                                                             EditorGUI.PropertyField(rect, indexProperty,
                                                                                     new
                                                                                         GUIContent($"Entry {index + 1}{(entryTitle == null ? "" : $" | {entryTitle}")}"),
                                                                                     true);

                                                             rect.x      += rect.width;
                                                             rect.width  =  25f;
                                                             rect.height =  EditorGUIUtility.singleLineHeight;

                                                             Color originalGUIColor = GUI.backgroundColor;

                                                             GUI.backgroundColor =
                                                                 SOFlowEditorSettings.DeclineContextColour;

                                                             if(EditorGUI.Toggle(rect, false, SOFlowStyles.Button))
                                                                 reorderableList
                                                                    .serializedProperty
                                                                    .DeleteArrayElementAtIndex(index);

                                                             EditorGUI.LabelField(rect, "-", SOFlowStyles.CenteredLabel);

                                                             GUI.backgroundColor = originalGUIColor;

                                                             reorderableList.elementHeight = rect.height + 4f;
                                                         };

                                                     reorderableList.elementHeightCallback +=
                                                         index =>
                                                         {
                                                             if(reorderableList.serializedProperty.arraySize <= index)
                                                                 return EditorGUIUtility.singleLineHeight;

                                                             return Mathf.Max(EditorGUIUtility.singleLineHeight,
                                                                              EditorGUI
                                                                                 .GetPropertyHeight(reorderableList.serializedProperty.GetArrayElementAtIndex(index),
                                                                                                    GUIContent.none,
                                                                                                    true)) + 4f;
                                                         };

                                                     _cachedReorderableLists
                                                        .Add($"{serializedObject.targetObject.name}{property.propertyPath}",
                                                             reorderableList);
                                                 }
                                                 else
                                                 {
                                                     reorderableList.serializedProperty = property;
                                                 }

                                                 try
                                                 {
                                                     reorderableList.DoLayoutList();
                                                 }
                                                 catch(Exception)
                                                 {
                                                     // Ignore
                                                 }

                                                 Rect listRect =
                                                     GUILayoutUtility.GetLastRect();

                                                 float listHeight = reorderableList.GetHeight();

                                                 listRect.y      -= listHeight - reorderableList.footerHeight;
                                                 listRect.height += listHeight - reorderableList.footerHeight;

                                                 if(listRect.Contains(Event.current.mousePosition))
                                                 {
                                                     if(Event.current.type == EventType.DragUpdated)
                                                     {
                                                         DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                                                         Event.current.Use();
                                                     }
                                                     else if(Event.current.type == EventType.DragPerform)
                                                     {
                                                         if(DragAndDrop.objectReferences.Length > 0)
                                                         {
                                                             foreach(Object _object in DragAndDrop.objectReferences)
                                                             {
                                                                 SerializedProperty newProperty = null;

                                                                 try
                                                                 {
                                                                     property.InsertArrayElementAtIndex(property
                                                                                                           .arraySize);

                                                                     newProperty =
                                                                         property.GetArrayElementAtIndex(property
                                                                                                            .arraySize -
                                                                                                         1);

                                                                     newProperty.objectReferenceValue = _object;

                                                                     if(newProperty.objectReferenceValue == null)
                                                                         throw new Exception();
                                                                 }
                                                                 catch(Exception e)
                                                                 {
                                                                     Debug
                                                                        .LogWarning($"Object [{_object?.GetType()}] not compatible with this list.\n" +
                                                                                    $"Expected object type : [{property.arrayElementType}]\n\n"       +
                                                                                    $"{e.Message}\n\n{e.StackTrace}");

                                                                     Debug
                                                                        .Log("Attempting to find embedded compatible types.");

                                                                     if(SetInnerPropertyReference(newProperty, _object))
                                                                     {
                                                                         Debug.Log("Compatible embedded type found.");
                                                                     }
                                                                     else
                                                                     {
                                                                         Debug
                                                                            .LogWarning("Unable to find compatible embedded type.");

                                                                         property.DeleteArrayElementAtIndex(property
                                                                                                               .arraySize -
                                                                                                            1);
                                                                     }
                                                                 }
                                                             }

                                                             serializedObject.ApplyModifiedProperties();
                                                         }

                                                         Event.current.Use();
                                                     }
                                                 }
                                             });
        }

        /// <summary>
        ///     Attempts to set an inner property reference for the given property to the provided object value.
        /// </summary>
        /// <param name="property"></param>
        /// <param name="_object"></param>
        /// <returns></returns>
        private static bool SetInnerPropertyReference(SerializedProperty property, Object _object)
        {
            property.NextVisible(true);

            do
            {
                try
                {
                    Type instanceType =
                        TypeExtensions.GetInstanceType(property.type.Replace("PPtr<$", "").Replace(">", ""));

                    if(instanceType == _object.GetType())
                    {
                        property.objectReferenceValue = _object;

                        return true;
                    }
                }
                catch
                {
                    return false;
                }
            } while(property.NextVisible(false));

            return false;
        }

        /// <summary>
        ///     Gets the cached version of the ReorderableList linked with the given property.
        /// </summary>
        /// <param name="property"></param>
        /// <param name="serializedObject"></param>
        /// <returns></returns>
        private static ReorderableList GetCachedReorderableList(SerializedObject   serializedObject,
                                                                SerializedProperty property)
        {
            ReorderableList result = null;

            _cachedReorderableLists.TryGetValue($"{serializedObject.targetObject.name}{property.propertyPath}",
                                                out result);

            return result;
        }

        /// <summary>
        ///     Draws all properties for the provided serialized object within custom layers.
        /// </summary>
        /// <param name="serializedObject"></param>
        public static void DrawLayeredProperties(SerializedObject serializedObject)
        {
            DrawPrimaryLayer(() =>
                             {
                                 SerializedProperty nextProperty = serializedObject.GetIterator();

                                 if(nextProperty.NextVisible(true))
                                     do
                                     {
                                         if(nextProperty.isArray &&
                                            nextProperty.propertyType != SerializedPropertyType.String)
                                         {
                                             DrawSecondaryLayer(() =>
                                                                {
                                                                    DrawListComponentProperty(serializedObject,
                                                                                              nextProperty,
                                                                                              SOFlowEditorSettings
                                                                                                 .TertiaryLayerColour);
                                                                });
                                         }
                                         else
                                         {
                                             SerializedProperty objectProperty =
                                                 serializedObject.FindProperty(nextProperty.name);

                                             DrawPropertyWithNullCheck(objectProperty);
                                         }
                                     } while(nextProperty.NextVisible(false));
                             });
        }

        /// <summary>
        ///     Draws the given property and indicates whether the property object value is null.
        /// </summary>
        /// <param name="property"></param>
        public static void DrawPropertyWithNullCheck(SerializedProperty property)
        {
            bool  nullDetected  = false;
            Color currentColour = _originalGUIColour;

            if(property.propertyType == SerializedPropertyType.ObjectReference)
            {
                nullDetected = property.objectReferenceValue == null;

                if(nullDetected)
                {
                    currentColour = GUI.color;
                    GUI.color     = SOFlowEditorSettings.DeclineContextColour;
                }
            }

            EditorGUILayout.PropertyField(property, true);

            if(nullDetected) GUI.color = currentColour;
        }
    }
}
#endif