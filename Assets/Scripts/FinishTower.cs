using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishTower : MonoBehaviour
{
    public GameObject Layer1;
    public GameObject Layer2;
    public GameObject parent;
    public int TowerHigh;
    private List<GameObject> towerList;
    private void Start()
    {
        TowerDesign();
    }
    public void TowerDesign()
    {
        Vector3 newposition= new Vector3(-0.25f,-15f,-12f);
        for(int i = 0; TowerHigh > i; i++)
        {
            GameObject gecici;
            if (i % 5==4)
            {
                gecici = Instantiate(Layer2, this.transform);

                gecici.transform.localPosition = newposition;
                newposition += new Vector3(0, 4.5f, 0);

            }
            else
            {

                gecici= Instantiate(Layer1,this.transform);
                gecici.GetComponent<TowerBlocks>().text.text = ((i+1) * 10).ToString();
                gecici.transform.localPosition=newposition;
                if ((i + 1) % 5 == 4)
                {
                    newposition += new Vector3(0, 4.5f, 0);

                }
                else
                {
                    newposition += new Vector3(0, 3f, 0);

                }
            }
        }
    }

}
