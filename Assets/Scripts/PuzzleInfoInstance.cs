using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PuzzleInfoInstance : MonoBehaviour {

    public static PuzzleInfoInstance Instance
    {
        get;
        set;
    }

    public AudioClip[] audioClips = null;

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        Instance = this;
    }
	
	void Start ()
    {

    }

    public void ResetPuzzleInfo()
    {
        pictureName = "";
        puzzleName = "";
        names.Clear();
    }

    public string pictureName = "";
    public string puzzleName = "";
    public List<string> names = new List<string>();
}
