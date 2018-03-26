using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    public static LevelManager Instance { get; private set; }

    public Player Player { get; private set; }
    public CameraController Camera { get; private set; }
    public TimeSpan RunningTime { get { return DateTime.UtcNow - started; } }

    public int CurrentTimeBonus
    {
        get
        {
            var secondDifference = (int)(BonusCutOffSeconds - RunningTime.TotalSeconds);
            return Mathf.Max(0, secondDifference) * BonusSecondsMultiplier;
        }
    }

    private List<Checkpoint> checkpoints;
    private int currentCheckpointIndex;
    private DateTime started;
    private int savedPoints;
    public static List<int> questionsAnswered = new List<int>() { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
    public static List<int> previousQuestions = new List<int>() { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };

    public Checkpoint DebugSpawn;
    public int BonusCutOffSeconds;
    public int BonusSecondsMultiplier;
    public int placeHolder = -1;

    public void Awake()
    {
        List<int> questionsAnswered = new List<int>(previousQuestions);
           
        savedPoints = GameMaster.Instance.Points;
        Instance = this;
    }

    public void Start()
    {
        
        checkpoints = FindObjectsOfType<Checkpoint>().OrderBy(t => t.transform.position.x).ToList();
        currentCheckpointIndex = checkpoints.Count > 0 ? 0 : -1;

        Player = FindObjectOfType<Player>();
        Camera = FindObjectOfType<CameraController>();

        started = DateTime.UtcNow;

#if UNITY_EDITOR
        if (DebugSpawn != null)
            DebugSpawn.SpawnPlayer(Player);
        else if (currentCheckpointIndex != -1)
            checkpoints[currentCheckpointIndex].SpawnPlayer(Player);
#else
        if(currentCheckpointIndex != -1)
        checkpoints[currentCheckpointIndex].SpawnPlayer(Player);
#endif
        }

    public void Update()
    {
        var isAtLastCheckpoint = currentCheckpointIndex + 1 >= checkpoints.Count;
        if (isAtLastCheckpoint)
            return;

        var distanceToNextCheckpoint = checkpoints[currentCheckpointIndex + 1].transform.position.x - Player.transform.position.x;
        if (distanceToNextCheckpoint >= 0)
            return;

        checkpoints[currentCheckpointIndex].PlayerLeftCheckpoint();
        currentCheckpointIndex++;
        checkpoints[currentCheckpointIndex].PlayerHitCheckpoint();

        GameMaster.Instance.AddPoints(CurrentTimeBonus);
        savedPoints = GameMaster.Instance.Points;
        started = DateTime.UtcNow;

        if(placeHolder == -1)
        {
            List<int> previousQuestions = new List<int>(questionsAnswered);
            placeHolder = 0;
        }
    }

    public void GoToNextLevel(string levelName)
    {
        StartCoroutine(GoToNextLevelCo(levelName));
    }

    private IEnumerator GoToNextLevelCo(string levelName)
    {
        Player.FinishLevel();
        FloatingText.Show(string.Format("{0} points!", GameMaster.Instance.Points), "CheckpointText", new CenteredTextPositioner(0.25f));
        yield return new WaitForSeconds(2f);

        if (string.IsNullOrEmpty(levelName))
            SceneManager.LoadScene("MainMenu");
        else
            SceneManager.LoadScene(levelName);
    }

    public void KillPlayer()
    {
        StartCoroutine(KillPlayerCo());
    }
    
    private IEnumerator KillPlayerCo()
    {
        Player.Kill();
        Camera.IsFollowing = false;
        yield return new WaitForSeconds(2f);

        Camera.IsFollowing = true;

        if (currentCheckpointIndex != -1)
            checkpoints[currentCheckpointIndex].SpawnPlayer(Player);

        started = DateTime.UtcNow;
        GameMaster.Instance.ResetPoints(savedPoints);
    }
}
