using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageType : ScriptableObject
{
    public List<TypeData> items;
    public Dictionary<string, TypeData> dicItem = new Dictionary<string, TypeData>();
    public Dictionary<string, TypeData> GetItems()
    {
        dicItem.Clear();
        for (int i = 0; i < items.Count; i++)
        {
            dicItem.Add(items[i].id, items[i]);
        }
        return dicItem;
    }
}
