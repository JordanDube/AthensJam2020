using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    Player playerScript;
    public GameObject player;
    public Image[] hearts;
    int startingLives;
    public GameObject restartButton;
    public Text scoreText;
    public Text finalScoreText;
    public GameObject finalScore;
    int score;
    public float scoreMultiplier = 0.25f;
    int currentLives;
    private void Awake()
    {
        playerScript = GameObject.Find("Jetpack").GetComponent<Player>();
        scoreText = GameObject.Find("Score").GetComponent<Text>();
        finalScoreText = GameObject.Find("FinalScore").GetComponent<Text>();
        startingLives = playerScript.lives;
    }

    private void Start()
    {
        finalScore.SetActive(false);
    }
    private void Update()
    {
        hearts[startingLives - playerScript.lives].enabled = false;
        if(playerScript.lives == 0)
        {
            restartButton.SetActive(true);
            finalScoreText.text = "" + score;
            finalScore.SetActive(true);
        }
        scoreText.text = "" + score;
        ComputeScore();
    }

    public void RestartButton()
    {
        SceneManager.LoadScene(0);
        
    }

    public void ComputeScore()
    {
        if(playerScript.lives > 0)
        {
            score += (int)(player.transform.position.x * scoreMultiplier);
        }
        
    }
}
