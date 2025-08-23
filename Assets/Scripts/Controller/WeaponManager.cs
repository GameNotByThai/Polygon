using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponManager 
{
    public string mainWeaponId;
    public string secWeaponId;

    RuntimeWeapon curWeapon;
    public RuntimeWeapon GetCurrent()
    {
        return curWeapon;
    }

    public void SetCurrent(RuntimeWeapon rw)
    {
        curWeapon = rw;
    }

    public RuntimeWeapon m_weapon;
    public RuntimeWeapon s_weapon;
}
