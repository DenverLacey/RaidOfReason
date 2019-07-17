using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Shop List/Add Item")]
public class Items : ScriptableObject
{
    public string Name;
    public string Description;
    public Sprite Icon;
    public int cashNeeded;

}
