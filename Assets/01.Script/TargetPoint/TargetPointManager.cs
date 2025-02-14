using System.Collections.Generic;
using UnityEngine;
using Crogen.CrogenPooling;
using Hashira.TargetPoint.UI;

namespace Hashira.TargetPoint
{
	public class TargetPointManager : MonoSingleton<TargetPointManager>
	{
		[SerializeField] private Transform _parentCanvas;
		[SerializeField] private UIPoolType _targetPointContentPoolType = UIPoolType.TargetPointContent;
		private Dictionary<Transform, TargetPointContent> _targetPointContentDictionary = new Dictionary<Transform, TargetPointContent>();

		public void ShowTargetPoint(Transform trm, Color color)
		{
			if (_targetPointContentDictionary.ContainsKey(trm)) return;

			TargetPointContent targetPoint = gameObject.Pop(_targetPointContentPoolType, _parentCanvas) as TargetPointContent;
			if (targetPoint == null) return;

			_targetPointContentDictionary.Add(trm, targetPoint);
			targetPoint.targetTrasform = trm;
			targetPoint.ImageColor = color;
		}

		public void CloseTargetPoint(Transform trm)
		{
			if (!_targetPointContentDictionary.ContainsKey(trm)) return;

			TargetPointContent targetPoint = _targetPointContentDictionary[trm];
			if (targetPoint == null) return;

			_targetPointContentDictionary.Remove(trm);
			targetPoint.Push();
		}

		private void OnDestroy()
		{
			_targetPointContentDictionary.Clear();
		}
	}
}
