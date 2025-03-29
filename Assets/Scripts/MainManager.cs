using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick brickPrefab;
    public Rigidbody ball;

    public GameObject gameOverText;
    public Text scoreText;
    public Text playerNameText;
    public Text highScoreText;

    public Button exitButton;

    public int lineCount = 6;
    private int _mPoints;
    private int _mHighScore;

    private string _mPlayerName;
    private string _mHighScorePlayerName;

    private bool _mStarted;
    private bool _mGameOver;

    private void Start()
    {
        // Load player name and high score
        _mPlayerName = PlayerPrefs.GetString("PlayerName", "Player");
        _mHighScore = PlayerPrefs.GetInt("HighScore", 0);
        _mHighScorePlayerName = PlayerPrefs.GetString("HighScorePlayerName", "None");

        UpdatePlayerNameText();
        UpdateHighScoreText();

        const float step = 0.6f;
        var perLine = Mathf.FloorToInt(4.0f / step);

        var pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (var i = 0; i < lineCount; ++i)
        {
            for (var x = 0; x < perLine; ++x)
            {
                var position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(brickPrefab, position, Quaternion.identity);
                brick.pointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }

        exitButton.onClick.AddListener(ExitGame);
    }

    private void Update()
    {
        if (!_mStarted)
        {
            if (!Input.GetKeyDown(KeyCode.Space)) return;
            _mStarted = true;
            var randomDirection = Random.Range(-1.0f, 1.0f);
            var forceDir = new Vector3(randomDirection, 1, 0);
            forceDir.Normalize();

            ball.transform.SetParent(null);
            ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
        }
        else if (_mGameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    private void AddPoint(int point)
    {
        _mPoints += point;
        scoreText.text = $"Score: {_mPoints}";

        // Check for new high score
        if (_mPoints <= _mHighScore) return;
        _mHighScore = _mPoints;
        _mHighScorePlayerName = _mPlayerName;
        PlayerPrefs.SetInt("HighScore", _mHighScore);
        PlayerPrefs.SetString("HighScorePlayerName", _mHighScorePlayerName);
        UpdateHighScoreText();
    }

    private void UpdatePlayerNameText()
    {
        playerNameText.text = $"Player: {_mPlayerName}";
    }

    private void UpdateHighScoreText()
    {
        highScoreText.text = $"High Score: {_mHighScore} by {_mHighScorePlayerName}";
    }

    public void GameOver()
    {
        _mGameOver = true;
        gameOverText.SetActive(true);
    }

    private void ExitGame()
    {
        Debug.Log("Exit Game");

        // Save player name and high score
        PlayerPrefs.SetString("PlayerName", _mPlayerName);
        PlayerPrefs.SetInt("HighScore", _mHighScore);
        PlayerPrefs.SetString("HighScorePlayerName", _mHighScorePlayerName);
        PlayerPrefs.Save();

        // Exit the application
        Application.Quit();

        // If running in the editor, stop playing
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}