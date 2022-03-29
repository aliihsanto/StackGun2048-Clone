using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnEnable()
    {
        Invoke("Disable", 1f); 
    }
    void Disable()
    {
        gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        CancelInvoke();   
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("enemy"))
        {

            Disable();

        }
    }
}
