using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : MonoBehaviour
{
    public Entity entity;
    // public PathfindingShips pfShips;
    // Start is called before the first frame update
    void Start()
    {
        entity = GetComponentInParent<Entity>();
    }

    // Update is called once per frame
    void Update()
    {
        if(entity != null)
            entity.selectionCircle.SetActive(entity.isSelected);
    }

    private void OnMouseDown()
    {
        //if (Input.GetMouseButtonDown(0)) {
        SelectionMgr.inst.SelectEntity(entity);
        // if (pfShips != null)
        // {
        //     pfShips.seeker = entity.transform;
        // }
        //}
    }


}
