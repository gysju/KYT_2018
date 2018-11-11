using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DLH_Highscores : MonoBehaviour {

    private const string _privateCode = "4KsxPie520iF-lT9kNPbOwWOhJIXBm-E6YmMkx1BZoCg";
    private const string _publicCode = "5be58b284cc83c0d546d8afe";
    private const string _webURL = "http://dreamlo.com/lb/";

    public Highscore[] highscores;

    private void Awake()
    {
        AddScore("dav", 50, 0, GameManager.buildVersion);
        AddScore("marie", 80, 1, GameManager.buildVersion);
        AddScore("gr", 11, 0, GameManager.buildVersion);
        AddScore("gr2", 12, 0, GameManager.buildVersion);
        AddScore("gr3", 120, 0, GameManager.buildVersion);
        AddScore("gr4", 140, 0, GameManager.buildVersion);
        AddScore("gr5", 110, 0, GameManager.buildVersion);

        GetScores();
    }

    public void AddScore(string username, int score, int levelId, string gameVersion)
    {
        StartCoroutine(UploadNewHighscore(username, score, levelId, gameVersion));
    }

    private IEnumerator UploadNewHighscore(string username, int score, int levelId, string gameVersion)
    {
        WWW www = new WWW(_webURL + _privateCode + "/add/" + WWW.EscapeURL(username) + "/" + score + "/" + levelId + "/" + gameVersion);
        yield return www;

        if (string.IsNullOrEmpty(www.error))
            Debug.Log("upload score success");
        else
        {
            Debug.Log("upload score error : " + www.error);
        }
    }

    public void GetScores()
    {
        StartCoroutine("DownloadNewHighscoreFromDB");
    }

    private IEnumerator DownloadNewHighscoreFromDB()
    {
        WWW www = new WWW(_webURL + _publicCode + "/xml-seconds-score-asc/");
        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {
            FormatHighscores(www.text);
        }
        else
        {
            Debug.Log("download score error : " + www.error);
        }
    }

    private void FormatHighscores(string textStream)
    {
        Debug.Log(textStream);
        string[] entries = textStream.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        highscores = new Highscore[entries.Length];
        for (int i = 0; i < entries.Length; i++)
        {
            string[] entryInfo = entries[i].Split('|');
            string username = entryInfo[0];
            int score = int.Parse(entryInfo[1]);
            highscores[i] = new Highscore(username, score);
            Debug.Log(highscores[i].username + ": " + highscores[i].score);
        }
    }

    public struct Highscore
    {
        public string username;
        public int score;

        public Highscore(string username, int score)
        {
            this.username = username;
            this.score = score;
        }
    }
}