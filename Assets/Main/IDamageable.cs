using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public interface IDamageable
    {
        int health { get; }
        int maxHealth { get; }
        bool IsDead { get; }
        Element.Type ElementType { get; }
        void TakeDamage(int damage, Element.Type attackType);
    }
}
