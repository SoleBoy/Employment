using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageTalent : ScriptableObject
{
    public List<TalentData> items;
    public Dictionary<string, TalentData> dicItem = new Dictionary<string, TalentData>();
    public Dictionary<string, TalentData> GetItems()
    {
        dicItem.Clear();
        for (int i = 0; i < items.Count; i++)
        {
            dicItem.Add(items[i].id, items[i]);
        }
        return dicItem;
    }
}
