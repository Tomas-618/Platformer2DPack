using System;

namespace Platformer2DPackage.Contracts
{
    public interface ICharacterMovementModule : IDisposable
    {
        void OnUpdate(CharacterMovement2D movement2D, float deltaTime);

        void OnPostUpdate(CharacterMovement2D movement2D, float deltaTime);
    }
}
