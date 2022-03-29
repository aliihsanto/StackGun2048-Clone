using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LevelController : MonoBehaviour
{
    public static LevelController Current;
    public bool IsGameActive = false;
    public GameObject startMenu, gameMenu, gameOverMenu, finishMenu;
    public TextMeshProUGUI currentLevelText;
    int currentLevel;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Level" + (currentLevel +1).ToString());
        PlayerPrefs.SetInt("currentLevel", 0);

        Current = this;
        currentLevel = PlayerPrefs.GetInt("currentLevel");

        if (SceneManager.GetActiveScene().name != "Level" + currentLevel)
        {

            SceneManager.LoadScene("Level" + currentLevel.ToString());
        }
        else
        {
            currentLevelText.text = "Level"+(currentLevel + 1).ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartLevel()
    {
        PlayerController.currrent.StartGame(PlayerController.currrent.RunningSpeed);
        startMenu.SetActive(false);
        gameMenu.SetActive(true);
        PlayerController.currrent.character.GetComponent<Animator>().SetBool("running",true);
        IsGameActive = true;
    }
    public void RestartLevel()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void LoadNextLevel()
    {

        SceneManager.LoadScene("Level" + (currentLevel+1).ToString() );
    }
    public void GameOver()
    {
        gameMenu.SetActive(false);
        gameOverMenu.SetActive(true);
        PlayerController.currrent.character.GetComponent<Animator>().SetBool("running", false);

        IsGameActive = false;
    }
    public void FinishGame()
    {
        PlayerPrefs.SetInt("currentLevel", currentLevel + 1);
        PlayerController.currrent.character.GetComponent<Animator>().SetBool("running", false);
    
        gameMenu.SetActive(false);
        finishMenu.SetActive(true);
        IsGameActive = false; 
    }


}
