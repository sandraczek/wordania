using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Slider healthSlider;
    
    [Header("Settings")]
    [SerializeField] private float smoothSpeed = 10f;

    private Player _player;
    private float _targetValue;

    private void OnEnable()
    {
        if (Player.Local != null)
        {
            Bind(Player.Local);
        }
        
        Player.OnLocalPlayerReady += Bind;
    }

    private void OnDisable()
    {
        Player.OnLocalPlayerReady -= Bind;
        Unbind();
    }
    public void Bind(Player player)
    {
        if (player == null || _player == player) return;
        Unbind();
        _player = player;
        
        _player.Health.OnHealthChanged += HandleHealthChanged;

        SetValues(_player.Health.Current, _player.Health.Max, DamageSource.INITIALIZE);
    }
    public void Unbind()
    {
        if(_player == null) return;
        _player.Health.OnHealthChanged -= HandleHealthChanged;
    }
    private void HandleHealthChanged(HealthUpdateArgs args)
    {
        SetValues(args.current, args.max, args.sourceID);
    }
    private void SetValues(float current, float max, int sourceId)
    {
        _targetValue = current / max;

        if (DamageSource.IsSystem(sourceId))
        {
            healthSlider.value = _targetValue;
        }
        Debug.Log(_targetValue);
    }

    private void Update()
    {
        if (!Mathf.Approximately(healthSlider.value, _targetValue))
        {
            healthSlider.value = Mathf.Lerp(healthSlider.value, _targetValue, Time.deltaTime * smoothSpeed);
        }
    }
}