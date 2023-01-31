using UnityEngine;

namespace _Project.Scripts.Gameplay.SFX
{
	public class SFXElement : MonoBehaviour
	{
		[SerializeField] private SFXHandler sfxHandler;

		[Space] 
		[SerializeField] private SFXHandler.AudioClipTypes audioClipType;
		
		
		public void PlayOneShotSound()
		{
			sfxHandler.PlayOneShotSound(audioClipType, this);
		}

		public void PlayRepeatedly()
		{
			if(sfxHandler.IsPlaying())
				return;
				
			sfxHandler.PlayRepeatedly(audioClipType, this);
		}

		public void StopPlayingRepeatedly()
		{
			sfxHandler.StopPlayingRepeatedly(this);
		}
	}
}
