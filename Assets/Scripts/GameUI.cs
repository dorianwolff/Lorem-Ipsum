using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class GameUI : MonoBehaviour
{
    public Button endTurnButton;
    public TextMeshProUGUI unitInfoText;
    public TextMeshProUGUI winnerText;
    public TextMeshProUGUI loserText;
    public GameObject endScreen;
    
    //instance

    public static GameUI instance;

    private void Awake()
    {
        instance = this;
    }

    public void OnEndTurnButton()
    {
        PlayerController.me.EndTurn();
    }

    public void ToggleEndTurnButton(bool toggle)
    {
        endTurnButton.interactable = toggle;
        unitInfoText.gameObject.SetActive(toggle);
        
    }

    public void SetUnitInfoText(Unit unit)
    {
        unitInfoText.text = "";
        unitInfoText.text += "<b>Unit Info Text :</b>\n";
        unitInfoText.text += "ATK : "+unit.unitDamage+"\n";
        unitInfoText.text += "HP : "+unit.currentHp+"/"+unit.maxHp+"\n";
        unitInfoText.text += "Speed : "+unit.maxMoveDistance+"\n";
        unitInfoText.text += "ATK Range : "+unit.maxAttackRange;
        
    }

    //
    public void SetWinText(string winnerName, string loserName)
    {
        
        winnerText.text = winnerName + " won";
        loserText.text = loserName + " lost";
        endScreen.SetActive(true);
    }
    
    
}
