using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BoardManager : MonoBehaviour
{
    public static BoardManager instance;     
    public List<Sprite> characters = new List<Sprite>();     //list of sprites used as tile pieces
    public GameObject tile;      // prefab instantiated 
    public int xSize, ySize;   // board dimensions
    private GameObject[,] tiles;      // 2d-array that stores tiles

    public bool IsShifting { get; set; }     // tells the game if a match is found and will refill if so
    public int totalScore;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timeText;

    void Start () {
        instance = GetComponent<BoardManager>();     // 7

        totalScore = 0;
        Vector2 offset = tile.GetComponent<SpriteRenderer>().bounds.size;
        CreateBoard(offset.x, offset.y);    
    }

    private void Update()
    {
        //print(rectTransform.rect);
        scoreText.text = "Score: " + totalScore;
    }

    private void CreateBoard (float xOffset, float yOffset) {
        tiles = new GameObject[xSize, ySize];
        /* //for tiles that span board, uneven spacing
        float bottomLeftX = transform.position.x - 6.7f;
        float bottomLeftY = transform.position.y - 4.0f;

        float topRightX = transform.position.x + 6.7f;
        float topRightY = transform.position.y + 4.0f;

        float deltaX = (topRightX - bottomLeftX) / (xSize);
        float deltaY = (topRightY - bottomLeftY) / (ySize);

        float startX = bottomLeftX;     // Finds starting position of board generation
        float startY = bottomLeftY;
        */
        float deltaX = xOffset;
        float deltaY = yOffset;

        float startX = transform.position.x - ((xSize - 1) * deltaX / 2.0f);
        float startY = transform.position.y - ((ySize - 1) * deltaY / 2.0f);

        //prevent matching 3 combos by checking the bottom and left side of new tile
        //Sprite[] previousLeft = new Sprite[ySize];
        //Sprite previousBelow = null;
        
        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                GameObject newTile = Instantiate(tile, new Vector3(startX + (deltaX * x), startY + (deltaY * y), 0), tile.transform.rotation);
                tiles[x, y] = newTile;
                newTile.transform.parent = transform; // parents all tiles
                List<Sprite> possibleCharacters = new List<Sprite>();
                possibleCharacters.AddRange(characters); // adds all the possible characters to the list

                //possibleCharacters.Remove(previousLeft[y]); // removes characters that appeared on the bottom and left side of new tile
                //possibleCharacters.Remove(previousBelow);

                Sprite newSprite = possibleCharacters[Random.Range(0, possibleCharacters.Count)]; // randomly choose one of the sprites
                newTile.GetComponent<SpriteRenderer>().sprite = newSprite; // sets newly created sprite to the randomly chosen sprite
                //previousLeft[y] = newSprite;
                //previousBelow = newSprite;
            }
        }
        
    }
}
