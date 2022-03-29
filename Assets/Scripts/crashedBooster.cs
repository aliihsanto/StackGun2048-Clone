using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crashedBooster : MonoBehaviour
{
    void Start()
    {
        Invoke("fade", 1.2f);
    }
    public void fade()
    {
        this.GetComponent<RayFire.RayfireRigid>().Fade();

    }
}
