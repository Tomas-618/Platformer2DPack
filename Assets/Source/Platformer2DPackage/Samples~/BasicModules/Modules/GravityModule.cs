using System;
using Platformer2DPackage;
using Platformer2DPackage.Contracts;
using Source.Platformer2DPackage.Samples.BasicModules.Modules.Contracts;
using UnityEngine;

namespace Source.Platformer2DPackage.Samples.BasicModules.Modules
{
    public class GravityModule : ICharacterMovementModule
    {
        private readonly ICollisionCheck _collisionCheck;
        private readonly float _gravityScale;
        private readonly float _maxFallSpeed;

        public GravityModule(ICollisionCheck collisionCheck, float gravityScale = 40f, float maxFallSpeed = 40f)
        {
            if (gravityScale < 0f)
                throw new ArgumentOutOfRangeException(nameof(gravityScale));

            if (maxFallSpeed < 0f)
                throw new ArgumentOutOfRangeException(nameof(maxFallSpeed));

            _collisionCheck = collisionCheck ?? throw new ArgumentNullException(nameof(collisionCheck));
            _gravityScale = gravityScale;
            _maxFallSpeed = maxFallSpeed;
        }

        public void Dispose()
        {
        }

        public void OnUpdate(CharacterMovement2D movement2D, float deltaTime)
        {
            Vector2 velocity = movement2D.Velocity;

            if (_collisionCheck.IsGrounded)
            {
                if (velocity.y < 0f)
                    velocity.y = 0f;

                movement2D.Velocity = velocity;

                return;
            }

            velocity.y -= _gravityScale * deltaTime;
            velocity.y = Mathf.Max(velocity.y, -_maxFallSpeed);

            movement2D.Velocity = velocity;
        }

        public void OnPostUpdate(CharacterMovement2D movement2D, float deltaTime)
        {
        }
    }
}
