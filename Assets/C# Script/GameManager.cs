using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("UI Canvas")]
    public Canvas gameOverCanvas;
    public Canvas pauseCanvas;
    public Canvas levelCompleteCanvas;
    public Canvas level3FinalCanvas;

    [Header("Score Texts")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverScoreText;
    public TextMeshProUGUI levelCompleteScoreText;

    [Header("Game State")]
    public int score = 0;
    public int currentHealth = 3;
    public List<string> collectedItems = new List<string>();

    #region UNITY LIFECYCLE

    private void Awake()
    {

        Time.timeScale = 1f;

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Time.timeScale = 1f;
        EnsureEventSystem();
        SetupUI();
        AutoAssignButtons();
    }

    #endregion

    #region UI SETUP

    public void SetupUI()
    {

        Canvas[] canvases = Object.FindObjectsByType<Canvas>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (Canvas c in canvases)
        {
            if (c.name == "GameOver_Canvas") gameOverCanvas = c;
            else if (c.name == "Pause_Canvas") pauseCanvas = c;
            else if (c.name == "LevelComplete_Canvas") levelCompleteCanvas = c;
            else if (c.name == "Level3Final_Canvas") level3FinalCanvas = c;
        }

        TextMeshProUGUI[] allTexts = Object.FindObjectsByType<TextMeshProUGUI>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var t in allTexts)
        {
            if (t.name == "ScoreText") scoreText = t;
            else if (t.name == "GameOver_Text") gameOverScoreText = t;
            else if (t.name == "FinalScore_Text") levelCompleteScoreText = t;
        }

        gameOverCanvas?.gameObject.SetActive(false);
        pauseCanvas?.gameObject.SetActive(false);
        levelCompleteCanvas?.gameObject.SetActive(false);

        if (scoreText != null) scoreText.text = score.ToString();
    }

    private void AutoAssignButtons()
    {
        Button[] allButtons = Object.FindObjectsByType<Button>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (Button btn in allButtons)
        {
            btn.onClick.RemoveAllListeners();
            string bName = btn.name.ToLower();

            if (bName.Contains("restart")) btn.onClick.AddListener(RestartLevel);
            else if (bName.Contains("nextlevel")) btn.onClick.AddListener(NextLevel);
            else if (bName.Contains("continue") || bName.Contains("resume")) btn.onClick.AddListener(ResumeGame);
            else if (bName.Contains("exit") || bName.Contains("quit")) btn.onClick.AddListener(ExitGame);
        }
    }

    private void EnsureEventSystem()
    {
        if (Object.FindFirstObjectByType<EventSystem>() == null)
        {
            GameObject es = new GameObject("EventSystem");
            es.AddComponent<EventSystem>();
            es.AddComponent<StandaloneInputModule>();
        }
    }

    #endregion

    #region ACTIONS

    public void AddScore(int amount)
    {
        score += amount;
        if (scoreText != null) scoreText.text = score.ToString();
    }

    public void RespawnPlayer() { OpenGameOverScreen(); }

    public void StartPlayerInvincibility(float duration) { StartCoroutine(InvincibilityCoroutine(duration)); }

    private IEnumerator InvincibilityCoroutine(float duration)
    {
        yield return new WaitForSeconds(duration);
        HealthManager hm = Object.FindFirstObjectByType<HealthManager>();
        hm?.EndInvincibility();
    }

    public void OpenGameOverScreen()
    {
        Time.timeScale = 0f;
        if (gameOverCanvas != null)
        {
            gameOverCanvas.gameObject.SetActive(true);

            gameOverScoreText = gameOverCanvas.transform.Find("FinalScore_Text")?.GetComponent<TextMeshProUGUI>();

            if (gameOverScoreText != null)
            {
                gameOverScoreText.text = "SCORE : " + score;
            }
        }
    }

    public void OpenLevelCompleteScreen()
    {
        Time.timeScale = 0f;

        if (SceneManager.GetActiveScene().name == "Level3" && level3FinalCanvas != null)
        {
            level3FinalCanvas.gameObject.SetActive(true);
        }
        else if (levelCompleteCanvas != null)
        {
            levelCompleteCanvas.gameObject.SetActive(true);

            levelCompleteScoreText = levelCompleteCanvas.transform.Find("FinalScore_Text")?.GetComponent<TextMeshProUGUI>();

            if (levelCompleteScoreText != null)
            {
                levelCompleteScoreText.text = "SCORE : " + score;
            }
        }
    }

    public void TogglePause()
    {
        if (Time.timeScale > 0f) { Time.timeScale = 0f; pauseCanvas?.gameObject.SetActive(true); }
        else ResumeGame();
    }

    public void ResumeGame() { Time.timeScale = 1f; pauseCanvas?.gameObject.SetActive(false); }

    public void RestartLevel()
    {
        currentHealth = 3;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel() { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); }

    public void ExitGame()
    {
        Debug.Log("Çýkýþ yapýlýyor...");
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    #endregion
}