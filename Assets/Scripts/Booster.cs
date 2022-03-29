using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Booster : MonoBehaviour
{
    public int WhichBoost;
    public List<float> BoostCount;
    public List<float> BulletVelocity;
    public List<Material> material;
    public TextMeshProUGUI text_up;
    public TextMeshProUGUI text_front;
    public bool newOne=false;
    public int boosterID;
    public Transform connectedNode;
    public GameObject crashableBoosterController;
    // Start is called before the first frame update
    void Start()
    {

        int temp = 1;
        for (int i = 1; i < 12; i++)
        {
            temp *= 2;
            BoostCount.Add(temp);
        }
        text_up.text = "x" + BoostCount[WhichBoost];
        text_front.text = "x" + BoostCount[WhichBoost];

        BulletVelocity.Add(0.5f);
        BulletVelocity.Add(0.25f);
        BulletVelocity.Add(0.166f);
        BulletVelocity.Add(0.125f);
        BulletVelocity.Add(0.1f);
        BulletVelocity.Add(0.083f);
        BulletVelocity.Add(0.0715f);
        BulletVelocity.Add(0.0625f);
        BulletVelocity.Add(0.055f);
        BulletVelocity.Add(0.05f);
        BulletVelocity.Add(0.041f);
        this.GetComponent<Renderer>().material = material[WhichBoost];

    }
    // Update is called once per frame
    void Update()
    {

        this.GetComponent<Renderer>().material = material[WhichBoost];

        if (WhichBoost > 8)
        {
            this.gameObject.GetComponent<Booster>().text_up.fontSize = 0.25f;
            this.gameObject.GetComponent<Booster>().text_front.fontSize = 0.25f;
            if (this.CompareTag("CollectedBooster"))
            {
                //Debug.Log(this.GetComponentInParent<PlayerController>().PooledBoosters.Count);
                if (boosterID != 0)
                {
                    int counter = this.GetComponentInParent<PlayerController>().PooledBoosters.Count;
                    for (int i = 0; counter > i; i++)
                    {
                        GameObject other = this.GetComponentInParent<PlayerController>().PooledBoosters[i];
                        if (boosterID - 1 == other.GetComponent<Booster>().boosterID)
                        {
                            connectedNode = other.transform;
                            float newX = (Mathf.Lerp(transform.localPosition.x, connectedNode.localPosition.x, Time.deltaTime * boosterID + 1 * 0.1f));
                            newX = Mathf.Clamp(newX, -0.3f, 0.3f);

                            transform.localPosition = new Vector3(newX,
                            transform.localPosition.y,
                            transform.localPosition.z
                            );
                        }
                    }
                }
                else if (boosterID == 0)
                {
                    connectedNode = transform.parent.transform;
                    float newX = (Mathf.Lerp(transform.localPosition.x, connectedNode.localPosition.x, Time.deltaTime * boosterID + 1 * 0.003f));
                    newX = Mathf.Clamp(newX, -0.05f, 0.05f);

                    transform.localPosition = new Vector3(newX,
                    transform.localPosition.y,
                    transform.localPosition.z
                    );
                }



            }

        }
        else
        {
            this.gameObject.GetComponent<Booster>().text_up.fontSize = 0.3f;
            this.gameObject.GetComponent<Booster>().text_front.fontSize = 0.3f;
            if (this.CompareTag("CollectedBooster"))
            {
                //Debug.Log(this.GetComponentInParent<PlayerController>().PooledBoosters.Count);
                if (boosterID != 0)
                {
                    int counter = this.GetComponentInParent<PlayerController>().PooledBoosters.Count;
                    for (int i = 0; counter > i; i++)
                    {
                        GameObject other = this.GetComponentInParent<PlayerController>().PooledBoosters[i];
                        if (boosterID - 1 == other.GetComponent<Booster>().boosterID)
                        {
                            connectedNode = other.transform;
                            float newX = (Mathf.Lerp(transform.localPosition.x, connectedNode.localPosition.x, Time.deltaTime * boosterID + 1 * 0.1f));
                            newX = Mathf.Clamp(newX, -0.3f, 0.3f);


                            transform.localPosition = new Vector3(newX,
                            transform.localPosition.y,
                            transform.localPosition.z
                            );
                        }
                    }
                }
                else if (boosterID==0)
                {
                    connectedNode = transform.parent.transform;
                    float newX = (Mathf.Lerp(transform.localPosition.x, connectedNode.localPosition.x, Time.deltaTime * boosterID + 1 * 0.003f));
                    newX = Mathf.Clamp(newX, -0.05f, 0.05f);


                    transform.localPosition = new Vector3(newX,
                    transform.localPosition.y,
                    transform.localPosition.z
                    );
                }



            }

        }



    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("enemy")&& this.CompareTag("CollectedBooster"))
        {

            crashableBoosterController.GetComponent<Booster_Controller>().Crashed(this.gameObject);
            Destroy(this.gameObject);

            this.GetComponentInParent<PlayerController>().BoosterID_update(boosterID);
            this.GetComponentInParent<PlayerController>().PooledBoosters.Remove(this.gameObject);
            this.GetComponentInParent<PlayerController>().Sort();
            this.GetComponentInParent<PlayerController>().updateFirePos();

        }

    }

    private void OnCollisionEnter(Collision other)
    {
        if (this.CompareTag("CollectedBooster"))
        {
            if (other.gameObject.CompareTag("Booster"))
            {
                this.GetComponentInParent<PlayerController>().newOne(other);
            }
        }
        else if (other.gameObject.CompareTag("BoostGun"))
        {
            //Physics.IgnoreCollision(other.gameObject.GetComponent<Collider>(), this.GetComponent<Collider>());
            Physics.IgnoreCollision(other.gameObject.GetComponent<Collider>(), this.GetComponent<Collider>(), true);

        }
        else if (other.gameObject.CompareTag( "DecreaseGun"))
        {
           // Physics.IgnoreCollision(other.gameObject.GetComponent<Collider>(), this.GetComponent<Collider>());
            Physics.IgnoreCollision(other.gameObject.GetComponent<Collider>(), this.GetComponent<Collider>(), true);

        }
        else if (this.CompareTag("FinishLine"))
        {
            LevelController.Current.FinishGame();

        }

    }
    
}


