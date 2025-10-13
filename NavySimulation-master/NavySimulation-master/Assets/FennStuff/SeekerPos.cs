using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekerPos : MonoBehaviour
{
    public Camera mainCamera;
    public LayerMask selectableLayers;
    public bool setPoint = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, selectableLayers))
            {
                setPoint = true;
                gameObject.transform.position = raycastHit.point;
            }
        }
    }
}
