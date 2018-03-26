using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class QuestionMarkManager : MonoBehaviour
{

    public static QuestionMarkManager Instance { get; private set; }

    public Player Player { get; private set; }

    private List<QuestionsMenu> questionMarks;
    private int currentQuestionMarkIndex;


    public void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        questionMarks = FindObjectsOfType<QuestionsMenu>().OrderBy(t => t.transform.position.x).ToList();
        currentQuestionMarkIndex = questionMarks.Count > 0 ? 0 : -1;

        Player = FindObjectOfType<Player>();
    }

    public void Update()
    {

        var isAtLastQuestionMark = currentQuestionMarkIndex + 1 >= questionMarks.Count;
        if (isAtLastQuestionMark)
            return;

        var distanceToNextQuestionMark = questionMarks[currentQuestionMarkIndex + 1].transform.position.x - Player.transform.position.x;
        if (distanceToNextQuestionMark >= 0)
            return;

        questionMarks[currentQuestionMarkIndex].PlayerLeftQuestionMark();
        currentQuestionMarkIndex++;
        questionMarks[currentQuestionMarkIndex].PlayerHitQuestionMark();

    }
}



