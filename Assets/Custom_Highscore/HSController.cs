using System;
using System.Collections;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class HSController : MonoBehaviour
{
    private const string addScoreURL = "http://davidmestdagh.com/highscores/blood_pressure/addscore.php?"; //be sure to add a ? to your url
    private const string highscoreURL = "http://davidmestdagh.com/highscores/blood_pressure/display.php?";

    public static HSController inst;

    private void Awake()
    {
        if (inst == null)
            inst = this;
    }

    public void GetScores(Action<Highscore[]> action)
    {
        StartCoroutine(C_GetScores(action, GameManager.inst.GetLevelIndex()));
    }
    public void GetScores(Action<Highscore[]> action, int levelIndex, int limit = 3, string buildVersion = null)
    {
        StartCoroutine(C_GetScores(action, levelIndex, limit, buildVersion));
    }

    public void PostScores(string name, int score)
    {
        StartCoroutine(C_PostScores(name.Replace(';', ','), score, GameManager.inst.GetLevelIndex(), GameManager.buildVersion));
    }

    // remember to use StartCoroutine when calling this function!
    private static IEnumerator C_PostScores(string name, int score, int levelIndex, string buildVersion)
    {
        //This connects to a server side php script that will add the name and score to a MySQL DB.
        // Supply it with a string representing the players name and the players score.
        string hash = Emcryption.CalculateMD5Hash(name + score + levelIndex + buildVersion);

        string post_url = addScoreURL + "name=" + WWW.EscapeURL(name) + "&score=" + score + "&level_index=" + levelIndex + "&build_version=" + buildVersion + "&hash=" + hash;
        Debug.Log(post_url);
        // Post the URL to the site and create a download object to get the result.
        WWW hs_post = new WWW(post_url);
        yield return hs_post; // Wait until the download is done

        if (hs_post.error != null)
        {
            Debug.Log("There was an error posting the high score: " + hs_post.error);
        }
    }

    // Get the scores from the MySQL DB to display in a GUIText.
    // remember to use StartCoroutine when calling this function!
    private static IEnumerator C_GetScores(Action<Highscore[]> action, int levelIndex, int limit = 3, string buildVersion = null)
    {
        print("Loading Scores");
        string url = highscoreURL + "&level_index=" + levelIndex + "&limit=" + limit;
        if (!string.IsNullOrEmpty(buildVersion)) url += "&buildVersion=" + buildVersion;
        WWW hs_get = new WWW(url);
        yield return hs_get;

        if (hs_get.error != null)
        {
            Debug.Log("There was an error getting the high score: " + hs_get.error);
        }
        else
        {
            Debug.Log(hs_get.text); // this is a GUIText that will display the scores in game.

            Highscore[] highscores = ConvertStringToHighscores(hs_get.text);
            action(highscores);
        }
    }

    private static Highscore[] ConvertStringToHighscores(string data)
    {
        string[] entries = data.Split('\n');
        Highscore[] highscores = new Highscore[entries.Length];
        for (int i = 0; i < entries.Length; i++)
        {
            string[] entry = entries[i].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            if (entry.Length < 4) continue;

            string name = entry[0];
            int score = int.Parse(entry[1]);
            int levelIndex = int.Parse(entry[2]);
            string buildVersion = entry[3];

            highscores[i].SetData(name, score, levelIndex, buildVersion);
        }

        return highscores;
    }

    private class Emcryption
    {
        private const string _secretKey = "12reyfjd67T78Hju54d01ztkjFPOKIDSG54E564HQ54dfDFs54wh4d340ehs4";

        public static string CalculateMD5Hash(string input)
        {
            input += _secretKey;

            // step 1, calculate MD5 hash from input
            MD5 md5 = System.Security.Cryptography.MD5.Create();

            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);

            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }

            return sb.ToString();
        }
    }

    public struct Highscore
    {
        public string name;
        public int score;
        public int levelIndex;
        public string buildVersion;

        public Highscore(string name, int score, int levelIndex, string buildVersion)
        {
            this.name = name;
            this.score = score;
            this.levelIndex = levelIndex;
            this.buildVersion = buildVersion;
        }

        public void SetData(string name, int score, int levelIndex, string buildVersion)
        {
            this.name = name;
            this.score = score;
            this.levelIndex = levelIndex;
            this.buildVersion = buildVersion;
        }
    }
}
