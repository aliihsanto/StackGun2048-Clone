using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool current;
    public GameObject pooledObject;

    public int pooledAmount;
    public bool canGrow;
    private List<GameObject> pooledObjects;


    // Start is called before the first frame update
    private void Awake()
    {
        current = this;

    }
    void Start()
    {
        pooledObjects = new List<GameObject>();

        for(int i = 0; i < pooledAmount; i++)
        {
            GameObject obj = Instantiate(pooledObject);
            obj.GetComponent<Rigidbody>().velocity = -Vector3.forward * 100;

            obj.SetActive(false);

            pooledObjects.Add(obj);

        }

    }
    public GameObject GetPooledObject()
    {
        for(int i = 0; i < pooledObjects.Count; i++)
        {
            if(!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
           
        }

        GameObject obj = Instantiate(pooledObject);
        obj.GetComponent<Rigidbody>().velocity = -Vector3.forward * 100;

        pooledObjects.Add(obj);
        return obj;
    }

}
