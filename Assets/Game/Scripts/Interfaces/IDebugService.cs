using UnityEngine;
using System;
public interface IDebugService
{
    bool IsGodModeActive { get; }
    void ToggleGodMode();

    event Action<bool> OnShowChunksChanged;
}