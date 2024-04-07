using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class EnemyMovementController : StateMachine
    {
        [Header("Movement Settings")]
        [SerializeField]
        float _walkSpeed = 1.5f;
        [SerializeField]
        float _runSpeed = 2.5f;
        [SerializeField]
        float _rotationSpeed = 8.0f;
        [SerializeField]
        float _rotateTimeRate = 10.0f;

        float _gravity = GameConfig.Gravity;

        EnemyController _enemyController;
        CharacterController _controller;

        float _currentSpeed;
        Vector3 _direction;
        Vector3 _currentVelocity;
        float _rotateTime = 0.0f;

        void Awake()
        {
            _enemyController = GetComponent<EnemyController>();
            _controller = GetComponent<CharacterController>();
        }

        void Start()
        {
            currentState = EnemyState.Move;
            _currentVelocity = Vector3.zero;
            _rotateTime = _rotateTimeRate;
        }

        // Put any code in here you want to run BEFORE the state's update function.
        // This is run regardless of what state you're in.
        protected override void EarlyGlobalSuperUpdate()
        {
        }

        // Put any code in here you want to run AFTER the state's update function.
        // This is run regardless of what state you're in.
        protected override void LateGlobalSuperUpdate()
        {
            // TODO:
            if (_controller.isGrounded && _currentVelocity.y < 0)
            {
                _currentVelocity.y = -2f;
            }
            _currentVelocity.y += _gravity * Time.deltaTime;
            _controller.Move(transform.up * _currentVelocity.y * Time.deltaTime);

            _controller.Move(transform.forward * _currentSpeed * Time.deltaTime);

            RotateTowardsMovementDir();
        }

        void RotateTowardsMovementDir()
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_direction), Time.deltaTime * _rotationSpeed);
        }

        private float CalculateJumpSpeed(float jumpHeight, float gravity)
        {
            return Mathf.Sqrt(-2f * jumpHeight * gravity);
        }

        private void Idle_EnterState()
        {
        }

        private void Idle_Update()
        {
            _currentSpeed = 0;
        }

        private void Move_EnterState()
        {
        }

        private void Move_Update()
        {
            if (_enemyController.Target != null)
            {
                Vector3 awayFromTarget = transform.position - _enemyController.Target.transform.position;
                _direction = Vector3.ProjectOnPlane(awayFromTarget, Vector3.up).normalized;
                _currentSpeed = _runSpeed;
            }
            else
            {
                _rotateTime += Time.deltaTime;
                if (_rotateTime >= _rotateTimeRate)
                {
                    var random = Random.Range(-90, 90);
                    _direction = Quaternion.Euler(0, random, 0) * transform.forward;
                    _rotateTime = 0.0f;
                }
                _currentSpeed = _walkSpeed;
            }
        }
    }
}
