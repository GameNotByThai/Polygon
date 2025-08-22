using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "SingleInstances/PlayerRefs")]
public class PlayerReferences : ScriptableObject
{
    public IntVariable curAmmo;
    public IntVariable curCarrying;
    public IntVariable health;
    public FloatVariable targetSpread;
}
