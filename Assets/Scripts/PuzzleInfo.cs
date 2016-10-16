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
    }

    public void OnBackButtonClicked()
    {
        SceneManager.LoadScene("MainMenu");
        PuzzleInfoInstance.Instance.ResetPuzzleInfo();
    }

    public void OnPuzzleNameEdited(string text)
    {
        PuzzleInfoInstance.Instance.puzzleName = text;
        PuzzleInfoInstance.Instance.names.Add("karan");
        PuzzleInfoInstance.Instance.names.Add("jeremy");
        PuzzleInfoInstance.Instance.names.Add("bryan");
        PuzzleInfoInstance.Instance.names.Add("lulu");
        PuzzleInfoInstance.Instance.names.Add("ajay");
        PuzzleInfoInstance.Instance.names.Add("yukun");
    }

    public void OnPersonNameEdited(string text)
    {
        Debug.Log("OnPersonNameEdited:" + text);
        text.Trim();
        if (text != "")
        {
            //PuzzleInfoInstance.Instance.AddName(text);
        }
    }
}
