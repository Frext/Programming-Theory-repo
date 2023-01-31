using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Project.Scripts.Gameplay.SFX
{

	
	public class SFXHandler : MonoBehaviour
	{
		[Header("Audio System References")]
		[SerializeField] private AudioSource mainAudioSource;

		[Serializable]
		public enum AudioClipTypes
		{
			Footsteps,
			Attack,
			Heal
		}
		
		[Serializable]
		private class AudioClipClass
		{
			public AudioClipTypes audioClipType;
			public AudioClip audioClip;
		}
		
		[Space]
		[SerializeField] private List<AudioClipClass> audioClipClassList = new ();
		
		public void PlayOneShotSound(AudioClipTypes audioClipType, SFXElement callerSFXElement)
		{
			CheckIfSFXElement(callerSFXElement);
			
			
			mainAudioSource.PlayOneShot(
				GetAudioClipFromType(audioClipType), mainAudioSource.volume);
		}
		
		private void CheckIfSFXElement(SFXElement callerSFXElement)
		{
			if (callerSFXElement == null)
				throw new Exception("SFXHandler cannot be called without an SFXElement.");
		}

		private AudioClip GetAudioClipFromType(AudioClipTypes audioClipType)
		{
			foreach (var audioClipClass in audioClipClassList.Where(audioClipClass => audioClipType == audioClipClass.audioClipType))
			{
				if (audioClipClass.audioClip != null)
					return audioClipClass.audioClip;
				
				throw new Exception("There was no audioClip for " + audioClipType + " in " + nameof(SFXHandler) +
				                    " of " + gameObject.name);
			}

			throw new Exception("There was no " + audioClipType + " audio type assigned in " + nameof(SFXHandler) +
			                    " of " + gameObject.name);
		}

		public void PlayRepeatedly(AudioClipTypes audioClipType, SFXElement callerSFXElement)
		{
			CheckIfSFXElement(callerSFXElement);
			
			
			mainAudioSource.clip = GetAudioClipFromType(audioClipType);
			mainAudioSource.Play();
		}
		
		public void StopPlayingRepeatedly(SFXElement callerSFXElement)
		{
			CheckIfSFXElement(callerSFXElement);
			
			// Don't call stop, because it stops the other PlayOneShot calls.
			mainAudioSource.clip = null;
		}
		
		public bool IsPlaying()
		{
			return mainAudioSource.isPlaying;
		}
	}
}
