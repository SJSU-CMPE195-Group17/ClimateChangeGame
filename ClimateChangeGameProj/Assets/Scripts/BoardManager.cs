using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public static BoardManager instance;     
        public List<Sprite> characters = new List<Sprite>();     //list of sprites used as tile pieces
        public GameObject tile;      // prefab instantiated 
        public int xSize, ySize;   // board dimensions

        private GameObject[,] tiles;      // 2d-array that stores tiles

        public bool IsShifting { get; set; }     // tells the game if a match is found and will refill if so

        void Start () {
            instance = GetComponent<BoardManager>();     // 7

            Vector2 offset = tile.GetComponent<SpriteRenderer>().bounds.size;
            CreateBoard(offset.x, offset.y);    
        }

        private void CreateBoard (float xOffset, float yOffset) {
            tiles = new GameObject[xSize, ySize];     

            float startX = transform.position.x - 3.5f;     // Finds starting position of board generation
            float startY = transform.position.y - 3.7f;

            //prevent matching 3 combos by checking the bottom and left side of new tile
            Sprite[] previousLeft = new Sprite[ySize];
            Sprite previousBelow = null;

            for (int x = 0; x < xSize; x++) {      
                for (int y = 0; y < ySize; y++) {
                    GameObject newTile = Instantiate(tile, new Vector3(startX + (xOffset * x), startY + (yOffset * y), 0), tile.transform.rotation);
                    tiles[x, y] = newTile;
                    newTile.transform.parent = transform; // parents all tiles
                    List<Sprite> possibleCharacters = new List<Sprite>(); 
                    possibleCharacters.AddRange(characters); // adds all the possible characters to the list

                    possibleCharacters.Remove(previousLeft[y]); // removes characters that appeared on the bottom and left side of new tile
                    possibleCharacters.Remove(previousBelow);

                    Sprite newSprite = possibleCharacters[Random.Range(0, possibleCharacters.Count)]; // randomly choose one of the sprites
                    newTile.GetComponent<SpriteRenderer>().sprite = newSprite; // sets newly created sprite to the randomly chosen sprite
                    previousLeft[y] = newSprite;
                    previousBelow = newSprite;
                }
            }
        }
}
