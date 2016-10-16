using UnityEngine;
using System.Collections;
using System;

public class Tile : MonoBehaviour {

    public int position;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseDown()
    {
        GameObject playView = GameObject.FindGameObjectWithTag("PlayView");
        Puzzle currentPuzzle= playView.GetComponent<PlayView>().currentPuzzle;
        playView.GetComponent<PlayView>().ResetAlphabetTiles();
        Word tempWord=new Word();
        foreach (var word in currentPuzzle.words)
        {
            foreach (var letter in word.letters)
            {
                if(position==letter.index)
                {
                    tempWord = word;
                }
                playView.GetComponent<PlayView>().grid[letter.index].GetComponent<SpriteRenderer>().sprite = playView.GetComponent<PlayView>().tileSprites[playView.GetComponent<PlayView>().GetIndexFromLetter(letter.value[0])];
            }
        }
        SelectWordTiles(tempWord, playView);
    }

    private void SelectWordTiles(Word word, GameObject playView)
    {
        foreach (var letter in word.letters)
        {
            playView.GetComponent<PlayView>().LoadAlphabetTiles(letter);
            playView.GetComponent<PlayView>().grid[letter.index].GetComponent<SpriteRenderer>().sprite = playView.GetComponent<PlayView>().tileSprites[53];
        }
    }

       
}
