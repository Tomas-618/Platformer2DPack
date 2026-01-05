using Platformer2DPackage;
using Source.Platformer2DPackage.Samples.BasicModules.Components;
using Source.Platformer2DPackage.Samples.BasicModules.Data;
using Source.Platformer2DPackage.Samples.BasicModules.Modules;
using Source.Platformer2DPackage.Samples.BasicModules.Services.Input;
using Source.Platformer2DPackage.Samples.BasicModules.Services.Input.Contracts;
using UnityEngine;

namespace Source.Platformer2DPackage.Samples.BasicModules.Infrastructure
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] private CharacterMovement2D _playerPrefab;

        private void Awake()
        {
            IInputService inputService = new InputService();

            CharacterMovement2D player = Instantiate(_playerPrefab, new Vector3(3f, 5f), Quaternion.identity);

            player.Initialize();
            player.GetComponent<CharacterFlipper>().Initialize(inputService);

            var groundCheckData = new CollisionCheckData(
                new Vector2(0.1f, -0.93f),
                new Vector2(0.5f, 0.01f),
                new ContactFilter2D
                {
                    useLayerMask = true,
                    layerMask = LayerMask.GetMask("Ground"),
                    useTriggers = false
                },
                0.01f);

            var ceilingCheckData = new CollisionCheckData(
                new Vector2(0.1f, 0.93f),
                new Vector2(0.5f, 0.01f),
                new ContactFilter2D
                {
                    useLayerMask = true,
                    layerMask = LayerMask.GetMask("Ground"),
                    useTriggers = false
                },
                0.01f);

            var collisionCheckModule = new CollisionCheckModule(groundCheckData, ceilingCheckData);
            var gravityModule = new GravityModule(collisionCheckModule);
            var horizontalMovementModule = new HorizontalMovementModule(collisionCheckModule,
                inputService, 8f, 80f, 100f);
            var jumpModule = new JumpModule(collisionCheckModule, inputService, 13.5f);
            var groundSnapModule = new GroundSnapModule(
                collisionCheckModule, groundCheckData);

            player.AddRangeModules(
                collisionCheckModule,
                horizontalMovementModule,
                gravityModule,
                jumpModule,
                groundSnapModule);
        }
    }
}
