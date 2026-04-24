using System.Collections.Generic;
using UnityEngine;

public enum ToolType
{
    None,
    Axe,
    Hoe,
    Pickaxe,
    Shovel,
}

public class Tool : MonoBehaviour
{
    [SerializeField] private ToolType type;
    [SerializeField] private int upgrade;
    [SerializeField] private int durability;

    public void LoadFrom(ToolData toolData)
    {
        if (toolData == null)
            return;


    }
}
