using UnityEngine;

[System.Serializable]
public class ItemIdentifier
{
    public string ID;
    public string name;
    public string description;
    public int maxStack;
    public Sprite inventoryIcon;
    public Color inventoryIconColor = Color.white;
    public WorldItem prefab;
}
