using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Tile : MonoBehaviour {

    public int position;
    public int originalValue;
    public int newValue=-1;
    GameObject obj;
    PlayView playView;
    internal bool isPlayable=true;
    // Use this for initialization
    void Start () {
        obj = GameObject.FindGameObjectWithTag("PlayView");
        playView = obj.GetComponent<PlayView>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseDown()
    {
        if(isPlayable)
        {
            if (name.Contains("Alphabet"))
            {
                bool foundLocation = false;
                for(int i=0; i< playView.tempWord.letters.Count;i++)
                {
                    Letter letter = playView.tempWord.letters[i];
                    if (foundLocation)
                    {                     
                      //  playView.CheckWord(playView.tempWord);
                        playView.currentLetterLocation = SetLetterLocation(playView.tempWord.letters, i);
                        break;
                    }

                    if (letter.index == playView.currentLetterLocation)
                    {
                        if (i == playView.tempWord.letters.Count - 1)
                        {
                            // this is the last item, check for correct word
                            playView.ChangeTile(position, true, playView.tempWord);
                        }
                        else
                        {
                            foundLocation = true;
                            playView.ChangeTile(position, false, playView.tempWord);
                        }

                    }
                }
            }
            else
            {
                Puzzle currentPuzzle = playView.currentPuzzle;
                playView.ResetAlphabetTiles();
                foreach (var word in currentPuzzle.words)
                {
                    foreach (var letter in word.letters)
                    {
                        if (position == letter.index)
                        {
                            playView.tempWord = word;
                        }
                        if(playView.grid[letter.index].GetComponent<Tile>().isPlayable)
                        playView.grid[letter.index].GetComponent<SpriteRenderer>().sprite = playView.GetComponent<PlayView>().tileSprites[PlayView.EMPTY_TILE_INDEX];
                    }
                }
                SelectEmptyWordTiles(playView.tempWord, playView);
            }
        }
       
    }

    private int SetLetterLocation(List<Letter> letters, int i)
    {
        if(!playView.grid[letters[i].index].GetComponent<Tile>().isPlayable)
        {
            if (i >= letters.Count-1) {
                playView.currentLetterLocation = letters[i].index;
                playView.CheckWord(playView.tempWord);
                return letters[i].index;
            }
            else
            {
                i++;
                SetLetterLocation(letters, i);
            }
                
        }
       
        return letters[i].index;
    }

    
    private void SelectEmptyWordTiles(Word word, PlayView playView)
    {
        bool first = true;
        Letter firstLetter = new Letter();
        playView.letterLocations.Clear();
        bool mainTileFound = false;
        foreach (var letter in word.letters)
        {
            playView.letterLocations.Add(letter.index);
            if (first)
            {
                firstLetter = letter;
                first = false;
            }
           
            if (playView.grid[letter.index].GetComponent<Tile>().isPlayable)
            {
                playView.LoadAlphabetTiles(letter);
                playView.grid[letter.index].GetComponent<SpriteRenderer>().sprite = playView.GetComponent<PlayView>().tileSprites[PlayView.OTHER_SELECTED_TILE_INDEX];

            }
            if (playView.grid[letter.index].GetComponent<Tile>().isPlayable && !mainTileFound)
            {
                playView.currentLetterLocation = letter.index;
                mainTileFound = true;
                playView.grid[letter.index].GetComponent<SpriteRenderer>().sprite = playView.GetComponent<PlayView>().tileSprites[PlayView.MAIN_SELECTED_TILE_INDEX];
            }
            }
    }

       
}
