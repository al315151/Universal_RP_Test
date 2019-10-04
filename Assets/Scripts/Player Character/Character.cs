using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    // Player and customization identifiers.
    [SerializeField]
    private int playerNumber;
    [SerializeField]
    protected int skin;

    // Input strings.
    private string horizontalString;
    private string verticalString;
    private string[] buttonStrings;
    private string leftTriggerString;
    private string rightTriggerString;

    // Input variables.
    protected bool inputEnabled;
    protected float horizontalInput;
    protected float verticalInput;
    protected bool leftTriggerInput;
    protected bool rightTriggerInput;


    // Common variables.
    protected static Vector3 horizontalAxis;
    protected static Vector3 verticalAxis;
    protected bool isAlive;
    //...

    public static Vector3 HorizontalAxis { get => horizontalAxis; set => horizontalAxis = value; }
    public static Vector3 VerticalAxis { get => verticalAxis; set => verticalAxis = value; }
    public bool InputEnabled { get => inputEnabled; set => inputEnabled = value; }
    public int PlayerNumber { get => playerNumber; }


    public virtual void SetUp(int player, int skin) 
    {
        // Initialize the character identifiers.
        this.playerNumber = player;
        this.skin = skin;

        // Set up the input strings.
        horizontalString = "Horizontal" + (playerNumber+1);
        verticalString = "Vertical" + (playerNumber + 1);
        leftTriggerString = "LeftTrigger" + (playerNumber + 1);
        rightTriggerString = "RightTrigger" + (playerNumber + 1);

        buttonStrings = new string[(int)AYBX.Count];
        for (int i = 0; i < buttonStrings.Length; i++)
        {
            buttonStrings[i] = "joystick " + (playerNumber + 1) + " button " + i;
        }

        // Set up skin
        SetUpAppearance(skin);
    }

    public virtual void Update()
    {
        if (inputEnabled)
        {
            // Read movement inputs.
            horizontalInput = Input.GetAxis(horizontalString);
            verticalInput = Input.GetAxis(verticalString);
            leftTriggerInput = Input.GetAxis(leftTriggerString) > 0;
            rightTriggerInput = Input.GetAxis(rightTriggerString) > 0;

            // Read cube inputs.
            for (int i = 0; i < buttonStrings.Length; i++)
            {
                if (Input.GetKeyDown(buttonStrings[i]))
                {
                    GameManager.Discard(i);
                }
            }
        }     
    }

    // Character customization functions
    public abstract void SetUpAppearance(int skin);

    // Game Logic functions
    public abstract void Spawn(); // Called at the beginning of the level.
    public abstract void Death(); // Called when the character falls off.
    public abstract void Respawn(); // Called after the player is dead.
    public abstract void EnterGoal(); // Called when the goal is reached.
}


public enum AYBX
{
    A,
    Y,
    B,
    X,
    Count
}