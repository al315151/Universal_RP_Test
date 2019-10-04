public class GameMenu : SimpleMenu<GameMenu>
{
	public override void OnBackPressed()
	{
		GameManager.Pause();
		PauseMenu.Show();
	}
}
