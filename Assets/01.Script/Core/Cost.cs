using UnityEditor;
using UnityEngine;

namespace Hashira
{
    public static class Cost
    {
        //혹시몰라서 넣은 값
        private const int MaxCost = 1000;

        public static int CurrentCost { get; private set; } = 0;

        public static void AddCost(int value)
        {
            CurrentCost += value;
            if (CurrentCost > MaxCost)
                CurrentCost = MaxCost;
        }
        public static void RemoveCost(int value)
        {
            CurrentCost -= value;
            if (CurrentCost < 0)
                CurrentCost = 0;
        }
        //웬만하면 이거 쓰기
        public static bool TryRemoveCost(int value)
        {
            if (CurrentCost >= value)
            {
                CurrentCost -= value;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
