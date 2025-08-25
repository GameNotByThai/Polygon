using UnityEngine;

public class ReplaceEventOnSession : MonoBehaviour
{
    public SessionsManager sessions;
    public GameEvent targetEvent;

    public void ReplaceSceneSingleEvent()
    {
        sessions.onSceneLoadedSingle = targetEvent;
    }

    public void ReplaceSceneAdditiveEvent()
    {
        sessions.onSceneLoadedAdditive = targetEvent;
    }
}
