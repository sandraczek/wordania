using VContainer;
using VContainer.Unity;
using UnityEngine;

public sealed class ProjectLifetimeScope : LifetimeScope
{
    [Header("Global Configurations")]
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private WorldSettings _defaultWorldSettings;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterInstance(_inputReader).AsSelf();
        builder.RegisterInstance(_defaultWorldSettings).AsSelf();

        //To do - save manager to POCO
        builder.Register<SaveManager>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            
    }
}
