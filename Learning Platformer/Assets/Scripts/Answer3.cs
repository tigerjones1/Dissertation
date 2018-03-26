using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Answer3 : MonoBehaviour {

    Text answer;

    List<string> thirdChoice = new List<string>()
    { "Dubai", "Oslo", "Cape Town", "Tehran", "Dublin",
        "Brasilia", "Johannesburg","Suva","Manila","Gaza City",
        "Karachi","Nairobi","Auckland","Monte Carlo","Mumbai",
        "Rhine-Ruhr","São Paulo","St. George's","Sydney","Hamburg",

        "13", "-3","-10","0","4",
        "-2","8","3","7/8","-2",
        "1/2","x > 7","x < -1","-15","-11/6",
        "x < -7","-5","-8","24","-8"};

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (QuestionScript.randQuestion > -1)
        {
            answer = GetComponent<Text>();
            answer.text = thirdChoice[QuestionScript.randQuestion];
        }
    }

    public void clicked()
    {
        //Debug.Log(gameObject.name);
        QuestionScript.selectedAnswer = gameObject.name;
        QuestionScript.choiceSelected = "y";
    }
}
