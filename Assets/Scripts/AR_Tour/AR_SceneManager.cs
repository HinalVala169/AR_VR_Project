using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System;

public class AR_SceneManager : MonoBehaviour
{
    public static event Action OnObjectSpawnerChildDestroyed;

    [Header("Elements to make AR")]
    [SerializeField] private ARPlaneManager planeManager; // in XR origin
    [SerializeField] private ARCameraManager cameraManager; // in main camera

    [Header("Model TO show")]
    [SerializeField] private GameObject model;
    [SerializeField] private GameObject btnPanle;

    [SerializeField] private GameObject objectSpawner; // XR object spawner

    [SerializeField] private bool isAnimatedView;

    private void Start()
    {
       //# InitializeScene();
    }

    private void InitializeScene()
    {
        // Initial setup
        ToggleARComponents();
        model.SetActive(isAnimatedView);
        btnPanle.SetActive(isAnimatedView);
    }

    public void ToggleAnimatedView()
    {
        isAnimatedView = !isAnimatedView;
        ToggleARComponents();
        model.SetActive(isAnimatedView);
        btnPanle.SetActive(isAnimatedView);

        if (isAnimatedView)
        {
            DestroyChildObjects();
        }

        // You can call any animation methods here if necessary
    }

    private void ToggleARComponents()
    {
        if (planeManager != null)
            planeManager.enabled = !isAnimatedView;

        if (cameraManager != null)
            cameraManager.enabled = !isAnimatedView;
    }

    private void DestroyChildObjects()
    {
        // Check if the objectSpawner has any children
        foreach (Transform child in objectSpawner.transform)
        {
            OnObjectSpawnerChildDestroyed?.Invoke();
            Destroy(child.gameObject);
        }
    }
}
