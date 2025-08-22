using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SingleInstances/RuntimeReferences")]
public class RuntimeReferences : ScriptableObject
{
    public List<RuntimeWeapon> runtimeWeapons = new List<RuntimeWeapon>();

    public void Init()
    {
        runtimeWeapons.Clear();
    }

    public RuntimeWeapon WeaponToRuntimeWeapon(Weapon w)
    {
        RuntimeWeapon rw = new RuntimeWeapon();
        rw.w_actual = w;
        rw.curAmmo = w.magazineAmmo;
        rw.curCarrying = w.maxAmmo;
        runtimeWeapons.Add(rw);
        return rw;
    }

    public void RemoveRuntimeWeapone(RuntimeWeapon rw)
    {
        if (rw.m_instance)
            Destroy(rw.m_instance);

        if(runtimeWeapons.Contains(rw))
            runtimeWeapons.Remove(rw);
    }
}

[System.Serializable]
public class RuntimeWeapon
{
    public int curAmmo;
    public int curCarrying;
    public GameObject m_instance;
    public WeaponHook w_hook;
    public Weapon w_actual;
}
