using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public InventorySlotUI slotTemplate;
    private InventorySlotUI slotPlaceholder;
    public RectTransform slotsContainer;

    public RectTransform dragSpace;
    public InventoryOutArea outArea;

    [Header("Selected Item")]
    public RectTransform tooltipPanel;
    public TextMeshProUGUI tooltipName;
    public TextMeshProUGUI tooltipDescription;

    private List<InventorySlotUI> currentSlots = new List<InventorySlotUI>();
    public bool IsDragging { get; private set; }

    public Action<int> onHover;
    public Action<int> onUnhover;
    public Action<int, int> onDrop;
    public Action<int> onDropOutside;

    private void Awake()
    {
        slotTemplate.SetCount(-1);
        slotTemplate.gameObject.SetActive(false);
        slotPlaceholder = CreateInventorySlot();

        outArea.onDrop += OnDropOutside;

        HideTooltip();
    }

    public void SetSlotsCount(int count)
    {
        if (count < 0) return;

        if(count > currentSlots.Count)
        {
            slotTemplate.gameObject.SetActive(true);
            int difference = count - currentSlots.Count;
            for (int i = 0; i < difference; i++)
            {
                currentSlots.Add(CreateInventorySlot());
            }
            slotTemplate.gameObject.SetActive(false);
        }
        else if (count < currentSlots.Count)
        {
            int difference = currentSlots.Count - count;
            for (int i = 0; i < difference; i++)
            {
                Destroy(currentSlots[0]);
                currentSlots.RemoveAt(0);
            }
        }

        for (int i = 0; i < currentSlots.Count; i++)
        {
            InventorySlotUI slot = currentSlots[i];
            slot.index = i;
            slot.SetEmpty();

            StopListeningToSlot(slot);
            StartListeningToSlot(slot);
        }
    }

    private void StopListeningToSlot(InventorySlotUI slot)
    {
        slot.onBeginDrag -= OnItemBeginDrag;
        slot.onStopDrag -= OnItemStopDrag;
        slot.onDrop -= OnDrop;
        slot.onHover -= OnHover;
        slot.onUnhover -= OnUnhover;
    }
    private void StartListeningToSlot(InventorySlotUI slot)
    {
        slot.onBeginDrag += OnItemBeginDrag;
        slot.onStopDrag += OnItemStopDrag;
        slot.onDrop += OnDrop;
        slot.onHover += OnHover;
        slot.onUnhover += OnUnhover;
    }

    public void SetSlotItem(int slotIndex, ItemIdentifier inventoryItem, int count)
    {
        InventorySlotUI slot = currentSlots[slotIndex];
        slot.SetItem(Instantiate(inventoryItem.inventoryItemPrefab), inventoryItem.inventoryIconColor, count);
    }
    public void SetSlotEmpty(int slotIndex)
    {
        InventorySlotUI slot = currentSlots[slotIndex];
        slot.SetEmpty();
    }

    private void OnItemBeginDrag(RectTransform itemIcon)
    {
        itemIcon.transform.SetParent(dragSpace);
        IsDragging = true;
    }
    private void OnItemStopDrag(RectTransform itemIcon)
    {
        IsDragging = false;
    }
    private void OnDrop(InventorySlotUI A, InventorySlotUI B)
    {
        onDrop?.Invoke(A.index, B.index);
    }
    private void OnDropOutside(InventorySlotUI A)
    {
        onDropOutside?.Invoke(A.index);
    }
    private void OnHover(InventorySlotUI slot)
    {
        onHover?.Invoke(slot.index);
    }
    private void OnUnhover(InventorySlotUI slot)
    {
        onUnhover?.Invoke(slot.index);
    }    

    private InventorySlotUI CreateInventorySlot()
    {
        InventorySlotUI newSlot = Instantiate(slotTemplate, slotsContainer, false);
        return newSlot;
    }

    public void ShowTooltip(string name, string description)
    {
        tooltipPanel.gameObject.SetActive(true);
        tooltipName.text = name;
        tooltipDescription.text = description;
    }

    public void HideTooltip()
    {
        tooltipPanel.gameObject.SetActive(false);
    }
}