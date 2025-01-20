using System;
using Crogen.AttributeExtension;
using Hashira.Core.EventSystem;
using UnityEngine;

namespace Hashira.Test
{
    public class SoundGenerator : MonoBehaviour
    {
        [SerializeField]
        private GameEventChannelSO _soundEventChannel;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GenerateSound();
            }
        }

        [Button("Generate", 20)]
        private void GenerateSound()
        {
            var evt = SoundEvents.SoundGeneratedEvent;
            evt.originPosition = transform.position;
            evt.loudness = 30;
            _soundEventChannel.RaiseEvent(evt);
        }
    }
}
