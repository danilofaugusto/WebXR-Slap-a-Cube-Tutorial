using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WhacButton : MonoBehaviour
{
    [SerializeField] Material _normalMaterial;
    [SerializeField] Material _activeMaterial;
    [SerializeField] MeshRenderer _meshRenderer;
    public UnityEvent OnHit;
    
    bool _active = false;

    private void OnTriggerEnter(Collider other)
    {
        
        if (_active)
        {
            Deactivate();
            OnHit.Invoke();
        }
    }

    public void Activate()
    {
        _meshRenderer.material = _activeMaterial;
        _active = true;
    }

    public void Deactivate()
    {
        _meshRenderer.material = _normalMaterial;
        _active = false;
    }

}
