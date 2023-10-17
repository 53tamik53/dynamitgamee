using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class winnerscript : MonoBehaviour
{
    public TextMeshProUGUI whoiswinneR;
    public static GameObject winner;



    private void Update()
    {
        whoiswinneR.text = winner.name;
    }






    public void RetryTheGame()
    {
        SceneManager.LoadScene(0);  
    }
}
