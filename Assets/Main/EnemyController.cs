using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class EnemyController : MonoBehaviour, IDamageable
    {
        [Header("StatusSettings")]
        [SerializeField]
        private int _maxHealth = 20;
        [SerializeField]
        private Element.Type _elementType;

        [SerializeField]
        private GameObject _effect;

        [HideInInspector]
        public bool useRootMotion = false;
        public Element.Type ElementType { get { return _elementType; } }
        public int health { get; private set; }
        public int maxHealth { get { return _maxHealth; } }
        public bool IsDead { get; private set; } = false;
        public GameObject Target { get; set; }

        public float groundDistance = 0.4f;
        public LayerMask groundMask;

        private CharacterController _controller;

        void Awake()
        {
            _controller = GetComponent<CharacterController>();
        }

        void Start()
        {
            health = _maxHealth;
        }

        public bool isGrounded()
        {
            Vector3 feet = _controller.transform.position + _controller.center - new Vector3(0, _controller.height / 2, 0);
            return Physics.CheckSphere(feet, groundDistance, groundMask);
        }

        public void TakeDamage(int damage, Element.Type attackType)
        {
            if (Element.IsStrongAgainst(attackType, ElementType))
            {
                damage *= 2;
            }
            else if (Element.IsStrongAgainst(ElementType, attackType))
            {
                damage = damage / 2;
            }
            health -= damage;
            CheckHealth();
        }

        void CheckHealth()
        {
            if (!IsDead && health <= 0)
            {
                Die();
            }
        }

        void Die()
        {
            if (IsDead) return;
            IsDead = true;
            Instantiate(_effect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
