using System;
using Platformer2DPackage;
using Platformer2DPackage.Contracts;
using Source.Platformer2DPackage.Samples.BasicModules.Modules.Contracts;
using Source.Platformer2DPackage.Samples.BasicModules.Services.Input.Contracts;
using UnityEngine;

namespace Source.Platformer2DPackage.Samples.BasicModules.Modules
{
    public class JumpModule : ICharacterMovementModule
    {
        private readonly ICollisionCheck _collisionCheck;
        private readonly IInputService _inputService;
        private readonly float _jumpForce;
        private readonly float _coyoteTime;
        private readonly float _jumpBufferTime;
        private readonly float _variableJumpCutoff;

        private bool _isJumping;
        private bool _jumpRequested;
        private float _coyoteTimeCounter;
        private float _jumpBufferCounter;
        private bool _jumpButtonReleased;

        public JumpModule(
            ICollisionCheck collisionCheck,
            IInputService inputService,
            float jumpForce,
            float coyoteTime = 0.1f,
            float jumpBufferTime = 0.1f,
            float variableJumpCutoff = 0.5f)
        {
            _collisionCheck = collisionCheck ?? throw new ArgumentNullException(nameof(collisionCheck));
            _jumpForce = jumpForce;
            _inputService = inputService ?? throw new ArgumentNullException(nameof(inputService));
            _coyoteTime = Mathf.Max(0f, coyoteTime);
            _jumpBufferTime = Mathf.Max(0f, jumpBufferTime);
            _variableJumpCutoff = Mathf.Clamp01(variableJumpCutoff);

            AddInputServiceListeners();
        }

        public void Dispose() =>
            RemoveInputServiceListeners();

        public void OnUpdate(CharacterMovement2D movement2D, float deltaTime)
        {
            if (_collisionCheck.IsHitCeiling
                && movement2D.Velocity.y > 0f)
            {
                movement2D.Velocity.y = 0f;
                _isJumping = false;

                return;
            }

            if (_collisionCheck.IsGrounded)
            {
                _coyoteTimeCounter = _coyoteTime;
                _isJumping = false;
            }
            else
            {
                _coyoteTimeCounter -= deltaTime;
            }

            if (_jumpRequested)
            {
                _jumpBufferCounter -= deltaTime;

                if (_jumpBufferCounter <= 0f)
                    _jumpRequested = false;
            }

            bool canJump = (_collisionCheck.IsGrounded || _coyoteTimeCounter > 0f)
                           && _isJumping == false
                           && _collisionCheck.IsHitCeiling == false;

            if (_jumpRequested && canJump)
                ExecuteJump(movement2D);

            if (_isJumping && _jumpButtonReleased && movement2D.Velocity.y > 0f)
            {
                movement2D.Velocity.y *= _variableJumpCutoff;
                _isJumping = false;
            }

            _jumpButtonReleased = false;
        }

        public void OnPostUpdate(CharacterMovement2D movement2D, float deltaTime)
        {
            if (_collisionCheck.IsHitCeiling && movement2D.Velocity.y > 0f)
                _isJumping = false;
        }

        private void ExecuteJump(CharacterMovement2D movement2D)
        {
            movement2D.Velocity.y = _jumpForce;

            _isJumping = true;
            _jumpRequested = false;
            _coyoteTimeCounter = 0f;
        }

        private void AddInputServiceListeners()
        {
            _inputService.JumpRequested += RequestJump;
            _inputService.JumpReleased += ReleaseJump;
        }

        private void RemoveInputServiceListeners()
        {
            _inputService.JumpRequested -= RequestJump;
            _inputService.JumpReleased -= ReleaseJump;
        }

        private void RequestJump()
        {
            _jumpRequested = true;
            _jumpBufferCounter = _jumpBufferTime;
        }

        private void ReleaseJump() =>
            _jumpButtonReleased = true;
    }
}
