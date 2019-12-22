// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using SOFlow.Extensions;
using SOFlow.Internal;
using UnityEditor;
using UnityEngine;

namespace SOFlow.ScriptableObjects
{
	[CustomPropertyDrawer(typeof(DropdownScriptableObject), true)]
	public class DropdownScriptableObjectAttributeDrawer : PropertyDrawer
	{
		/// <summary>
		/// The set of available dropdowns.
		/// </summary>
		public static readonly Dictionary<Type, List<ScriptableObject>> AvailableDropdowns = new Dictionary<Type, List<ScriptableObject>>();
		
		/// <summary>
		/// The none dropdown option.
		/// </summary>
		private readonly GUIContent _noneOption = new GUIContent("None");

		/// <inheritdoc />
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			Type objectType = TypeExtensions.GetInstanceType(property.type);

			if(objectType == null || SOFlowEditorSettings.DrawDefaultProperties)
			{
				EditorGUI.PropertyField(position, property, label);
			}
			else
			{
				List<ScriptableObject> dropdowns;

				if(!AvailableDropdowns.TryGetValue(objectType, out dropdowns))
				{
					AvailableDropdowns.Add(objectType, new List<ScriptableObject>());
				}
				else
				{
					ValidateDropdownEntries();
					SortDropdownEntries();
					
					int selection = 0;
					int optionLength = dropdowns.Count + 1;
					
					GUIContent[] options = new GUIContent[optionLength];
					options[0] = _noneOption;

					for(int i = 1; i < optionLength; i++)
					{
						if(dropdowns[i - 1] == property.objectReferenceValue)
						{
							selection = i;
						}

						options[i] = new GUIContent($"{dropdowns[i - 1].name} ({dropdowns[i - 1].GetType().Name})", AssetDatabase.GetCachedIcon(AssetDatabase.GetAssetPath(dropdowns[i - 1])));
					}

					EditorGUI.BeginChangeCheck();
					
					selection = EditorGUI.Popup(position, label, selection, options);

					if(EditorGUI.EndChangeCheck())
					{
						property.objectReferenceValue = selection < 1 ? null : dropdowns[selection - 1];
						property.serializedObject.ApplyModifiedProperties();
					}
				}
			}
		}

		/// <summary>
		/// Removes any non-existing entries from the list of available dropdown items.
		/// </summary>
		private static void ValidateDropdownEntries()
		{
			foreach(KeyValuePair<Type, List<ScriptableObject>> dropdownData in AvailableDropdowns)
			{
				dropdownData.Value.ValidateListEntries(dropdown => dropdown);
			}
		}

		/// <summary>
		/// Sorts the dropdown entries.
		/// </summary>
		private static void SortDropdownEntries()
		{
			foreach(KeyValuePair<Type, List<ScriptableObject>> dropdownData in AvailableDropdowns)
			{
				dropdownData.Value.Sort((first, second) => string.Compare(first.name, second.name, StringComparison.Ordinal));
			}
		}
	}
}
#endif