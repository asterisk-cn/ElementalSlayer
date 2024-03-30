using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MyGame
{
    public class PlayerInputs : MonoBehaviour
    {
        public Vector2 move { get; private set; }
        public Vector2 look { get; private set; }
        public bool attack { get; private set; }
        public bool jump { get; private set; }
        public bool changeWeapon { get; private set; }

        public bool HasMoveInput => move != Vector2.zero;

        public void OnMove(InputValue value)
        {
            move = value.Get<Vector2>();
        }

        public void OnLook(InputValue value)
        {
            look = value.Get<Vector2>();
        }

        public void OnJump(InputValue value)
        {
            jump = value.isPressed;
        }

        public void OnAttack(InputValue value)
        {
            attack = value.isPressed;
        }

        public void OnChangeWeapon(InputValue value)
        {
            changeWeapon = value.isPressed;
        }
    }
}
