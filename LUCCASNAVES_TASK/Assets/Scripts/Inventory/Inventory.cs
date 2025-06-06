using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public InventoryUI inventoryUI;
    public int defaultInventorySize = 16;
    private int inventorySize;

    public Action<ItemIdentifier, int> onDropOutside;

    private List<InventorySlot> inventorySlots = new List<InventorySlot>();

    private void Awake()
    {
        SetUIActive(false);

        inventoryUI.onHover += OnHover;
        inventoryUI.onUnhover += OnUnhover;
        inventoryUI.onDrop += OnDrop;
        inventoryUI.onDropOutside += OnDropOutside;
    }

    private void Update()
    {
        // TODO: TEMPORARY INPUT, IMPLEMENT NEW INPUT SYSTEM
        if (Input.GetKeyDown(KeyCode.I)) AlternateUIActivation();
    }

    public void SetInventorySize(int size)
    {
        inventorySize = size;
        inventoryUI.SetSlotsCount(inventorySize);

        inventorySlots.Clear();
        for(int i = 0; i < size; i++)
        {
            InventorySlot slot = new InventorySlot();
            slot.index = i;
            inventorySlots.Add(slot);
        }
    }
    public void SetInventorySize(InventorySlot[] slots)
    {
        inventorySize = slots.Length;
        inventoryUI.SetSlotsCount(inventorySize);

        inventorySlots.Clear();
        for (int i = 0; i < inventorySize; i++)
        {
            InventorySlot slot = slots[i];
            slot.index = i;
            inventorySlots.Add(slot);
            if (slot.inventoryItem == null) inventoryUI.SetSlotEmpty(slot.index);
            else inventoryUI.SetSlotItem(slot.index, slot.inventoryItem, slot.currentItemCount);
        }
    }

    public bool TryAddItem(ItemIdentifier item, int count)
    {
        if (SearchForFirstEmptySlot(out InventorySlot slot))
        {
            slot.SetItem(item, count);
            inventoryUI.SetSlotItem(slot.index, slot.inventoryItem, slot.currentItemCount);
            return true;
        }
        return false;
    }

    public bool SearchForFirstEmptySlot(out InventorySlot emptySlot)
    {
        foreach (InventorySlot slot in inventorySlots)
        {
            if (slot.IsEmpty())
            {
                emptySlot = slot;
                return true;
            }
        }
        emptySlot = null;
        return false;
    }

    private void OnHover(int itemIndex)
    {
        InventorySlot slot = inventorySlots[itemIndex];
        ItemIdentifier item = slot.inventoryItem;
        
        if(item == null || inventoryUI.IsDragging)
        {
            inventoryUI.HideTooltip();
        }
        else
        {
            inventoryUI.ShowTooltip(item.name, item.description);
        }
    }
    private void OnUnhover(int itemIndex)
    {
        inventoryUI.HideTooltip();
    }
    private void OnDrop(int aIndex, int bIndex)
    {
        InventorySlot A = inventorySlots[aIndex];
        ItemIdentifier AItem = A.inventoryItem;
        int ACount = A.currentItemCount;

        InventorySlot B = inventorySlots[bIndex];
        ItemIdentifier BItem = B.inventoryItem;
        int BCount = B.currentItemCount;

        if (BItem == null)
        {
            A.SetEmpty();
            inventoryUI.SetSlotEmpty(aIndex);
        }
        else
        {
            A.SetItem(BItem, BCount);
            inventoryUI.SetSlotItem(aIndex, A.inventoryItem, A.currentItemCount);
        }

        if (AItem == null)
        {
            B.SetEmpty();
            inventoryUI.SetSlotEmpty(bIndex);
        }
        else
        {
            B.SetItem(AItem, ACount);
            inventoryUI.SetSlotItem(bIndex, B.inventoryItem, B.currentItemCount);
        }
    }

    private void OnDropOutside(int itemIndex)
    {
        InventorySlot slot = inventorySlots[itemIndex];
        ItemIdentifier item = slot.inventoryItem;
        int count = slot.currentItemCount;

        if (item != null)
        {
            slot.SetEmpty();
            inventoryUI.SetSlotEmpty(itemIndex);
        }

        onDropOutside?.Invoke(item, count);
    }

    public InventorySlot[] GetSlots()
    {
        return inventorySlots.ToArray();
    }

    public void AlternateUIActivation()
    {
        SetUIActive(!inventoryUI.gameObject.activeSelf);
    }
    public void SetUIActive(bool active)
    {
        inventoryUI.gameObject.SetActive(active);
    }
}