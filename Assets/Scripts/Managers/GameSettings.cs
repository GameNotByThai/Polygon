using UnityEngine;

[CreateAssetMenu(menuName = "SingleInstances/GameSettings")]
public class GameSettings : ScriptableObject
{
    public ResourcesManager r_manager;
    public int version = 0;
    public string username;
    public bool isConneted;
    public Job curJob;
    public GameEvent onJobChanged;

    public void UpdateCurrentJob(Job targetJob)
    {
        curJob = targetJob;
        if (onJobChanged != null)
            onJobChanged.Raise();
    }
}
