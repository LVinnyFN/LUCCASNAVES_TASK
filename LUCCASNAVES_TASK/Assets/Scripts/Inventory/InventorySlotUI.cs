using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlotUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI countText;

    [Header("Transforms")]
    public RectTransform inventoryItemParent;
    public RectTransform inventoryItemIconParent;
    public InventoryItem inventoryItem;

    public int index;
    public bool isDragging;
    private bool canDrag;

    public Action<RectTransform> onBeginDrag;
    public Action<RectTransform> onStopDrag;
    public Action<InventorySlotUI, InventorySlotUI> onDrop;
    public Action<InventorySlotUI> onHover;
    public Action<InventorySlotUI> onUnhover;

    public void SetEmpty()
    {
        if(inventoryItem) Destroy(inventoryItem.gameObject);
        inventoryItem = null;
        SetCanDrag(false);
        SetCount(-1);
    }
    public void SetItem(InventoryItem item, Color iconColor, int count)
    {
        SetEmpty();

        inventoryItem = item;
        ResetItemIconParent();
        SetColor(iconColor);
        SetCount(count);
        SetCanDrag(true);
    }

    private void SetColor(Color color) => inventoryItem.image.color = color;
    public void SetCount(int count)
    {
        if(count < 0) countText.gameObject.SetActive(false);
        else
        {
            countText.gameObject.SetActive(true);
            countText.text = count.ToString();
        }
    }
    public void SetCanDrag(bool value) => canDrag = value;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (canDrag == false) return;

        isDragging = true;
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            inventoryItemParent.anchoredPosition += eventData.delta;
            onBeginDrag?.Invoke(inventoryItemParent);            
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            isDragging = false;
            onStopDrag?.Invoke(inventoryItemParent); 
            ResetItemIconParent();
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if(eventData.pointerDrag.TryGetComponent(out InventorySlotUI slot))
        {
            //Debug.Log("Drop " + slot.index + " on " + index);
            onDrop?.Invoke(slot, this);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        onHover?.Invoke(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        onUnhover?.Invoke(this);
    }

    private void ResetItemIconParent()
    {
        inventoryItemParent.SetParent(transform);

        inventoryItemParent.anchorMin = Vector2.zero;
        inventoryItemParent.anchorMax = Vector2.one;
        inventoryItemParent.anchoredPosition = Vector2.zero;
        inventoryItemParent.sizeDelta = Vector2.zero;

        if (inventoryItem)
        {
            inventoryItem.rectTransform.SetParent(inventoryItemIconParent);

            inventoryItem.rectTransform.anchorMin = Vector2.zero;
            inventoryItem.rectTransform.anchorMax = Vector2.one;
            inventoryItem.rectTransform.anchoredPosition = Vector2.zero;
            inventoryItem.rectTransform.sizeDelta = Vector2.zero;
        }
    }
}