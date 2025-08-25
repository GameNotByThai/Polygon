using UnityEngine;
using UnityEngine.UI;

public class UITextUpdateJob : MonoBehaviour
{
    public Text modeType;
    public Text levelName;
    public Text currentUser;
    public Text maxUsers;
    public Job targetJob;

    private void Start()
    {
        LoadJob(targetJob);
    }

    public void LoadJob(Job job)
    {
        targetJob = job;
        modeType.text = StaticFunctions.JobTypeToString(job.jobType);
        levelName.text = job.targetLevel;
    }

    public void UpdateJobOnGameSettings()
    {
        GameSettings gameSettings = Resources.Load("GameSetting") as GameSettings;
        gameSettings.UpdateCurrentJob(targetJob);
    }
}
