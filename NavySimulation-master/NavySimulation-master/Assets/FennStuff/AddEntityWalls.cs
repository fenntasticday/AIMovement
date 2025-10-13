using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddEntityWalls : MonoBehaviour
{
    public EntityMgr entityMgr;
    // public Entity entity;


    // Start is called before the first frame update
    void Start()
    {
        if (entityMgr != null)
            {
                Entity entityObj = gameObject.GetComponent<Entity>();
                entityObj.position = gameObject.transform.position;
                entityMgr.entities.Add(entityObj);
            }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
