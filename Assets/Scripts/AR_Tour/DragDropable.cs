using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;


public class DragDropable : MonoBehaviour
{
    public static event Action<GameObject> OnDragStart;

    public static event Action OnObjectDestroyed;

    [SerializeField] private List<string> hitList;
    [SerializeField] private InputAction press, screenPos;

    private Vector3 curScreenPos;

    Camera camera;
    private bool isDragging;

    private Vector3 WorldPos
    {
        get
        {
            float z = camera.WorldToScreenPoint(transform.position).z;
            return camera.ScreenToWorldPoint(curScreenPos + new Vector3(0,0,z));
        }
    }

    private bool isCLickedOn
    {
        
        get
        {
            hitList.Clear();

            Ray ray = camera.ScreenPointToRay(curScreenPos);

            RaycastHit hit;
            if(Physics.Raycast(ray, out hit))
            {
                hitList.Add(hit.transform.gameObject.name);
                DestroyCollectible(hit.transform.gameObject);
                OnDragStart?.Invoke((hit.transform.gameObject));
                return hit.transform == transform;
            }
            return false;
        }
    }

    private void Awake()
    {
        camera = Camera.main;

        screenPos.Enable();
        press.Enable();

        screenPos.performed += context => { curScreenPos = context.ReadValue<Vector2>(); };
        press.performed += _ => { if(isCLickedOn) StartCoroutine(Drag()); };
        press.canceled += _  => { isDragging = false; };
    }

    private IEnumerator Drag()
    {
        isDragging = true;

        Vector3 offset = transform.position - WorldPos;

        //grab object
        while(isDragging)
        {
         //#   transform.position = WorldPos + offset; //to move with drag
            //draging
            yield return null;
        }

        //drop
    }

    //////////////////////////////////
    ///
    public void DestroyCollectible(GameObject obj)
    {
        hitList.Clear();
        if (this.gameObject != null)
        {
            if (obj.name == this.gameObject.name)
            {
                OnObjectDestroyed?.Invoke();
                
                Destroy(this.gameObject);
            }
        }
    }
}
