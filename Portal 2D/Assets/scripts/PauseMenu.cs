using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

	public static bool IsPaused = false;

	public GameObject pauseMenuUI;

	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			if (IsPaused) {
				Resume ();
			} else {
				Pause ();
			}
		}
	}

	void Resume(){
		pauseMenuUI.SetActive (false);
		Time.timeScale = 1f;
		IsPaused = false;
	}

	void Pause(){
		pauseMenuUI.SetActive (true);
		Time.timeScale = 0f;
		IsPaused = true;
	}

	public void ButtonResume(){
		pauseMenuUI.SetActive (false);
		Time.timeScale = 1f;
		IsPaused = false;
	}

	public void ReturnToMenu(){
		Resume ();
		SceneManager.LoadScene(0);
	}

	public void QuitGame(){
		Debug.Log ("Quit");
		Application.Quit ();
	}

	public bool isPause(){
		return IsPaused;
	}
}
