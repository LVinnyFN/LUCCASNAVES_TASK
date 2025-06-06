using UnityEngine;

public class GameController : MonoBehaviour
{
    public PlayerController playerController;
    public bool deleteGame;

    private void Awake()
    {
        if(deleteGame) DeleteGame();
        LoadGame();
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    public void SaveGame()
    {
        InventorySlot[] inventorySlots = playerController.playerInventory.GetSlots();
        InventorySlotArraySaveData inventorySlotArraySaveData = new InventorySlotArraySaveData();
        inventorySlotArraySaveData.inventorySlots = new InventorySlotSaveData[inventorySlots.Length];

        for (int i = 0; i < inventorySlotArraySaveData.inventorySlots.Length; i++)
        {
            if (inventorySlots[i].inventoryItem == null)
            {
                inventorySlotArraySaveData.inventorySlots[i].itemID = "";
                inventorySlotArraySaveData.inventorySlots[i].amount = -1;
            }
            else 
            {
                inventorySlotArraySaveData.inventorySlots[i].itemID = inventorySlots[i].inventoryItem.ID;
                inventorySlotArraySaveData.inventorySlots[i].amount = inventorySlots[i].currentItemCount;
            }
        }

        SaveLoad.Save(inventorySlotArraySaveData, "PlayerInventory");
    }

    public void LoadGame()
    {
        if(SaveLoad.Load("PlayerInventory", out InventorySlotArraySaveData playerInventoryData))
        {
            InventorySlot[] inventorySlots = new InventorySlot[playerInventoryData.inventorySlots.Length];

            for (int i = 0; i < inventorySlots.Length; i++)
            {
                InventorySlot slot = new InventorySlot();
                inventorySlots[i] = slot;
                if (playerInventoryData.inventorySlots[i].itemID == "")
                {
                    slot.SetEmpty();
                }
                else
                {
                    ItemIdentifier itemIdentifier = ItemUtilities.GetItemIdentifier(playerInventoryData.inventorySlots[i].itemID);
                    slot.SetItem(itemIdentifier, playerInventoryData.inventorySlots[i].amount);
                }
            }

            playerController.playerInventory.SetInventorySize(inventorySlots);
        }
        else
        {
            playerController.playerInventory.SetInventorySize(playerController.playerInventory.defaultInventorySize);
        }
    }
    public void DeleteGame()
    {
        SaveLoad.Delete("PlayerInventory");
    }
}
