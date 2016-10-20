using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        Invoke("ChangeScene", 2);
	}
	
    void ChangeScene()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
