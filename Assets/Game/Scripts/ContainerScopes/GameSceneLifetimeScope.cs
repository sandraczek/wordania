using VContainer;
using VContainer.Unity;
using UnityEngine;

public sealed class GameSceneLifetimeScope : LifetimeScope
    {
        [Header("Scene References")]
        [SerializeField] private CameraService _cameraService;
        [SerializeField] private WorldRenderer _worldRenderer;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(_cameraService)
                .AsImplementedInterfaces();
                
            builder.RegisterComponent(_worldRenderer);

            builder.Register<WorldService>(Lifetime.Scoped)
                .AsImplementedInterfaces()
                .AsSelf();

            builder.Register<WorldPassTerrain>(Lifetime.Singleton).As<IWorldGenerationPass>();
            builder.Register<WorldPassCave>(Lifetime.Singleton).As<IWorldGenerationPass>();
            builder.Register<WorldPassVariations>(Lifetime.Singleton).As<IWorldGenerationPass>();
            builder.Register<WorldPassBarrier>(Lifetime.Singleton).As<IWorldGenerationPass>();

            builder.Register<WorldGenerator>(Lifetime.Singleton);
            

            // EntryPoint: Klasa, która zainicjuje start gry na tej scenie
            builder.RegisterEntryPoint<GameplayState>();
        }
    }
