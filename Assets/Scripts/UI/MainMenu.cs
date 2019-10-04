using UnityEngine;

public class MainMenu : SimpleMenu<MainMenu>
{
	public void OnPlayPressed()
	{
		LoadingScreenMenu.Show(1);
	}

    public void OnOptionsPressed()
    {
		OptionsMenu.Show();
    }

	public override void OnBackPressed()
	{
		Application.Quit();
	}
}
