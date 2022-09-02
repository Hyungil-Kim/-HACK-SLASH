using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : Singleton<SoundManager>
{
    protected SoundManager()
	{

	}
	#region Inspector
	public AudioSource _BGMAudioSource;
	[SerializeField]
	private AudioMixerGroup _BGMAudioMixer;
	[SerializeField]
	private AudioSource _SFXAudioSource;
	[SerializeField]
	private AudioMixerGroup _SFXAudioMixer;

	public float _BGM = 1f;
	public float _SFX = 1f;
	#endregion Inspector

	#region MonoBehaviour
	private void Start()
	{
		_BGMAudioSource.outputAudioMixerGroup = _BGMAudioMixer;
		if(_BGMAudioSource.clip != null)
		{
			currentClip = _BGMAudioSource.clip;
			PlayBGM(currentClip);
		}
		DontDestroyOnLoad(this);
	}
	#endregion MonoBehaviour

	#region private variable

	#endregion private varialbe

	#region public variable
	[System.NonSerialized]
	public AudioClip currentClip;
	#endregion public variable

	#region private Function

	#endregion private Function

	#region public Function
	public void PlayBGM(AudioClip clip, float spatialBlend = 0f)
	{
		_BGMAudioSource.spatialBlend = spatialBlend;
		GameManager.Instance.SettingMusic(_BGMAudioSource);
		if (clip == null)
		{
			return;
		}
		else
		{
			if (_BGMAudioSource.isPlaying)
			{
				if (_BGMAudioSource.clip == clip) { return; }
				_BGMAudioSource.Stop();
			}
			currentClip = clip;
			_BGMAudioSource.clip = currentClip;
			_BGMAudioSource.Play();
		}
	}

	public void PlaySFX(AudioClip clip, float spatialBlend = 1f)
	{
		_SFXAudioSource.spatialBlend = spatialBlend;
		if (clip == null)
		{
			return;
		}
		else
		{
			_SFXAudioSource.PlayOneShot(clip);
		}
	}
	#endregion public Function

}
