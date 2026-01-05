using System;
using Source.Platformer2DPackage.Samples.BasicModules.Components.Constants;
using Source.Platformer2DPackage.Samples.BasicModules.Services.Input.Contracts;
using UnityEngine;

namespace Source.Platformer2DPackage.Samples.BasicModules.Components
{
    public class CharacterFlipper : MonoBehaviour
    {
        private IInputService _inputService;
        private bool _isFacingRight = true;

        public void Initialize(IInputService inputService) =>
            _inputService = inputService ?? throw new ArgumentNullException(nameof(inputService));

        private void LateUpdate()
        {
            float input = _inputService.GetHorizontalInput();

            if (Mathf.Abs(input) < CharacterConstants.VelocityThreshold)
                return;

            bool shouldFaceRight = input > 0f;

            if (_isFacingRight == shouldFaceRight)
                return;

            _isFacingRight = shouldFaceRight;
            transform.rotation = Quaternion.Euler(0f, _isFacingRight ? 0f : 180f, 0f);
        }
    }
}
