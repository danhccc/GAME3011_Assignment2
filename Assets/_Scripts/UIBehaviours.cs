using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIBehaviours : MonoBehaviour
{
    public GameObject lockpickMiniGame;

    public TextMeshProUGUI text;

    [SerializeField]
    private LockpickMinigame game;

   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        text.text = game.pinAvailable.ToString();
    }

    public void enterMiniGame()
    {
        lockpickMiniGame.SetActive(true);
    }

    public void DisplaySystemMessage()
    {
        
    }
}
