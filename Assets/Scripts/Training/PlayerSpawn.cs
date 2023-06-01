using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerSpawn : MonoBehaviour
{
    public GameObject[] playerPrefabs;
    public Transform[] spawnPoints;

   
    private void Start()
    {
        int index = Index.indexX;
        int randomNumber = Random.Range(0, spawnPoints.Length);

        Transform spawnPoint = spawnPoints[randomNumber];

        //GameObject playerToSpawn = playerPrefabs[index];
        //Instantiate(playerToSpawn, spawnPoint.position, Quaternion.identity);

        GameObject unit = Instantiate(playerPrefabs[index], spawnPoint.position, Quaternion.identity);
        
    }
}
