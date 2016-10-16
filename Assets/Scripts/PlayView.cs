using UnityEngine;
using System.Collections;
using System.Xml.Serialization;
using System.IO;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;

public class PlayView : MonoBehaviour {

    [SerializeField]
    GameObject tile;
    private static readonly int COLUMNS = 10;
    private static readonly int ROWS = 10;
    private static readonly int EMPTY_TILE_INDEX = 52;

    public Sprite[] tileSprites;
    public List<GameObject> grid = new List<GameObject>();

    public List<GameObject> alphabetGrid = new List<GameObject>();
    public Puzzle currentPuzzle = null;
    
    // Use this for initialization
    void Start () {
        tileSprites = Resources.LoadAll<Sprite>("Word_Tiles");        
        CreateTiles();
        CreateAlphabetTiles();
        ReadPuzzle("Assets/Data/WriteSample.xml");
    }
	
	// Update is called once per frame
	void Update () {

    }

    private void CreateTiles()
    {
        var offset_x = -3.5f;
        var offset_y = 3.5f;

        for (int i = 0; i < ROWS; ++i)
        {
            for (int j = 0; j < COLUMNS; ++j)
            {
                GameObject newTile = Instantiate(tile) as GameObject;
                newTile.transform.position = new Vector3(offset_x + j * 0.7f, offset_y - i * 0.7f, 0);
                newTile.name = "Tile" + i + "," + j;
                newTile.SetActive(false);
                grid.Add(newTile);
            }
        }
    }


    private void CreateAlphabetTiles()
    {
        var offset_x = -3.5f;
        var offset_y = -5.5f;

        for (int i = 0; i < 2; ++i)
        {
            for (int j = 0; j < COLUMNS; ++j)
            {
                GameObject newTile = Instantiate(tile) as GameObject;
                newTile.transform.position = new Vector3(offset_x + j * 0.7f, offset_y - i * 0.7f, 0);
                newTile.name = "AlphabetTile" + i + "," + j;
                newTile.SetActive(false);
                alphabetGrid.Add(newTile);
            }
        }
    }

    internal void LoadAlphabetTiles(Letter letter)
    {
        while(true)
        {
            System.Random rnd = new System.Random();
            int index = rnd.Next(0, 15);
            if (!alphabetGrid[index].activeSelf)
            {
                alphabetGrid[index].GetComponent<SpriteRenderer>().sprite = tileSprites[GetIndexFromLetter(letter.value[0])];
                //grid[letter.index].GetComponent<Tile>().position = letter.index;
                alphabetGrid[index].SetActive(true);
                break;
            }
        }
       
       
    }

    internal void ResetAlphabetTiles()
    {
        foreach (var alphabet in alphabetGrid)
        {
            alphabet.SetActive(false);
        }
    }

    private void ReadPuzzle(string filePath)
    {
        // deserialize the file
        var serializer = new XmlSerializer(typeof(Puzzle));
        var stream = new FileStream(filePath, FileMode.Open);
        currentPuzzle = serializer.Deserialize(stream) as Puzzle;

        // set tiles for each letter of each word
        foreach (var word in currentPuzzle.words)
        {
            foreach (var letter in word.letters)
            {
                grid[letter.index].GetComponent<SpriteRenderer>().sprite = tileSprites[GetIndexFromLetter(letter.value[0])];
                grid[letter.index].GetComponent<Tile>().position = letter.index;
                grid[letter.index].SetActive(true);
            }
        }
    }

    public int GetIndexFromLetter(char letter)
    {
        return ((int)letter) - 97;
    }

    public void OnBackButtonClicked()
    {
        SceneManager.LoadScene("PuzzleSelect");
    }
}

