using VContainer;
using VContainer.Unity;
using UnityEngine;

public sealed class ProjectLifetimeScope : LifetimeScope
{
    [SerializeField] private BlockDatabase _blockDatabase;
    [SerializeField] private ItemDatabase _itemDatabase;
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private WorldSettings _defaultWorldSettings;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterInstance(_blockDatabase).AsImplementedInterfaces().AsSelf();
        builder.RegisterInstance(_itemDatabase).AsImplementedInterfaces().AsSelf();
        builder.RegisterInstance(_inputReader).AsImplementedInterfaces().AsSelf();
        builder.RegisterInstance(_defaultWorldSettings);

        builder.Register<DebugService>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();

        builder.Register<JsonPersistenceProvider>(Lifetime.Singleton).AsImplementedInterfaces();
        builder.Register<SaveService>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();


            
    }
}
