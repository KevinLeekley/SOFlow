﻿// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace SOFlow.ScriptableObjects
{
	public class DropdownScriptableObject : ScriptableObject
	{
		public virtual void OnValidate()
		{
			RegisterDropdownEntry();
		}

		public virtual void Awake()
		{
			RegisterDropdownEntry();
		}

		public virtual void OnEnable()
		{
			RegisterDropdownEntry();
		}

		/// <summary>
		/// Registers this dropdown ScriptableObject to the available dropdowns list. 
		/// </summary>
		public void RegisterDropdownEntry()
		{
			List<ScriptableObject> dropdowns;
			Type                   type = GetType();

			if(!DropdownScriptableObjectAttributeDrawer.AvailableDropdowns.TryGetValue(type, out dropdowns))
			{
				dropdowns = new List<ScriptableObject>
				            {
					            this
				            };

				DropdownScriptableObjectAttributeDrawer.AvailableDropdowns.Add(type, dropdowns);
			}
			else
			{
				if(!dropdowns.Contains(this))
				{
					dropdowns.Add(this);
					DropdownScriptableObjectAttributeDrawer.AvailableDropdowns[type] = dropdowns;
				}
			}

			foreach(KeyValuePair<Type, List<ScriptableObject>> dropdownData in DropdownScriptableObjectAttributeDrawer
			   .AvailableDropdowns)
			{
				if(dropdownData.Key != type && type.IsSubclassOf(dropdownData.Key))
				{
					if(!dropdownData.Value.Contains(this))
					{
						dropdownData.Value.Add(this);
					}
				}
			}
		}
	}
}