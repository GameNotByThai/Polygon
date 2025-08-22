using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SingleInstances/Resources")]
public class ResourcesManager : ScriptableObject
{
    public RuntimeReferences runtime;
    public Weapon[] all_weapons;
    Dictionary<string, int> w_dict = new Dictionary<string, int>();
    public void Init()
    {
        for (int i = 0; i < all_weapons.Length; i++)
        {
            if (w_dict.ContainsKey(all_weapons[i].id))
            {

            }
            else
            {
                w_dict.Add(all_weapons[i].id, i);
            }
        }
    }

    public Weapon GetWeapon(string id)
    {
        Weapon retVal = null;
        int index = -1;
        
        if (w_dict.TryGetValue(id, out index))
        {
            retVal = all_weapons[index];
        }

        return retVal;
    }
}
