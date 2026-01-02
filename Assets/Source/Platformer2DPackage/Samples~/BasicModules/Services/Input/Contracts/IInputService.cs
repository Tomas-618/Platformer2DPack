using System;

namespace Source.Platformer2DPackage.Samples.BasicModules.Services.Input.Contracts
{
    public interface IInputService : IDisposable
    {
        event Action JumpRequested;

        event Action JumpReleased;

        float GetHorizontalInput();
    }
}
