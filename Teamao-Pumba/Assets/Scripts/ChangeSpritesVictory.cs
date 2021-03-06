﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeSpritesVictory : MonoBehaviour
{
    public List<Sprite> highlightedSprites = new List<Sprite>();
    public List<Sprite> buttonSprites = new List<Sprite>();
    public List<Button> menuButtons = new List<Button>();
    public ScoreSelect menuSelect;

    void Awake()
    {
        menuSelect = menuSelect.GetComponent<ScoreSelect>();
    }


    void Update()
    {
        ChangeButtonSprites();
    }

    private void ChangeButtonSprites()
    {
        switch (menuSelect.CoordenadaPlayers[0])
        {
            case 0:
                menuButtons[0].GetComponent<Button>().image.sprite = highlightedSprites[0];
                menuButtons[1].GetComponent<Button>().image.sprite = buttonSprites[1];
                break;
            case 1:
                menuButtons[0].GetComponent<Button>().image.sprite = buttonSprites[0];
                menuButtons[1].GetComponent<Button>().image.sprite = highlightedSprites[1];
                break;
            default:
                menuButtons[0].GetComponent<Button>().image.sprite = highlightedSprites[0];
                menuButtons[1].GetComponent<Button>().image.sprite = buttonSprites[1];
                break;
        }
    }
}
