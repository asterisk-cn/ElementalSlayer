using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class Element
    {
        public enum Type
        {
            Fire,
            Water,
            Earth
        }

        static bool[,] _elementalMatrix = new bool[3, 3]
        {
            { false, false, true },
            { true, false, false },
            { false, true, false }
        };
        public static bool IsStrongAgainst(Type attacker, Type defender)
        {
            return _elementalMatrix[(int)attacker, (int)defender];
        }
    }
}
