using System;
using Source.Platformer2DPackage.Samples.BasicModules.Services.Input.Contracts;

namespace Source.Platformer2DPackage.Samples.BasicModules.Services.Input
{
    public class InputService : IInputService
    {
        // private readonly InputSystemActions _actions;
        //
        // public InputService()
        // {
        //     _actions = new InputSystemActions();
        //     _actions.Enable();
        //
        //     _actions.Player.Jump.performed += OnJumpStarted;
        //     _actions.Player.Jump.canceled += OnJumpCanceled;
        // }
        //
        // public event Action JumpRequested;
        //
        // public event Action JumpReleased;
        //
        // public void Dispose()
        // {
        //     _actions.Player.Jump.performed -= OnJumpStarted;
        //     _actions.Player.Jump.canceled -= OnJumpCanceled;
        //
        //     _actions.Disable();
        //     _actions.Dispose();
        // }
        //
        // public float GetHorizontalInput() =>
        //     _actions.Player.Move.ReadValue<Vector2>().x;
        //
        // private void OnJumpStarted(InputAction.CallbackContext context) =>
        //     JumpRequested?.Invoke();
        //
        // private void OnJumpCanceled(InputAction.CallbackContext context) =>
        //     JumpReleased?.Invoke();
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public event Action JumpRequested;

        public event Action JumpReleased;

        public float GetHorizontalInput()
        {
            throw new NotImplementedException();
        }
    }
}
