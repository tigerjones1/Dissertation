using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class QuestionScript : MonoBehaviour {

    public GameObject QuestionMark;
    public AudioClip CorrectAnswerSound;

    Text question;

    public static int randQuestion = -1;

    List<string> questions = new List<string>()
    { "What is the capital city of Belgium", "What is the capital city of Sweden", "What is the capital city of Wales",
    "What is the capital city of Iraq" ,"What is the capital city of Ireland" ,"What is the capital city of Brazil" ,
    "What is the capital city of Russia" ,"What is the capital city of Vietnam" ,"What is the capital city of Fiji" ,
    "What is the capital city of Jamaica" ,"What is the capital city of Morocco" ,"What is the capital city of Kenya" ,
    "What is the capital city of Canada" ,"What is the capital city of Italy" ,"What is the capital city of Egypt" ,
    "What is the capital city of Japan" ,"What is the capital city of Portugal" ,"What is the capital city of Bangladesh" ,
    "What is the capital city of Ukraine" ,"What is the capital city of South Korea",
    "x + 8 = -5", "7x - 1 = 6x - 4","7(x - 3) = 2 + 6(x - 5)","9x - 5 = 8x","5x - 30 + 3x = 10 + 3x -20",
    "7x = -14","-6x = 48","8 - 3x = 17","11x - 5 = 3x + 2","8x - 5 = 6x - 5",
    "6(2x + 1) - 5x = 3x + 2","x - 2 >5","3 - x < 4","18 is 3 less than a number. What is the number?","3 - (2x + 5) = 4",
    "7 - 2x < 21","(-7)(-2)","(3)(-5)","30/-6","-12/-4"};

    List<string> correctAnswer = new List<string>()
    {   "Answer4", "Answer1", "Answer2", "Answer4", "Answer3",
        "Answer3", "Answer4", "Answer1", "Answer3", "Answer1",
        "Answer4", "Answer3", "Answer2", "Answer1", "Answer2",
        "Answer4", "Answer2", "Answer2", "Answer1", "Answer1",

        "Answer4", "Answer1", "Answer2", "Answer4", "Answer3",
        "Answer3", "Answer4", "Answer1", "Answer3", "Answer1",
        "Answer4", "Answer3", "Answer2", "Answer1", "Answer2",
        "Answer4", "Answer2", "Answer2", "Answer1", "Answer1",};

    //public static List<int> previousQuestions = new List<int>() {-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1};
    public int questionNumber = 0;

    public static string selectedAnswer;

    public static string choiceSelected = "n";

    public int correctAnswerPoints = 20;
    public int incorrectAnswerMinusPoints = 5;

    public int topicMod = 0;

    void Start()
    {
        if(CategorySelect.topicSelect == "Capital")
        {
            topicMod = 0;
        }

        if(CategorySelect.topicSelect == "Math")
        {
            topicMod = 20;
        }
    }

	// Update is called once per frame
	void Update () {
	   if(randQuestion == -1)
        {
            randQuestion = Random.Range(0 + topicMod, 20 + topicMod);
            for(int i = 0; i < 22; i++)
            {
                if(randQuestion != LevelManager.previousQuestions[i])
                {

                }else
                {
                    randQuestion = -1;
                }
            }
        }
       if(randQuestion > -1)
        {
            question = GetComponent<Text>();
            LevelManager.previousQuestions[questionNumber] = randQuestion;
            question.text = questions[randQuestion];
            
        }

        if (choiceSelected == "y")
        {
            choiceSelected = "n";
            questionNumber += 1;

            if (correctAnswer[randQuestion] == selectedAnswer)
            {
                Debug.Log("Correct!!" + "  " + randQuestion);
                QuestionMark.GetComponent<QuestionsMenu>().Resume();
                GameMaster.Instance.AddPoints(correctAnswerPoints);
                //FloatingText.Show(string.Format("+{0}", correctAnswerPoints), "CorrectAnswerText", new FromWorldPointTextPositioner(Camera.main, transform.position, 1.5f, 50));
                //SoundManager.Instance.PlayClip3D(CorrectAnswerSound, transform.position);
                randQuestion = -1;
            }
            else
            {
                Debug.Log("Incorrect!!" + "  " + randQuestion);
                QuestionMark.GetComponent<QuestionsMenu>().Resume();
                GameMaster.Instance.AddPoints(-5);
                randQuestion = -1;
            }
        }
    }
}
