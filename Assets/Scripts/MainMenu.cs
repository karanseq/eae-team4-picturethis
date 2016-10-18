using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Debug.Log("Inside MainMenu.Start...");
	}

    public void OnPlayClicked()
    {
        SceneManager.LoadScene("PuzzleSelect");
    }

    public void OnCreateClicked()
    {
        SceneManager.LoadScene("PuzzleInfo");
    }
}
