using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;

public class GeneralLogic : MonoBehaviour
{
    // Start is called before the first frame update
    public CardScript[,] Grid;
    public int rows;
    public int cols;

    public Canvas HUD;
    
    
    public Button StartBTN;
    public Button NewGameBTN;
    public TMP_Text StartText;
    public TMP_Text NewGameText;

    public TMP_Text winnerText;
    public TMP_Text winnernumText;
    
    public bool started;
    public bool ended;
        
    public int player1Points;
    public TMP_Text score1Text;
    
    public int player2Points;
    public TMP_Text score2Text;

    public TMP_Text currentPlayer;

    public CardScript card1;
    public CardScript card2;

    public int cardsTurned;
    void Start()
    {
        winnerText.enabled = false;
        winnernumText.enabled = false;
        started = false;
        ended = false;
        HUD.enabled = false;
        NewGameText.enabled = false;
        NewGameBTN.enabled = false;
        NewGameBTN.image.enabled = false;
        player1Points = 0;
        player2Points = 0;
        score1Text.text = player1Points.ToString();
        score2Text.text = player2Points.ToString();
        
    }

    public void AddCard(CardScript card)
    {
        if (Grid == null)
        {
            Grid = new CardScript[rows, cols];
        }
        
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
                if (Grid[i,j] == null)
                {
                    Grid[i, j] = card;
                    return;
                }
        }
    }

    public void Shuffle()
    {
        started = true;
        StartBTN.enabled = false;
        StartBTN.image.enabled = false;
        StartText.enabled = false;
        HUD.enabled = true;
        var rng1 = new System.Random();
        var rng2 = new System.Random();
        for(int r = 0; r < rows; r++){
            
            for(int c = 0; c < cols; c++)
            {
                int k1 = rng1.Next(r);
                int k2 = rng2.Next(c);
                (Grid[r, c], Grid[k1, k2]) = (Grid[k1, k2], Grid[r, c]);
            }
        }
        StartCoroutine(Sort());
        
    }

    public IEnumerator Sort()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                Grid[i,j].Reposition(i,j);
            }
        }
        yield return new WaitForSeconds(2);
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                Grid[i,j].Turn(); 
            }
        }
    }
    
    public IEnumerator EndTurn()
    {
        if (card1.cardtype == card2.cardtype)
        {
            if (currentPlayer.text == "1")
            {
                player1Points++;
                score1Text.text =  player1Points.ToString();
            }
            else
            {
                player2Points++;
                score2Text.text = player2Points.ToString();
            }

            yield return new WaitForSeconds(2);
            card1.RemoveCard();
            card1 = null;
            cardsTurned--;
            cardsTurned--;
            card2.RemoveCard();
            card2 = null;
            if (player1Points + player2Points >= cols * rows / 2)
            {
                ended = true;
                NewGameBTN.enabled = true;
                NewGameBTN.image.enabled = true;
                NewGameText.enabled = true;
                winnerText.enabled = true;
                winnernumText.text = currentPlayer.text;
                winnernumText.enabled = true;
                HUD.enabled = false;
            }
        }
        else
        {
            yield return new WaitForSeconds(2);
            card1.Turn();
            card2.Turn();
            currentPlayer.text = currentPlayer.text == "1" ? "2" : "1";
        }
        
    }   
    public void ReloadScene()
    {
        // Get the index of the current scene
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Reload the current scene by loading it again
        SceneManager.LoadScene(currentSceneIndex);
    }


    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && started && !ended)
        {
            NewGameBTN.enabled = !NewGameBTN.enabled;
            NewGameBTN.image.enabled = !NewGameBTN.image.enabled;
            NewGameText.enabled = !NewGameText.enabled;
            HUD.enabled = !HUD.enabled;
        }
    }
}
