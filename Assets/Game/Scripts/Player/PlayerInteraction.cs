using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System;
using Unity.VisualScripting;

public class PlayerInteraction : MonoBehaviour
{
    private PlayerStateMachine _ctx;
    private bool _isPrimaryActionHeld = false;
    [SerializeField] private float _actionRange = 6f;
    [field: SerializeField] private float _actionRate = 0.05f;
    [Header("Mining")]
    [field: SerializeField] private float _minePower = 1f; 
    private float _nextActionTime;
    [field: SerializeField] private bool _areaMine = false;
    [field: SerializeField] private float _areaRadius = 1.5f;
    [Header("Building")]
    [SerializeField] private BlockData _buildingBlock;
    private enum InteractionMode
    {
        mine,
        build
    }
    public void Awake()
    {
        _ctx = GetComponent<PlayerStateMachine>();
        _nextActionTime = 0f;
    }
    private InteractionMode _interactionMode = InteractionMode.build;
    public void OnEnable()
    {
        _ctx.Controller.OnHotbarSlotPressed += SetTool;
        _ctx.Controller.OnToolSettingChanged += CycleToolSetting;
        _ctx.Controller.OnPrimaryActionHeld += SetPrimaryActionHeld;
    }
    public void OnDisable()
    {
        _ctx.Controller.OnHotbarSlotPressed -= _ctx.Controller.Interaction.SetTool;
        _ctx.Controller.OnToolSettingChanged -= _ctx.Controller.Interaction.CycleToolSetting;
        _ctx.Controller.OnPrimaryActionHeld -= SetPrimaryActionHeld;
    }
    public void Update()
    {
        if(_isPrimaryActionHeld && _ctx.CurrentState.CanPerformActions)
        {
            ExecutePrimaryAction(_ctx.Controller.GetWorldAimPosition());
        }
    }
    public bool ExecutePrimaryAction(Vector2 targetWorldPos)
    {
        if (Time.time < _nextActionTime) return false;
        // checking range
        float deltaRoundX = Mathf.Abs(Mathf.Round(targetWorldPos.x - 0.5f) - Mathf.Round(transform.position.x));
        float deltaRoundY = Mathf.Abs(Mathf.Round(targetWorldPos.y - 0.5f) - 2f - Mathf.Round(transform.position.y)); // distance from arms so -2f
        if (deltaRoundX > _actionRange || deltaRoundY > _actionRange) return false;
        switch (_interactionMode){
            case InteractionMode.build:
                if(!WorldManager.Instance.TryPlaceBlock(targetWorldPos, _buildingBlock.ID)) return false;
                break;
            case InteractionMode.mine:
                if(!_areaMine){
                    if(!WorldManager.Instance.TryDamageBlock(targetWorldPos, _minePower)) return false;
                }
                else{
                    if(!WorldManager.Instance.TryDamageCircle(targetWorldPos, _areaRadius, _minePower)) return false;
                }
                break;
        }
        _nextActionTime = Time.time + _actionRate;
        return true;
    }
    public void CycleToolSetting()
    {
        if(!_ctx.CurrentState.CanSetSlot) return;
        switch (_interactionMode){
            case InteractionMode.mine:
                _areaMine = !_areaMine;
                break;
            case InteractionMode.build:
                break;
        }
    }
    public void SetTool(int i)
    {
        if(!_ctx.CurrentState.CanSetSlot) return;
        if(i == 1)
        {
            _interactionMode = InteractionMode.build;
            return;
        }
        if(i == 2)
        {
            _interactionMode = InteractionMode.mine;
            return;
        }
    }
    private void SetPrimaryActionHeld(bool boo)
    {
        _isPrimaryActionHeld = boo;
    }
}
