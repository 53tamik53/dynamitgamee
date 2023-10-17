using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;


public class Bomb : MonoBehaviour
{
    public static float counttime = 3;
    public GameObject currentObject;
    public GameObject previousObject;
   
    [SerializeField] private Transform Hands;
    [SerializeField] private GameObject WhatisBomb;

    private int Howmanyleft;
    [SerializeField] private GameObject WinnerPanel;

    private void Start()
    {

        Howmanyleft = 4;
        Time.timeScale = 1;
        WinnerPanel.SetActive(false);

        GameObject[] humans = GameObject.FindGameObjectsWithTag("human");

        currentObject = humans[Random.Range(0, 4)];
        previousObject = currentObject;
        WhatisBomb = this.gameObject;

        Hands = previousObject.transform.Find("kol").gameObject.transform;
        WhatisBomb.transform.parent = previousObject.transform;
        WhatisBomb.transform.position = Hands.position;
        currentObject.transform.tag = "bomberman";

    }
    private void Update()
    {
        if(Howmanyleft==1)
        {
            Time.timeScale = 0;
            winnerscript.winner = currentObject;
            WinnerPanel.SetActive(true);
        }


        counttime -= Time.deltaTime*0.25f;


        if(counttime<=0)
        {
            
            Hands = previousObject.transform.Find("kol").gameObject.transform;
            WhatisBomb.transform.parent = previousObject.transform;
            WhatisBomb.transform.position = Hands.position;
            previousObject.transform.tag = "bomberman";
           
            Howmanyleft--;
            currentObject.SetActive(false);

            currentObject = previousObject;
            counttime = 3;
           


            Debug.Log("Boom!");
        }
    }

}
