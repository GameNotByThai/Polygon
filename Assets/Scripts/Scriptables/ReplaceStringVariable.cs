using UnityEngine;

public class ReplaceStringVariable : MonoBehaviour
{
    public StringVariable variableTo;
    public UI_UpdateText texUpdater;

    public void Replace()
    {
        texUpdater.stringVariable = variableTo;
        texUpdater.UpdateFromStringVariable();
    }
}
