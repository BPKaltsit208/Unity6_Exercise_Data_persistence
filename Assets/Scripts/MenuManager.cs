using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public InputField playerNameInput;
    public Button startButton;
    public Button exitButton;
    public Button resetButton;
    public Text highScoreText;

    private void Start()
    {
        // Load and display the high score when the menu starts
        UpdateHighScoreDisplay();
        
        // Assign the button click event
        startButton.onClick.AddListener(StartGame);
        resetButton.onClick.AddListener(ResetHighScore);
        exitButton.onClick.AddListener(ExitGame);
    }

    public void StartGame()
    {
        // Save the entered player name
        var playerName = playerNameInput.text;
        PlayerPrefs.SetString("PlayerName", playerName);
        
        // Load the main game scene
        SceneManager.LoadScene("main");
    }

    private void UpdateHighScoreDisplay()
    {
        // Retrieve saved high score data
        var highScore = PlayerPrefs.GetInt("HighScore", 0);
        var highScorePlayer = PlayerPrefs.GetString("HighScorePlayerName", "None");
        
        // Update the UI Text
        highScoreText.text = $"Best Score: {highScore} by {highScorePlayer}";
    }
    
    private void ResetHighScore()
    {
        PlayerPrefs.DeleteKey("HighScore");
        PlayerPrefs.DeleteKey("HighScorePlayerName");
        UpdateHighScoreDisplay(); // Refresh UI
    }
    
    private void ExitGame()
    {
        Debug.Log("Exit Game");
        
        // Save player name and high score
        PlayerPrefs.Save();
        
        // Exit the application
        Application.Quit();
        
        // If running in the editor, stop playing
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}