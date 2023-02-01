using System;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Managers.SFX
{
	public class SFXElement : MonoBehaviour
	{
		[SerializeField] private SFXHandler sfxHandler;

		[Space] 
		[SerializeField] private SFXHandler.AudioClipTypes audioClipType;

		void Start()
		{
			if (sfxHandler == null)
				throw new Exception("There was no " + nameof(sfxHandler) + " assigned in the "
				                    + nameof(SFXElement) + " script of " + gameObject.name);
		}

		#region Methods Used By Other Scripts

		public void PlayOneShotSound()
		{
			sfxHandler.PlayOneShotSound(audioClipType, this);
		}

		public void PlayRepeatedly()
		{
			sfxHandler.PlayRepeatedly(audioClipType, this);
		}

		public void StopPlayingRepeatedly()
		{
			sfxHandler.StopPlayingRepeatedly(this);
		}

		#endregion
	}
}
