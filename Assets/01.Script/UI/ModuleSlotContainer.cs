using UnityEngine;

namespace Hashira.UI
{
    public class ModuleSlotContainer : MonoBehaviour
    {
        [SerializeField] private ModuleSlot _moduleSlotPrefab;
        [SerializeField] private float _range = 500f;
        private void OnEnable()
        {
            LoadModules();
        }

        private void LoadModules()
        {
            int count = 10;

            for (int i = 0; i < count; i++)
            {
                var moduleSlot = Instantiate(_moduleSlotPrefab, transform);
                float value = i / (float)count * 2 * Mathf.PI;
                (moduleSlot.transform as RectTransform).anchoredPosition = new Vector2(Mathf.Sin(value), Mathf.Cos(value)) * _range;
            }
        }
    }
}
