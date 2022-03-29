using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_Controller : MonoBehaviour
{
    public GameObject crashableBlock;
    public void Crashed()
    {
        crashableBlock.SetActive(true);
    }

}
