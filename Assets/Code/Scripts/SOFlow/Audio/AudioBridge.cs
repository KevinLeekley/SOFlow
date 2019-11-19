// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

using UnityEngine;

namespace SOFlow.Audio
{
    public class AudioBridge : MonoBehaviour
    {
	    /// <summary>
	    ///     The audio link.
	    /// </summary>
	    public AudioLink AudioLink;

	    /// <summary>
	    ///     The audio source.
	    /// </summary>
	    public AudioSource AudioSource;

        private void Awake()
        {
            AudioLink.AudioSource = AudioSource;
        }

        private void Update()
        {
            AudioLink.ClearClipCache();
        }
    }
}