using System.Collections.Generic;
using UnityEngine;
using Crogen.CrogenPooling;
using System;

public class PoolManager : MonoBehaviour
{
    internal static Dictionary<string, Stack<IPoolingObject>> poolDict = new Dictionary<string, Stack<IPoolingObject>>();
    public List<PoolCategorySO> poolBaseList;
    public List<PoolPair> poolingPairs;
    public static Transform Transform;

	private void Awake()
	{
        poolDict?.Clear();
        Transform = transform;
        PopCore.Init(this, poolBaseList);
        PushCore.Init(this);

		for (int i = 0; i < poolBaseList.Count; ++i)
		{
            if (poolBaseList[i] == null) continue;
            MakeObj(poolBaseList[i]);
		}
    }

	private void MakeObj(PoolCategorySO poolCategory)
    {
        PoolPair[] poolPairs = poolCategory.pairs.ToArray();

        int currentPairIndex = 0;

		foreach (PoolPair poolPair in poolPairs)
		{
            try
            {
                poolDict.Add($"{poolCategory.name}.{poolPair.poolType}", new Stack<IPoolingObject>());
            }
            catch (System.Exception)
            {
                Debug.LogError("Press to \"Generate Enum\"");
                return;
            }
            for (int i = 0; i < poolPair.poolCount; ++i)
            {
                IPoolingObject poolingObject = CreateObject(poolCategory, poolPair, Vector3.zero, Quaternion.identity);
                PoolingObjectInit(poolingObject, $"{poolCategory.name}.{poolPair.poolType}", transform);
                ++currentPairIndex;
            }
        }
    }

    public static IPoolingObject CreateObject(PoolCategorySO poolCategory, PoolPair poolPair, Vector3 vec, Quaternion rot)
    {
        GameObject poolObject = Instantiate(poolPair.prefab);
        IPoolingObject poolingObject = poolObject.GetComponent<IPoolingObject>();

        poolingObject.OriginPoolType = $"{poolCategory.name}.{poolPair.poolType}";
        poolingObject.gameObject = poolObject;

        poolObject.transform.localPosition = vec;
        poolObject.transform.localRotation = rot;
        poolObject.transform.name = poolObject.name.Replace("(Clone)","");

        return poolingObject;
    }

    public static void PoolingObjectInit(IPoolingObject poolObject, string type, Transform parent)
	{
        poolObject.gameObject.transform.SetParent(parent);
        poolObject.gameObject.SetActive(false);
        poolDict[type].Push(poolObject);
    }
}
    