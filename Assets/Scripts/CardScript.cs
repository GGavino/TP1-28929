using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardScript : MonoBehaviour
{
    public Animator animator;
    public int cardtype;
    public bool isTurned;

    public GeneralLogic genLogic;

    // Start is called before the first frame update

    public void Start()
    {
        genLogic.AddCard(this);
    }

    public void Turn()
    {
        animator.SetBool("Unturn", false);
        animator.SetBool("Turn",true);
        if(!isTurned) isTurned = true;
        if (genLogic.cardsTurned > 0) genLogic.cardsTurned--;
        if (genLogic.card1 == this) genLogic.card1 = null;
        if (genLogic.card2 == this) genLogic.card2 = null;
    }
    
    public void Unturn()
    {
        if (genLogic.cardsTurned < 2)
        {
            animator.SetBool("Turn", false);
            animator.SetBool("Unturn",true);
            if (isTurned)
            {
                isTurned = false;
                if (genLogic.cardsTurned == 0)
                {
                    genLogic.card1 = this;
                }
                else
                {
                    genLogic.card2 = this;
                    StartCoroutine(genLogic.EndTurn());
                }
                genLogic.cardsTurned++;
            }
        }

    }
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            this.Unturn();
        }
    }

    public void Reposition(int i, int j)
    {
        
        transform.position = new Vector3((i-1)*0.75f, 0.25f , (j-1.5f)*1.25f);
    }

    public void RemoveCard()
    {
        Destroy(gameObject);
    }
}
