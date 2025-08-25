using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SessionsManager : MonoBehaviour
{
    public GameSettings gameSettings;
    public GameEvent onGameStart;
    public GameEvent onSceneLoadedAdditive;
    public GameEvent onSceneLoadedSingle;

    private void Awake()
    {
        gameSettings.r_manager.Init();
    }

    private void Start()
    {
        if (onGameStart != null)
            onGameStart.Raise();
    }

    public void LoadSceneAsyncAdditive(string lv1)
    {
        StartCoroutine(LoadSceneAsyncAdditive_Actual(lv1));
    }

    IEnumerator LoadSceneAsyncAdditive_Actual(string lv1)
    {
        yield return SceneManager.LoadSceneAsync(lv1, LoadSceneMode.Additive);
        if (onSceneLoadedAdditive != null)
        {
            onSceneLoadedAdditive.Raise();
            onSceneLoadedAdditive = null;
        }
    }

    public void LoadSceneAsyncSingle(string lv1)
    {
        StartCoroutine(LoadSceneAsyncSingle_Actual(lv1));
    }

    IEnumerator LoadSceneAsyncSingle_Actual(string lv1)
    {
        yield return SceneManager.LoadSceneAsync(lv1, LoadSceneMode.Single);
        if (onSceneLoadedSingle != null)
        {
            onSceneLoadedSingle.Raise();
            onSceneLoadedSingle = null;
        }
    }
}
