using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.Pathfind
{
    public enum DirectionType
    {
        Zero,
        Up,
        UpperRight,
        Right,
        LowerRight,
        Down,
        LowerLeft,
        Left,
        UpperLeft,
    }

    public static class Direction2D
    {
        private static Dictionary<DirectionType, Vector2Int> _directionDict;

        static Direction2D()
        {
            _directionDict = new Dictionary<DirectionType, Vector2Int>()
            {
                { DirectionType.Zero, Vector2Int.zero },
                { DirectionType.Up, Vector2Int.up },
                { DirectionType.UpperRight, new Vector2Int(1,1)},
                { DirectionType.Right, Vector2Int.right },
                { DirectionType.LowerRight, new Vector2Int(1,-1)},
                { DirectionType.Down, Vector2Int.down },
                { DirectionType.LowerLeft, new Vector2Int(-1,-1) },
                { DirectionType.Left, Vector2Int.left },
                { DirectionType.UpperLeft, new Vector2Int(-1,1)},
            };
        }

        public static Vector2[] fourDirections = new Vector2[4]
            {
                Vector2.up,
                Vector2.right,
                Vector2.down,
                Vector2.left,
            };
        public static Vector2[] fiveDirection = new Vector2[5]
            {
                Vector2.zero,
                Vector2.up,
                Vector2.right,
                Vector2.down,
                Vector2.left,
            };

        public static Vector2Int[] fourIntDirections = new Vector2Int[4]
            {
                Vector2Int.up,
                Vector2Int.right,
                Vector2Int.down,
                Vector2Int.left,
            };
        public static Vector2Int[] fiveIntDirection = new Vector2Int[5]
            {
                Vector2Int.zero,
                Vector2Int.up,
                Vector2Int.right,
                Vector2Int.down,
                Vector2Int.left,
            };

        public static Vector2[] eightDirections = new Vector2[8]
            {
                Vector2.up,
                new Vector2(1,1),
                Vector2.right,
                new Vector2(1,-1),
                Vector2.down,
                new Vector2(-1,-1),
                Vector2.left,
                new Vector2(-1,1),
            };
        public static Vector2[] nineDirections = new Vector2[9]
            {
                Vector2.zero,
                Vector2.up,
                new Vector2(1,1),
                Vector2.right,
                new Vector2(1,-1),
                Vector2.down,
                new Vector2(-1,-1),
                Vector2.left,
                new Vector2(-1,1),
            };

        public static Vector2Int[] eightIntDirections = new Vector2Int[8]
            {
                Vector2Int.up,
                new Vector2Int(1,1),
                Vector2Int.right,
                new Vector2Int(1,-1),
                Vector2Int.down,
                new Vector2Int(-1,-1),
                Vector2Int.left,
                new Vector2Int(-1,1),
            };
        public static Vector2Int[] nineIntDirections = new Vector2Int[9]
            {
                Vector2Int.zero,
                Vector2Int.up,
                new Vector2Int(1,1),
                Vector2Int.right,
                new Vector2Int(1,-1),
                Vector2Int.down,
                new Vector2Int(-1,-1),
                Vector2Int.left,
                new Vector2Int(-1,1),
            };

        public static Vector2Int GetDirection(DirectionType directionType)
            => _directionDict[directionType];

        public static Vector2Int[] GetDirections(params DirectionType[] directionTypes)
        {
            Vector2Int[] directions = new Vector2Int[directionTypes.Length];
            for(int i = 0; i < directionTypes.Length; i++)
            {
                directions[i] = _directionDict[directionTypes[i]];
            }
            return directions;
        }
    }
}
