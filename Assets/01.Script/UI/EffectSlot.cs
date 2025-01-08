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

        public void HandleCoolTime(float currentCoolTime, float duration)
        {
            _coolTimeGauge.fillAmount = currentCoolTime / duration;
            _coolTimeText.text = currentCoolTime.ToString("0.0");
        }
        
        public bool Equals(EffectSystem.Effect target)
        {
            return effectBase.name == target.name;
        }
    }
}
