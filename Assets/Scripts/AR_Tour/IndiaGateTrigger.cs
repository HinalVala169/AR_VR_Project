using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
     


public class IndiaGateTrigger : MonoBehaviour
{
    public static event Action<bool> OnIndiaGateTrigger;

    [SerializeField] private bool cameraToCenterTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            OnIndiaGateTrigger?.Invoke(cameraToCenterTrigger);
        }
    }
}
