using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public InventorySlotUI slotTemplate;
    private InventorySlotUI slotPlaceholder;
    public RectTransform slotsContainer;

    public RectTransform dragSpace;
    public InventoryOutArea outArea;

    private List<InventorySlotUI> currentSlots = new List<InventorySlotUI>();

    public Action<int, int> onDrop;
    public Action<int> onDropOutside;

    private void Awake()
    {
        slotTemplate.SetCount(-1);
        slotTemplate.gameObject.SetActive(false);
        slotPlaceholder = CreateInventorySlot();

        outArea.onDrop += OnDropOutside;
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

            slot.onBeginDrag -= OnItemBeginDrag;
            slot.onDrop -= OnDrop;

            slot.onBeginDrag += OnItemBeginDrag;
            slot.onDrop += OnDrop;
        }
    }

    public void SetSlotItem(int slotIndex, ItemIdentifier inventoryItem, int count)
    {
        InventorySlotUI slot = currentSlots[slotIndex];
        slot.SetItem(inventoryItem.inventoryIcon, inventoryItem.inventoryIconColor, count);
    }
    public void SetSlotEmpty(int slotIndex)
    {
        InventorySlotUI slot = currentSlots[slotIndex];
        slot.SetEmpty();
    }

    private void OnItemBeginDrag(Image itemIcon)
    {
        itemIcon.transform.SetParent(dragSpace);
    }
    private void OnDrop(InventorySlotUI A, InventorySlotUI B)
    {
        onDrop?.Invoke(A.index, B.index);
    }
    private void OnDropOutside(InventorySlotUI A)
    {
        onDropOutside?.Invoke(A.index);
    }

    private InventorySlotUI CreateInventorySlot()
    {
        InventorySlotUI newSlot = Instantiate(slotTemplate, slotsContainer, false);
        return newSlot;
    }
}