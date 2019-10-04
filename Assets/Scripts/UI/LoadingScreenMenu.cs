using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class LoadingScreenMenu : Menu<LoadingScreenMenu>
{
	public Image loadingIcon;

    public static void Show(int scene)
	{
		Open();
		Instance.StartCoroutine(Instance.LoadScene(scene));
    }

	public static void Hide()
	{
		Close();
	}

	public override void OnBackPressed()
	{

	}

	IEnumerator LoadScene(int scene)
	{
		float currentVelocity = 0;
		float smoothTime = 0.2f;
        loadingIcon.fillAmount = 0;

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);

        while (!asyncLoad.isDone || loadingIcon.fillAmount <= 0.99f)
        {
			loadingIcon.fillAmount = Mathf.SmoothDamp(loadingIcon.fillAmount,asyncLoad.progress,ref currentVelocity, smoothTime);
            yield return null;
        }
		GameMenu.Show();
	}
}
