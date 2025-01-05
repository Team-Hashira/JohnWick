using System;
using Hashira.Entities;
using UnityEngine;

namespace Hashira.Players
{
    public class PlayerMover : EntityMover
    {
        Player _player;
        
        private void Start()
        {
            _player = _entity as Player;    
            //_player.InputReader.            
        }

        public void UnderJump(bool isUnderJump)
        {
            foreach (var platformEffector in FindObjectsByType<PlatformEffector2D>(FindObjectsSortMode.None))
            {
                platformEffector.rotationalOffset = isUnderJump ? 180 : 0;
            }
        }
    }
}