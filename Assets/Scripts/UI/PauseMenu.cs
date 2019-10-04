using UnityEngine.SceneManagement;

public class PauseMenu : SimpleMenu<PauseMenu>
{
	public override void OnBackPressed()
	{
		PauseMenu.Hide();
		GameManager.Resume();
	}

    public void OnQuitPressed()
	{
		Hide();
		Destroy(this.gameObject); // This menu does not automatically destroy itself
		GameMenu.Hide();
		LoadingScreenMenu.Hide();

		SceneManager.LoadScene(0);
	}
}
