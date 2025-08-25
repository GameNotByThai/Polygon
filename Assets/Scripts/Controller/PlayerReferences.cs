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

    public void Init()
    {
        curAmmo.value = 0;
        curCarrying.value = 0;
        health.value = 100;
        isAming.value = false;
        isLeftPivot.value = false;
        isCrouching.value = false;
        targetSpread.value = 30;
    }
}
