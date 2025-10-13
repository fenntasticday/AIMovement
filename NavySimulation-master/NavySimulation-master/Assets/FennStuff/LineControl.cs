using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineControl : MonoBehaviour
{
    LineRenderer lR;
    //public bool inactive = false;

    [HideInInspector]
    public Transform[] waypoints;
    // Start is called before the first frame update
    void Start()
    {
        lR = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // drawLine();
    }
    public void drawLine()
    {
        lR.positionCount = waypoints.Length;
        for (int i = 0; i < waypoints.Length; i++)
        {
            lR.SetPosition(i, waypoints[i].position);
        }
    }

}
