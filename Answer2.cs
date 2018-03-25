using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Answer2 : MonoBehaviour {

    Text answer;

    List<string> secondChoice = new List<string>()
    { "Havana", "Paris", "Cardiff", "Berlin", "Victoria",
        "Jakarta","Jerusalem","Tokyo","Seoul","Beirut",
        "Valletta","Amsterdam","Ottawa","Doha","Cairo",
        "Bern","Lisbon","Dhaka","Kampala","Sana'a",

        "3", "-3/13","-7","-5","6",
        "2","42","-25/3","15","3",
        "2","x < 3","x > -1","15","-3",
        "x > 7","14","-15","5","-3"};

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (QuestionScript.randQuestion > -1)
        {
            answer = GetComponent<Text>();
            answer.text = secondChoice[QuestionScript.randQuestion];
        }
    }

    public void clicked()
    {
        //Debug.Log(gameObject.name);
        QuestionScript.selectedAnswer = gameObject.name;
        QuestionScript.choiceSelected = "y";
    }
}
