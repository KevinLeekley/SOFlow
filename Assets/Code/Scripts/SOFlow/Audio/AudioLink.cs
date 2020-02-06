// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

using System.Collections.Generic;
using System.Linq;
using SOFlow.Data.Primitives;
using UnityEngine;

namespace SOFlow.Audio
{
    [CreateAssetMenu(menuName = "SOFlow/Audio/Audio Link")]
    public class AudioLink : ScriptableObject
    {
	    /// <summary>
	    ///     The audio source.
	    /// </summary>
	    public AudioSource AudioSource;

	    /// <summary>
	    ///     The maximum amount of allowed concurrent audio clips.
	    /// </summary>
	    public IntField MaximumConcurrentClips = new IntField();

	    /// <summary>
	    ///     The audio clip cache.
	    /// </summary>
	    private readonly Dictionary<AudioClip, int> _clipCached = new Dictionary<AudioClip, int>();

	    /// <summary>
	    ///     Indicates whether a clip is present within the clip cache.
	    /// </summary>
	    private bool _clipPresent;

	    /// <summary>
	    /// Indicates whether the audio source has been created.
	    /// </summary>
	    protected bool _audioSourceGenerated = false;

	    private void OnValidate()
	    {
		    _audioSourceGenerated = false;
	    }

	    /// <summary>
	    ///     Plays the given audio clip.
	    /// </summary>
	    /// <param name="clip"></param>
	    public void PlayAudio(AudioClip clip)
        {
            int clipCount;

            if(_clipCached.TryGetValue(clip, out clipCount))
            {
                if(clipCount < MaximumConcurrentClips)
                {
                    _clipCached[clip] = ++clipCount;
                    AudioSource.PlayOneShot(clip);
                    _clipPresent = true;
                }
            }
            else
            {
                _clipCached.Add(clip, 1);
                AudioSource.PlayOneShot(clip);
                _clipPresent = true;
            }
        }

	    /// <summary>
	    /// Plays the given audio data.
	    /// </summary>
	    /// <param name="data"></param>
	    public void PlayAudio(AudioData data)
	    {
		    if(!_audioSourceGenerated)
		    {
			    AudioSource = new GameObject(name).AddComponent<AudioSource>();
			    AudioSource.loop = false;
			    AudioSource.playOnAwake = false;

			    _audioSourceGenerated = true;
		    }

		    AudioSource.clip = data.AudioClip;
		    AudioSource.volume = data.Volume;
		    AudioSource.pitch = data.Pitch;
		    
		    AudioSource.Play();
	    }

	    /// <summary>
	    ///     Clears the clip cache.
	    /// </summary>
	    public void ClearClipCache()
        {
            if(_clipPresent)
            {
                foreach(AudioClip clip in _clipCached.Keys.ToList()) _clipCached[clip] = 0;

                _clipPresent = false;
            }
        }

	    /// <summary>
	    /// Stops the currently playing audio.
	    /// </summary>
	    public void StopAudio()
	    {
		    AudioSource.Stop();
	    }
    }
}