using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    public class PlayerMovementController : StateMachine
    {
        [Header("Movement Settings")]
        [SerializeField]
        float _runSpeed = 6.0f;
        [SerializeField]
        float _jumpHeight = 3.0f;
        [SerializeField]
        float _rotationSpeed = 10.0f;

        float _gravity = GameConfig.Gravity;

        PlayerController _playerController;
        Camera _look;
        CharacterController _controller;
        PlayerInputs _inputs;

        float _currentSpeed;

        Vector3 _currentVelocity;

        void Awake()
        {
            _playerController = GetComponent<PlayerController>();
            _look = Camera.main;
            _controller = GetComponent<CharacterController>();
            _inputs = GetComponent<PlayerInputs>();
        }

        void Start()
        {
            currentState = PlayerState.Idle;
            _currentVelocity = Vector3.zero;
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

            // TODO: apply to current velocity
            if (_playerController.canMove && _inputs.HasMoveInput)
            {
                _controller.Move(transform.forward * _currentSpeed * Time.deltaTime);

                _playerController.SetAnimatorFloat("Animation Speed", _currentSpeed / 2);
            }
            else
            {
                _playerController.SetAnimatorFloat("Animation Speed", 1);
            }

            RotateTowardsMovementDir();

            _playerController.SetAnimatorFloat("Velocity", _currentSpeed);
        }

        void RotateTowardsMovementDir()
        {
            if (_inputs.HasMoveInput)
            {
                Vector3 direction = Vector3.zero;
                direction += Vector3.ProjectOnPlane(_look.transform.right, Vector3.up).normalized * _inputs.move.x;
                direction += Vector3.ProjectOnPlane(_look.transform.forward, Vector3.up).normalized * _inputs.move.y;
                direction.Normalize();

                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * _rotationSpeed);
            }
        }

        private float CalculateJumpSpeed(float jumpHeight, float gravity)
        {
            return Mathf.Sqrt(-2f * jumpHeight * gravity);
        }

        private void Idle_EnterState()
        {
            _playerController.LockJump(false);
            _playerController.SetAnimatorInt("Jumping", 0);
            _playerController.SetAnimatorBool("Moving", false);
        }

        private void Idle_Update()
        {
            if (_playerController.canJump && _inputs.jump)
            {
                currentState = PlayerState.Jump;
                return;
            }
            else if (_playerController.canMove && _inputs.HasMoveInput)
            {
                currentState = PlayerState.Move;
                return;
            }
            _currentSpeed = 0;
        }

        private void Move_EnterState()
        {
            _playerController.SetAnimatorBool("Moving", true);
        }

        private void Move_Update()
        {
            if (_playerController.canJump && _inputs.jump)
            {
                currentState = PlayerState.Jump;
                return;
            }
            else if (_playerController.canMove && _inputs.HasMoveInput)
            {
                _currentSpeed = _runSpeed;
            }
            else
            {
                currentState = PlayerState.Idle;
            }
        }

        // TODO: Jump
        private void Jump_EnterState()
        {
            _playerController.SetAnimatorInt("Jumping", 1);
            _playerController.SetAnimatorTrigger(AnimatorTrigger.JumpTrigger);
            _currentVelocity += transform.up * CalculateJumpSpeed(_jumpHeight, _gravity);
            _playerController.LockJump(true);
            // _playerController.Jump();
        }

        private void Jump_Update()
        {
            if (_currentVelocity.y < 0)
            {
                currentState = PlayerState.Fall;
                return;
            }

            _currentVelocity.y += _gravity * Time.deltaTime;
        }

        private void Fall_EnterState()
        {
            _playerController.LockJump(true);
            _playerController.SetAnimatorInt("Jumping", 2);
            _playerController.SetAnimatorTrigger(AnimatorTrigger.JumpTrigger);
        }

        private void Fall_Update()
        {
            if (_controller.isGrounded)
            {
                currentState = PlayerState.Idle;
                return;
            }

            _currentVelocity.y += _gravity * Time.deltaTime;
        }

        private void Fall_ExitState()
        {
            _playerController.SetAnimatorInt("Jumping", 0);
            _playerController.SetAnimatorTrigger(AnimatorTrigger.JumpTrigger);
        }
    }
}
