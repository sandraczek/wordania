using UnityEngine;
using UnityEngine.UI;
using VContainer;

[RequireComponent(typeof(ProgressBarUI))]
public class HealthBarPresenter : MonoBehaviour
{
    private IHealthService _health;
    private ProgressBarUI _healthBarView;

    [Inject]
    public void Construct(IHealthService health) => _health = health;
    private void Awake()
    {
        _healthBarView = GetComponent<ProgressBarUI>();
    }
    private void OnEnable()
    {
        _health.OnHealthChanged += HandleHealthChange;
    }
    private void OnDisable()
    {
        _health.OnHealthChanged -= HandleHealthChange;
    }

    private void HandleHealthChange()
    {
        _healthBarView.UpdateBar(_health.Current, _health.Max);
    }
    private void HandleMaxChange()
    {
        _healthBarView.UpdateBar(_health.Current, _health.Max);
    }
}