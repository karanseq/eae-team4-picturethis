﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.IO;
using System.Collections.Generic;
using System.Xml;
using UnityEngine.UI;

public class PuzzleSelect : MonoBehaviour {

    [SerializeField]
    Canvas canvas = null;

    private List<string> puzzleFileNames = new List<string>();

	// Use this for initialization
	void Start () {
        Transform[] ts = canvas.GetComponentsInChildren<Transform>();
        foreach (Transform transform in ts)
        {
            if (transform.gameObject.name.Contains("PuzzleButton"))
            {
                transform.gameObject.SetActive(false);
            }
        }
        PopulatePuzzleFileNames();
        CreatePuzzleList();
    }

    private void PopulatePuzzleFileNames()
    {
        string path = "";
        //if (Application.platform == RuntimePlatform.WindowsPlayer)
        //{
        //    path = "Assets/Data/";
        //}
        //else if (Application.platform == RuntimePlatform.Android)
        {
            path = Application.persistentDataPath + "/";
        }

        var dirInfo = new DirectoryInfo(path);
        var fileInfo = dirInfo.GetFiles();
        foreach (var file in fileInfo)
        {
            if (file.Extension.Equals(".xml"))
            {
                puzzleFileNames.Add(file.Name);
            }
        }
    }

    private void CreatePuzzleList()
    {
        string path = "";
        //if (Application.platform == RuntimePlatform.WindowsPlayer)
        //{
        //    path = "Assets/Data/";
        //}
        //else if (Application.platform == RuntimePlatform.Android)
        {
            path = Application.persistentDataPath + "/";
        }

        int index = 0;
        foreach (var fileName in puzzleFileNames)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(path + fileName);
            XmlElement root = xml.DocumentElement;
            var puzzleName = root.GetAttribute("name").ToString();

            Button puzzleButton = canvas.transform.GetChild(index).gameObject.GetComponent<Button>();
            puzzleButton.gameObject.SetActive(true);

            Text puzzleButtonText = puzzleButton.transform.GetChild(0).gameObject.GetComponent<Text>();
            puzzleButtonText.text = puzzleName;
            ++index;            
        }
    }

    public void OnPuzzleButtonClicked(Button puzzleButton)
    {
        Debug.Log("Puzzle selected:" + puzzleButton.transform.GetChild(0).gameObject.GetComponent<Text>().text);
        if (puzzleButton != null)
        {
            PuzzleInfoInstance.Instance.puzzleName = puzzleButton.transform.GetChild(0).gameObject.GetComponent<Text>().text;
            SceneManager.LoadScene("PlayView");

            AudioSource source = PuzzleInfoInstance.Instance.gameObject.GetComponent<AudioSource>();
            source.PlayOneShot(PuzzleInfoInstance.Instance.audioClips[3]);
        }
    }

    public void OnBackButtonClicked()
    {
        SceneManager.LoadScene("MainMenu");

        AudioSource source = PuzzleInfoInstance.Instance.gameObject.GetComponent<AudioSource>();
        source.PlayOneShot(PuzzleInfoInstance.Instance.audioClips[3]);
    }
}
