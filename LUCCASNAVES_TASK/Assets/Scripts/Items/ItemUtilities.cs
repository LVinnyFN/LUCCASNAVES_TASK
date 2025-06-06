using System.Collections.Generic;
using UnityEngine;

public static class ItemUtilities
{
    private static Dictionary<string, ItemIdentifier> itemIdentifierByID = new Dictionary<string, ItemIdentifier>();

    public static ItemIdentifier GetItemIdentifier(string ID)
    {
        if(itemIdentifierByID.Count == 0)
        {
            ItemIdentifierSO[] itemIdentifiers = Resources.LoadAll<ItemIdentifierSO>("Items");
            foreach(ItemIdentifierSO itemIdentifier in itemIdentifiers)
            {
                itemIdentifierByID.Add(itemIdentifier.identifier.ID, itemIdentifier.identifier);
            }
        }

        return itemIdentifierByID[ID];
    }
}
