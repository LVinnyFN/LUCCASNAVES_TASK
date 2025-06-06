using UnityEngine;

[System.Serializable]
public class ItemIdentifier
{
    public string ID;
    public int maxStack;
    public Sprite inventoryIcon;
    public Color inventoryIconColor = Color.white;
    public WorldItem prefab;
}
