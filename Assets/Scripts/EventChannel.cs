using System;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public abstract class EventChannelBase : ScriptableObject
{
    internal abstract void ClearListeners();

    void OnDisable()
    {
        ClearListeners();
    }
}

public abstract class EventChannel<T> : EventChannelBase
{
    readonly List<Action<T>> listeners = new();

    public void Raise(T fact)
    {
        Action<T>[] snapshot = listeners.ToArray();
        for (int i = 0; i < snapshot.Length; i++)
            snapshot[i](fact);
    }

    public void Register(Action<T> listener)
    {
        if (listener == null)
            throw new ArgumentNullException(nameof(listener));

        if (listeners.Contains(listener))
            return;

        listeners.Add(listener);
    }

    public void Unregister(Action<T> listener)
    {
        if (listener == null)
            return;

        listeners.Remove(listener);
    }

    internal override void ClearListeners()
    {
        listeners.Clear();
    }
}

#if UNITY_EDITOR
[InitializeOnLoad]
static class EventChannelPlayModeReset
{
    static EventChannelPlayModeReset()
    {
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        if (state != PlayModeStateChange.ExitingPlayMode)
            return;

        EventChannelBase[] channels = Resources.FindObjectsOfTypeAll<EventChannelBase>();
        for (int i = 0; i < channels.Length; i++)
            channels[i].ClearListeners();
    }
}
#endif
