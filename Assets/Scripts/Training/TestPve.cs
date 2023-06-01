using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;


public enum Turn
{
    playerTurn,
    dummyTurn
}


public class TestPve : MonoBehaviour
{
    public GameObject player;
    public GameObject CubeForWeapons;
    public GameObject playerSprite;
    public GameObject dummy;
    public Camera playerCamera;

    public GameObject selectPlayer;
    public bool isPlayerSelected = false;
    public bool hasMoved = false;

    public GameObject weaponSelect;
    
    private Turn turn;
    private Random random = new Random();

    public Sprite[] listOfPlayers;

    //Dummy Stats
    public int currentHpDummy;
    public int maxHpDummy;
    public int unitDamageDummy;
    public int rangeDummy;
    private int theStartMaxHpDummy;
    
    
    //Player Stats
    public int currentHp;
    public int maxHp;
    public int moveSpeed;
    public int playerDamage;
    
    public int maxAttackRange;
    
    [Header("UI")] 
    public Image healthFillImagePlayer;
    public Image healthFillImageDummy;

    
    public Sprite[] listOfWeaponSprites;
    
    public GameObject weaponSprite;
    
    public TextMeshProUGUI weaponName;
    public TextMeshProUGUI atkWeapon;
    public TextMeshProUGUI hpWeapon;
    public TextMeshProUGUI rangeWeapon;
    public TextMeshProUGUI dummyLifeCount;

    public TextMeshProUGUI playerInfoText;
    
    public int tempAtkWeaponStat=0;
    public int tempHpWeaponStat=0;
    public int tempRangeWeaponStat=0;

    public int atkWeaponStat=0;
    public int hpWeaponStat=0;
    public int rangeWeaponStat=0;
    public int maxHpWeaponStat;

    public GameObject currentWeaponIcon;

    public int weaponIndexForShow=0;

    //public GameObject endTurnButton;

    public GameObject DeathScreen;
    public int dieCount;
    public int killCount;
    public TextMeshProUGUI deathCountText;
    public TextMeshProUGUI killCountText;
    
    public TextMeshProUGUI deathCountText1;
    public TextMeshProUGUI killCountText1;

    private int totalDamageDealt;
    private int totalDamageTaken;
    public TextMeshProUGUI totalDamageDealtText;
    public TextMeshProUGUI totalDamageTakenText;
    public TextMeshProUGUI points;
    public TextMeshProUGUI HighScore;

    void Start()
    {
        playerSprite.gameObject.GetComponent<SpriteRenderer>().sprite = listOfPlayers[Index.indexX];
        playerSprite.transform.localScale = new Vector3(Index.scale, Index.scale);
        currentHp = Index.hp;
        maxHp = Index.hp;
        moveSpeed = Index.speed;
        playerDamage = Index.atk;
        maxAttackRange = Index.range;
        healthFillImagePlayer.fillAmount = 1.0f;
        healthFillImageDummy.fillAmount = 1.0f;
        turn = Turn.playerTurn;
        dummyLifeCount.text = currentHpDummy + "/" + currentHpDummy;
        
        tempAtkWeaponStat=0; 
        tempHpWeaponStat=0; 
        tempRangeWeaponStat=0;
        
        
        atkWeaponStat=0; 
        hpWeaponStat=0; 
        rangeWeaponStat=0;
        maxHpWeaponStat = 0;
        UpdatePlayerInfoText();
        theStartMaxHpDummy = maxHpDummy;
        totalDamageDealt = 0;
        totalDamageTaken = 0;
        
        currentWeaponIcon.gameObject.SetActive(false);
    }
    
    
    
    private void Update() //uses main cam
    {
        if (Input.GetMouseButtonDown(0) && turn == Turn.playerTurn)
        {
            Vector3 pos = playerCamera.ScreenToWorldPoint(Input.mousePosition);
            
            TrySelect(new Vector3(Mathf.RoundToInt(pos.x),Mathf.RoundToInt(pos.y),0));

        }

        else if (turn == Turn.dummyTurn)
        {
            TryTargetPlayer();
            int horizontalOrVertical = random.Next(0, 2);
            int xy = random.Next(0, 2);

            if (xy == 0)
                xy = -1;
            var dummyPos = dummy.transform.position;
            
            //Check zone
            if (dummyPos.x <= 7 && horizontalOrVertical == 0)
                xy = 1;
            if (dummyPos.x >= 11 && horizontalOrVertical == 0)
                xy = -1;
            if (dummyPos.y <= -3 && horizontalOrVertical == 1)
                xy = 1;
            if (dummyPos.y >= 1 && horizontalOrVertical == 1)
                xy = -1;


            MoveDummy(xy,horizontalOrVertical);

            turn = Turn.playerTurn;
            //endTurnButton.SetActive(true);
            hasMoved = false;

        }
        
        
    }

    private void MoveDummy(int xy, int horizontalOrVertical)
    {
        var targetPos = new Vector3();
        if (dummy.transform.position.x+xy+2 == player.transform.position.x &&
            dummy.transform.position.y-1 == player.transform.position.y)
            horizontalOrVertical = horizontalOrVertical == 0? 1 : 0;
        
        if (horizontalOrVertical == 0) 
            targetPos = new Vector3(dummy.transform.position.x+xy, dummy.transform.position.y);
        else
        {
            targetPos = new Vector3(dummy.transform.position.x, dummy.transform.position.y+xy);
        }
        StartCoroutine(MoveOverTime());
        
        IEnumerator MoveOverTime()
        {
            
            while (dummy.transform.position != targetPos)
            {
                dummy.transform.position = Vector3.MoveTowards(dummy.transform.position, targetPos, 0.8f * Time.deltaTime);
                //usedThisTurn = true;
                yield return null;
            }
        }
    }
    
    void TryTargetPlayer()
    {
        if (rangeDummy == 1)
        {
            Vector3[] vectors = {Vector3.down, Vector3.up, Vector3.left, Vector3.right};
            foreach (var vect in vectors)
            {
                if (dummy.transform.position + vect + new Vector3(2,-1)== player.transform.position)
                {
                    DummyAttack();
                    break;
                }
            }
        }

        else if (rangeDummy == 2)
        {
            Vector3[] vectors = {Vector3.down, Vector3.up, Vector3.left, Vector3.right,
                new Vector3(0, -2, 0),new Vector3(0, 2, 0),new Vector3(2, 0, 0),
                new Vector3(-2, 0, 0)};
            foreach (var vect in vectors)
            {
                if (dummy.transform.position + vect + new Vector3(2,-1)== player.transform.position)
                {
                    DummyAttack();
                    break;
                }
            }
        }
        else //range is 3
        {
            Vector3[] vectors = {Vector3.down, Vector3.up, Vector3.left, Vector3.right,
                new Vector3(0, -2, 0),new Vector3(0, 2, 0),new Vector3(2, 0, 0),
                new Vector3(-2, 0, 0),
                new Vector3(0, -3, 0),new Vector3(0, 3, 0),new Vector3(3, 0, 0),
                new Vector3(-3, 0, 0)
            };
            foreach (var vect in vectors)
            {
                if (dummy.transform.position + vect + new Vector3(2,-1)== player.transform.position)
                {
                    DummyAttack();
                    break;
                }
            }
        }



    }

    void DummyAttack()
    {
        totalDamageTaken += unitDamageDummy;
        //hpWeaponStat -= unitDamageDummy;
        if (hpWeaponStat > 0)
        {
            hpWeaponStat -= unitDamageDummy;
            if (hpWeaponStat < 0)
            {
                currentHp -= hpWeaponStat;
                hpWeaponStat = 0;
            }
        }
        else if (hpWeaponStat <= 0)
        {
            currentHp = hpWeaponStat + currentHp - unitDamageDummy;
            hpWeaponStat = 0;
        }
        
        if (hpWeaponStat <= 0 && currentHp<=0)
        {
            dieCount += 1;
            deathCountText1.text = "Death Count : " + dieCount;
            PlayerDedRestart();
        }
        else
        {
            UpdatePlayerInfoText();
            UpdateHealthBarPlayer((float) currentHp / (float)maxHp);
        }
    }

    public void UpdateHealthBarPlayer(float fillAmount)
    {
        healthFillImagePlayer.fillAmount = fillAmount;
    }
    
    void TrySelect(Vector3 selectPos)
    {
        if (selectPos == player.transform.position)
        {
            selectPlayer.SetActive(true);
            isPlayerSelected = true;
        }
        else if (selectPos == dummy.transform.position+new Vector3(2,-1))
        {
            if (isPlayerSelected)
                PlayerTryAttack();
            EndTurn();

        }
        else if (selectPos == CubeForWeapons.transform.position+new Vector3(0,+0.5f))
        {
            if (isPlayerSelected)
                PlayerTryToLoot();
            UpdatePlayerInfoText();
            EndTurn();
        }
        else
        {
            if (hasMoved)
                EndTurn();
            if (isPlayerSelected && !hasMoved)
            {
                PlayerTryMove(selectPos);
                selectPlayer.SetActive(false);
                isPlayerSelected = false;
            }
        }
        
    }

    //here mimic weapons
    public void PlayerTryToLoot()
    {
        
        string[] weaponNames = {"Katana","Armageddon Staff","Poison Staff","Cup Staff","Cleric Staff","Normal Sword"};
        
        // Name of the chara --> HP ATK SPD Scale
        var characterStat = new Dictionary<string, (int, int, int, float, float)>
        {
            { "Katana", (80,70,0,3f,3f) },
            { "Armageddon Staff", (100,40,0,1.2f,1.2f) },
            { "Poison Staff", (80,40,0,1.2f,1.2f) },
            { "Cup Staff", (20,30,1,1.2f,1.2f) },
            { "Cleric Staff", (10,30,0,1.2f,1.2f) },
            { "Normal Sword", (30,10,0,3f,3f) }
        };

        //
        
        Vector3 playerCoord = player.transform.position;
        Vector3 lootCoord = CubeForWeapons.transform.position;

        float diffX = playerCoord.x - lootCoord.x;
        float diffY = playerCoord.y - lootCoord.y-0.5f;

        if (2 >= Math.Round(Math.Sqrt(diffX * diffX + diffY * diffY)))
        {
            int weaponIndex = random.Next(0, listOfWeaponSprites.Length);
            weaponIndexForShow = weaponIndex;
            string weapName = weaponNames[weaponIndex];
            weaponName.text = weaponNames[weaponIndex];
            (int hp, int atk, int range,float scaleX, float scaleY)  = characterStat[weapName];

            tempAtkWeaponStat = atk;
            tempHpWeaponStat = hp;
            tempRangeWeaponStat = range;
            
            hpWeapon.text = "HP : " + hp;
            atkWeapon.text = "ATK : " + atk;
            rangeWeapon.text = "Range : " + range;

            weaponSelect.SetActive(true);
            weaponSprite.SetActive(true);
            //Weapon = listOfWeapons[weaponIndex];
            weaponSprite.GetComponent<SpriteRenderer>().sprite = listOfWeaponSprites[weaponIndex];
            weaponSprite.transform.localScale = new Vector3(scaleX, scaleY);


        }
    }

    void PlayerTryAttack()
    {
        Vector3 playerCoord = player.transform.position;
        Vector3 dummyCoord = dummy.transform.position;

        float diffX = playerCoord.x - dummyCoord.x-2;
        float diffY = playerCoord.y - dummyCoord.y+1;

        if (maxAttackRange + rangeWeaponStat>= Math.Round(Math.Sqrt(diffX * diffX + diffY * diffY)))
        {
            totalDamageDealt+=(playerDamage + atkWeaponStat);
            currentHpDummy -= (playerDamage + atkWeaponStat);
            if(currentHpDummy<=0)
            {
                DestroyDummy();
            }
            else
            {
                dummyLifeCount.text = currentHpDummy + "/"+maxHpDummy;
                UpdateHealthBarDummy((float) currentHpDummy / (float)maxHpDummy);
            }
            
        }
    }

    
    void DestroyDummy()
    {
        int xDummy = random.Next(-4, 14);
        int yDummy = random.Next(-5, 8);
        dummy.transform.position = new Vector3(xDummy,yDummy);
        killCount += 1;
        killCountText1.text = "Kill Count : " + killCount;
        currentHpDummy = theStartMaxHpDummy;
        maxHpDummy = theStartMaxHpDummy;
        dummyLifeCount.text = maxHpDummy + "/"+maxHpDummy;
        UpdateHealthBarDummy((float) maxHpDummy / (float)maxHpDummy);
    }

    void UpdateHealthBarDummy(float fillAmount)
    {
        healthFillImageDummy.fillAmount = fillAmount;
    }

    void PlayerTryMove(Vector3 selectPos)
    {
        Vector3 playerCoord = player.transform.position;

        float diffX = playerCoord.x - selectPos.x;
        float diffY = playerCoord.y - selectPos.y;

        if (moveSpeed>= Math.Round(Math.Sqrt(diffX * diffX + diffY * diffY)))
        {
            StartCoroutine(MoveOverTime());
            
            IEnumerator MoveOverTime()
            {
            
                while (player.transform.position != selectPos)
                {
                    player.transform.position = Vector3.MoveTowards(player.transform.position, selectPos, 1.5f*Time.deltaTime);
                    //usedThisTurn = true;
                    yield return null;
                }
            }
            hasMoved = true;
            selectPlayer.SetActive(false);
            isPlayerSelected = false;
        }
    }
    
    
    public void EndTurn()
    {
        //endTurnButton.SetActive(false);
        selectPlayer.SetActive(false);
        isPlayerSelected = false;
        
        turn = Turn.dummyTurn;
    }

    public void FromInPveToMenu()
    {
        int pointsTemp = (totalDamageDealt * (killCount+1) + totalDamageTaken) / 1 + dieCount;
        totalDamageDealtText.text = "Damage Dealt : "+totalDamageDealt;
        totalDamageTakenText.text = "Damage Taken : "+totalDamageTaken;
        points.text = "Points : " + pointsTemp;
        if (Index.difficulty == 1)
        {
            if (pointsTemp > Index.HighScoreEasy)
            {
                HighScore.text = "Congragulations! You beat your previous highscore of " + Index.HighScoreEasy;
                Index.HighScoreEasy = pointsTemp;
            }
            else
            {
                HighScore.text = "Your current highscore is " + Index.HighScoreEasy;
            }
        }
        else if (Index.difficulty == 2)
        {
            if (pointsTemp > Index.HighScoreMedium)
            {
                HighScore.text = "Congragulations! You beat your previous highscore of " + Index.HighScoreMedium;
                Index.HighScoreMedium = pointsTemp;
            }
            else
            {
                HighScore.text = "Your current highscore is " + Index.HighScoreMedium;
            }
        }
        else
        {
            if (pointsTemp > Index.HighScoreHard)
            {
                HighScore.text = "Congragulations! You beat your previous highscore of " + Index.HighScoreMedium;
                Index.HighScoreHard = pointsTemp;
            }
            else
            {
                HighScore.text = "Your current highscore is " + Index.HighScoreHard;
            }
        }
        deathCountText.text = "Death Count : " + dieCount;
        killCountText.text = "Kill Count : " + killCount;
        DeathScreen.SetActive(true);
    }

    public void BackOutWeapons()
    {
        weaponSprite.SetActive(false);
        weaponSelect.SetActive(false);
    }

    public void SelectedAWeapon()
    {
        atkWeaponStat = tempAtkWeaponStat;
        hpWeaponStat = tempHpWeaponStat;
        rangeWeaponStat = tempRangeWeaponStat;
        maxHpWeaponStat = tempHpWeaponStat;
        UpdatePlayerInfoText();
        //weaponSelect.SetActive(false);
        currentWeaponIcon.SetActive(true);
        currentWeaponIcon.GetComponent<SpriteRenderer>().sprite = listOfWeaponSprites[weaponIndexForShow];
        BackOutWeapons();
    }

    public void UpdatePlayerInfoText()
    {
        playerInfoText.text = "ATK : " + playerDamage + " + (" + atkWeaponStat +")\n"+
        "HP : " + currentHp +"/"+maxHp+" + (" + hpWeaponStat +"/"+ maxHpWeaponStat +")\n"+
        "RANGE : " + maxAttackRange + " + (" + rangeWeaponStat +")\n"+
        "SPEED : " + moveSpeed;
        
    }

    public void OnClickBackTM()
    {
        DeathScreen.SetActive(false);
        killCount = 0;
        dieCount = 0;
        Index.firstLogin = false;
        SceneManager.LoadScene("MainMenu");
    }

    public void PlayerDedRestart()
    {
        int xPlayer = random.Next(-5, 24);
        int yPlayer = random.Next(-7, 7);
        player.transform.position = new Vector3(xPlayer,yPlayer);
        currentHp = Index.hp;
        maxHp = Index.hp;
        moveSpeed = Index.speed;
        playerDamage = Index.atk;
        maxAttackRange = Index.range;
        healthFillImagePlayer.fillAmount = 1.0f;
        healthFillImageDummy.fillAmount = 1.0f;
        turn = Turn.playerTurn;

        tempAtkWeaponStat=0; 
        tempHpWeaponStat=0; 
        tempRangeWeaponStat=0;
        
        
        atkWeaponStat=0; 
        hpWeaponStat=0; 
        rangeWeaponStat=0;
        maxHpWeaponStat = 0;
        UpdatePlayerInfoText();
        
        currentWeaponIcon.gameObject.SetActive(false);
    }
}
