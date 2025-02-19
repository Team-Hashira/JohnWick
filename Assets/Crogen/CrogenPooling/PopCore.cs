using System;
using System.Collections.Generic;
using UnityEngine;

namespace Crogen.CrogenPooling
{
    public static class PopCore
    {
        private static PoolManager _poolManager { get; set; }
        private static List<PoolCategorySO> _poolBaseList;
        
        public static void Init(PoolManager poolManager, List<PoolCategorySO> poolBaseList)
        {
            _poolManager = poolManager;
            _poolBaseList = poolBaseList;
        }

        public static IPoolingObject Pop(this GameObject target, Enum type, Transform parent = null)
        {
            string typeName = type.GetType().ToString().Replace("Crogen.CrogenPooling.", "").Replace("PoolType", "");
            string enumName = type.ToString();

            return Pop(target, $"{typeName}.{enumName}", parent);
        }
        public static IPoolingObject Pop(this GameObject target, Enum type, Vector3 pos, Quaternion rot)
        {
            string typeName = type.GetType().ToString().Replace("Crogen.CrogenPooling.", "").Replace("PoolType", "");
            string enumName = type.ToString();

            return Pop(target, $"{typeName}.{enumName}", pos, rot);
        }

        public static IPoolingObject Pop(this GameObject target, string typeName, Transform parent = null)
        {
            try
            {
                IPoolingObject poolingObject;

                string[] poolName = typeName.Split('.');

                if (PoolManager.poolDict[typeName].Count == 0)
                {
					foreach (var poolBase in _poolBaseList)
					{
                        if (poolName[0] != poolBase.name) continue;
                        for (int i = 0; i < poolBase.pairs.Count; i++)
                        {
                            if (poolBase.pairs[i].poolType.Equals(poolName[1]))
                            {
                                poolingObject = PoolManager.CreateObject(poolBase, poolBase.pairs[i], Vector3.zero, Quaternion.identity);
                                PoolManager.PoolingObjectInit(poolingObject, typeName, PoolManager.Transform);
                                break;
                            }
                        }
                    }
                }
                poolingObject = PoolManager.poolDict[typeName].Pop();
                GameObject obj = poolingObject.gameObject;

                obj.SetActive(true);

                obj.transform.SetParent(parent);
                obj.transform.localPosition = Vector3.zero;
                obj.transform.localRotation = Quaternion.identity;
                poolingObject.OnPop();

                return poolingObject;
            }
            catch (KeyNotFoundException)
            {
                Debug.LogError("You should make 'PoolManager'!");
                throw;
            }
        }
        public static IPoolingObject Pop(this GameObject target, string typeName, Vector3 pos, Quaternion rot)
        {
            try
            {
                IPoolingObject poolingObject;

                string[] poolName = typeName.Split('.');

                if (PoolManager.poolDict[typeName].Count == 0)
                {
					foreach (var poolBase in _poolBaseList)
					{
                        if (poolName[0] != poolBase.name) continue;
                        for (int i = 0; i < poolBase.pairs.Count; i++)
                        {
                            if (poolBase.pairs[i].poolType.Equals(poolName[1]))
                            {
                                poolingObject = PoolManager.CreateObject(poolBase, poolBase.pairs[i], Vector3.zero, Quaternion.identity);
                                PoolManager.PoolingObjectInit(poolingObject, typeName, PoolManager.Transform);
                                break;
                            }
                        }
                    }
                }
                poolingObject = PoolManager.poolDict[typeName].Pop();
                GameObject obj = poolingObject.gameObject;

                obj.SetActive(true);

                obj.transform.SetParent(null);
                obj.transform.position = pos;
                obj.transform.rotation = rot;
                poolingObject.OnPop();

                return poolingObject;
            }
            catch (KeyNotFoundException)
            {
                Debug.LogError("You should make 'PoolManager'!");
                throw;
            }
        }

        public static IPoolingObject Pop(Enum type, Transform parent = null)
        {
            string typeName = type.GetType().ToString().Replace("Crogen.CrogenPooling.", "").Replace("PoolType", "");
            string enumName = type.ToString();

            return Pop($"{typeName}.{enumName}", parent);
        }
        public static IPoolingObject Pop(Enum type, Vector3 pos, Quaternion rot)
        {
            string typeName = type.GetType().ToString().Replace("Crogen.CrogenPooling.", "").Replace("PoolType", "");
            string enumName = type.ToString();

            return Pop($"{typeName}.{enumName}", pos, rot);
        }

        public static IPoolingObject Pop(string typeName, Transform parent = null)
        {
            try
            {
                IPoolingObject poolingObject;

                string[] poolName = typeName.Split('.');

                if (PoolManager.poolDict[typeName].Count == 0)
                {
                    foreach (var poolBase in _poolBaseList)
                    {
                        if (poolName[0] != poolBase.name) continue;
                        for (int i = 0; i < poolBase.pairs.Count; i++)
                        {
                            if (poolBase.pairs[i].poolType.Equals(poolName[1]))
                            {
                                poolingObject = PoolManager.CreateObject(poolBase, poolBase.pairs[i], Vector3.zero, Quaternion.identity);
                                PoolManager.PoolingObjectInit(poolingObject, typeName, PoolManager.Transform);
                                break;
                            }
                        }
                    }
                }
                poolingObject = PoolManager.poolDict[typeName].Pop();
                GameObject obj = poolingObject.gameObject;

                obj.SetActive(true);

                obj.transform.SetParent(parent);
                obj.transform.localPosition = Vector3.zero;
                obj.transform.localRotation = Quaternion.identity;
                poolingObject.OnPop();

                return poolingObject;
            }
            catch (KeyNotFoundException)
            {
                Debug.LogError("You should make 'PoolManager'!");
                throw;
            }
        }
        public static IPoolingObject Pop(string typeName, Vector3 pos, Quaternion rot)
        {
            try
            {
                IPoolingObject poolingObject;

                string[] poolName = typeName.Split('.');

                if (PoolManager.poolDict[typeName].Count == 0)
                {
                    foreach (var poolBase in _poolBaseList)
                    {
                        if (poolName[0] != poolBase.name) continue;
                        for (int i = 0; i < poolBase.pairs.Count; i++)
                        {
                            if (poolBase.pairs[i].poolType.Equals(poolName[1]))
                            {
                                poolingObject = PoolManager.CreateObject(poolBase, poolBase.pairs[i], Vector3.zero, Quaternion.identity);
                                PoolManager.PoolingObjectInit(poolingObject, typeName, PoolManager.Transform);
                                break;
                            }
                        }
                    }
                }
                poolingObject = PoolManager.poolDict[typeName].Pop();
                GameObject obj = poolingObject.gameObject;

                obj.SetActive(true);

                obj.transform.SetParent(null);
                obj.transform.position = pos;
                obj.transform.rotation = rot;
                poolingObject.OnPop();

                return poolingObject;
            }
            catch (KeyNotFoundException)
            {
                Debug.LogError("You should make 'PoolManager'!");
                throw;
            }
        }
    }
}