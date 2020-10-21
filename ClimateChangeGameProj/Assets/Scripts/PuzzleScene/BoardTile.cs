using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardTile : MonoBehaviour
{
    private static Color selectedColor = new Color(.5f, .5f, .5f, 1.0f);
    private static BoardTile globalLastTileSelected = null;
    private BoardTile previousSelected = null;
    private int numInChain = 0;

    private SpriteRenderer render;
    private bool isSelected = false;
    private bool mouseDown;

    void Update()
    {
        mouseDown = Input.GetMouseButton(0);

        if(!mouseDown && isSelected && BoardManager.instance.IsActive)
        {
            Score(globalLastTileSelected);
            DeselectChain(globalLastTileSelected);
        }

    }

    void Awake()
    {
        render = GetComponent<SpriteRenderer>();
    }

    private void Select()
    {
        if(isSelected)
        {
            print("already Selected: " + numInChain);
        }
        //no chain has begun
        else if(globalLastTileSelected == null)
        {
            isSelected = true;
            render.color = selectedColor;
            numInChain = 1;
            print("new global tile: " + numInChain);
            globalLastTileSelected = gameObject.GetComponent<BoardTile>();
        }
        //chain has begun and render matches
        else if(globalLastTileSelected.render.sprite == render.sprite)
        {
            
            isSelected = true;
            render.color = selectedColor;
            previousSelected = globalLastTileSelected;
            numInChain = previousSelected.numInChain + 1;
            print("new tile to chain: " + numInChain);
            globalLastTileSelected = gameObject.GetComponent<BoardTile>();
        }
        //chain has begun but render does not match
        else
        {
            Score(globalLastTileSelected);
            DeselectChain(globalLastTileSelected);
        }

        //SFXManager.instance.PlaySFX(Clip.Select);
    }

    private void Deselect()
    {
        numInChain = 0;
        isSelected = false;
        render.color = Color.white;
        globalLastTileSelected = null;
    }

    private void DeselectChain(BoardTile bt)
    {
        bt.Deselect();
        if (bt.previousSelected != null)
        {
            bt.DeselectChain(bt.previousSelected);
            bt.previousSelected = null;
        }
    }

    private void Score(BoardTile bt)
    {
        if (bt.numInChain > 2)
        {
            print("Score: " + bt.numInChain + " " + globalLastTileSelected.render.sprite.name);
            BoardManager.instance.totalScore += bt.numInChain;
        }
    }

    private void OnMouseDown()
    {
        if (render.sprite == null || !BoardManager.instance.IsActive)
        {
            return;
        }
        Select();
    }

    private void OnMouseEnter()
    {
        if (render.sprite == null || !BoardManager.instance.IsActive || !mouseDown)
        {
            return;
        }
        Select();
    }
}
