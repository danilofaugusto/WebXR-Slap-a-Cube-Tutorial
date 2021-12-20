using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonEvent : MonoBehaviour
{
    public UnityEvent OnPress;

    private void OnTriggerEnter(Collider other)
    {
        OnPress.Invoke();
    }
}
