// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

using System.Collections;
using System.Collections.Generic;
using SOFlow.Data.Primitives;
using SOFlow.ScriptableObjects;
using UnityEngine;

namespace SOFlow.Audio
{
	[CreateAssetMenu(menuName = "SOFlow/Audio/Audio Data")]
	public class AudioData : DropdownScriptableObject
	{
		/// <summary>
		/// The audio clip.
		/// </summary>
		public AudioClip AudioClip;

		/// <summary>
		/// The volume.
		/// </summary>
		public FloatField Volume = new FloatField
		                           {
			                           Value = 1
		                           };

		/// <summary>
		/// The pitch.
		/// </summary>
		public FloatField Pitch = new FloatField
		                          {
			                          Value = 1
		                          };
	}
}