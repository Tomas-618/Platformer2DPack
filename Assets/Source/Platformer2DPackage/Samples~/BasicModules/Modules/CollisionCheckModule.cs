using Platformer2DPackage;
using Platformer2DPackage.Contracts;
using Source.Platformer2DPackage.Samples.BasicModules.Data;
using Source.Platformer2DPackage.Samples.BasicModules.Modules.Contracts;
using UnityEngine;

namespace Source.Platformer2DPackage.Samples.BasicModules.Modules
{
    public class CollisionCheckModule : ICharacterMovementModule, ICollisionCheck
    {
        private readonly RaycastHit2D[] _groundHits = new RaycastHit2D[1];
        private readonly RaycastHit2D[] _ceilingHits = new RaycastHit2D[1];
        private readonly CollisionCheckData _groundCheckData;
        private readonly CollisionCheckData _ceilingCheckData;

        public CollisionCheckModule(
            in CollisionCheckData groundCheckData,
            in CollisionCheckData ceilingCheckData)
        {
            _groundCheckData = groundCheckData;
            _ceilingCheckData = ceilingCheckData;
        }

        public RaycastHit2D LastGroundHit { get; private set; }

        public bool IsGrounded { get; private set; }

        public bool IsHitCeiling { get; private set; }

        public void Dispose()
        {
        }

        public void OnUpdate(CharacterMovement2D movement2D, float deltaTime)
        {
            Vector2 position = movement2D.Position;

            int groundHits = Physics2D.BoxCast(
                position + _groundCheckData.Offset,
                _groundCheckData.CheckSize,
                0f,
                Vector2.down,
                _groundCheckData.ContactFilter,
                _groundHits,
                _groundCheckData.CastDistance);

            if (groundHits > 0)
            {
                IsGrounded = true;
                LastGroundHit = _groundHits[0];
            }
            else
            {
                IsGrounded = false;
                LastGroundHit = default;
            }

            int ceilingHits = Physics2D.BoxCast(
                position + _ceilingCheckData.Offset,
                _ceilingCheckData.CheckSize,
                0f,
                Vector2.up,
                _ceilingCheckData.ContactFilter,
                _ceilingHits,
                _ceilingCheckData.CastDistance);

            IsHitCeiling = movement2D.Velocity.y > 0f
                           && ceilingHits > 0;
        }

        public void OnPostUpdate(CharacterMovement2D movement2D, float deltaTime)
        {
        }
    }
}
