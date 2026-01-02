using System;
using UnityEngine;

namespace Source.Platformer2DPackage.Samples.BasicModules.Data
{
    public readonly struct CollisionCheckData
    {
        public readonly Vector2 Offset;
        public readonly Vector2 CheckSize;
        public readonly ContactFilter2D ContactFilter;
        public readonly float CastDistance;

        public CollisionCheckData(
            Vector2 offset,
            Vector2 checkSize,
            ContactFilter2D contactFilter,
            float castDistance = 0.1f)
        {
            if (castDistance <= 0f)
                throw new ArgumentOutOfRangeException(nameof(castDistance));

            Offset = offset;
            CheckSize = checkSize;
            ContactFilter = contactFilter;
            CastDistance = castDistance;
        }
    }
}
