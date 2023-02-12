using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MItem : MonoBehaviour
{
    public enum Type
    {
        Ammo, Grenade, Heart, Weapon
    }

    public Type type;
    public int value;
}
