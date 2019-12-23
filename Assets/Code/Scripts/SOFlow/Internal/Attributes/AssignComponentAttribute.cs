// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

using System;
using UnityEngine;

namespace SOFlow.Internal
{
	[AttributeUsage(AttributeTargets.Field)]
	public class AssignComponentAttribute : PropertyAttribute
	{
		/// <summary>
		/// The type to assign to the component.
		/// </summary>
		public Type AssigningType = null;

		public AssignComponentAttribute()
		{
		}

		public AssignComponentAttribute(Type type)
		{
			AssigningType = type;
		}
	}
}