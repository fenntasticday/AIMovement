using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalPos : MonoBehaviour
{
    public Camera mainCamera;
    public LayerMask selectableLayers;
    public PathfindingShips pfShips;
    public PathfindingOfficeEntity pfOfficeE;
    // public LayerMask layerMask;
    // Start is called before the first frame update
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, selectableLayers))
            {
                gameObject.transform.position = raycastHit.point;
                if (pfShips != null)
                { pfShips.target = gameObject.transform; }
                if (pfOfficeE != null)
                { pfOfficeE.target = gameObject.transform; }
            }
            // Vector3 mousePos = Input.mousePosition;
            // {
            //     gameObject.transform.position = mousePos;
            // }
        }
    }
}
