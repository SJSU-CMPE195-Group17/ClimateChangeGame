using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardTile : MonoBehaviour
{
    private static Color selectedColor = new Color(.5f, .5f, .5f, 1.0f);
    private static BoardTile globalLastTileSelected = null;
    private BoardTile previousSelected = null;
    private BoardTile nextSelected = null;
    private int numInChain = 0;

    //belle
    // Vector2 nw = Vector2(-1,1);
    // Vector2 sw = Vector2(-1,-1);
    // Vector2 ne = Vector2(1,1);
    // Vector2 se = Vector2(1,-1);
    private Vector2[] allDirections = new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right};
    
    private GameObject bm;
    private BoardManager bmScript;
    float timeLeft;

    private SpriteRenderer render;

    private enum tileState { neutral, selected, randomizing };
    private tileState currState;
    private float randomizingTimer = 0.0f;

    private bool mouseDown;

    public Transform explosion;
    public Transform spawnEmitter;
    private bool emitterStarted = false;

    //initialize reference to Puzzle game object, BoardManager script and timeRemaining
    void Start() {
        bm = GameObject.Find("Puzzle");
        bmScript = bm.GetComponent<BoardManager>();
        timeLeft = bmScript.timeRemaining;
    }

    void Update()
    {
        mouseDown = Input.GetMouseButton(0);

        timeLeft = bmScript.timeRemaining;

        //if time is up and there are still some tiles highlighted, deselect them
        if (timeLeft == 0) {
            SetDeselected();
        }

        if (currState == tileState.randomizing)
        {
            if (randomizingTimer < 1.1 && spawnEmitter && !emitterStarted)
            {
                GameObject emitter = ((Transform)Instantiate(spawnEmitter, this.transform.position, this.transform.rotation)).gameObject;
                Destroy(emitter, 2f);
                emitterStarted = true;
            }
            if (randomizingTimer > 0)
            {
                randomizingTimer -= Time.deltaTime;
            }
            else
            {
                //Debug.Log("Tile Timer finished");
                SetDeselected();
                randomizingTimer = 0.0f; // lock the timer so it doesn't turn negative
                currState = tileState.neutral;
                RandomizeSprite();
                emitterStarted = false;
            }
        }

        if (!mouseDown && (globalLastTileSelected == this) && BoardManager.instance.IsActive)
        {
            ResolveDeselect();
        }

    }

    void Awake()
    {
        render = GetComponent<SpriteRenderer>();
    }

    private void Select()
    {
        
        if(currState == tileState.randomizing)
        {
            if(globalLastTileSelected != null)
                ResolveDeselect();
        }
        //this tile has already been selected
        else if (currState == tileState.selected)
        {
            print("already Selected: " + numInChain);
            if (nextSelected != null)
            {
                DeselectChainForwards(nextSelected);
                globalLastTileSelected = this;
            }
        }
        //no chain has begun
        else if(globalLastTileSelected == null)
        {
            currState = tileState.selected;
            render.color = selectedColor;
            numInChain = 1;
            //print("new global tile: " + numInChain);
            globalLastTileSelected = gameObject.GetComponent<BoardTile>();
        }
        //chain has begun and render matches
        else if(globalLastTileSelected.render.sprite == render.sprite)
        {

            currState = tileState.selected;
            render.color = selectedColor;
            //set this local tiles previous
            previousSelected = globalLastTileSelected;
            //increment numInChain
            numInChain = previousSelected.numInChain + 1;
            //set global last tile selected tot his
            globalLastTileSelected = gameObject.GetComponent<BoardTile>();
            //set local previous's next to this
            previousSelected.nextSelected = globalLastTileSelected;

            //print("new tile to chain: " + numInChain);
        }
        //chain has begun but render does not match
        else
        {
            ResolveDeselect();
        }

        //SFXManager.instance.PlaySFX(Clip.Select);
    }

    private void ResolveDeselect()
    {
        if (globalLastTileSelected.numInChain > 2)
        {
            Score(globalLastTileSelected);
            float timerSet = 0.0f;
            if(numInChain == 3)
            {
                timerSet = 7.0f;
            }
            else if(numInChain < 6)
            {
                timerSet = 5.0f;
            }
            else
            {
                timerSet = 7.0f / numInChain;
            }

            RandomizingChainBackwards(globalLastTileSelected, timerSet);
        }
        else
        {
            DeselectChainBackwards(globalLastTileSelected);
        }
        globalLastTileSelected = null;
    }

    private void SetDeselected()
    {
        numInChain = 0;
        currState = tileState.neutral;
        render.color = Color.white;
    }

    private void DeselectChainBackwards(BoardTile bt)
    {
        bt.nextSelected = null;
        bt.SetDeselected();
        if (bt.previousSelected != null)
        {
            bt.DeselectChainBackwards(bt.previousSelected);
            bt.previousSelected = null;
        }
    }

    private void DeselectChainForwards(BoardTile bt)
    {
        bt.previousSelected = null;
        bt.SetDeselected();
        if (bt.nextSelected != null)
        {
            bt.DeselectChainForwards(bt.nextSelected);
            bt.nextSelected = null;
        }
    }

    private void SetRandomizing(float time)
    {
        //print("Set rerolling: " + numInChain + " timer:" + time);
        numInChain = 0;
        currState = tileState.randomizing;
        randomizingTimer = time;
        render.color = new Color(1f,1f,1f,0f);
        if (explosion)
        {
            GameObject exploder = ((Transform)Instantiate(explosion, this.transform.position, this.transform.rotation)).gameObject;
            Destroy(exploder, 2.0f);
        }
    }

    private void RandomizingChainBackwards(BoardTile bt, float time)
    {
        bt.nextSelected = null;
        bt.SetRandomizing(time);
        if (bt.previousSelected != null)
        {
            bt.RandomizingChainBackwards(bt.previousSelected, time);
            bt.previousSelected = null;
        }
    }

    private void Score(BoardTile bt)
    {
        int scaledScore;
        if (bt.numInChain < 7)
        {
            scaledScore = (bt.numInChain - 3) * 2 + 3;
        }
        else if(bt.numInChain < 13)
        {
            scaledScore = (bt.numInChain - 3) * 3 + 3;
        }
        else
        {
            scaledScore = (bt.numInChain - 3) * 4 + 3;
        }

        //Resource Allocation
        if(globalLastTileSelected.render.sprite.name == "P_Education")
        {
            print("Education " + scaledScore);
            BoardManager.instance.educationVal += scaledScore;
        }
        else if(globalLastTileSelected.render.sprite.name == "P_Coop")
        {
            print("Coop " + scaledScore);
            BoardManager.instance.globalCoopVal += scaledScore;
        }
        else if (globalLastTileSelected.render.sprite.name == "P_Money")
        {
            print("Money " + scaledScore);
            BoardManager.instance.moneyVal += scaledScore;
        }
        else if (globalLastTileSelected.render.sprite.name == "P_Science")
        {
            print("Science " + scaledScore);
            BoardManager.instance.scienceVal += scaledScore;
        }
        else
        {

            for(int i = 0; i < scaledScore; i++)
            {
                int r = Random.Range(0, 4);
                switch(r)
                {
                    case 0:
                        print("Education 1");
                        BoardManager.instance.educationVal++;
                        break;
                    case 1:
                        print("Coop 1");
                        BoardManager.instance.globalCoopVal++;
                        break;
                    case 2:
                        print("Money 1");
                        BoardManager.instance.moneyVal++;
                        break;
                    case 3:
                        print("Science 1");
                        BoardManager.instance.scienceVal++;
                        break;

                }
            }
            print("Other" + scaledScore);
        }

        BoardManager.instance.totalScore += scaledScore * 100;
        if (bt.numInChain > BoardManager.instance.highestChain)
            BoardManager.instance.highestChain = numInChain;
    }

    private void RandomizeSprite()
    {
        GetComponent<SpriteRenderer>().sprite = BoardManager.instance.getRandomSprite();
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

    //belle
    // private BoardTile GetSurrounding(Vector2 castDir) {
    //     RaycastHit2D hit = Physics2D.Raycast(transform.position, castDir);
    //     if (hit.collider != null) {
    //         return hit.collider.BoardTile;
    //     }
    //     return null;
    // }

    // //belle
    // private List<BoardTile> GetAllSurroundingTiles() {
    //     List<BoardTile> surrTiles = new List<BoardTile>();
    //     for (int i = 0; i < allDirections.Length; i++) {
    //         surrTiles.Add(GetSurrounding(allDirections[i]));
    //     }
    //     return surrTiles;
    // }
}
