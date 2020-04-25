using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    Player playerScript;

    public Image[] hearts;
    int startingLives;
    public GameObject restartButton;
    private void Awake()
    {
        playerScript = GameObject.Find("Jetpack").GetComponent<Player>();

        startingLives = playerScript.lives;
    }

    private void Update()
    {
        hearts[startingLives - playerScript.lives].enabled = false;
        if(playerScript.lives == 0)
        {
            restartButton.SetActive(true);
        }
    }

    public void RestartButton()
    {
        SceneManager.LoadScene(0);
    }
}
