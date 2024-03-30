using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField]
        private Element.Type _weaponType = Element.Type.Water;
        [SerializeField]
        private int _damage = 10;

        private Collider _collider;
        // TODO: PlayerAudioManager
        public AudioSource AudioSource{ private get; set;}
        public AudioClip HitSound{ private get; set;}

        private List<GameObject> _collidedEnemies = new List<GameObject>();

        void Start()
        {
            _collider = GetComponent<Collider>();
            _collider.enabled = false;
        }

        public void ActivateCollider(bool state)
        {
            _collider.enabled = state;
        }

        public void ResetWeapon()
        {
            _collidedEnemies.Clear();
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Enemy")
            {
                if (_collidedEnemies.Contains(other.gameObject))
                {
                    return;
                }
                else
                {
                    _collidedEnemies.Add(other.gameObject);
                }
                AudioSource.PlayOneShot(HitSound);
                IDamageable damageable = other.gameObject.TryGetComponent<IDamageable>(out damageable) ? damageable : null;
                if (damageable != null)
                {
                    damageable.TakeDamage(_damage, _weaponType);
                }
            }
        }
    }
}
