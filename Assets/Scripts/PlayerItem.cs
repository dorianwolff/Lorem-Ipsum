using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.PlayerLoop;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerItem : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI playerName;
    
    public GameObject leftArrowButton;
    public GameObject rightArrowButton;

    private ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();
    public Image playerAvatar;
    public Sprite[] avatars;
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI characterNameReUse;
    
    public Image playerAvatarPixel;
    public Sprite[] avatarsPixel;

    public GameObject roomInfo;
    public GameObject CharacterModel;

    private Player player;

    public TextMeshProUGUI atkStat;
    public TextMeshProUGUI hpStat;
    public TextMeshProUGUI speedStat;
    public TextMeshProUGUI rangeStat;

    /*public void Start()
    {
        PhotonNetwork.SetPlayerCustomProperties(playerProperties);
    }*/

    public void SetPlayerInfo(Player _player)
    {
        playerName.text = _player.NickName;
        player = _player;
        UpdatePlayerItem(player);
    }

    public void ApplyLocalChanges()
    {
        leftArrowButton.SetActive(true);
        rightArrowButton.SetActive(true);
    }

    public void OnClickedRoomInfo()
    {
        roomInfo.SetActive(true);
        CharacterModel.SetActive(false);
    }

    public void OnClickedBackInfo()
    {
        CharacterModel.SetActive(true);
        roomInfo.SetActive(false);
    }

    public void onClickLeftArrow()
    {
        if ((int)playerProperties["playerAvatar"] == 0)
        {
            playerProperties["playerAvatar"] = avatars.Length - 1;
            
            playerProperties["playerAvatarPixel"] = avatarsPixel.Length - 1;
            
        }
        else
        {
            playerProperties["playerAvatar"] = (int)playerProperties["playerAvatar"] - 1;
            
            playerProperties["playerAvatarPixel"] = (int)playerProperties["playerAvatarPixel"] - 1;
        }
        
        PhotonNetwork.SetPlayerCustomProperties(playerProperties);
    }

    public void onClickRightArrow()
    {
        if ((int)playerProperties["playerAvatar"] == avatars.Length - 1)
        {
            playerProperties["playerAvatar"] = 0;
            
            playerProperties["playerAvatarPixel"] = 0;
        }
        else
        {
            playerProperties["playerAvatar"] = (int)playerProperties["playerAvatar"] + 1;
            
            playerProperties["playerAvatarPixel"] = (int)playerProperties["playerAvatarPixel"] + 1;
        }

        PhotonNetwork.SetPlayerCustomProperties(playerProperties);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer,
        ExitGames.Client.Photon.Hashtable targetPlayerProperties)
    {
        if (player == targetPlayer)
        {
            UpdatePlayerItem(targetPlayer);
        }
    }

    void UpdatePlayerItem(Player player)
    {
        List<string> characterNames = new List<string>();
        characterNames.Add("Archangel");
        characterNames.Add("Assassin");
        characterNames.Add("Battle Mage");
        characterNames.Add("Golem");
        characterNames.Add("Ent");
        characterNames.Add("Vulcan");
        characterNames.Add("Snow Man");

        List<string> characterHP = new List<string>();
        characterHP.Add("HP : 120");
        characterHP.Add("HP : 80");
        characterHP.Add("HP : 80");
        characterHP.Add("HP : 150");
        characterHP.Add("HP : 150");
        characterHP.Add("HP : 90");
        characterHP.Add("HP : 100");
        
        List<string> characterATK = new List<string>();
        characterATK.Add(" ATK : 20");
        characterATK.Add(" ATK : 30");
        characterATK.Add(" ATK : 35");
        characterATK.Add(" ATK : 50");
        characterATK.Add(" ATK : 20");
        characterATK.Add(" ATK : 30");
        characterATK.Add(" ATK : 25");
        
        List<string> characterSpeed = new List<string>();
        characterSpeed.Add("Speed : 5");
        characterSpeed.Add("Speed : 7");
        characterSpeed.Add("Speed : 4");
        characterSpeed.Add("Speed : 1");
        characterSpeed.Add("Speed : 3");
        characterSpeed.Add("Speed : 6");
        characterSpeed.Add("Speed : 2");

        List<string> characterRange = new List<string>();
        characterRange.Add("Range : 2");
        characterRange.Add("Range : 2");
        characterRange.Add("Range : 5");
        characterRange.Add("Range : 1");
        characterRange.Add("Range : 3");
        characterRange.Add("Range : 3");
        characterRange.Add("Range : 2");
        
        
        if (player.CustomProperties.ContainsKey("playerAvatar"))
        {
            playerAvatar.sprite = avatars[(int)player.CustomProperties["playerAvatar"]];
            playerProperties["playerAvatar"] = (int)player.CustomProperties["playerAvatar"];
            
            playerAvatarPixel.sprite = avatarsPixel[(int)player.CustomProperties["playerAvatarPixel"]];
            playerProperties["playerAvatarPixel"] = (int)player.CustomProperties["playerAvatarPixel"];
            
            characterName.text = characterNames[(int)playerProperties["playerAvatar"]];
            characterNameReUse.text = characterNames[(int)playerProperties["playerAvatar"]];
            
            hpStat.text = characterHP[(int)playerProperties["playerAvatar"]];
            atkStat.text = characterATK[(int)playerProperties["playerAvatar"]];
            speedStat.text = characterSpeed[(int)playerProperties["playerAvatar"]];
            rangeStat.text = characterRange[(int)playerProperties["playerAvatar"]];
        }
        else
        {
            playerProperties["playerAvatar"] = 0;
            
            playerProperties["playerAvatarPixel"] = 0;
            
            characterName.text = characterNames[(int)playerProperties["playerAvatar"]];
            characterNameReUse.text = characterNames[(int)playerProperties["playerAvatar"]];
            
            hpStat.text = characterHP[(int)playerProperties["playerAvatar"]];
            atkStat.text = characterATK[(int)playerProperties["playerAvatar"]];
            speedStat.text = characterSpeed[(int)playerProperties["playerAvatar"]];
            rangeStat.text = characterRange[(int)playerProperties["playerAvatar"]];
        }
    }
}
