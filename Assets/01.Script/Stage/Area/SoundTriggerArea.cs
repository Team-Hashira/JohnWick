using Hashira.Core.EventSystem;
using UnityEngine;

namespace Hashira.Stage.Area
{
	public class SoundTriggerArea : TriggerArea
	{
		[SerializeField] private GameEventChannelSO _soundEventChannel;
		[SerializeField] private float _limitLoudness = 3f;

		private void Awake()
		{
			_soundEventChannel.AddListener<NearbySoundPointEvent>(HandleOnSoundGenerated);
		}

		private void OnDestroy()
		{
			_soundEventChannel.RemoveListener<NearbySoundPointEvent>(HandleOnSoundGenerated);
		}

		private void HandleOnSoundGenerated(NearbySoundPointEvent evt)
		{
			// �Ѱ谪 �̻��� ������ �߻���?
			if (evt.loudness < _limitLoudness) return;

			// ���� ���� ����?
			if (Physics2D.OverlapBox(transform.position, size, transform.eulerAngles.z, whatIsTarget) == false) return;

			// �׷� ����
			Invoke();
		}
	}
}
