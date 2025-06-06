using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryOutArea : MonoBehaviour, IDropHandler
{
    public Action<InventorySlotUI> onDrop;
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.TryGetComponent(out InventorySlotUI slot))
        {
            //Debug.Log("Drop " + slot.index + " outside inventory");
            onDrop?.Invoke(slot);
        }
    }
}
