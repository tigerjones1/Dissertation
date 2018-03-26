using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Answer1 : MonoBehaviour {

    Text answer;

    List<string> firstChoice = new List<string>()
    { "Berlin", "Stockholm", "Athens", "Islamabad", "Warsaw",
        "Bucharest", "Bangkok","Hanoi","Kabul","Kingston",
        "Tirana","Buenos Aires","Vienna","Rome","Hamilton",
        "Gaborone","George Town","Santiago","Kiev","Seoul",

        "-3", "-3","-14","5/17","4/3",
        "-7","54","-3","-3/8","0",
        "3/4","x < 7","x > 1","21","-3/2",
        "x < 7","-14","15","-5","3"};

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (QuestionScript.randQuestion > -1)
        {
            answer = GetComponent<Text>();
            answer.text = firstChoice[QuestionScript.randQuestion];
        }
    }

    public void clicked()
    {
        //Debug.Log(gameObject.name);
        QuestionScript.selectedAnswer = gameObject.name;
        QuestionScript.choiceSelected = "y";
    }
}
