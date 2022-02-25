using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBehaviours : MonoBehaviour
{
    public GameObject lockpickMiniGame;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void enterMiniGame()
    {
        lockpickMiniGame.SetActive(true);
    }
}
