using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;


public class FinishLine : MonoBehaviour
{
    [HideInInspector]
    public PlayerController Pcontroller;
    [HideInInspector]
    public LevelController LevelController;
    public Camera camera;
    private GameObject character;
    private Ease animEase = Ease.InOutExpo;
    public float IncreaseY;

    void Start()
    {
       Pcontroller= this.GetComponent<PlayerController>();
       LevelController = this.GetComponent<LevelController>();
        character = Pcontroller.character;
    }

    void Update()
    {
        
    }
    public void FinishLevel()
    {
        //Pcontroller.RunningSpeed = 0;
        //Pcontroller.xSpeed = 0;
        LevelController.Current.IsGameActive = false;
        Pcontroller.character.GetComponent<Rigidbody>().useGravity = false;
        for(int i = 0; i < Pcontroller.PooledBoosters.Count; i++)
        {
            int gecici = (int)Pcontroller.PooledBoosters[i].GetComponent<Booster>().WhichBoost;
            IncreaseY += Mathf.Pow(2,gecici);
        }
        MoveToTop();
        
    }
    public void MoveToTop()
    {
        DOTween.Sequence().Append(this.transform.DOMoveZ(this.transform.position.z - 15f, 1))
            .Join(this.transform.DOMoveX(0, 1).OnComplete(Complated))
            .Append(Pcontroller.transform.DOMoveY(this.transform.position.y + IncreaseY, 3))
            .Join(character.transform.DORotate(new Vector3(0, 0, 0), 2))
            .Join(camera.transform.DOLocalMove(new Vector3(0,0.5f,5), 1.5f))
            .Join(camera.transform.DORotate(new Vector3(15,180,0),1))
            ;
            
    }
    public void Complated()
    {
        Pcontroller.animator.SetBool("running", false);
        Pcontroller.animator.SetTrigger("finish");
        Invoke("FinishMenu", 3);
    }
    public void FinishMenu()
    {
        LevelController.Current.FinishGame();
    }
}
