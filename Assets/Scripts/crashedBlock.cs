using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crashedBlock : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("fade", 1.2f);
    }
    public void fade()
    {
        //this.GetComponent<RayFire.RayfireRecorder>().StartPlay();

        this.GetComponent<RayFire.RayfireRigid>().Fade();

    }


}
