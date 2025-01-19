using Crogen.CrogenPooling;
using System;
using UnityEngine;

namespace Hashira
{
    //Init이 따로 있으면 상속해서 쓰세요
    public class StartupPop : MonoBehaviour
    {
        [SerializeField] private GameObject _poolablePrefab;
        private IPoolingObject _poolingObject;

        private void Start()
        {
            PopObject();
        }

        protected virtual GameObject PopObject()
        {
            return gameObject.Pop(_poolingObject.OriginPoolType, transform).gameObject;
        }

        private void OnValidate()
        {
            if (_poolablePrefab.TryGetComponent(out IPoolingObject poolingObject))
                _poolingObject = poolingObject;
            else
            {
                _poolablePrefab = null;
                Debug.LogError($"{_poolablePrefab.name} is not IPoolingObject");
            }

        }
    }
}
