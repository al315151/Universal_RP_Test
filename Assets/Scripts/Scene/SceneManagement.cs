using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManagement : MonoBehaviour
{
    public Camera mainCamera;
    public Camera secondaryCamera;

    float cameraMovCooldown = 0;
       
    // Start is called before the first frame update
    void Start()
    {
        secondaryCamera.gameObject.SetActive(false);
        mainCamera.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
		// CAMERA CONTROLS //
        cameraMovCooldown += Time.deltaTime;
        if (cameraMovCooldown > 0.5f)
        {
            if (/*Input.GetKeyDown(KeyCode.P) ||*/ Input.GetAxis("LT") != 0)
            {
                if (mainCamera.gameObject.activeInHierarchy)
                {
                    mainCamera.gameObject.SetActive(false);
                    secondaryCamera.gameObject.SetActive(true);
                }
                else
                {
                    secondaryCamera.gameObject.SetActive(false);
                    mainCamera.gameObject.SetActive(true);
                }
                cameraMovCooldown = 0f;
            }
        }
    }
}
