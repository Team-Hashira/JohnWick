using UnityEngine;

namespace Hashira.Core.StatSystem
{
    //Stat�� ������ �ʴ� ���� ����������
    [CreateAssetMenu(fileName = "StatElement", menuName = "SO/Stat/StatElement")]
    public class StatElementSO : ScriptableObject
    {
        public string statName;
        public string displayName;
        public Vector2 minMaxValue;
        public Sprite statIcon;
    }
}