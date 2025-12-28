using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "Block Database")]
public class BlockDatabase : ScriptableObject
{
    public List<BlockData> allBlocks = new();
    private Dictionary<string, BlockData> _blockMap;

    public void Initialize()
    {
        _blockMap = new Dictionary<string, BlockData>();
        foreach (var block in allBlocks)
        {
            if (block != null) _blockMap.TryAdd(block.ItemID, block);
        }
    }

    public BlockData GetBlock(string id)
    {
        if (_blockMap.TryGetValue(id, out var block)) return block;
        return null;
    }

    #if UNITY_EDITOR
    [ContextMenu("Auto-Find All Blocks")]
    public void FindAllBlocksInProject()
    {
        allBlocks.Clear();

        string[] guids = AssetDatabase.FindAssets("t:BlockData");

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            BlockData block = AssetDatabase.LoadAssetAtPath<BlockData>(path);
            
            if (block != null)
            {
                allBlocks.Add(block);
            }
        }
        
        Debug.Log($"Sukces! Znaleziono i dodano {allBlocks.Count} bloków do bazy.");
        
        EditorUtility.SetDirty(this); 
    }
#endif
}
