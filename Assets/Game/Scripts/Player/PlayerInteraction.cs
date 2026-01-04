using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerInteraction : MonoBehaviour
{
    private Player _player;
    [field:SerializeField] private WorldManager worldManager;
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
    private InteractionMode _interactionMode = InteractionMode.build;
    public void Awake()
    {
        _player = GetComponent<Player>();
        _nextActionTime = 0f;
    }
    public void OnEnable()
    {
        _player.Inputs.OnHotbarSlotPressed += SetTool;
        _player.Inputs.OnCycleActionSettings += CycleToolSetting;
        _player.Inputs.OnPrimaryActionHeld += SetPrimaryActionHeld;
    }
    public void OnDisable()
    {
        _player.Inputs.OnHotbarSlotPressed -= SetTool;
        _player.Inputs.OnCycleActionSettings -= CycleToolSetting;
        _player.Inputs.OnPrimaryActionHeld -= SetPrimaryActionHeld;
    }
    public void Update()
    {
        if(_isPrimaryActionHeld && _player.States.CurrentState.CanPerformActions)
        {
            ExecutePrimaryAction(_player.Controller.GetWorldAimPosition());
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
                if(!TryBuild(targetWorldPos)) return false;
                break;
            case InteractionMode.mine:
                TryMine(targetWorldPos);  // Interaction even if didnt mine anything
                break;
        }
        _nextActionTime = Time.time + _actionRate;
        return true;
    }
    private bool TryBuild(Vector2 targetWorldPos)
    {
        if (_buildingBlock == null) return false;
        foreach (Ingredient ingredient in _buildingBlock.recipe.Requirements){
            if (_player.Inventory.GetQuantity(ingredient.item.Id) < ingredient.amount) return false;
        }

        if(!worldManager.TryPlaceBlock(targetWorldPos, _buildingBlock.ID)) return false;

        foreach (Ingredient ingredient in _buildingBlock.recipe.Requirements){
            Debug.Assert(_player.Inventory.TryRemoveItem(ingredient.item, ingredient.amount));
        }

        return true;
    }
    private bool TryMine(Vector2 targetWorldPos)
    {
        if(!_areaMine){
            if(!worldManager.TryDamageBlock(targetWorldPos, _minePower)) return false;
        }
        else{
            if(!worldManager.TryDamageCircle(targetWorldPos, _areaRadius, _minePower)) return false;
        }
        return true;
    }
    public void CycleToolSetting()
    {
        if(!_player.States.CurrentState.CanSetSlot) return;
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
        if(!_player.States.CurrentState.CanSetSlot) return;
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

    private enum InteractionMode
    {
        mine,
        build
    }
}
