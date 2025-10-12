using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr : MonoBehaviour
{
    public static GameMgr inst;
    private GameInputs input;
    public int spawnCount;
    public int maxX, maxZ, minX, minZ;
    private void Awake()
    {
        inst = this;
        input = new GameInputs();
        input.Enable();
    }

    // Start is called before the first frame update
    void Start()
    {
        Vector3 position = Vector3.zero;
        for (int i = 0; i < spawnCount; i++)
        {
            int positionX = Random.Range(minX, maxX);
            int positionZ = Random.Range(minZ, maxZ);
            int randomEntity = Random.Range(0, 8);
            Entity ent = EntityMgr.inst.CreateEntity(EntityMgr.inst.entityPrefabs[0].GetComponent<Entity>().entityType, new Vector3(positionX, 0, positionZ), Vector3.zero);
            ent.isSelected = false;
        }
        // foreach (GameObject go in EntityMgr.inst.entityPrefabs)
        //     {
        //         Entity ent = EntityMgr.inst.CreateEntity(go.GetComponent<Entity>().entityType, position, Vector3.zero);
        //         ent.isSelected = false;
        //         position.x += 200;
        //     }
    }

    public Vector3 position;
    public float spread = 20;
    public float colNum = 10;
    public float initZ;
    // Update is called once per frame
    void Update()
    {
        if (input.Entities.Create100.triggered) {
            initZ = position.z;
            for (int i = 0; i < 10; i++) {
                for (int j = 0; j < 10; j++) {
                    Entity ent = EntityMgr.inst.CreateEntity(EntityType.PilotVessel, position, Vector3.zero);
                    position.z += spread;
                }
                position.x += spread;
                position.z = initZ;
            }
            DistanceMgr.inst.Initialize();
        }
    }
}
