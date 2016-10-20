using UnityEngine;
using System.Collections;
using System.Xml.Serialization;
using System.IO;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;

public class PlayView : MonoBehaviour
{
    [SerializeField]
    Button hintButton;

    [SerializeField]
    GameObject tile;

    [SerializeField]
    GameObject fireworkParticle;

    [SerializeField]
    GameObject burstParticle;

    private static readonly int COLUMNS = 10;
    private static readonly int ROWS = 10;
    internal static readonly int EMPTY_TILE_INDEX = 52;
    internal static readonly int MAIN_SELECTED_TILE_INDEX = 53;
    internal static readonly int OTHER_SELECTED_TILE_INDEX = 54;

    public Sprite[] tileSprites;
    public List<GameObject> grid = new List<GameObject>();
    bool toggleZoom = true;
    Vector3 initialPosition = Vector3.zero;

    public List<GameObject> alphabetGrid = new List<GameObject>();
    public Puzzle currentPuzzle = null;
    Sprite loadPic;
    public Image photograph;
    GameObject backButton;

    public List<int> letterLocations = new List<int>();

    public int currentLetterLocation;
    public Word tempWord;

    public AudioClip wordFinishCorrect;
    public AudioClip wordFinishWrong;
    public AudioClip puzzleCompleted;
    public AudioClip letterAdded;
    public AudioClip wordSelected;

    GameObject[] finishObjects;

    private bool isHintAvailable = false;
    private int hintTimer = 5;

    // Use this for initialization
    void Start()
    {

        backButton= GameObject.FindGameObjectWithTag("BackButton");
        tileSprites = Resources.LoadAll<Sprite>("Word_Tiles");
        finishObjects = GameObject.FindGameObjectsWithTag("GameOverScreen");
        HideFinishScreen();
        LoadImage();
        CreateTiles();
        CreateAlphabetTiles();
        ReadPuzzle("Assets/Data/" + PuzzleInfoInstance.Instance.puzzleName + ".xml");

        isHintAvailable = false;
        hintButton.interactable = false;
        Invoke("EnableHint", hintTimer);
    }

   

    private void LoadImage()
    {
        loadPic = Resources.Load<Sprite>("photo2");
        photograph.sprite = loadPic;
        initialPosition = photograph.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
     
    }

    

    public void ImageClick()
    {
       // GameObject backButton = ;
        if (toggleZoom)
        {
            photograph.transform.Rotate(Vector3.forward * -90);
            photograph.transform.localScale = new Vector3(0.96f, 0.96f, 1f);
            photograph.transform.position = new Vector3(5.45f, -0.02f, 0);
            toggleZoom = false;
            backButton.SetActive(false);
        }
        else
        {
            photograph.transform.Rotate(Vector3.forward * 90);
            photograph.transform.localScale = new Vector3(0.535f, 0.535f, 1);
            photograph.transform.position = initialPosition;
            toggleZoom = true;
            backButton.SetActive(true);
        }
    }

   

    private void CreateTiles()
    {
        var offset_x = -3.5f;
        var offset_y = 2f;

        for (int i = 0; i < ROWS; ++i)
        {
            for (int j = 0; j < COLUMNS; ++j)
            {
                GameObject newTile = Instantiate(tile) as GameObject;
                Tile tempTile = newTile.GetComponent<Tile>();
                tempTile.playView = this;

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
        if (grid[currentLetterLocation].GetComponent<Tile>().isPlayable)
        {
            grid[currentLetterLocation].GetComponent<SpriteRenderer>().sprite = alphabetGrid[alphabetPosition].GetComponent<SpriteRenderer>().sprite;
            grid[currentLetterLocation].GetComponent<Tile>().newValue = alphabetGrid[alphabetPosition].GetComponent<Tile>().originalValue;


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
                if (index == currentLetterLocation)
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
        int i = letterLocations.IndexOf(currentLetterLocation);
        i++;
        //letterLocation = letterLocations[i];
        if (!grid[letterLocations[i]].GetComponent<Tile>().isPlayable)
        {
            currentLetterLocation = letterLocations[i];
            Debug.Log("Checking for next tile");
            CheckForNextPlayableTile();
        }
        
    }

    private void CreateAlphabetTiles()
    {
        var offset_x = -3.75f;
        var offset_y = -7f;

        // TWO rows
        for (int i = 0; i < 2; ++i)
        {
            // SIX columns
            for (int j = 0; j < 6; ++j)
            {
                GameObject newTile = Instantiate(tile) as GameObject;
                newTile.transform.position = new Vector3(offset_x + j * 1.5f, offset_y - i * 1.25f, 0);
                newTile.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
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
            index = rnd.Next(0, 12);
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
            index = rnd.Next(0, 12);
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

        if (currentPuzzle == null)
        {
            // deserialize the file
            var serializer = new XmlSerializer(typeof(Puzzle));
            var stream = new FileStream(filePath, FileMode.Open);
            currentPuzzle = serializer.Deserialize(stream) as Puzzle;
        }

        photograph.sprite = Resources.Load<Sprite>("Pictures/" + currentPuzzle.picture);

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

        AudioSource source = PuzzleInfoInstance.Instance.gameObject.GetComponent<AudioSource>();
        source.PlayOneShot(PuzzleInfoInstance.Instance.audioClips[3]);
    }

    public void OnHintButtonClicked()
    {
        if (!isHintAvailable)
        {
            return;
        }
        isHintAvailable = false;
        hintButton.interactable = false;
        Invoke("EnableHint", hintTimer);

        foreach(var letter in tempWord.letters)
        {
            Debug.Log(grid[letter.index].GetComponent<Tile>().isPlayable+" "+ grid[letter.index].GetComponent<Tile>().newValue);
            if(grid[letter.index].GetComponent<Tile>().isPlayable && grid[letter.index].GetComponent<Tile>().newValue==-1)
            {
                grid[letter.index].GetComponent<SpriteRenderer>().sprite = tileSprites[GetIndexFromLetter(letter.value[0])];
                grid[letter.index].GetComponent<Tile>().newValue = GetIndexFromLetter(letter.value[0]);
                grid[letter.index].GetComponent<Tile>().isPlayable = false;
                foreach(var alphabet in alphabetGrid)
                {
                    if (grid[letter.index].GetComponent<Tile>().newValue == alphabet.GetComponent<Tile>().originalValue)
                    {
                       int index= alphabet.GetComponent<Tile>().position;
                        alphabetGrid[index].SetActive(false);
                        break;
                    }
                }
                
                break;
            }
        }
    }

    private void EnableHint()
    {
        if (isHintAvailable)
        {
            return;
        }

        isHintAvailable = true;
        hintButton.interactable = true;
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
            Vector3 centerPosition = Vector3.zero;
            int letterIndex = 0;
            foreach (var letter in word.letters)
            {
                grid[letter.index].GetComponent<Tile>().isPlayable = false;
               // grid[letter.index].GetComponent<SpriteRenderer>().sprite = tileSprites[(GetIndexFromLetter(letter.value[0]) + 26)];

                if (letterIndex == (word.letters.Count / 2))
                {
                    centerPosition = grid[letter.index].transform.position;
                }
                ++letterIndex;
            }
            Debug.Log("Word is correct");
            ShowBurstParticle(centerPosition);

            AudioSource source = PuzzleInfoInstance.Instance.gameObject.GetComponent<AudioSource>();
            source.PlayOneShot(PuzzleInfoInstance.Instance.audioClips[0]);

            AudioSource.PlayClipAtPoint(wordFinishCorrect, new Vector3(0, 0, 0));
            if (CheckPuzzleComplete())
            {
                fireworkParticle.SetActive(true);
                AudioSource.PlayClipAtPoint(puzzleCompleted, new Vector3(0, 0, 0));

                ShowFinishScreen();
            }
        }
        else
        {
            if(currentLetterLocation==word.letters[word.letters.Count-1].index)
            AudioSource.PlayClipAtPoint(wordFinishWrong, new Vector3(0, 0, 0));
            Debug.Log("Word is not correct");
        }
    }

    private bool CheckPuzzleComplete()
    {        
        foreach (var word in currentPuzzle.words)
        {            
            foreach (var letter in word.letters)
            {
               if (grid[letter.index].GetComponent<Tile>().isPlayable)
                {
                    goto PuzzleNotCompleted;
                }
            }
        }
        return true;

        PuzzleNotCompleted: return false;
    }

    private void HideFinishScreen()
    {
        foreach (GameObject g in finishObjects)
        {
            g.SetActive(false);
        }
    }

    private void ShowFinishScreen()
    {
        foreach (GameObject g in finishObjects)
        {
            g.SetActive(true);
        }
        ResetAlphabetTiles();
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("PuzzleSelect");
    }

    private void ShowBurstParticle(Vector3 position)
    {
        Debug.Log("Show burst particle at:" + position.x + "," + position.y);
        burstParticle.SetActive(true);
        burstParticle.transform.position = position;
        Invoke("HideBurstParticle", 2);
    }

    private void HideBurstParticle()
    {
        burstParticle.SetActive(false);
    }
}

