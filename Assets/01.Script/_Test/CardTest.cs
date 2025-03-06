using Hashira.Cards;
using Hashira.Core;
using Hashira.Entities;
using UnityEngine;

namespace Hashira
{
    public class CardTest : MonoBehaviour
    {
        [SerializeField] private CardSO cardSO;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                if (Cost.TryRemoveCost(cardSO.needCost))
                {
                    PlayerManager.Instance.Player.GetEntityComponent<EntityEffector>().AddEffect(cardSO.GetEffectClass());
                    Debug.Log(Cost.CurrentCost);
                }
            }
            if (Input.GetKeyDown(KeyCode.T))
            {
                Cost.AddCost(100);
                Debug.Log(Cost.CurrentCost);
            }
        }
    }
}
