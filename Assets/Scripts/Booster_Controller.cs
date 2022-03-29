using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster_Controller : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> pooledCrashable;
    private int i = 0;
    public void Crashed(GameObject other)
    {
        pooledCrashable[i].transform.position = other.transform.position;
        pooledCrashable[i].SetActive(true);
        i++;
    }

}
