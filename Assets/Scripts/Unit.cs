using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Unit : MonoBehaviourPun
{
    public int currentHp;
    public int maxHp;
    public float moveSpeed;
    public int unitDamage;

    public int maxMoveDistance;
    public int maxAttackRange;

    public bool usedThisTurn;
    public bool movedThisTurn;

    public GameObject selectedVisual;
    public SpriteRenderer sprite; //tutos sprightVisual

    [Header("UI")] 
    public Image healthFillImage;
    
    [Header("Sprite Variants")] 
    public Sprite playerOneSprite; //left
    public Sprite playerTwoSprite; //right

    //need to rotate and improve movements
    //punRPC.isMine?a
    
    
    [PunRPC]
    void Initialize(bool isMine)
    {
        if (isMine)
            PlayerController.me.units.Add(this);
        else
        {
            GameManager.instance.GetOtherPlayer(PlayerController.me).units.Add(this);
        }

        healthFillImage.fillAmount = 1.0f;
        
        //set sprite variant
        sprite.sprite = transform.position.x < 0 ? playerOneSprite : playerTwoSprite;
        
        //rotate to face enemy
        //sprite.transform.up = transform.position.x < 0 ? Vector3.right : Vector3.left;
    }
    
    public bool CanSelect()
    {
        if (usedThisTurn)
            return false;
        else
            return true;
    }

    public bool CanMove(Vector3 movePos)
    {
        if (movedThisTurn)
            return false;
        if (Vector3.Distance(transform.position, movePos) <= maxMoveDistance)
            return true;
        else
        {
            return false;
        }
    }

    public bool CanAttack(Vector3 attackPos)
    {
        if (Vector3.Distance(transform.position, attackPos) <= maxAttackRange)
            return true;
        else
        {
            return false;
        }
    }

    public void ToggleSelect(bool selected)
    {
        Debug.Log("selected visual should be true");
        selectedVisual.SetActive(selected);
    }

    public void Move(Vector3 targetPos)
    {
        movedThisTurn = true;
        //where i would trigger move animation

        //face away 4 tanks
        //Vector3 dir = (transform.position - targetPos).normalized;
        //sprite.transform.up = -dir;

        StartCoroutine(MoveOverTime());
        
        IEnumerator MoveOverTime()
        {
            while (transform.position != targetPos)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
                //usedThisTurn = true;
                yield return null;
            }
        }
    }

    public void Attack(Unit unitToAttack)
    {
        usedThisTurn = true;
        unitToAttack.photonView.RPC("TakeDamage",PlayerController.enemy.photonPlayer, unitDamage);
    }

    [PunRPC]
    void TakeDamage(int damage)
    {
        currentHp -= damage;

        if (currentHp<=0) //ded check
        {
            
            photonView.RPC("Die", RpcTarget.All);
        }
        else
        {
            //update health
            photonView.RPC("UpdateHealthBar", RpcTarget.All, (float) currentHp / (float)maxHp);
            
        }
    }

    [PunRPC]
    void UpdateHealthBar(float fillAmount)
    {
        healthFillImage.fillAmount = fillAmount;
    }

    [PunRPC]
    void Die()
    {
        //PlayerController.me.units.Remove(this);
        GameManager.instance.WinCon(); 
        //PhotonNetwork.Destroy(gameObject);
        
    }
    
}
