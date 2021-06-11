using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemData
{
    public string id;
    public string type;
    public string realname;
    public string attack;
}

[System.Serializable]
public class RoleData
{
    public string id;
    public string name;
    public string quality;
    public string exp;
    public string exp_upgrade;
    public string hp;
    public string hp_upgrade;
    public string attack;
    public string attack_upgrade;
    public string def;
    public string def_upgrade;
    public string talentId;
}

[System.Serializable]
public class TypeData
{
    public string id;
    public string name;
    public string itemId;
    public string itemNum;
    public string itemInfo;
}

[System.Serializable]
public class TalentData
{
    public string id;
    public string name;
    public string probability;
    public string value;
    public string duration;
    public string info;
}