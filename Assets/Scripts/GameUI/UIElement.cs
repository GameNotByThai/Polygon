using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIElement : MonoBehaviour
{
    public UIUpdater updater;

    private void Awake()
    {
        updater = UIUpdater.singleton;

        if (updater != null)
            updater.elements.Add(this);
    }

    public virtual void Init()
    {

    }

    public virtual void Tick(float dt)
    {

    }
}
