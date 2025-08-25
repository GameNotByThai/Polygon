using UnityEngine;

public class ReplaceEvent : MonoBehaviour
{
    public GameEvent targetEvent;
    public GameEventListener listener;

    public void Replace()
    {
        listener.gameEvent.UnRegister(listener);
        listener.gameEvent = targetEvent;
        targetEvent.Register(listener);
    }
}
