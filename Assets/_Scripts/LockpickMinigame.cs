using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class LockpickMinigame : MonoBehaviour
{
    private float pinLocation, lockCenterPosition;
    private Animator anim;

    [Header("Lockpick options")]
    public float pinTurnSpeed;
    public float lockrotateSpeed;
    private float lockResetSpeed;

    public bool gamePause;

    public float unlockPosition;
    public float leanency;
    public Camera cam;
    public Transform pin;
    private float eulerAngle;
    public float maxAngle = 90;

    public bool isShaking;
    public float pinHealth;
    public float pinDamageRate;

    /// Difficulty
    public int pinAvailable;
    public int currentPin;

    [Range(1, 100)]
    public int playerLevel;
    public float autoUnlockChance;

    public float maxTimer;
    public float timer;

    [Range(0,1)]
    public float sweetSpot;
    //UI
    public TextMeshProUGUI systemMsg;
    public GameObject inGameUI;

    public float MaxRotationDistance
    {
        get { return 1f - Mathf.Abs(unlockPosition - PinPosition) + leanency; }
    }

    // public getter and setter
    public float PinPosition
    {
        get 
        { return pinLocation; }
        set 
        {
            pinLocation = value;
            pinLocation = Mathf.Clamp(pinLocation, 0f, 1f);
        }
    }
    public float LockCenterPosition
    {
        get
        { return lockCenterPosition; }
        set
        {
            lockCenterPosition = value;
            lockCenterPosition = Mathf.Clamp(lockCenterPosition, 0f, MaxRotationDistance);
        }
    }

    private void Start()
    {
        systemMsg.text = "";
        anim = GetComponent<Animator>();

       NewGame();
    }

    private void Update()
    {
        if (gamePause) return;

        if (Input.GetAxisRaw("Vertical") == 0)
        {
            TurnThePin();
        }
        Shaking();
        
        if (Input.GetMouseButton(0))
        {
            TurnTheLock();
        }
        else ResetLockRotation();
            
        UpdateAnimation();

        Timer(); 
        updateSuccessChance();

    }

    private void Shaking()
    {
        isShaking = MaxRotationDistance - LockCenterPosition < 0.05f;

        if (isShaking)
        {
            pinHealth -= Time.deltaTime * pinDamageRate;
            if (pinHealth <= 0)
            {
                currentPin--;
                systemMsg.text = "You broke a lockpick,now you have " + currentPin +" pin left";
                Debug.Log("You have" + currentPin +"pin left" );
                ResetPin();
            }
        }
    }

    private void ResetPin()
    {
        if (currentPin>0)
        {
            pinHealth = 1;
            pinLocation = 0.5f;
        }
        else
        {
            Gameover();
        }
    }
    private void Gameover()
    {
        Debug.Log("Your pin has broken!");
        systemMsg.text = "Game Over!";
        gamePause = true;
        Time.timeScale = 0;

        inGameUI.SetActive(false);
    }

    private void TurnThePin()
    {
        //// Keyboard control for pin, use this as last resort
        PinPosition += Input.GetAxisRaw("Horizontal") * Time.deltaTime * pinTurnSpeed;
        
    }

    private void TurnTheLock()
    {
        LockCenterPosition += Mathf.Abs(Time.deltaTime * lockrotateSpeed);

        if (LockCenterPosition >= sweetSpot) // The closer to 1.0f, the more accurate to unlock
        {
            Win();
        }
    }

    private void Win()
    {
        Debug.Log("Lock open successfully!");
        systemMsg.text = "GG you picked the lock!";
        gamePause = true;
        Time.timeScale = 0;

        inGameUI.SetActive(false);
    }

    private void ResetLockRotation()
    {
        LockCenterPosition -= lockResetSpeed * Time.deltaTime;
    }

    private void UpdateAnimation()
    {
        anim.SetFloat("PinPosition", PinPosition);
        anim.SetFloat("LockAngle", LockCenterPosition);
        anim.SetBool("Shaking", isShaking);
    }


    public void NewGame()
    {
        Time.timeScale = 1;
        PinPosition = 0.5f;
        LockCenterPosition = 0f;
        unlockPosition = Random.Range(0.0f, 1.0f);
        currentPin = pinAvailable;
        timer = maxTimer;
        autoUnlockChance = (float)playerLevel * 2 / 3;

        lockResetSpeed = lockrotateSpeed / 2;
        gamePause = false;
    }

    public void SetDifficulty(float sweetspot, int pinavailable, float maxtimer)
    {
        sweetSpot = sweetspot;
        pinAvailable = pinavailable;
        maxTimer = maxtimer;
        NewGame();
    }

    public void Timer()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Gameover();
        }
    }

    public void updateSuccessChance()
    {
        autoUnlockChance = (float)playerLevel * 2 / 3;
    }

    public void autoUnlock()
    {
        if (currentPin <= 0) return;
        autoUnlockChance = (float)playerLevel * 2 / 3;
        print(autoUnlockChance);

        if (Random.Range(0, 100) <= autoUnlockChance)
        {
            Win();
        }
        else
        {
            currentPin--;
            systemMsg.text = "Auto unlock failed!";
            if (currentPin == 0)
            {
                Gameover();
            }
        }
    }

    
}