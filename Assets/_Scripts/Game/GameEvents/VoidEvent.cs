using NaughtyAttributes;
using System;
using UnityEngine;

[Serializable]
public class FailMessagePrinter
{
    [ResizableTextArea]
    [SerializeField] private string _warningLog;

    public void PrintFailMessage()
    {
        Debug.LogWarning(_warningLog);
    }
}

[CreateAssetMenu(menuName = "Game/VoidEvent", fileName = "VoidEvent")]
public class VoidEvent : ScriptableObject
{
    public event Action OnEventTrigger;

    [Label("Enable missing listener warning")]
    [SerializeField] private bool _isEventListenerMissingNotified;
    [ShowIf("_isEventListenerMissingNotified")]
    [SerializeField] FailMessagePrinter _failMessagePrinter;

    [Button]
    public void RequestRaiseEvent()
    {
        if (OnEventTrigger != null)
        {
            OnEventTrigger.Invoke();
        }
        else if (_isEventListenerMissingNotified)
        {
            _failMessagePrinter.PrintFailMessage();
        }
    }
}
