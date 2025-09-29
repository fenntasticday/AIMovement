using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalPos : MonoBehaviour
{
    public Camera mainCamera;
    // public LayerMask layerMask;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit))
            {
                gameObject.transform.position = raycastHit.point;
            }
            // Vector3 mousePos = Input.mousePosition;
            // {
            //     gameObject.transform.position = mousePos;
            // }
        }
    }
}
