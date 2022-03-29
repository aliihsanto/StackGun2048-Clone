using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{
    public GameObject character;
    public float RunningSpeed;
    private float _currentRunningSpeed;
    public float limitX;
    public float xSpeed;
    
    public Transform fireposition;
    public Transform BoosterTransform;
    public Transform FinishPosition;
    
    public GameObject bullet;


    [HideInInspector]
    public static PlayerController currrent;
    [HideInInspector]
    public Transform FirstTransform;
    [HideInInspector]
    public Animator animator;
    [HideInInspector]
    public float waitingTime;
    [HideInInspector]
    public List<GameObject> PooledBoosters;

    private float time = 0.0f;
    private Ease animEase= Ease.InOutExpo;
    Sequence seq;
    private bool canMerge = false;
    private bool IsMergingNow = false;
    private int GunLevel = 1;





    // Start is called before the first frame update
    void Start()
    {
        currrent = this;
        FirstTransform = BoosterTransform;
        animator = character.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(LevelController.Current == null || !LevelController.Current.IsGameActive)
        {
            return;
        }
        float newX = 0;
        float touchXDelta = 0;
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            touchXDelta = Input.GetTouch(0).deltaPosition.x / Screen.width;
        }
        else if (Input.GetMouseButton(0))
        {
            touchXDelta = Input.GetAxis("Mouse X");

        }

        newX = transform.position.x - xSpeed * touchXDelta * Time.deltaTime;
        newX = Mathf.Clamp(newX, -limitX, limitX);

        Vector3 newPosition = new Vector3(newX, transform.position.y, transform.position.z - _currentRunningSpeed * Time.deltaTime);
        transform.position = newPosition;


        time += Time.deltaTime;

        if (time >= waitingTime )
        {
            time = time - waitingTime;
            fire();
        }

        if (canMerge)
        {
            Debug.Log("merge");
            Merge();
        }
        //Sort();




    }



    public void StartGame(float value)
    {
        _currentRunningSpeed = value;

    }
    private void fire()
    {
        if (GunLevel == 1)
        {
            GameObject obj = ObjectPool.current.GetPooledObject();
            obj.transform.position = fireposition.position ;
            obj.transform.rotation = fireposition.rotation;
            obj.SetActive(true);
            obj.GetComponent<Rigidbody>().velocity = -Vector3.forward * 100;
        }
        else if (GunLevel == 2)
        {
            GameObject obj = ObjectPool.current.GetPooledObject();
            obj.transform.position = fireposition.position;
            obj.transform.rotation = fireposition.rotation;
            obj.SetActive(true);
            obj.GetComponent<Rigidbody>().velocity = -Vector3.forward * 100;
            GameObject obj1 = ObjectPool.current.GetPooledObject();
            obj1.transform.position = fireposition.position + new Vector3(0.6f,0,0);
            obj1.transform.rotation = fireposition.rotation;
            obj1.SetActive(true);
            obj1.GetComponent<Rigidbody>().velocity = -Vector3.forward * 100;

        }
        else if (GunLevel == 3)
        {
            GameObject obj = ObjectPool.current.GetPooledObject();
            obj.transform.position = fireposition.position;
            obj.transform.rotation = fireposition.rotation;
            obj.SetActive(true);
            obj.GetComponent<Rigidbody>().velocity = -Vector3.forward * 100;
            GameObject obj1 = ObjectPool.current.GetPooledObject();
            obj1.transform.position = fireposition.position + new Vector3(0.6f, 0, 0);
            obj1.transform.rotation = fireposition.rotation;
            obj1.SetActive(true);
            obj1.GetComponent<Rigidbody>().velocity = -Vector3.forward * 100;
            GameObject obj2 = ObjectPool.current.GetPooledObject();
            obj2.transform.position = fireposition.position + new Vector3(-0.6f, 0, 0);
            obj2.transform.rotation = fireposition.rotation;
            obj2.SetActive(true);
            obj2.GetComponent<Rigidbody>().velocity = -Vector3.forward * 100;

        }
    }
    private void Merge()
    {
        canMerge = false;
        IsMergingNow = true;
        if (PooledBoosters.Count > 0)
        {
            for (int i = 0; PooledBoosters.Count > i; i++)
            {
                int temp_which = PooledBoosters[i].GetComponent<Booster>().WhichBoost;
                float temp_Boostcount = PooledBoosters[i].GetComponent<Booster>().BoostCount[temp_which];

                for (int j = 0; PooledBoosters.Count  > j; j++)
                {
                    int temp2_which = PooledBoosters[j].GetComponent<Booster>().WhichBoost;
                    float temp2_Boostcount = PooledBoosters[j].GetComponent<Booster>().BoostCount[temp2_which];
                    float temp_velocity = PooledBoosters[j].GetComponent<Booster>().BulletVelocity[temp_which + 1];


                    if (temp_Boostcount == temp2_Boostcount && PooledBoosters[i].name != PooledBoosters[j].name)
                    {
                        
                        BoosterID_update(i);
                        moveTo(j);
                        if (temp2_which != 10 || temp_which != 10)
                        {
                            StartCoroutine(MergeCorutine(1, temp_which, i));
                        }

                        if (waitingTime > temp_velocity)
                        {
                            waitingTime = temp_velocity;
                        }
                        
                        Destroy(PooledBoosters[j],1.02f);
                        PooledBoosters.Remove(PooledBoosters[j]);

                        Invoke("Sort", 1.4f);
                        Invoke("updateFirePos", 1.4f);
                        break;

                    }
                }

            }
        }
        else
        {
            Sort();

        }
        ChooseFastest();
    }

    public void CheckCanMerge()
    {
        if (PooledBoosters.Count > 0)
        {
            //Debug.Log(PooledBoosters.Count);
            for (int i = 0; PooledBoosters.Count-1 > i; i++)
            {
                    if(PooledBoosters[i].GetComponent<Booster>().WhichBoost == PooledBoosters[i+1].GetComponent<Booster>().WhichBoost)
                    {
                        canMerge = true;
                        break;
                    }
            }
        }
    }
    IEnumerator MergeCorutine(float time,int temp_which,int i)
    {
        yield return new WaitForSeconds(time);

        PooledBoosters[i].GetComponent<Booster>().WhichBoost += 1;
        PooledBoosters[i].GetComponent<Renderer>().material = PooledBoosters[i].GetComponent<Booster>().material[temp_which + 1];
        beBigger(i);
        Change_text();
        IsMergingNow = false;
        CheckCanMerge();
        Sort();

    }

    public void beBigger(int i)
    {
        DOTween.Sequence().Append(PooledBoosters[i].transform.DOScale(new Vector3(0.9f, 0.3f, 0.6f), .2f).SetEase(animEase))
            .Append(PooledBoosters[i].transform.DOScale(new Vector3(0.6f, 0.3f, 0.4f), .2f).SetEase(animEase));
    }
    public void moveTo(int k)
    {
        for (int i = PooledBoosters.Count; k < i; i--)
        {
            if (i - 1 != 0)
            {
                if (i - 1 == k)
                {
                    seq.Append(PooledBoosters[i - 1].transform.DOLocalMoveZ(PooledBoosters[i - 2].transform.localPosition.z, 1f)
                                            .SetEase(animEase)).OnComplete(() => Change_text());


                }
                else
                {
                    seq.Append(PooledBoosters[i - 1].transform.DOLocalMoveZ(PooledBoosters[i - 2].transform.localPosition.z, 1f)
                                            .SetEase(animEase));
                }
            }
        }

    }
    
    public void BoosterID_update(int k)
    {
        for ( int i=k+1 ; PooledBoosters.Count > i; i++)
        {
            PooledBoosters[i].GetComponent<Booster>().boosterID=i-1;
        }

    }
    public void ChooseFastest()
    {
        for (int i = 0; PooledBoosters.Count > i; i++)
        {
            int temp_which = PooledBoosters[i].GetComponent<Booster>().WhichBoost;
            float temp_velocity = PooledBoosters[i].GetComponent<Booster>().BulletVelocity[temp_which];

            for (int j = 0; PooledBoosters.Count - 1 > j; j++)
            {
                int temp2_which = PooledBoosters[j].GetComponent<Booster>().WhichBoost;
                float temp_velocity2 = PooledBoosters[j].GetComponent<Booster>().BulletVelocity[temp2_which]; ;

                if (temp_velocity < temp_velocity2 && temp_velocity < waitingTime)
                {
                    waitingTime = temp_velocity;
                }

            }
        }
        if (PooledBoosters.Count == 0)
        {
            waitingTime = 0.6f;
        }
    }
    public void DeleteFromList()
    {
        for (int i = 0; PooledBoosters.Count > i; i++)
        {
            if (PooledBoosters[i] == null)
            {
                PooledBoosters.Remove(PooledBoosters[i]);

                Sort();
            }
        }
    }
    public void Sort()
    {

        for (int i = 0; PooledBoosters.Count > i; i++)
        {

            for (int j = 0; PooledBoosters.Count > j; j++)
            {
                if (i != j)
                {

                    int temp_which = PooledBoosters[i].GetComponent<Booster>().WhichBoost;
                    int temp2_which = PooledBoosters[j].GetComponent<Booster>().WhichBoost;
                    bool temp_newOne = PooledBoosters[i].GetComponent<Booster>().newOne;
                    bool temp2_newOne = PooledBoosters[j].GetComponent<Booster>().newOne;


                    if (temp_which > temp2_which)
                    {
                        PooledBoosters[i].GetComponent<Booster>().WhichBoost = temp2_which;
                        PooledBoosters[j].GetComponent<Booster>().WhichBoost = temp_which;


                        if (temp2_newOne == true)
                        {
                            PooledBoosters[i].GetComponent<Booster>().newOne = true;
                            PooledBoosters[j].GetComponent<Booster>().newOne = false;
                            Debug.Log(PooledBoosters[i].name); 
                        }

                    }
                }
            }
        }

        for (int i = 0; PooledBoosters.Count > i; i++)
        {
            float whichone = PooledBoosters[i].GetComponent<Booster>().BoostCount[PooledBoosters[i].GetComponent<Booster>().WhichBoost];
            PooledBoosters[i].GetComponent<Booster>().text_up.text = "x" + whichone;


            PooledBoosters[i].GetComponent<Transform>().transform.localPosition = FirstTransform.localPosition + new Vector3(0, 0, -0.4f) * PooledBoosters[i].GetComponent<Booster>().boosterID;
        }
    }
    public void beCollected()
    {
        for (int i = 0; PooledBoosters.Count > i; i++)
        {
            if (PooledBoosters[i].GetComponent<Booster>().newOne == true)
            {
                PooledBoosters[i].transform.DOScale(new Vector3(0.6f, 0.3f, 0.4f), 0.25f).SetEase(animEase);
                PooledBoosters[i].transform.DOScale(new Vector3(0.6f, 0.3f, 0.4f), 0.25f).SetEase(animEase);
                PooledBoosters[i].GetComponent<Booster>().newOne = false;
            }
        }
    }
    public void Change_text()
    {
        for (int i = 0; PooledBoosters.Count > i; i++)
        {
            float whichone = PooledBoosters[i].GetComponent<Booster>().BoostCount[PooledBoosters[i].GetComponent<Booster>().WhichBoost];
            PooledBoosters[i].GetComponent<Booster>().text_up.text = "x" + whichone;

        }
    }
    public void AddtoList(Collision other)
    {
        other.gameObject.GetComponent<Booster>().boosterID = PooledBoosters.Count;
        other.transform.DOScale(new Vector3(0, 0, 0), 0);


        updateFirePos();

        int whichBoost = other.gameObject.GetComponent<Booster>().WhichBoost;
        float Temproryvelocity = other.gameObject.GetComponent<Booster>().BulletVelocity[whichBoost];

        updateRB(other);

        //text size ayarlanýr
        if (whichBoost > 8)
        {
            other.gameObject.GetComponent<Booster>().text_up.fontSize = 0.25f;

        }
        else if (whichBoost < 8)
        {
            other.gameObject.GetComponent<Booster>().text_up.fontSize = 0.3f;
        }

        PooledBoosters.Add(other.gameObject);

        other.transform.parent = currrent.transform;
        other.transform.localPosition = FirstTransform.localPosition;

        if (waitingTime >= Temproryvelocity)
        {
            waitingTime = Temproryvelocity;
        }
        Sort();

    }
    public void newOne(Collision other)
    {
        other.gameObject.GetComponent<Booster>().newOne = true;


        AddtoList(other);
        Sort();
        beCollected();
        CheckCanMerge();
    }
    public void updateFirePos()
    {
        if (PooledBoosters.Count > 0)
        {
            fireposition.localPosition = BoosterTransform.localPosition + ((PooledBoosters.Count-1) * new Vector3(0, 0, -0.4f)) + new Vector3(0, 0, -0.125f);
        }
        else
        {
            fireposition.localPosition = BoosterTransform.localPosition + new Vector3(0, 0, -0.25f);
        }

    }
    public void updateRB(Collision other)
    {
        other.gameObject.tag = "CollectedBooster";
        other.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
        other.gameObject.GetComponent<Rigidbody>().freezeRotation = true;

        other.gameObject.GetComponent<Rigidbody>().useGravity = false;

        other.gameObject.GetComponent<Booster>().text_front.text = "";
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Booster") )
        {

            newOne(other);
        }
        else if (other.gameObject.CompareTag("FinishLine"))
        {
            this.GetComponent<FinishLine>().FinishLevel();

        }


    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("BoostGun"))
        {
            GunLevel += 1;
        }
        else if (other.gameObject.CompareTag("DecreaseGun"))
        {
            if (GunLevel != 1)
            {
                GunLevel -= 1;
            }
        }

    }

}
