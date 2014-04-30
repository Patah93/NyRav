using UnityEngine;
using System.Collections;

public class StartScreen : MonoBehaviour {

	Rect _startRect;
	

	void OnGUI(){

		GUI.Box (new Rect (Screen.width/2.0f-(Screen.width*0.2f)/2, Screen.height*0.1f, Screen.width*0.2f, Screen.height*0.6f), "Menu");

		if(GUI.Button (new Rect (Screen.width/2.0f-(Screen.width*0.2f)/2, Screen.height*0.1f, 70, 30), "Patrik")){
			Application.LoadLevel("PatrikScene");
		}

		if(GUI.Button (new Rect (30, 90, 70, 30), "Lars")){
			Application.LoadLevel("LurzScene");
		}

	}
}
