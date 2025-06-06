using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public TextMeshProUGUI countText;
    public Image iconImage;

    public int index;
    public bool isDragging;
    private bool canDrag;

    public Action<Image> onBeginDrag;
    public Action<InventorySlotUI, InventorySlotUI> onDrop;

    public void SetEmpty()
    {
        SetIcon(null);
        SetColor(Color.white);
        SetCount(-1);
        SetCanDrag(false);
    }
    public void SetItem(Sprite icon, Color iconColor, int count)
    {
        SetIcon(icon);
        SetColor(iconColor);
        SetCount(count);
        SetCanDrag(true);
    }

    public void SetIcon(Sprite iconSprite) => iconImage.sprite = iconSprite;
    public void SetColor(Color color) => iconImage.color = color;
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
            iconImage.rectTransform.anchoredPosition += eventData.delta;
            onBeginDrag?.Invoke(iconImage);
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            isDragging = false;
            iconImage.transform.SetParent(transform);
            iconImage.rectTransform.anchoredPosition = Vector2.zero;
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
}