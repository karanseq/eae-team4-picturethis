using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        Screen.SetResolution(540, 960, false);
        Invoke("ChangeScene", 2);
	}
	
    void ChangeScene()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
