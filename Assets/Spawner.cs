using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    private List<Object> AllObjects = new List<Object>();
    public GameObject[] BigObjects = new GameObject[7];
    public GameObject[] SmallObjects = new GameObject[7];

    private int RandomNumber;
    private GameObject SmallObject;

    public void spawnNext()
    {
        if (SmallObject != null) { Destroy(SmallObject); }
        var randomObject = Instantiate(BigObjects[RandomNumber], transform.position, Quaternion.identity);
        AllObjects.Add(randomObject);

        RandomNumber = Random.Range(0, 7);
        SmallObject = Instantiate(SmallObjects[RandomNumber]);
    }

    void Start()
    {
        RandomNumber = Random.Range(0, 7);
    }
}