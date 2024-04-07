using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField]
        float _groundDistance = 0.4f;
        [SerializeField]
        LayerMask _groundMask;
        [Header("Weapon Settings")]
        [SerializeField]
        List<Weapon> _weapons;
        [SerializeField]
        List<GameObject> _weaponUIs;
        [SerializeField]
        float _attackTime = 0.7f;

        [Header("Sound Settings")]
        [SerializeField]
        AudioClip _attackSound;
        [SerializeField]
        AudioClip _hitSound;

        [HideInInspector]
        public bool useRootMotion = false;
        public bool isMoving { get; set; } = false;
        public bool allowedInput { get;  set;} = true;
        public bool canMove { get; set; } = true;
        public bool canAction { get; set; } = true;
        public bool canJump { get; set; } = true;
        public bool canChangeWeapon { get { return canAction; } }

        [HideInInspector]
        public PlayerInputs _inputs;
        CharacterController _controller;
        Animator _animator;
        AudioSource _audioSource;


        int _currentWeaponIndex;
        Weapon _currentWeapon{ get { return _weapons[_currentWeaponIndex]; } }
        GameObject _currentWeaponUI { get { return _weaponUIs[_currentWeaponIndex]; } }
        private float _animationSpeed;

        void Awake()
        {
            _controller = GetComponentInChildren<CharacterController>();
            _inputs = GetComponent<PlayerInputs>();
            _audioSource = GetComponentInChildren<AudioSource>();
            Debug.Assert(_audioSource != null, "AudioSource not found in children of PlayerController");
            _animator = GetComponentInChildren<Animator>();
            Debug.Assert(_animator != null, "Animator not found in children of PlayerController");

            _animator.gameObject.AddComponent<PlayerCharacterAnimatorEvents>();
            _animator.GetComponent<PlayerCharacterAnimatorEvents>().playerController = this;
            _animator.gameObject.AddComponent<AnimatorParentMove>();
            _animator.GetComponent<AnimatorParentMove>().animator = _animator;
            _animator.GetComponent<AnimatorParentMove>().playerController = this;
            _animator.updateMode = AnimatorUpdateMode.AnimatePhysics;
            _animator.cullingMode = AnimatorCullingMode.CullUpdateTransforms;
        }

        void Start()
        {
            foreach (Weapon weapon in _weapons)
            {
                weapon.gameObject.SetActive(false);
                weapon.AudioSource = _audioSource;
                weapon.HitSound = _hitSound;
            }
            foreach (GameObject weaponUI in _weaponUIs)
            {
                weaponUI.SetActive(false);
            }

            _currentWeaponUI.gameObject.SetActive(true);
            _currentWeapon.gameObject.SetActive(true);
            SetAnimatorFloat("Animation Speed", _animationSpeed);
        }

        void Update()
        {
            if (IsGrounded() && canAction)
            {
                Attacking();
            }

            if (IsGrounded() && canChangeWeapon)
            {
                SwitchingWeapon();
            }
        }

        private void Attacking()
        {
            if (_inputs.attack)
            {
                Attack();
            }
        }

        public void Attack()
        {
            SetAnimatorTrigger(AnimatorTrigger.AttackTrigger);
            Lock(true, true, true, 0, _attackTime);
        }

        private void SwitchingWeapon()
        {
            if (_inputs.changeWeapon)
            {
                SwitchWeapon();
            }
        }

        public void SwitchWeapon()
        {
            _currentWeapon.gameObject.SetActive(false);
            _currentWeaponUI.gameObject.SetActive(false);
            if (_currentWeaponIndex < _weapons.Count - 1)
            {
                _currentWeaponIndex++;
            }
            else
            {
                _currentWeaponIndex = 0;
            }
            _currentWeapon.gameObject.SetActive(true);
            _currentWeaponUI.gameObject.SetActive(true);
            Lock(false, true, true, 0, 0.2f);
        }

        public bool IsGrounded()
        {
            Vector3 feet = _controller.transform.position + _controller.center - new Vector3(0, _controller.height / 2, 0);
            return Physics.CheckSphere(feet, _groundDistance, _groundMask);
        }

        // TODO: Jump/Land

        public void Lock(bool lockMovement, bool lockAction, bool timed, float delayTime, float lockTime)
        {
            StopCoroutine("_Lock");
            StartCoroutine(_Lock(lockMovement, lockAction, timed, delayTime, lockTime));
        }

        //Timed -1 = infinite, 0 = no, 1 = yes.
        public IEnumerator _Lock(bool lockMovement, bool lockAction, bool timed, float delayTime, float lockTime)
        {
            if (delayTime > 0) { yield return new WaitForSeconds(delayTime); }
            if (lockMovement) { LockMove(true); }
            if (lockAction) { LockAction(true); }
            if (timed)
            {
                if (lockTime > 0)
                {
                    yield return new WaitForSeconds(lockTime);
                    UnLock(lockMovement, lockAction);
                }
            }
        }

        public void LockMove(bool b)
        {
            if (b)
            {
                SetAnimatorBool("Moving", false);
                SetAnimatorRootMotion(true);
                canMove = false;
            }
            else
            {
                canMove = true;
                SetAnimatorRootMotion(false);
            }
        }

        public void LockAction(bool b)
        {
            canAction = !b;
        }

        public void LockJump(bool b)
        {
            canJump = !b;
        }

        public void UnLock(bool movement, bool actions)
        {
            if (movement) { LockMove(false); }
            if (actions) {
                _currentWeapon.ResetWeapon();
                canAction = true;
            }
        }

        public void SetAnimatorBool(string name, bool b)
        {
            _animator.SetBool(name, b);
        }

        public void SetAnimatorRootMotion(bool b)
        {
            useRootMotion = b;
        }

        public void SetAnimatorTrigger(AnimatorTrigger trigger)
        {
            _animator.SetInteger("Trigger Number", (int)trigger);
            _animator.SetTrigger("Trigger");
        }

        public void SetAnimatorFloat(string name, float f)
        {
            _animator.SetFloat(name, f);
        }

        public void SetAnimatorInt(string name, int i)
        {
            _animator.SetInteger(name, i);
        }

        public void ActivateWeaponCollider()
        {
            _currentWeapon.ActivateCollider(true);
        }

        public void DeactivateWeaponCollider()
        {
            _currentWeapon.ActivateCollider(false);
        }

        public void PlayAttackSound()
        {
            _audioSource.PlayOneShot(_attackSound);
        }
    }
}
