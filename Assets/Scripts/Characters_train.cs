using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using TMPro;



public class Characters_train : MonoBehaviour
{

    public GameObject leftArrowButton;
    public GameObject rightArrowButton;

    
    public Image playerAvatar;
    public Sprite[] avatars;
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI characterNameReUse;
    
    public Image playerAvatarPixel;
    public Sprite[] avatarsPixel;

    public GameObject roomInfo;
    public GameObject CharacterModel;

    public GameObject Characters;
    

    public int index = 0;
    public Toggle godToggle;



    public TextMeshProUGUI atkStat;
    public TextMeshProUGUI hpStat;
    public TextMeshProUGUI speedStat;
    public TextMeshProUGUI rangeStat;
    
    
    

    public void Start()
    {
        roomInfo.SetActive(false);
        hpStat.text = "HP : " + 120;
        atkStat.text = "ATK : " + 20;
        speedStat.text = "Speed : " + 5;
        rangeStat.text = "Range : " + 2;
        ApplyLocalChanges();
        onClickLeftArrow();
        onClickRightArrow();

    }


    public void ApplyLocalChanges()
    {
        leftArrowButton.SetActive(true);
        rightArrowButton.SetActive(true);
    }

    public void OnClickRoomInfo()
    {
        roomInfo.SetActive(true);
        leftArrowButton.SetActive(false);
        rightArrowButton.SetActive(false);
    }
    
    public void OnClickOutInfo()
    {
        roomInfo.SetActive(false);
        leftArrowButton.SetActive(true);
        rightArrowButton.SetActive(true);
    }



    public void onClickLeftArrow()
    {
        if (index== 0)
            index = avatars.Length - 1;
        else
            index--;

        playerAvatar.sprite = avatars[index];
        playerAvatarPixel.sprite = avatarsPixel[index];
        UpdatePlayerItem();
    }

    public void onClickRightArrow()
    {
        if (index== avatars.Length - 1)
            index = 0;
        else
            index++;

        playerAvatar.sprite = avatars[index];
        playerAvatarPixel.sprite = avatarsPixel[index];
        UpdatePlayerItem();
    }

    

    void UpdatePlayerItem()
    {
        string[] characterNames = {"Archangel","Assassin","Mage","Ent","Vulcan","SnowMan"};
        
        // Name of the chara --> HP ATK SPD RNG 
        var characterStat = new Dictionary<string, (int, int, int, int, float)>
        {
            { "Archangel", (120, 20, 5, 2,0.15f) },
            { "Assassin", (80, 30, 7, 2,0.15f) },
            { "Mage", (80, 35, 4, 5,0.15f) },
            { "Ent", (150, 20, 3, 3,0.45f) },
            { "Vulcan", (90, 30, 6, 3,0.4f) },
            { "SnowMan", (100, 25, 2, 2,0.5f) }
        };

        string charName = characterNames[index];
        characterName.text = characterNames[index];
        characterNameReUse.text = characterNames[index];
        (int hp, int atk, int spd, int rng, float scale)  = characterStat[charName];

        hpStat.text = "HP : " + hp;
        atkStat.text = "ATK : " + atk;
        speedStat.text = "Speed : " + spd;
        rangeStat.text = "Range : " + rng;
        
        Index.hp = hp;
        Index.atk = atk;
        Index.speed = spd;
        Index.range = rng;
        Index.scale = scale;

    }
    
    public void TrainToPlay()
    {
        Characters.SetActive(false);

        Index.indexX = index;

        if (godToggle.isOn)
        {
            Index.hp = 500;
            Index.atk = 50;
            Index.speed = 7;
            Index.range = 7;
        }

        Index.difficulty = 1;
        SceneManager.LoadScene("TrainMap");
    }
    
    public void GODMODEACTIVATE()
    {
        if (godToggle.isOn)
        {
            hpStat.text = "HP : " + 500;
            atkStat.text = "ATK : " + 50;
            speedStat.text = "Speed : " + 7;
            rangeStat.text = "Range : " + 7;
        }
        else
        {
            UpdatePlayerItem();
        }
    }
    
    
    public void TrainToPlayMedium()
    {
        Characters.SetActive(false);

        Index.indexX = index;

        if (godToggle.isOn)
        {
            Index.hp = 500;
            Index.atk = 50;
            Index.speed = 7;
            Index.range = 7;
        }

        Index.difficulty = 2;
        SceneManager.LoadScene("TrainMap 1");
    }
    public void TrainToPlayHard()
    {
        Characters.SetActive(false);

        Index.indexX = index;

        if (godToggle.isOn)
        {
            Index.hp = 500;
            Index.atk = 50;
            Index.speed = 7;
            Index.range = 7;
        }

        Index.difficulty = 3;
        SceneManager.LoadScene("TrainMap 2");
    }
}
