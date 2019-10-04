using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishTrigger : MonoBehaviour
{
    public const int Layer = 10; 
    private new bool enabled = true;
    private int winner;

    public GameObject elevatorPlatform;
    public GameObject targetPosition;
    private Transform platform;
    bool elevatorStarted;

    Vector3 start;
    Vector3 target;
    float time = 5f;
    float timer;
    float t;

    void Start()
    {
        start = elevatorPlatform.transform.position;
        target = targetPosition.transform.position;
        platform = elevatorPlatform.transform;
        t = 0;
        timer = 0;
    }

    public void StartElevator()
    {
        elevatorStarted = true;
    }


    public void OnTriggerEnter(Collider other)
    {
        if (enabled)
        {
            enabled = false;
            winner = other.GetComponent<Character>().PlayerNumber;

            for (int j = 0; j < GameManager.NumberOfPlayers; j++)
            {
                GameManager.FreezePlayer(j, true);
            }

            Invoke("FinishLevel",time/2f);
            StartElevator();
        }
        
    }
    private void Update()
    {
        if (elevatorStarted)
        {
            timer += Time.deltaTime;
            t = Mathf.SmoothStep(0,1,(timer/time));
            platform.position = new Vector3(platform.position.x, Mathf.Lerp(start.y,target.y,t), platform.position.z);

        }

    }

    void FinishLevel()
    {
        GameManager.OnLevelFinished(winner);
    }
}
