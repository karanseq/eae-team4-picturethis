using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PuzzleSelect : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnPuzzleButtonClicked(string buttonName)
    {
        SceneManager.LoadScene("PlayView");
    }

    public void OnBackButtonClicked()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
