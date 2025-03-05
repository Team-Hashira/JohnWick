using UnityEngine;

namespace Hashira.LatestUI
{
    public abstract class UIBase : MonoBehaviour
    {
        public RectTransform RectTransform => transform as RectTransform;
    }
}
