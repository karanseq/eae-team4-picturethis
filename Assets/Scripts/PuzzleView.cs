﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PuzzleView : MonoBehaviour
{
    [SerializeField]
    GameObject tile;
    [SerializeField]
    Image picture;

    private static readonly int COLUMNS = 10;
    private static readonly int ROWS = 10;

    private Sprite[] tileSprites;
    private List<GameObject> grid = new List<GameObject>();
    private List<string> words = new List<string>();
    private Dictionary<string, Tuple<int, int, int>> horizontalWords = new Dictionary<string, Tuple<int, int, int>>();
    private Dictionary<string, Tuple<int, int, int>> verticalWords = new Dictionary<string, Tuple<int, int, int>>();
    private List<string> order;

    Crossword board = new Crossword(COLUMNS, ROWS);

    // Use this for initialization
    void Start ()
    {
        Debug.Log("Inside PuzzleView.Start...");
        tileSprites = Resources.LoadAll<Sprite>("Word_Tiles");

        // initialize
        SetPicture();
        SetWords();
        CreateTiles();

        // generate
        GeneratePuzzle();
    }
	
	private void SetPicture()
    {
        //var picturePath = "Assets/Resources/Pictures/" + PuzzleInfoInstance.Instance.pictureName;
        picture.sprite = Resources.Load<Sprite>("Pictures/" + PuzzleInfoInstance.Instance.pictureName);
    }

    private void SetWords()
    {
        words = PuzzleInfoInstance.Instance.names;
    }

    static int Comparer(string a, string b)
    {
        var temp = a.Length.CompareTo(b.Length);
        return temp == 0 ? a.CompareTo(b) : temp;
    }

    private void CreateTiles()
    {
        var offset_x = -3.5f;
        var offset_y = 1.75f;

        for (int i = 0; i < board.N; ++i)
        {
            for (int j = 0; j < board.M; ++j)
            {
                GameObject newTile = Instantiate(tile) as GameObject;
                newTile.transform.position = new Vector3(offset_x + i * 0.7f, offset_y - j * 0.7f, 0);
                newTile.name = "Tile" + i + "," + j;
                newTile.SetActive(false);
                grid.Add(newTile);
            }
        }
    }

    private void GeneratePuzzle()
    {
        words.Sort(Comparer);
        words.Reverse();
        order = words;
        GenerateCrossword();
    }

    private void GenerateCrossword()
    {
        board.Reset();
        ClearBoard();

        foreach (var word in order)
        {
            var wordLocation = board.AddWord(word);
            switch (wordLocation.third)
            {
                case 0:
                    verticalWords.Add(word, wordLocation);
                    Debug.Log("Added vertical word:" + word + " at:" + wordLocation.first + "," + wordLocation.second);
                    break;
                case 1:
                    horizontalWords.Add(word, wordLocation);
                    Debug.Log("Added horizontal word:" + word + " at:" + wordLocation.first + "," + wordLocation.second);
                    break;
                default:
                    Debug.Log("Did not add word:" + word + " at:" + wordLocation.first + "," + wordLocation.second);
                    break;
            }
        }

        ActualizeData();
    }

    private void ActualizeData()
    {
        var boardcopy = board.GetBoard;
        var index = 0;

        for (var i = 0; i < board.N; ++i)
        {
            for (var j = 0; j < board.M; ++j)
            {
                var letter = boardcopy[i, j] == '*' ? ' ' : boardcopy[i, j];
                
                if (letter != ' ')
                {
                    grid[index].GetComponent<SpriteRenderer>().sprite = tileSprites[GetIndexFromLetter(letter)];
                    grid[index].SetActive(true);
                }

                ++index;
            }
        }
    }

    private void ClearBoard()
    {
        var index = 0;
        for (var i = 0; i < board.N; ++i)
        {
            for (var j = 0; j < board.M; ++j)
            {
                grid[index++].SetActive(false);
            }
        }
    }

    private void ExportPuzzle()
    {
        var puzzle = new Puzzle();
        puzzle.name = PuzzleInfoInstance.Instance.puzzleName;
        puzzle.picture = PuzzleInfoInstance.Instance.pictureName;

        // add horizontal words
        foreach (KeyValuePair<string, Tuple<int, int, int>> hWord in horizontalWords)
        {
            var wordString = hWord.Key;
            var wordLocation = hWord.Value;

            var word = new Word();
            var startIndex = wordLocation.first + wordLocation.second * COLUMNS;
            for (var i = 0; i < wordString.Length; ++i)
            {
                var letter = new Letter();
                letter.index = startIndex + i;
                letter.value = wordString[i] + "";
                word.letters.Add(letter);
            }
            puzzle.words.Add(word);
        }

        // add vertical words
        foreach (KeyValuePair<string, Tuple<int, int, int>> vWord in verticalWords)
        {
            var wordString = vWord.Key;
            var wordLocation = vWord.Value;

            var word = new Word();
            var startIndex = wordLocation.first + wordLocation.second * COLUMNS;
            for (var i = 0; i < wordString.Length; ++i)
            {
                var letter = new Letter();
                letter.index = startIndex + (i * COLUMNS);
                letter.value = wordString[i] + "";
                word.letters.Add(letter);
            }
            puzzle.words.Add(word);
        }

        var serializer = new XmlSerializer(typeof(Puzzle));
        FileStream stream = null;
        //if (Application.platform == RuntimePlatform.WindowsPlayer)
        //{
        //    stream = new FileStream("Assets/Data/" + PuzzleInfoInstance.Instance.puzzleName + ".xml", FileMode.Create);
        //}
        //else if (Application.platform == RuntimePlatform.Android)
        {
            stream = new FileStream(Application.persistentDataPath + "/" + PuzzleInfoInstance.Instance.puzzleName + ".xml", FileMode.Create);
        }
        serializer.Serialize(stream, puzzle);
        stream.Close();
    }

    public void OnBackButtonClicked()
    {
        PuzzleInfoInstance.Instance.ResetPuzzleInfo();
        SceneManager.LoadScene("MainMenu");

        AudioSource source = PuzzleInfoInstance.Instance.gameObject.GetComponent<AudioSource>();
        source.PlayOneShot(PuzzleInfoInstance.Instance.audioClips[3]);
    }

    public void OnResetButtonClicked()
    {
        horizontalWords.Clear();
        verticalWords.Clear();
        GeneratePuzzle();

        AudioSource source = PuzzleInfoInstance.Instance.gameObject.GetComponent<AudioSource>();
        source.PlayOneShot(PuzzleInfoInstance.Instance.audioClips[3]);
    }

    public void OnSaveButtonClicked()
    {
        ExportPuzzle();
        PuzzleInfoInstance.Instance.ResetPuzzleInfo();
        SceneManager.LoadScene("MainMenu");

        AudioSource source = PuzzleInfoInstance.Instance.gameObject.GetComponent<AudioSource>();
        source.PlayOneShot(PuzzleInfoInstance.Instance.audioClips[3]);
    }

    private int GetIndexFromLetter(char letter)
    {
        return ((int)letter) - 97;
    }
}
