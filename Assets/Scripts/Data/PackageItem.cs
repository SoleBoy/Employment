using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageItem : ScriptableObject
{
    public List<ItemData> items;
    public Dictionary<string, ItemData> dicItem = new Dictionary<string, ItemData>();
    public Dictionary<string, ItemData> GetItems()
    {
        dicItem.Clear();
        for (int i = 0; i < items.Count; i++)
        {
            dicItem.Add(items[i].id, items[i]);
        }
        return dicItem;
    }
}
