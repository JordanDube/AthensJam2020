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
    public GameObject endScreen;
    public GameObject startScreen;
    int score;
    public float scoreMultiplier = 0.25f;
    int currentLives;
    private void Awake()
    {
        endScreen.SetActive(false);
        playerScript = GameObject.Find("Player").GetComponent<Player>();
        scoreText = GameObject.Find("Score").GetComponent<Text>();
        //finalScoreText = GameObject.Find("FinalScore").GetComponent<Text>();
        startingLives = playerScript.lives;
    }

    private void Start()
    {
        
        startScreen.SetActive(true);
    }
    private void Update()
    {
        hearts[startingLives - playerScript.lives].enabled = false;
        if(playerScript.lives == 0)
        {
            endScreen.SetActive(true);
            finalScore.GetComponent<Text>().text = "" + score;
            scoreText.enabled = false;
        }
        if(playerScript.startedMoving)
        {
            startScreen.SetActive(false);
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
