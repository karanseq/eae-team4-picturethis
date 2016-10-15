using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PuzzleView : MonoBehaviour
{
    [SerializeField]
    GameObject tile;

    private static readonly int COLUMNS = 10;
    private static readonly int ROWS = 10;

    private Sprite[] tileSprites;
    private List<GameObject> grid = new List<GameObject>();
    private List<string> words = new List<string>();
    private List<string> order;

    Crossword board = new Crossword(COLUMNS, ROWS);

    // Use this for initialization
    void Start ()
    {
        Debug.Log("Inside PuzzleView.Start...");
        tileSprites = Resources.LoadAll<Sprite>("Word_Tiles");

        AddTestWords();
        CreateTiles();

        words.Sort(Comparer);
        words.Reverse();
        order = words;
        GenerateCrossword();
    }
	
	// Update is called once per frame
	void Update ()
    {
        
    }

    private void AddTestWords()
    {
        words.Add("karan");
        //words.Add("sequeira");
        words.Add("jeremy");
        //words.Add("hodges");
        words.Add("bryan");
        //words.Add("sorensen");
        words.Add("lulu");
        //words.Add("hedrick");
        words.Add("yukun");
        //words.Add("peng");
        words.Add("ajay");
        //words.Add("satish");
    }

    static int Comparer(string a, string b)
    {
        var temp = a.Length.CompareTo(b.Length);
        return temp == 0 ? a.CompareTo(b) : temp;
    }

    private void CreateTiles()
    {
        var offset_x = -3.5f;
        var offset_y = 3.5f;

        for (int i = 0; i < board.N; ++i)
        {
            for (int j = 0; j < board.M; ++j)
            {
                GameObject newTile = Instantiate(tile) as GameObject;
                newTile.transform.position = new Vector3(offset_x + i * 0.7f, offset_y - j * 0.7f, 0);
                newTile.name = "Tile" + i + "," + j;
                //newTile.SetActive(false);
                grid.Add(newTile);
            }
        }
    }

    void GenerateCrossword()
    {
        board.Reset();
        ClearBoard();

        foreach (var word in order)
        {
            Debug.Log("Added word:" + board.AddWord(word));
        }

        ActualizeData();
    }

    void ActualizeData()
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

    void ClearBoard()
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

    private int GetIndexFromLetter(char letter)
    {
        return ((int)letter) - 97;
    }
}
