using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    [FormerlySerializedAs("BrickPrefab")] public Brick brickPrefab;
    [FormerlySerializedAs("LineCount")] public int lineCount = 6;
    [FormerlySerializedAs("Ball")] public Rigidbody ball;

    [FormerlySerializedAs("ScoreText")] public Text scoreText;
    [FormerlySerializedAs("GameOverText")] public GameObject gameOverText;
    public Text playerNameText;
    public Text highScoreText;
    
    private bool _mStarted;
    private int _mPoints;
    private string _mPlayerName;
    private int _mHighScore;
    private string _mHighScorePlayerName;
    
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
        
        var pointCountArray = new [] {1,1,2,2,5,5};
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
}