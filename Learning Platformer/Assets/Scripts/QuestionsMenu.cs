using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class QuestionsMenu : MonoBehaviour
{

    public static bool isPaused = false;
    public GameObject QuestionMenuUI;

    // Update is called once per frame
    void Update()
    {
      
       
    }

    public void PlayerHitQuestionMark()
    {
        QuestionMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void PlayerLeftQuestionMark()
    {

    }

    public void Resume()
    {
        QuestionMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void Pause()
    {
        QuestionMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }
}
