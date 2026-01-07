using UnityEngine;
using System.Data; 
using Mono.Data.Sqlite; 
using System.IO;
using TMPro;
using System;
using System.Collections.Generic;

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager instance;
    private string dbPath;

    [Header("UI References")]
    public TMP_InputField nickInput;
    public TextMeshProUGUI[] leaderboardTexts; 
    public GameObject inputArea;

    private void Awake()
    {
        if (instance == null) instance = this;

        dbPath = "URI=file:" + Application.persistentDataPath + "/PlayerScores.db";
        CreateSchema();
    }

    private void CreateSchema()
    {
        using (var connection = new SqliteConnection(dbPath))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {

                command.CommandText = "CREATE TABLE IF NOT EXISTS Scores (id INTEGER PRIMARY KEY AUTOINCREMENT, name TEXT, score INTEGER, date TEXT);";
                command.ExecuteNonQuery();
            }
        }
    }

    public void SaveScore()
    {

        GameObject.Find("FinalScore_Display").GetComponent<TextMeshProUGUI>().text = "SKOR: " + GameManager.instance.score;
        string playerName = nickInput.text;
        if (string.IsNullOrEmpty(playerName)) playerName = "Adsiz_Kanka";

        int currentScore = GameManager.instance.score;
        string currentDate = DateTime.Now.ToString("dd.MM.yyyy");

        using (var connection = new SqliteConnection(dbPath))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO Scores (name, score, date) VALUES (@name, @score, @date);";
                command.Parameters.AddWithValue("@name", playerName);
                command.Parameters.AddWithValue("@score", currentScore);
                command.Parameters.AddWithValue("@date", currentDate);
                command.ExecuteNonQuery();
            }
        }


        if (inputArea != null) inputArea.SetActive(false);


        ShowTop10AndPlayerRank(playerName, currentScore);
    }

    public void ShowTop10AndPlayerRank(string pName, int pScore)
    {
        using (var connection = new SqliteConnection(dbPath))
        {
            connection.Open();


            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT name, score, date FROM Scores ORDER BY score DESC LIMIT 10;";
                using (IDataReader reader = command.ExecuteReader())
                {
                    int i = 0;
                    while (reader.Read() && i < 10)
                    {
                        leaderboardTexts[i].text = (i + 1) + ". " + reader.GetString(0) + " - " + reader.GetInt32(1) + " (" + reader.GetString(2) + ")";
                        i++;
                    }
                }
            }


            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT COUNT(*) + 1 FROM Scores WHERE score > @pScore;";
                command.Parameters.AddWithValue("@pScore", pScore);
                long rank = (long)command.ExecuteScalar();


                leaderboardTexts[10].text = rank + ". " + pName + " - " + pScore + " (BUGÜN)";
                leaderboardTexts[10].color = Color.yellow;
            }
        }
    }
}