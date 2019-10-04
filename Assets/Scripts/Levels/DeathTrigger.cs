using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        try
        {
            GameManager.PlayerToSpawn(other.gameObject.GetComponent<Character>().PlayerNumber);
        }
        catch
        {
            throw new UnityException("Alberto la lió parda aquí.");
        }
    }
}
