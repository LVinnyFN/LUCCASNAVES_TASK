public class InventorySlot
{
    public ItemIdentifier inventoryItem;
    public int currentItemCount;
    public int maxItemCount;

    public int index;

    public void SetItem(ItemIdentifier item, int count = 1)
    {
        inventoryItem = item;
        if (item.maxStack == -1)
        {
            maxItemCount = -1;
        }
        else
        {
            maxItemCount = item.maxStack;
        }
        currentItemCount = count;
    }
    public void SetEmpty()
    {
        inventoryItem = null;
        currentItemCount = 0;
        maxItemCount = -1;
    }

    public bool AddItem()
    {
        if (currentItemCount >= maxItemCount) return false;
        currentItemCount++;
        return true;
    }
    public void AddItem(int count, out int remainingCount)
    {
        if(currentItemCount >= maxItemCount)
        {
            remainingCount = count;
            return;
        }
        else
        {
            int remainingSlots = maxItemCount - currentItemCount;
            if(count <= remainingSlots)
            {
                currentItemCount += count;
                remainingCount = 0;
            }
            else
            {
                currentItemCount += remainingSlots;
                remainingCount = count - remainingSlots;
            }
        }        
    }
    public bool RemoveItem()
    {
        if(currentItemCount == 0)
        {
            inventoryItem = null;
            return false;
        }
        currentItemCount--; 
        if (currentItemCount == 0) inventoryItem = null;
        return true;
    }
    public void RemoveItem(int count, out int removedCount)
    {
        if (currentItemCount == 0)
        {
            removedCount = count;
            return;
        }
        else
        {           
            if (count <= currentItemCount)
            {
                currentItemCount -= count;
                removedCount = count;
            }
            else
            {
                removedCount = currentItemCount;
                currentItemCount = 0;
            }
        }
    }
    public bool IsEmpty()
    {
        return currentItemCount == 0;
    }
}