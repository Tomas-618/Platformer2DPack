using System;
using Platformer2DPackage;
using Platformer2DPackage.Contracts;
using Source.Platformer2DPackage.Samples.BasicModules.Data;
using Source.Platformer2DPackage.Samples.BasicModules.Modules.Contracts;
using UnityEngine;

namespace Source.Platformer2DPackage.Samples.BasicModules.Modules
{
    public class GroundSnapModule : ICharacterMovementModule
    {
        private readonly ICollisionCheck _collisionCheck;
        private readonly float _snapValue;

        public GroundSnapModule(
            ICollisionCheck collisionCheck,
            CollisionCheckData groundCheckData)
        {
            _collisionCheck = collisionCheck ?? throw new ArgumentNullException(nameof(collisionCheck));
            _snapValue = Mathf.Abs(groundCheckData.Offset.y)
                         + groundCheckData.CheckSize.y / 2f +
                         groundCheckData.CastDistance;
        }

        public void Dispose()
        {
        }

        public void OnUpdate(CharacterMovement2D movement2D, float deltaTime)
        {
        }

        public void OnPostUpdate(CharacterMovement2D movement2D, float deltaTime)
        {
            if (_collisionCheck.IsGrounded == false
                || movement2D.Velocity.y > 0f)
                return;

            RaycastHit2D hit = _collisionCheck.LastGroundHit;

            Vector2 playerPosition = movement2D.Position;

            playerPosition.y = hit.point.y + _snapValue;
            movement2D.Position = playerPosition;
        }
    }
}
