using UnityEngine;
using System.Collections;

public class StartScreen : MonoBehaviour {

	bool _isFullscreen;

	void OnGUI(){

		//GUI.Box (new Rect (Screen.width/2.0f-(Screen.width*0.2f)/2, Screen.height*0.1f, Screen.width*0.2f, Screen.height*0.6f), "Menu");

		if(GUI.Button (new Rect (Screen.width/2.0f-(Screen.width*0.2f)/2, Screen.height*0.1f, 250, 30), "New Game")){
			Application.LoadLevel("PimsScene");
		}

		if(GUI.Button (new Rect (Screen.width/2.0f-(Screen.width*0.2f)/2, Screen.height*0.2f, 250, 30), "Options")){
			gameObject.GetComponent<OptionsScreen>().enabled = true;
			gameObject.GetComponent<StartScreen>().enabled = false;
		}

		if(GUI.Button (new Rect (Screen.width/2.0f-(Screen.width*0.2f)/2, Screen.height*0.3f, 250, 30), "Exit")){
			Application.Quit();
		}
	}
}
