using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static CubeManager;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    // Player Character
    private int numberOfPlayers = 2;
    public Character[] players;
    private bool[] inputsEnabled;

    // Camera
    public CameraScript cameraScript;

    // Level Load
    private ColorPalette colorPalette;
    private const string levelPath = "Levels/";
    public static int levelNumber = 0;
    private Level[] allLevels;
    private Level level;

    private List<int> passedLevels;

    // Animations
    private const string messageTop = "Player {0} WINS!";
    private const string messageBottom = "Player {0} loses";
    private const string triggerMessage = "Message";
    private const string triggerFadeIn = "Fade in";
    private const string triggerFadeOut = "Fade out";
    public Text textTop;
    public Text textBottom;
    private Animator animator;

    public static bool[] InputsEnabled 
    { 
        get => instance.inputsEnabled;
        set => instance.inputsEnabled = value;
    }
    public static int NumberOfPlayers
    {
        get => instance.numberOfPlayers;
        set => instance.numberOfPlayers = value;
    }

    void Awake()
    {
        // Create the singleton
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        //DontDestroyOnLoad(gameObject);

        //Initialize components
        animator = GetComponent<Animator>();

        // Prevent cubes from colliding with each other
        Physics.IgnoreLayerCollision(CubeManager.Layer, CubeManager.Layer,true);
        Physics.IgnoreLayerCollision(CubeManager.Layer, FinishTrigger.Layer,true);

        // Load all the levels 
        allLevels = Resources.LoadAll<Level>(levelPath);
    }

    void Start()
    {
        // Calculate the movement axes based on the camera.
        Character.HorizontalAxis = Vector3.ProjectOnPlane(Camera.main.transform.TransformDirection(Vector3.right), Vector3.up).normalized;
        Character.VerticalAxis = Vector3.ProjectOnPlane(Camera.main.transform.TransformDirection(Vector3.forward), Vector3.up).normalized;

        // Initialize the players and the enabled inputs
        //players = new Character[numberOfPlayers];
        inputsEnabled = new bool[(int)AYBX.Count];

        for (int i = 0; i < numberOfPlayers; i++)
        {
            players[i].SetUp(i,i);
        }
        
        for (int i = 0; i < inputsEnabled.Length; i++)
        {
            inputsEnabled[i] = true;
        }

        // Resume the game in case it was paused.
        Resume();

        passedLevels = new List<int>();
        passedLevels.Add(levelNumber);
        LoadLevel(levelNumber);
        colorPalette = GetComponent<ColorPalette>();
    }

 
    void LoadLevel(int number)
    {

        if (level != null)
        {
            Destroy(level.gameObject);
        }
        level = Instantiate(allLevels[number]);


        for (int i = 0; i < numberOfPlayers; i++)
        {
            PlayerToSpawn(i);
        }

        cameraScript.CenterCamera();

        ChangeColorPalette();
    }

    public static void Discard(int color)
    {
        if (InputsEnabled[color])
        {
            instance.level.Discard(color);
        }
    }

    public static void AddCube(Cube cube, int color)
    {
        instance.level.AddCube(cube,color);
    }

    public static void PlayerToSpawn(int player)
    {
        instance.players[player].GetComponent<CharacterController>().enabled = false;
        instance.players[player].transform.position = instance.level.SpawnPoints[player].position;
        instance.players[player].transform.rotation = instance.level.SpawnPoints[player].rotation;
        instance.players[player].GetComponent<CharacterController>().enabled = true;
        FreezePlayer(player,false);
    }

    public static void FreezePlayer(int player, bool freeze)
    {
        instance.players[player].InputEnabled = !freeze;
    }
    
    void StartAnotherLevel()
    {
        if (passedLevels.Count >= allLevels.Length)
        {
            passedLevels.Clear();
        }

        int rand = levelNumber;
        while (passedLevels.Contains(rand))
        {
            rand = Random.Range(0, instance.allLevels.Length);
        }

        instance.LoadLevel(levelNumber = rand);
        passedLevels.Add(rand);

        animator.SetTrigger(triggerFadeIn);
    }

    public static void OnLevelFinished(int winner)
    {
        if (winner == 1)
        {
            instance.textTop.text = string.Format(messageTop,2);
            instance.textBottom.text = string.Format(messageBottom,1);     
        }
        else
        {
            instance.textTop.text = string.Format(messageTop,1);
            instance.textBottom.text = string.Format(messageBottom,2); 
        }
        

        instance.animator.SetTrigger(triggerMessage);
        instance.animator.SetTrigger(triggerFadeOut);
    }

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.PageDown))
        {
            levelNumber = (levelNumber == 0) ? allLevels.Length-1 : levelNumber-1;
            LoadLevel(levelNumber);
        }
        if (Input.GetKeyDown(KeyCode.PageUp))
        {
            levelNumber = (levelNumber == allLevels.Length-1) ? 0 : levelNumber+1;
            LoadLevel(levelNumber);
        }
    }

    public void ChangeColorPalette()
    {
        if (colorPalette != null)
        {
            colorPalette.ChangeColorPalette();
        }
    }

    public static void Pause()
    {
        Time.timeScale = 0;
        for (int i = 0; i < instance.numberOfPlayers; i++)
        {
            FreezePlayer(i,true);
        }
    }

    public static void Resume()
    {
        Time.timeScale = 1;
        for (int i = 0; i < instance.numberOfPlayers; i++)
        {
            FreezePlayer(i,false);
        }
    }
}
