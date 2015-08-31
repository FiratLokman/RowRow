using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    public GameObject[] BigObjects = new GameObject[8];
    public GameObject[] SmallObjects = new GameObject[8];
    public GameMaster GameMaster;

    private int RandomNumber;
    private GameObject SmallObject;

    public void spawnNext()
    {
        Vector2 v = Grid.roundVec2(transform.position);

        if (Grid.grid[(int)v.x, (int)v.y] != null)
        {
            GameMaster.GameOver = true;
        }
        else
        {
            GameObject BigObject = Instantiate(BigObjects[RandomNumber], transform.position, Quaternion.identity) as GameObject;
            BigObject.transform.parent = gameObject.transform;
            RandomNumber = Random.Range(0, 8);
            if (SmallObject != null) { Destroy(SmallObject); }
            SmallObject = Instantiate(SmallObjects[RandomNumber],new Vector3(900,1700,-100), Quaternion.identity) as GameObject;
            SmallObject.transform.parent = gameObject.transform;
        }
    }

    void Start()
    {
        RandomNumber = Random.Range(0, 8);
    }
}