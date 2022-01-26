using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	[SerializeReference] GameObject pressAnyContainer;
	[SerializeReference] GameObject mainMenuButtons;
	[SerializeReference] GameObject optionsMenu;

	private bool optionMenuOpened;

	public void PlayGame() {
		// transizione alla prima scene del gioco
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}
	
	public void QuitGame() {
		Application.Quit();
	}
	
	public void EnableOptions(bool enable) {
		optionMenuOpened = enable;
		optionsMenu.SetActive(enable);
		mainMenuButtons.SetActive(!enable);
	}
	
	void Update() {
		if (Input.anyKey)
		{
			pressAnyContainer.SetActive(false);
			mainMenuButtons.SetActive(true);
		}
		if(optionMenuOpened && Input.GetKey(KeyCode.Escape)){
			EnableOptions(false);
		}
	}
	
}
