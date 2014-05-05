 using UnityEngine;
using System.Collections;

public class OptionsScreen : MonoBehaviour {
	
	bool _isFullscreen;
	
	void Start(){
		_isFullscreen = Screen.fullScreen;
	}

	void OnGUI(){
		
		//GUI.Box (new Rect (Screen.width/2.0f-(Screen.width*0.2f)/2, Screen.height*0.1f, Screen.width*0.2f, Screen.height*0.6f), "Menu");

		if(GUI.Button (new Rect (Screen.width/2.0f-(Screen.width*0.2f)/2, Screen.height*0.2f, 250, 30), "Fullscreen")){
			Screen.fullScreen = !Screen.fullScreen;
		}
		
		if(GUI.Button (new Rect (Screen.width/2.0f-(Screen.width*0.2f)/2, Screen.height*0.3f, 250, 30), "1920 x 1080")){
			Screen.SetResolution(1920, 1080, Screen.fullScreen);
		}

		if(GUI.Button (new Rect (Screen.width/2.0f-(Screen.width*0.9f)/2, Screen.height*0.1f, 250, 30), "Back to Main Menu")){
			gameObject.GetComponent<StartScreen>().enabled = true;
			gameObject.GetComponent<OptionsScreen>().enabled = false;
		}
		
	}
}
