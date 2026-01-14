using VContainer;
using VContainer.Unity;
using UnityEngine;

public sealed class GameSceneLifetimeScope : LifetimeScope
    {
        [Header("Scene References")]
        [SerializeField] private CameraService _cameraService;
        [SerializeField] private WorldRenderer _worldRenderer;
        [SerializeField] private ChunkFactory _chunkFactory;
        [SerializeField] private WorldChunksRoot _chunksParent;
        [SerializeField] private LootEvent _lootEvent;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_cameraService).AsImplementedInterfaces();

            builder.Register<WorldPassTerrain>(Lifetime.Scoped).As<IWorldGenerationPass>();
            builder.Register<WorldPassCave>(Lifetime.Scoped).As<IWorldGenerationPass>();
            builder.Register<WorldPassVariations>(Lifetime.Scoped).As<IWorldGenerationPass>();
            builder.Register<WorldPassBarrier>(Lifetime.Scoped).As<IWorldGenerationPass>();

            builder.Register<WorldGenerator>(Lifetime.Scoped).AsImplementedInterfaces();

            builder.RegisterInstance(_lootEvent);

            builder.Register<WorldService>(Lifetime.Scoped).AsImplementedInterfaces();

            builder.RegisterComponent(_chunksParent);
            builder.Register<ChunkFactory>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.Register<WorldRenderer>(Lifetime.Scoped).AsImplementedInterfaces();
            
            builder.Register<Player>(Lifetime.Scoped);

            builder.RegisterEntryPoint<GameplayState>();
        }
    }
