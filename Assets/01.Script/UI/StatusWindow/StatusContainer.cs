using System;
using Crogen.UIExtension;
using UnityEngine;

namespace Hashira.UI.StatusWindow
{
    public class StatusContainer : MonoBehaviour
    {
        [SerializeField] private InputReaderSO _inputReaderSO;
        private Tap _tap;
        
        private void Awake()
        {
            _tap = GetComponent<Tap>();
            _inputReaderSO.OnStatusTapMoveToSideEvent += HandleMoveToSide;
        }

        private void OnDestroy()
        {
            _inputReaderSO.OnStatusTapMoveToSideEvent -= HandleMoveToSide;
        }

        private void HandleMoveToSide(float axis)
        {
            switch (axis)
            {
                case < 0:
                    _tap.MoveToLeft();
                    break;
                case > 0:
                    _tap.MoveToRight();
                    break;
            }
        }
    }
}
