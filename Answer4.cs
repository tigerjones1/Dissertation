using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Answer4 : MonoBehaviour {

    Text answer;

    List<string> fourthChoice = new List<string>()
    { "Brussels", "Milan", "Lahore", "Baghdad", "Trindade",
        "Madrid", "Moscow","Barcelona","Geneva","Birmingham",
        "Rabat","Caracas","Toronto","Kampala","Mbabane",
        "Tokyo","Pretoria","Victoria","Lima","Malé",

        "-13", "5","1","5","-6",
        "-21","-8","6","1","undefined",
        "-1","x > 3","x < 1","6","2",
        "x > -7","-9","-2","-36","-16" };

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
            answer.text = fourthChoice[QuestionScript.randQuestion];
        }
    }

    public void clicked()
    {
        //Debug.Log(gameObject.name);
        QuestionScript.selectedAnswer = gameObject.name;
        QuestionScript.choiceSelected = "y";

    }
}