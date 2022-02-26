using System;
using System.Collections;
using System.Collections.Generic;
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
    public int playerLevel;


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
        anim = GetComponent<Animator>();

        PinPosition = 0;
        LockCenterPosition = 0;
        unlockPosition = Random.Range(0.0f, 1.0f);

        lockResetSpeed = lockrotateSpeed / 2;
        gamePause = false;
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

    }

    private void Shaking()
    {
        isShaking = MaxRotationDistance - LockCenterPosition < 0.05f;

        if (isShaking)
        {
            pinHealth -= Time.deltaTime * pinDamageRate;
            if (pinHealth <= 0)
            {
                pinAvailable--;
                Debug.Log("You have" + pinAvailable +"pin left" );
                ResetPin();
            }
        }
    }

    private void ResetPin()
    {
        if (pinAvailable>0)
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
        /// Only pause for now; maybe add restart?
        Debug.Log("Your pin has broken!");
        gamePause = true;
        Time.timeScale = 0;
    }

    private void TurnThePin()
    {
        //// Keyboard control for pin, use this as last resort
        PinPosition += Input.GetAxisRaw("Horizontal") * Time.deltaTime * pinTurnSpeed;
        
    }

    private void TurnTheLock()
    {
        LockCenterPosition += Mathf.Abs(Time.deltaTime * lockrotateSpeed);

        if (LockCenterPosition >= 0.9f) // The closer to 1.0f, the more accurate to unlock
        {
            Win();
        }
    }

    private void Win()
    {
        Debug.Log("Lock open successfully!");
        gamePause = true;
        Time.timeScale = 0;
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

    public void StartNewGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("SampleScene");
    }

    public void NewGame()
    {
        Time.timeScale = 1;
        PinPosition = 0;
        LockCenterPosition = 0.5f;
        unlockPosition = Random.Range(0.0f, 1.0f);
        pinAvailable = 3;
        
        lockResetSpeed = lockrotateSpeed / 2;
        gamePause = false;
    }
}