using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponManager 
{
    public string mainWeaponId;
    public string secWeaponId;

    public RuntimeWeapon m_weapon;
    public RuntimeWeapon s_weapon;
}
