using System;
using System.Collections.Generic;
using Platformer2DPackage.Contracts;
using UnityEngine;

namespace Platformer2DPackage
{
    [RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
    public class CharacterMovement2D : MonoBehaviour
    {
        public Vector2 Velocity;

        private readonly HashSet<ICharacterMovementModule> _modules = new();

        private Rigidbody2D _rigidbody2D;

        public Vector2 Position
        {
            get => transform.position;
            set => transform.position = value;
        }

        public void Initialize()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();

            _rigidbody2D.gravityScale = 0f;
            _rigidbody2D.freezeRotation = true;
            _rigidbody2D.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            _rigidbody2D.interpolation = RigidbodyInterpolation2D.Interpolate;
            _rigidbody2D.sleepMode = RigidbodySleepMode2D.NeverSleep;

            Collider2D colliderComponent = GetComponent<Collider2D>();

            var playerPhysicsMaterial = new PhysicsMaterial2D("PlayerMaterial")
            {
                friction = 0f,
                bounciness = 0f
            };

            colliderComponent.sharedMaterial = playerPhysicsMaterial;
        }

        private void OnDisable()
        {
            foreach (ICharacterMovementModule module in _modules)
                module.Dispose();

            _modules.Clear();
        }

        public void AddModule(ICharacterMovementModule module)
        {
            if (module == null)
                throw new ArgumentNullException(nameof(module));

            _modules.Add(module);
        }

        public void RemoveModule(ICharacterMovementModule module)
        {
            if (module == null)
                throw new ArgumentNullException(nameof(module));

            _modules.Remove(module);
        }

        private void FixedUpdate()
        {
            float deltaTime = Time.fixedDeltaTime;

            foreach (ICharacterMovementModule module in _modules)
                module.OnUpdate(this, deltaTime);

            _rigidbody2D.linearVelocity = Velocity;

            foreach (ICharacterMovementModule module in _modules)
                module.OnPostUpdate(this, deltaTime);
        }
    }
}
