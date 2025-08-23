using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "SingleInstances/PlayerRefs")]
public class PlayerReferences : ScriptableObject
{
    [Header("HUD")]
    public IntVariable curAmmo;
    public IntVariable curCarrying;
    public IntVariable health;

    [Header("State")]
    public BoolVariable isAming;
    public BoolVariable isLeftPivot;
    public BoolVariable isCrouching;

    [Header("UI")]
    public FloatVariable targetSpread;
    public GameEvent e_UpdateUI;
}
