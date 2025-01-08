using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Hashira.UI.Effect
{
    public class EffectSlot : MonoBehaviour
    {
        [SerializeField] private Image _coolTimeGauge;
        [SerializeField] private TextMeshProUGUI _coolTimeText;

        public EffectSystem.Effect effectBase;

        public void Init(EffectSystem.Effect effectBase)
        {
            this.effectBase = effectBase;
            //여기서 다른 UI 정보들까지 싹 다 초기화
            effectBase.CoolTimeEvent += HandleCoolTime;
        }

        private void OnDestroy()
        {
            effectBase.CoolTimeEvent -= HandleCoolTime;
        }

        public void HandleCoolTime(float currentCoolTime, float duration)
        {
            _coolTimeGauge.fillAmount = currentCoolTime / duration;
            _coolTimeText.text = (duration-currentCoolTime).ToString("0.0");
        }
        
        public bool Equals(EffectSystem.Effect target)
        {
            return effectBase.name.Equals(target.name);
        }
    }
}
