using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ProgressBarUI))]
public class HealthBarPresenter : MonoBehaviour
{
    [SerializeField] private HealthVariable _current;
    [SerializeField] private FloatReference _max;
    private ProgressBarUI _healthBarView;

    private void Awake()
    {
        _healthBarView = GetComponent<ProgressBarUI>();
    }
    private void OnEnable()
    {
        _current.OnHealthChangedWithContext += HandleHealthChange;
        _max.Variable.OnValueChanged += HandleMaxChange;
    }
    private void OnDisable()
    {
        _current.OnHealthChangedWithContext -= HandleHealthChange;
        _max.Variable.OnValueChanged -= HandleMaxChange;
    }

    private void HandleHealthChange(HealthChangeContext context)
    {
        _healthBarView.UpdateBar(context.NewValue, _max);

        // switch (context.SourceID)
        // {
        //     case DamageSource.FALL_DAMAGE:
        //         //PlayFallDamageEffect();
        //         break;
        //     case DamageSource.POISON:
        //         _healthBarView.SetColor(Color.green);
        //         break;
        //     default:
        //         _healthBarView.SetColor(Color.red);
        //         break;
        // }
    }
    private void HandleMaxChange(float newValue)
    {
        _healthBarView.UpdateBar(_current, newValue);
    }
}