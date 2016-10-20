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

        AudioSource source = PuzzleInfoInstance.Instance.gameObject.GetComponent<AudioSource>();
        source.PlayOneShot(PuzzleInfoInstance.Instance.audioClips[3]);
    }

    public void OnCreateClicked()
    {
        SceneManager.LoadScene("PuzzleInfo");

        AudioSource source = PuzzleInfoInstance.Instance.gameObject.GetComponent<AudioSource>();
        source.PlayOneShot(PuzzleInfoInstance.Instance.audioClips[3]);
    }
}
