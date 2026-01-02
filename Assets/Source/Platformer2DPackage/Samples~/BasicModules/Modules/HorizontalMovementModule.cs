using System;
using Platformer2DPackage;
using Platformer2DPackage.Contracts;
using Source.Platformer2DPackage.Samples.BasicModules.Modules.Contracts;
using Source.Platformer2DPackage.Samples.BasicModules.Services.Input.Contracts;
using UnityEngine;

namespace Source.Platformer2DPackage.Samples.BasicModules.Modules
{
    public class HorizontalMovementModule : ICharacterMovementModule
    {
        private readonly ICollisionCheck _collisionCheck;
        private readonly IInputService _inputService;
        private readonly float _maxSpeed;
        private readonly float _acceleration;
        private readonly float _deceleration;
        private readonly float _airAccelerationMultiplier;
        private readonly float _airDecelerationMultiplier;

        public HorizontalMovementModule(
            ICollisionCheck collisionCheck,
            IInputService inputService,
            float maxSpeed,
            float acceleration,
            float deceleration,
            float airAccelerationMultiplier = 0.7f,
            float airDecelerationMultiplier = 0.2f)
        {
            _collisionCheck = collisionCheck ?? throw new ArgumentNullException(nameof(collisionCheck));
            _inputService = inputService ?? throw new ArgumentNullException(nameof(inputService));
            _maxSpeed = Mathf.Max(0f, maxSpeed);
            _acceleration = Mathf.Max(0f, acceleration);
            _deceleration = Mathf.Max(0f, deceleration);
            _airAccelerationMultiplier = Mathf.Clamp01(airAccelerationMultiplier);
            _airDecelerationMultiplier = Mathf.Clamp01(airDecelerationMultiplier);
        }

        public void Dispose()
        {
        }

        public void OnUpdate(CharacterMovement2D movement2D, float deltaTime)
        {
            float currentX = movement2D.Velocity.x;

            float horizontalInput = _inputService.GetHorizontalInput();
            bool hasInput = Mathf.Abs(horizontalInput) > 0.01f;

            bool isGrounded = _collisionCheck.IsGrounded;
            float currentAcceleration = isGrounded ? _acceleration : _acceleration * _airAccelerationMultiplier;

            bool wasSpeedGreaterThanMax = Mathf.Abs(currentX) <= _maxSpeed;

            if (hasInput &&
                (wasSpeedGreaterThanMax
                 || (int)Mathf.Sign(currentX) != (int)Mathf.Sign(horizontalInput)))
            {
                currentX += horizontalInput * currentAcceleration * deltaTime;

                if (wasSpeedGreaterThanMax)
                    currentX = Mathf.Clamp(currentX, -_maxSpeed, _maxSpeed);
            }
            else
            {
                float currentDeceleration = isGrounded ? _deceleration : _deceleration * _airDecelerationMultiplier;
                float decelerationAmount = currentDeceleration * deltaTime;

                if (Mathf.Abs(currentX) <= decelerationAmount)
                    currentX = 0f;
                else
                    currentX -= Mathf.Sign(currentX) * decelerationAmount;
            }

            movement2D.Velocity.x = currentX;
        }

        public void OnPostUpdate(CharacterMovement2D movement2D, float deltaTime)
        {
        }
    }
}
