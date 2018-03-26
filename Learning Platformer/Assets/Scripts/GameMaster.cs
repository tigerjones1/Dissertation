using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameMaster {

    private static GameMaster instance;
    public static GameMaster Instance { get { return instance ?? (instance = new GameMaster()); } }

    public int Points { get; private set; }

    private GameMaster()
    {

    }

    public void Reset()
    {
        Points = 0;
    }

    public void ResetPoints(int points)
    {
        Points = points;
    }

    public void AddPoints(int pointsToAdd)
    {
        Points += pointsToAdd;
    }
}
