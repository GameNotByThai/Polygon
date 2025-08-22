using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Weapon", order = 0)]
public class Weapon : ScriptableObject
{
    public string id;
    public IKPositions m_h_ik;
    public GameObject modelPrefabs;
    public float fireRate = .1f;
    public int magazineAmmo = 30;
    public int maxAmmo = 160;
    public bool onIdleDisableOh;
    public int weaponType;
    public AnimationCurve recoilY;
    public AnimationCurve recoilZ;
}
