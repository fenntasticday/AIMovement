using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.XR;

public class EnvironmentSetup : MonoBehaviour
{
    public GameObject square;
    public GameObject cylinder;
    public GameObject parent;
    public bool isSquare = false;
    public int generationPoint = 0;
    public int generationAmount;
    public EntityMgr entityMgr;
    public EntityMgr secondSituation;
    public DistanceMgr distanceMgr;
    public bool isPF = false;
    public GameObject movableEntitiesRoot;
    public GameObject todestroy;
    public LineMgr lineMgr;

    public  Vector3 HoldPos = new Vector3(1000, 0, 0);

    public Grid aGrid;
    public GameMgr gameMgr;

    public GameObject seeker;
    public GameObject goal;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("c"))
        {
            if(isPF == true)
            {
                Debug.Log("This is working");
                //entityMgr.entities.Clear();
                //foreach (Entity ent in movableEntitiesRoot.GetComponentsInChildren<Entity>())
                //{
                //    entityMgr.entities.Add(ent);
                //}
                if (entityMgr != null)
                {
                    Debug.Log("This is working");
                    entityMgr.entities.Clear();
                    foreach (var line in lineMgr.lines)
                    {
                        Destroy(line.gameObject);
                    }

                    lineMgr.lines.Clear();


                    foreach (Transform child in todestroy.transform)
                    {
                        Destroy(child.gameObject);
                    }
                    gameMgr.reinstate();
                }
            }

            if (isPF != true)
            {
                if (secondSituation != null)
                {
                    Debug.Log("This is working");
                    secondSituation.entities.Clear();
                    foreach (var line in lineMgr.lines)
                    {
                        Destroy(line.gameObject);
                    }

                    lineMgr.lines.Clear();
                

                    foreach (Transform child in todestroy.transform)
                    {
                        Destroy(child.gameObject);
                    }
                    gameMgr.reinstate();
                }

            }

            RandomizeBoard();
        }
    }

    public void RandomizeBoard()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("toDes");
        if (gameObjects != null)
        {
            foreach (GameObject go in gameObjects)
            {
                Destroy(go);
            }

        }

        GameObject newObj = new GameObject("declared");


        for (int i = 0; i < generationAmount; i++)
        {
            int xPoint = Random.Range(0, 625);
            int yPoint = Random.Range(0, 350);
            int isNeg = Random.Range(0, 4);
            //GameObject newObj = new GameObject("declared");
            if (isNeg == 0)
            {

                if (isSquare)
                    newObj = Instantiate(square, new Vector3(xPoint, 0, yPoint), Quaternion.identity);
                if (isSquare == false)
                    newObj = Instantiate(cylinder, new Vector3(xPoint, 0, yPoint), Quaternion.identity);
            }

            if (isNeg == 1)
            {
                if (isSquare)
                    newObj = Instantiate(square, new Vector3(-xPoint, 0, yPoint), Quaternion.identity);
                if (isSquare == false)
                    newObj = Instantiate(cylinder, new Vector3(-xPoint, 0, yPoint), Quaternion.identity);
            }

            if (isNeg == 2)
            {
                if (isSquare)
                    newObj = Instantiate(square, new Vector3(xPoint, 0, -yPoint), Quaternion.identity);
                if (isSquare == false)
                    newObj = Instantiate(cylinder, new Vector3(xPoint, 0, -yPoint), Quaternion.identity);
            }

            if (isNeg == 3)
            {
                // if (yPoint >= 300)
                // {
                //     yPoint = Random.Range(0, 300);
                // }
                if (isSquare)
                    newObj = Instantiate(square, new Vector3(-xPoint, 0, -yPoint), Quaternion.identity);
                if (isSquare == false)
                    newObj = Instantiate(cylinder, new Vector3(-xPoint, 0, -yPoint), Quaternion.identity);
            }

            newObj.gameObject.tag = "toDes";
            if (entityMgr != null)
            {
                Entity entityObj = newObj.GetComponent<Entity>();
                entityObj.position = newObj.transform.position;
                entityMgr.entities.Add(entityObj);
            }
            //GameObject des = GameObject.Find("declared");
            //Destroy(des);
        }

        GameObject des = GameObject.Find("declared");
        Destroy(des);
        //int counter = generationAmount;
        //while   (entityMgr.entities.Count > 1)      
        //    {
        //    entityMgr.entities.RemoveAt(counter);
        //    counter--;
        //    Debug.Log("Current");
        //}

        StartCoroutine(DelayedGridCreation());
        if(distanceMgr!= null)
        DistanceMgr.inst.Initialize();


        //GameObject[] gameObjects2 = GameObject.FindGameObjectsWithTag("toDes");
        //if (gameObjects != null)
        //{
        //    foreach (GameObject go in gameObjects)
        //    {
        //        Destroy(go);
        //    }

        //}
        //aGrid.CreateGrid();
    }
    //aGrid.CreateGrid();

    IEnumerator DelayedGridCreation()
    {
        // Wait one frame to let Unity register the colliders
        yield return null;

        // Now recreate the grid
        aGrid.CreateGrid();
        // distanceMgr.Initialize();
    }
}
