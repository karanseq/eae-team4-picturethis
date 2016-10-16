using UnityEngine;
using System.Collections;
using System.Xml.Serialization;
using System.IO;
using System.Collections.Generic;

public class PlayView : MonoBehaviour {

    [SerializeField]
    GameObject tile;
    private static readonly int COLUMNS = 10;
    private static readonly int ROWS = 10;
    private static readonly int EMPTY_TILE_INDEX = 52;

    private Sprite[] tileSprites;
    private List<GameObject> grid = new List<GameObject>();
    private Puzzle currentPuzzle = null;
    
    // Use this for initialization
    void Start () {
        tileSprites = Resources.LoadAll<Sprite>("Word_Tiles");        
        CreateTiles();

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
                grid[letter.index].SetActive(true);
            }
        }
    }

    private int GetIndexFromLetter(char letter)
    {
        return ((int)letter) - 97;
    }
}

