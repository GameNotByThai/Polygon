using UnityEngine;

[CreateAssetMenu(menuName = "Job")]
public class Job : ScriptableObject
{
    public string targetLevel;
    public JobType jobType;
    public int maxPlayers = 10;
}

public enum JobType
{
    shootout, heist, teamdeathmatch
}
