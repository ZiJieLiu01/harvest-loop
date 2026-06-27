using UnityEngine;

public interface IInteractable
{
    /// <summary>
    /// When player hovers the tile and left click
    /// </summary>
    public virtual void OnPrimaryInteract(IItemReceiver receiver, ToolData toolData) {  }

    /// <summary>
    /// When player hovers the tile and right click
    /// </summary>
    public virtual void OnSecondaryInteract(IItemReceiver receiver) {  }

    public virtual string GetDebugInfo() { return ""; }
}