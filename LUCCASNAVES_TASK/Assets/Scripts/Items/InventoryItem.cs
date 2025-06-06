using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    public ItemIdentifierSO itemIdentifierSO;
    public ItemIdentifier itemIdentifier;
    [Min(1)] public int amount;

    public RectTransform rectTransform;
    public Image image;

    private void Awake()
    {
        rectTransform = (RectTransform)transform;
    }

    private void OnValidate()
    {
        if (itemIdentifierSO) itemIdentifier = itemIdentifierSO.identifier;
    }
}
