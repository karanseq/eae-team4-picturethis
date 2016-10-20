using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PuzzleInfo : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        PuzzleInfoInstance.Instance.ResetPuzzleInfo();
    }

    public void OnCreateButtonClicked()
    {
        SceneManager.LoadScene("PuzzleView");
        Debug.Log("PuzzleInfo says puzzle-name:" + PuzzleInfoInstance.Instance.puzzleName + " num-names:" + PuzzleInfoInstance.Instance.names.Count);

        AudioSource source = PuzzleInfoInstance.Instance.gameObject.GetComponent<AudioSource>();
        source.PlayOneShot(PuzzleInfoInstance.Instance.audioClips[3]);
    }

    public void OnBackButtonClicked()
    {
        SceneManager.LoadScene("MainMenu");
        PuzzleInfoInstance.Instance.ResetPuzzleInfo();

        AudioSource source = PuzzleInfoInstance.Instance.gameObject.GetComponent<AudioSource>();
        source.PlayOneShot(PuzzleInfoInstance.Instance.audioClips[3]);
    }

    public void OnPuzzleNameEdited(string text)
    {
        PuzzleInfoInstance.Instance.puzzleName = text;
    }

    public void OnPersonNameEdited(string text)
    {
        Debug.Log("OnPersonNameEdited:" + text);
        text.Trim();
        if (text != "")
        {
            PuzzleInfoInstance.Instance.names.Add(text);
        }
    }
}
