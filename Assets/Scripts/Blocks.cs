using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class Blocks : MonoBehaviour
{
    public TextMeshProUGUI text;
    public int Block_Count;
    public GameObject booster=null;
    public Ease animEase;
    public SpriteRenderer sr;
    private Material currentColor;
    public Material shootedColor;
    private Sequence seq;

    // Start is called before the first frame update
    void Start()
    {
        currentColor = this.GetComponent<MeshRenderer>().material;
        seq = DOTween.Sequence();
        text.text = Block_Count.ToString();
        if (booster != null)
        {
            booster.GetComponent<Rigidbody>().useGravity = false;
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("bullet") )
        {
            Block_Count -= 1;
            text.text = Block_Count.ToString();
            transform.DORewind();
            seq.Append(this.transform.DOPunchScale(new Vector3(0.1f, 0.4f, 0), .25f).SetEase(animEase))
                .Join(currentColor.DOColor(shootedColor.color,0.01f))
                .Append(currentColor.DOColor(currentColor.color, 0.25f));

            if (Block_Count == 0)
            {
                Destroy(this.gameObject);
                this.GetComponentInParent<Block_Controller>().Crashed();
            }

        }
        else if (other.CompareTag("character"))
        {
            other.GetComponent<PlayerController>().character.GetComponent<Animator>().SetTrigger("dead");
            LevelController.Current.GameOver();
        }

    }
}
