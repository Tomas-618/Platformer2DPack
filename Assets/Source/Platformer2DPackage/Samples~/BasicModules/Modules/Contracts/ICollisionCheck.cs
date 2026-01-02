using UnityEngine;

namespace Source.Platformer2DPackage.Samples.BasicModules.Modules.Contracts
{
    public interface ICollisionCheck
    {
        bool IsGrounded { get; }

        bool IsHitCeiling { get; }

        RaycastHit2D LastGroundHit { get; }
    }
}
