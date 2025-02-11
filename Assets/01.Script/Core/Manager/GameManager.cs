using Hashira.Players;
using UnityEngine;
using UnityEngine.Rendering;

namespace Hashira
{
    public class GameManager : MonoSingleton<GameManager>
    {
        [field:SerializeField] public Player Player { get; private set; }
        [field:SerializeField] public Volume Volume { get; private set; }
    }
}
