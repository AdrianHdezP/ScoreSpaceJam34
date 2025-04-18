using UnityEngine;

[System.Serializable]
public class Highscore
{
    public int score;
    public string username;
    public Highscore(int score, string name)
    {
        this.score = score;
        this.username = name;
    }
}
