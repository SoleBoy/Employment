using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageRole : ScriptableObject
{
    public List<RoleData> items;
    public Dictionary<string, RoleData> dicItem = new Dictionary<string, RoleData>();
    public Dictionary<string, RoleData> GetItems()
    {
        dicItem.Clear();
        for (int i = 0; i < items.Count; i++)
        {
            dicItem.Add(items[i].id, items[i]);
        }
        return dicItem;
    }
}
