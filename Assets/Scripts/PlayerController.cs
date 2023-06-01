using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.Demo.Cockpit;
using Photon.Realtime;
using UnityEngine.UI;

public class PlayerController : MonoBehaviourPun
{
    public Player photonPlayer;
    public string[] playerToSpawn; //tutos units to spawn
    public Transform[] spawnPoints;

    public List<Unit> units = new List<Unit>();
    private Unit selectedUnit;

    public static PlayerController me;
    public static PlayerController enemy;
    
    public GameObject[] playerPrefabs;

    [PunRPC]
    void Initialize(Player player)
    {
        photonPlayer = player;
        if (player.IsLocal)
        {
            me = this;
            SpawnUnits();
        }
        else
        {
            enemy = this;
        }
        //set the player ui text
    }

    void SpawnUnits()
    {
        //Transform spawnPoint = spawnPoints[0];
        //GameObject playerToSpawn = playerPrefabs[(int)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"]];
        int index = (int)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"];
        //PhotonNetwork.Instantiate(playerToSpawn.name, spawnPoint.position, Quaternion.identity);
        GameObject unit = PhotonNetwork.Instantiate(playerToSpawn[index], 
            spawnPoints[0].position, Quaternion.identity);
        unit.GetPhotonView().RPC("Initialize", RpcTarget.Others, false);
        unit.GetPhotonView().RPC("Initialize", photonPlayer, true);
    }

    private void Update() //uses main cam
    {
        
        if (!photonView.IsMine)
        {
            return;
        }

        //where i want to add ultimate move
        
        if (Input.GetMouseButtonDown(0) &&  GameManager.instance.currentPlayer == this)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
            //I had to add -2 xD
            TrySelect(new Vector3(Mathf.RoundToInt(pos.x)-2,Mathf.RoundToInt(pos.y),0));
        }
    }

    void TrySelect(Vector3 selectPos)
    {
        //r we selecting our unit

        // if it is our unit : continue 
        // else cant select 
        
        Unit unit = units[1];

        if (selectPos == units[1].transform.position)
            unit = units[1];
        else
        {
            unit = null;
        }

        //Unit unit = units.Find(x => x.transform.position == selectPos);
        //

        if (unit != null)
        {
            SelectUnit(unit);
            return;
        }
        
        if(!selectedUnit)
            return;
        
        //are we selecting an enemy unit
        
        Unit enemyUnit = enemy.units[1];
        if (selectPos == enemy.units[1].transform.position)
            enemyUnit = enemy.units[1];
        else
        {
            enemyUnit = null;
        }

        //Unit enemyUnit = enemy.units.Find(x => x.transform.position == selectPos);

        if (enemyUnit != null)
        {
            TryAttack(enemyUnit);
            return;
        }
        TryMove(selectPos);
    }

    void SelectUnit(Unit unitToSelect)
    {
        if (!unitToSelect.CanSelect())
            return;
        if (selectedUnit!=null)
            selectedUnit.ToggleSelect(false);
        selectedUnit = unitToSelect;
        selectedUnit.ToggleSelect(true);
        
        //set the unit ui text
        GameUI.instance.SetUnitInfoText(selectedUnit);
    }

    void DeSelectUnit()
    {
        selectedUnit.ToggleSelect(false);
        selectedUnit = null;
        
        //disable the unit info text
        GameUI.instance.unitInfoText.gameObject.SetActive(false);
    }
    
    void SelectNextAvailableUnit()
    {
        Unit availableUnit = units[1];
        //Unit availableUnit = units.Find(x => x.CanSelect());
        if (units[1].CanSelect())
            availableUnit = units[1];
        else
        {
            availableUnit = null;
        }
        
        if (availableUnit!=null)
            SelectUnit(availableUnit);
        else
        {
            DeSelectUnit();
        }

    }

    void TryAttack(Unit enemyUnit)
    {
        if (selectedUnit.CanAttack(enemyUnit.transform.position))
        {
            selectedUnit.Attack(enemyUnit);
            SelectNextAvailableUnit();
        }
        
        //update ui
    }
    
    void TryMove(Vector3 movePos)
    {
        if (selectedUnit.CanMove(movePos))
        {
            selectedUnit.Move(movePos);
            SelectNextAvailableUnit();
        }

        //update ui
    }
    
    public void EndTurn()
    {
        if (selectedUnit != null)
        {
            DeSelectUnit();
        }
        
        //start next turn
        
        GameManager.instance.photonView.RPC("SetNextTurn", RpcTarget.All);
        //update ui
    }

    public void BeginTurn()
    {

        units[1].usedThisTurn = false;
        units[1].movedThisTurn = false;

        //update ui
        GameUI.instance.SetUnitInfoText(units[1]);
    }


}
