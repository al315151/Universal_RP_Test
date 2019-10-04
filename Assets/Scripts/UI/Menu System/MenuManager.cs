using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour
{
	public MainMenu MainMenuPrefab;
	public GameMenu GameMenuPrefab;
	public PauseMenu PauseMenuPrefab;
	public OptionsMenu OptionsMenuPrefab;
	public AwesomeMenu AwesomeMenuPrefab;
	public LoadingScreenMenu LoadingScreenMenuPrefab;

    private Stack<Menu> menuStack;
    private EventSystem eventSystem;

    public static MenuManager Instance { get; set; }

    private void Awake()
    {
        // Create a singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject); return; // Prevent the duplicates to execute the following code.
        }
        DontDestroyOnLoad(gameObject);

        // Set variables and component references
        eventSystem = GetComponent<EventSystem>();
        menuStack = new Stack<Menu>();

        // Open the main menu
        MainMenu.Show();
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

	public void CreateInstance<T>() where T : Menu
	{
		var prefab = GetPrefab<T>();

		Instantiate(prefab, transform);
	}

	public void OpenMenu(Menu instance)
    {
        if (menuStack.Count > 0)
        {
            Menu previousMenu = menuStack.Peek();

            // The pointer to the used UI element is stored. Then, the interactivity of the previous menu is disabled.
            previousMenu.SelectedItem = eventSystem.currentSelectedGameObject;
            previousMenu.Interactable = false;

            //if specified, all the menus beneath are disabled.
            if (instance.DisableMenusUnderneath)
			{
				foreach (var menu in menuStack)
				{
					menu.gameObject.SetActive(false);

					if (menu.DisableMenusUnderneath)
						break;
				}
			}

            var topCanvas = instance.GetComponent<Canvas>();
            var previousCanvas = previousMenu.GetComponent<Canvas>();
			topCanvas.sortingOrder = previousCanvas.sortingOrder + 1;
        }

        // Select the default element in the new top menu
        SelectItem(instance.SelectedItem);
        menuStack.Push(instance);
    }

    private T GetPrefab<T>() where T : Menu
    {
        // Get prefab dynamically, based on public fields set from Unity
		// You can use private fields with SerializeField attribute too
        var fields = this.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        foreach (var field in fields)
        {
            var prefab = field.GetValue(this) as T;
            if (prefab != null)
            {
                return prefab;
            }
        }

        throw new MissingReferenceException("Prefab not found for type " + typeof(T));
    }
	
	public void CloseMenu(Menu menu)
	{
		if (menuStack.Count == 0)
		{
			Debug.LogErrorFormat(menu, "{0} cannot be closed because menu stack is empty", menu.GetType());
			return;
		}

		if (menuStack.Peek() != menu)
		{
			Debug.LogErrorFormat(menu, "{0} cannot be closed because it is not on top of stack", menu.GetType());
			return;
		}

		CloseTopMenu();
	}

	public void CloseTopMenu()
    {
        var instance = menuStack.Pop();

		if (instance.DestroyWhenClosed)
        	Destroy(instance.gameObject);
		else
			instance.gameObject.SetActive(false);

        // Re-activate top menu
		// If a re-activated menu is an overlay we need to activate the menu under it
		foreach (var menu in menuStack)
		{
            menu.gameObject.SetActive(true);

			if (menu.DisableMenusUnderneath)
				break;
		}
        
        // However, only the top menu will be interactable.
        if (menuStack.Count > 0)
        {
            Menu topMenu = menuStack.Peek();

            // Select the default element in the new top menu
            topMenu.Interactable = true;
            SelectItem(topMenu.SelectedItem);
        } 
    }

    // Select an item in the EventSystem
    public void SelectItem(GameObject item)
    {
        StartCoroutine(Select(item));
    }
    // This coroutine avoids item highlighting problems
    IEnumerator Select(GameObject item)
    {
        eventSystem.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        eventSystem.SetSelectedGameObject(item);
        yield return null;
    }

    private void Update()
    {
        // On Android the back button is sent as Esc
        if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Escape")) && menuStack.Count > 0)
        {
            menuStack.Peek().OnBackPressed();
        }

        // Prevent the UI from missing the pointer during navigation.
        if (Mathf.RoundToInt(Input.GetAxis("Horizontal1")) != 0 || Mathf.RoundToInt(Input.GetAxis("Vertical1")) != 0)
        {

            if (eventSystem.currentSelectedGameObject == null)
            {
                SelectItem(Instance.menuStack.Peek().SelectedItem);

            }
        }
        

       
    }
}

