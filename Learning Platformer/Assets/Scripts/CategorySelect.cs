using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CategorySelect : MonoBehaviour {

    public static string topicSelect;

    public void Capital()
    {
        topicSelect = "Capital";
        SceneManager.LoadScene("Level1");
    }

    public void Math()
    {
        topicSelect = "Math";
        SceneManager.LoadScene("Level1");
    }
}
