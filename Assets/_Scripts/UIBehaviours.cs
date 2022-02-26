using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIBehaviours : MonoBehaviour
{
    public GameObject lockpickMiniGame;

    public TextMeshProUGUI text;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI playerLevelText;
    public TextMeshProUGUI autoUnlockChance;

    public GameObject ingameUI;

    public Button startbutton;
    public Button restartbutton;
    [SerializeField]
    private LockpickMinigame game;

   
    // Start is called before the first frame update
    void Start()
    {
        restartbutton.gameObject.SetActive(false);
        ingameUI.SetActive(false);
        game.systemMsg.text = "please select difficulty";
    }

    // Update is called once per frame
    void Update()
    {
        text.text = game.currentPin.ToString();
        timerText.text = game.timer.ToString("F2");
        playerLevelText.text = "Player level: " + game.playerLevel.ToString();
        autoUnlockChance.text = game.autoUnlockChance.ToString("F2");
    }

    public void enterMiniGame()
    {
        lockpickMiniGame.SetActive(true);
        startbutton.gameObject.SetActive(false);
        restartbutton.gameObject.SetActive(true);
        ingameUI.SetActive(true);
    }

    public void restartGame()
    {
        game.NewGame();
        ingameUI.SetActive(true);
    }

    public void HandleInputData(int val)
    {
        if (val == 0)
        {
            game.systemMsg.text = "Easy Mode";
            game.SetDifficulty(0.9f,3,60f);
        }
        if (val == 1)
        {
            game.systemMsg.text = "Normal Mode";
            game.SetDifficulty(0.95f,2,30f);
        }
        if (val == 2)
        {
            game.systemMsg.text = "Hard Mode";
            game.SetDifficulty(0.95f,1,15f);
        }
    }
}
