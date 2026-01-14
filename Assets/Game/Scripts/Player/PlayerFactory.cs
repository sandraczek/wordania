using UnityEngine;
using VContainer;
using VContainer.Unity;

public sealed class PlayerFactory : IPlayerFactory
{
    private readonly IObjectResolver _container;
    private readonly PlayerConfig _config;

    public PlayerFactory(IObjectResolver container, PlayerConfig config)
    {
        _container = container;
        _config = config;
    }

    public IPlayerFacade Create(Vector2 position)
    {
        // Tworzymy sub-scope dla gracza
        var scope = _container.CreateScope(builder =>
        {
            // Rejestrujemy konkretne implementacje dla tego konkretnego gracza
            builder.Register<PlayerMovementHandler>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.Register<PlayerHealthHandler>(Lifetime.Scoped).AsImplementedInterfaces();
            // ... reszta sub-systemów
        });

        var prefab = _config.PlayerPrefab;
        var instance = _container.Instantiate(prefab, position, Quaternion.identity);
        
        // Inicjalizacja Facade z wstrzykniętym scope
        var facade = instance.GetComponent<PlayerFacade>();
        facade.SetScope(scope);
        
        return facade;
    }
}