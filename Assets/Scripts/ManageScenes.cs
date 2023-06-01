using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class ManageScenes : MonoBehaviour
{
    public GameObject Menu;
    public GameObject Settings;
    public GameObject Play;
    public GameObject Characters;


    private Characters_train PlayerInfo;

    public void Awake()
    {
        if (Index.firstLogin)
        {
            Index.HighScoreEasy = 0;
            Index.HighScoreMedium = 0;
            Index.HighScoreHard = 0;
        }

        Index.difficulty = 0;
    }

    public void MenuToSettings() //changes panels, from Menu to settings, others are self explanatory
    {
        Settings.SetActive(true);
        Menu.SetActive(false);
    }

    public void SettingsToMenu()
    {
        Menu.SetActive(true);
        Settings.SetActive(false);
    }
    
    public void MenuToPlay()
    {
        Play.SetActive(true);
        Menu.SetActive(false);
    }
    
    public void PlayToMenu()
    {
        Menu.SetActive(true);
        Play.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
    public void MenuToTrain()
    {
        Characters.SetActive(true);
        Menu.SetActive(false);
    }
    
    public void TrainToMenu()
    {
        Menu.SetActive(true);
        Characters.SetActive(false);
    }
    
    
    
}
