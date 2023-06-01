using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking.Types;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UIElements;


public class GameManager : MonoBehaviourPun
{
    /*public static GameManager Instance;
    public GameState State;
    public static event Action<GameState> OnGameStateChange;
    public GameObject EndScreen;
    public GameObject quitButton;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateGameState(GameState.CoinFlip);
    }

    public void UpdateGameState(GameState newState)
    {
        State = newState;

        switch (newState)
        {
            case GameState.CoinFlip:
                break;
            case GameState.firstPlayerTurn:
                HandleFirstTurn(); //starting player
                break;
            case GameState.firstSkillUse:
                break;
            case GameState.firstMovement:
                break;
            case GameState.firstCombat:
                break;
            case GameState.firstFinishMovement:
                break;
            case GameState.secondPlayerTurn:
                HandleSecondTurn(); //second player
                break;
            case GameState.secondSkillUse:
                break;
            case GameState.secondMovement:
                break;
            case GameState.secondCombat:
                break;
            case GameState.secondFinishMovement:
                break;
            case GameState.GameEnd:
                HandleGameEnd(); //win/death screen
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnGameStateChange?.Invoke(newState);
    }
    
    private async void HandleGameEnd()
    {
        await Task.Delay(500);
        EndScreen.SetActive(true);
    }
    private async void HandleFirstTurn()
    {
    
    }
    private async void HandleSecondTurn()
    {
    
    }
    public enum GameState
    {
        //Player 1 is "first" and player 2 is "second", all enums have these in beginning
        CoinFlip,
        firstPlayerTurn,
        firstSkillUse,
        firstMovement,
        firstCombat,
        firstFinishMovement,
        secondPlayerTurn,
        secondSkillUse,
        secondMovement,
        secondCombat,
        secondFinishMovement,
        GameEnd
    }
    public void QuitButton()
    {
        EndScreen.SetActive(true);
        quitButton.SetActive(false);
    }
    public void ExitGame()
    {
        StartCoroutine(DisconnectAllAndLoad());
    }
    IEnumerator DisconnectAllAndLoad()
    {
        PhotonNetwork.Disconnect();
        while (PhotonNetwork.IsConnected)
            yield return null;
        SceneManager.LoadScene("MainMenu");
    }*/

    //public GameObject EndScreen;
    public GameObject quitButton;
    
    public PlayerController playerOne; //left player
    public PlayerController playerTwo; //rightPlayer

    public PlayerController currentPlayer = null;

    public float postGameTime;

    
    
    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        
        //the master client will set the players
        if (PhotonNetwork.IsMasterClient)
            SetPlayers();
        
    }

    private void SetPlayers()
    {
        //set owners of both players photon views
        playerOne.photonView.TransferOwnership(1);
        playerTwo.photonView.TransferOwnership(2);
        
        //initialize the players
        playerOne.photonView.RPC("Initialize", RpcTarget.AllBuffered, 
            PhotonNetwork.CurrentRoom.GetPlayer(1));
        playerTwo.photonView.RPC("Initialize", RpcTarget.AllBuffered, 
            PhotonNetwork.CurrentRoom.GetPlayer(2));
        
        photonView.RPC("SetNextTurn", RpcTarget.AllBuffered);
        
    }

    [PunRPC]
    void SetNextTurn()
    {
        
        //first turn
        if (currentPlayer == null)
        {
            currentPlayer = playerOne;
        }
        
        //set next player turn
        else
            currentPlayer = currentPlayer == playerOne? playerTwo : playerOne;
        
        if (currentPlayer == PlayerController.me)
            PlayerController.me.BeginTurn();
        //
        else
        {
            PlayerController.me.BeginTurn();
        }
        
        
        //toggle end turn button
        GameUI.instance.ToggleEndTurnButton(currentPlayer == PlayerController.me);
    }

    public PlayerController GetOtherPlayer(PlayerController player)
    {
        return player == playerOne ? playerTwo : playerOne;
        
    }
    
    

    //
    public void WinCon()
    {
        photonView.RPC("WinGame", RpcTarget.All, PlayerController.enemy == playerOne? 0 : 1);
        
    }
    

    [PunRPC]
    void WinGame(int winner)
    {
        PlayerController playerWon = winner == 0 ? playerOne : playerTwo;
        PlayerController playerLost = winner == 0 ? playerTwo : playerOne;

        //EndScreen.SetActive(true);
        
        GameUI.instance.SetWinText(playerWon.photonPlayer.NickName, playerLost.photonPlayer.NickName);
        
        Invoke("GoBackToMenu", postGameTime);
    }

    void GoBackToMenu()
    {
        //PhotonNetwork.LeaveRoom();
        ExitGame();
        //SceneManager.LoadScene("MainMenu"); //here is problem
    }
    
    //
    
    //my salt
    public void QuitButton()
    {
        quitButton.SetActive(false);
        photonView.RPC("WinGame", RpcTarget.All, PlayerController.enemy == playerOne? 0 : 1);
        
    }
    
    public void ExitGame()
    {
        StartCoroutine(DisconnectAllAndLoad());
    }
    IEnumerator DisconnectAllAndLoad()
    {
        PhotonNetwork.Disconnect();
        while (PhotonNetwork.IsConnected)
            yield return null;
        SceneManager.LoadScene("MainMenu");
        
    }


}
