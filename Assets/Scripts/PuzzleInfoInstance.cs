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
        puzzleName = "";
        names.Clear();
    }

    public string puzzleName = "";
    public List<string> names = new List<string>();

   /* public string PuzzleName
    {
        get
        {
            return puzzleName;
        }
        set
        {
            puzzleName = value;
        }
    }

    public List<string> Names
    {
        get
        {
            return names;
        }
    }

    public void AddName(string name)
    {
        names.Add(name);
    }*/
}
