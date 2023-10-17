using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Ui : MonoBehaviour
{

    private TextMeshProUGUI text;

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();

    }

    private void Update()
    {
        text.text = Bomb.counttime.ToString("0.0") + "\n"+ "BOOM";
    }
}
