using UnityEngine;
using System.Collections;
using System.Xml.Serialization;
using System.IO;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;

public class PlayView : MonoBehaviour
{

    [SerializeField]
    GameObject tile;
    private static readonly int COLUMNS = 10;
    private static readonly int ROWS = 10;
    internal static readonly int EMPTY_TILE_INDEX = 52;
    internal static readonly int MAIN_SELECTED_TILE_INDEX = 53;
    internal static readonly int OTHER_SELECTED_TILE_INDEX = 54;

    public Sprite[] tileSprites;
    public List<GameObject> grid = new List<GameObject>();
    bool zoomedPic = true;
    Vector3 pictureScale;
    Vector3 picturePosition;

    public List<GameObject> alphabetGrid = new List<GameObject>();
    public Puzzle currentPuzzle = null;
    Sprite photograph;

    public List<int> letterLocations = new List<int>();

    public int letterLocation;
    public Word tempWord;

    // Use this for initialization
    void Start()
    {
        tileSprites = Resources.LoadAll<Sprite>("Word_Tiles");
        photograph = Resources.Load<Sprite>("Family-Pic");
        LoadPhotograph();
        CreateTiles();
        CreateAlphabetTiles();
        ReadPuzzle("Assets/Data/" + PuzzleInfoInstance.Instance.puzzleName + ".xml");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Pressed left click, casting ray.");
            CastRay();
        }
    }

    void CastRay()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
        // Debug.Log("Hit object: " + hit.collider.gameObject.name+" "+ hit.collider.gameObject.name.Trim().Equals("Family-Pic"));
        if (hit.collider != null && hit.collider.gameObject.name.Trim().Equals("Family-pic"))
        {
            // Debug.DrawLine(ray.origin, hit.point);
            if (zoomedPic)
            {
                hit.collider.gameObject.transform.Rotate(Vector3.forward * -90);
                hit.collider.gameObject.transform.localScale = new Vector3(2f, 1f, 0);
                hit.collider.gameObject.transform.position = new Vector3(0, 0, 0);
                zoomedPic = false;
            }
            else
            {
                hit.collider.gameObject.transform.Rotate(Vector3.forward * 90);
                hit.collider.gameObject.transform.localScale = pictureScale;
                hit.collider.gameObject.transform.position = picturePosition;
                zoomedPic = true;
            }

        }
    }

    void LoadPhotograph()
    {
        GameObject photObject = GameObject.FindGameObjectWithTag("picture");
        //rend.sprite = photograph;
        pictureScale = photObject.transform.localScale = new Vector3(1f, 0.56f, 0);
        picturePosition = photObject.transform.position = new Vector3(0, 6.5f, 0);
        // GetComponent<SpriteRenderer>().sprite = photograph;
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

    internal void ChangeTile(int alphabetPosition, bool isLastTile, Word word)
    {
        //CheckForNextPlayableTile();
       
        if (grid[letterLocation].GetComponent<Tile>().isPlayable)
        {
            grid[letterLocation].GetComponent<SpriteRenderer>().sprite = alphabetGrid[alphabetPosition].GetComponent<SpriteRenderer>().sprite;
            grid[letterLocation].GetComponent<Tile>().newValue = alphabetGrid[alphabetPosition].GetComponent<Tile>().originalValue;


            bool found = false;
            foreach (var index in letterLocations)
            {
                if (found)
                {
                    if (grid[index].GetComponent<Tile>().isPlayable)
                    {
                        grid[index].GetComponent<SpriteRenderer>().sprite = tileSprites[PlayView.MAIN_SELECTED_TILE_INDEX];
                       // letterLocation = index;
                        break;
                    }
                }
                if (index == letterLocation)
                {
                    found = true;
                }
            }
            alphabetGrid[alphabetPosition].SetActive(false);
        }
        if (isLastTile)
        {
            CheckWord(word);
        }

    }

    private void CheckForNextPlayableTile()
    {
        int i = letterLocations.IndexOf(letterLocation);
        i++;
        //letterLocation = letterLocations[i];
        if (!grid[letterLocations[i]].GetComponent<Tile>().isPlayable)
        {
            letterLocation = letterLocations[i];
            Debug.Log("Checking for next tile");
            CheckForNextPlayableTile();
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
        int index;
        System.Random rnd = new System.Random();
        for (int i = 0; i < 4; i++)
        {
            index = rnd.Next(0, 20);
            if (!alphabetGrid[index].activeSelf)
            {
                int letterIndex = rnd.Next(97, 124);
                //char letterValue = GetCharacterFromIndex(letterIndex);
                alphabetGrid[index].GetComponent<SpriteRenderer>().sprite = tileSprites[GetIndexFromLetter((char)letterIndex)];
                alphabetGrid[index].GetComponent<Tile>().position = index;
                alphabetGrid[index].GetComponent<Tile>().originalValue = GetIndexFromLetter((char)letterIndex);
                alphabetGrid[index].SetActive(true);
                break;
            }
        }
        while (true)
        {
            Debug.Log("Test");
            index = rnd.Next(0, 20);
            if (!alphabetGrid[index].activeSelf)
            {
                int letterIndex = GetIndexFromLetter(letter.value[0]);
                alphabetGrid[index].GetComponent<SpriteRenderer>().sprite = tileSprites[letterIndex];
                alphabetGrid[index].GetComponent<Tile>().position = index;
                alphabetGrid[index].GetComponent<Tile>().originalValue = letterIndex;
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
        System.Random rnd = new System.Random();
        // deserialize the file
        var serializer = new XmlSerializer(typeof(Puzzle));
        var stream = new FileStream(filePath, FileMode.Open);
        currentPuzzle = serializer.Deserialize(stream) as Puzzle;

        // set tiles for each letter of each word
        foreach (var word in currentPuzzle.words)
        {
            int lettersShown = 0;
            List<int> showTileIndex=new List<int>();
            showTileIndex.Add(rnd.Next(0, word.letters.Count));
            int i = 0;
            foreach (var letter in word.letters)
            {
                i++;
                if ((showTileIndex.IndexOf(i)!=-1 && (Math.Floor((float)word.letters.Count / 2) >= 3)) 
                    || (showTileIndex.IndexOf(i) != -1 && (Math.Floor((float)word.letters.Count / 2) < 3)))
                {
                    lettersShown++;
                    grid[letter.index].GetComponent<SpriteRenderer>().sprite = tileSprites[GetIndexFromLetter(letter.value[0])];
                    grid[letter.index].GetComponent<Tile>().newValue = GetIndexFromLetter(letter.value[0]);
                    grid[letter.index].GetComponent<Tile>().isPlayable = false;
                }
                else
                {
                    if(grid[letter.index].GetComponent<Tile>().isPlayable)
                    grid[letter.index].GetComponent<SpriteRenderer>().sprite = tileSprites[EMPTY_TILE_INDEX];
                   
                }
                grid[letter.index].GetComponent<Tile>().position = letter.index;
                grid[letter.index].GetComponent<Tile>().originalValue = GetIndexFromLetter(letter.value[0]);
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

    internal void CheckWord(Word word)
    {
        bool isCorrect = true;
        foreach (var letter in word.letters)
        {
            if (!(grid[letter.index].GetComponent<Tile>().originalValue == grid[letter.index].GetComponent<Tile>().newValue))
            {
                isCorrect = false;
                break;
            }
        }
        if (isCorrect)
        {
            foreach (var letter in word.letters)
            {
                grid[letter.index].GetComponent<Tile>().isPlayable = false;
               // grid[letter.index].GetComponent<SpriteRenderer>().sprite = tileSprites[(GetIndexFromLetter(letter.value[0]) + 26)];

            }
            Debug.Log("Word is correct");
        }
        else
        {
            Debug.Log("Word is not correct");
        }
    }
}

