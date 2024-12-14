using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndiaGateCameraTransition : MonoBehaviour
{
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private List<Vector3> mainCameraPositions;

    private int currentCameraPositionIndex = 0;

    private void OnEnable()
    {
        IndiaGateTrigger.OnIndiaGateTrigger += IndiaGateTrigger_OnIndiaGateTrigger;
    }

    private void OnDisable()
    {
        IndiaGateTrigger.OnIndiaGateTrigger -= IndiaGateTrigger_OnIndiaGateTrigger;
    }

    private void IndiaGateTrigger_OnIndiaGateTrigger(bool obj)
    {
        if (obj)
        {
            // If true, move to the next position in the list
            currentCameraPositionIndex = (currentCameraPositionIndex + 1) % mainCameraPositions.Count;
        }
        else
        {
            // If false, move to the previous position in the list
            currentCameraPositionIndex = (currentCameraPositionIndex - 1 + mainCameraPositions.Count) % mainCameraPositions.Count;
        }

        // Set the camera position to the current index in the list
        mainCamera.transform.position = mainCameraPositions[currentCameraPositionIndex];
    }
}
