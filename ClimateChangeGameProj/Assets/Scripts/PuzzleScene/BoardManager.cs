using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Random=UnityEngine.Random;

public class BoardManager : MonoBehaviour
{
    public static BoardManager instance;     
    public List<Sprite> characters = new List<Sprite>();     //list of sprites used as tile pieces
    public GameObject tile;      // prefab instantiated 
    public int xSize, ySize;   // board dimensions
    private GameObject[,] tiles;      // 2d-array that stores tiles

    public bool IsActive { get; set; }
    public float timeRemaining;
    public bool timerIsRunning = false;
    public int totalScore;
    public int highestChain;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI chainText;

    public int moneyVal;
    public int scienceVal;
    public int globalCoopVal;
    public int educationVal;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI scienceText;
    public TextMeshProUGUI globalCoopText;
    public TextMeshProUGUI educationText;

    //Change path name to your ProjDir/Assets/Resources 
    const string LOCAL_PATH = Serializer.path;
    //private XDocument doc = XDocument.Load(DATABASE_PATH);

    private int updatedMoneyVal;
    private int updatedScienceVal;
    private int updatedGlobalCoopVal;
    private int updatedEducationVal;

    void Start () {
        instance = GetComponent<BoardManager>();     // 7
        moneyVal = 0;
        scienceVal = 0;
        globalCoopVal = 0;
        educationVal = 0;
        totalScore = 0;
        highestChain = 0;
        timeRemaining = 10.0f;
        timerIsRunning = true;
        IsActive = true;
        Vector2 offset = tile.GetComponent<SpriteRenderer>().bounds.size;
        CreateBoard(offset.x, offset.y);    
    }

    private void Update()
    {
        //print(rectTransform.rect);
        if(totalScore == 0)
            scoreText.text = "Score: 000" ;
        else
            scoreText.text = "Score: " + totalScore;
        chainText.text = "Best Chain: " + highestChain;
        if (timerIsRunning) {
            if (timeRemaining > 0) {
                timeRemaining -= Time.deltaTime;
                DisplayTimeMS(timeRemaining);
            }
            else {
                Debug.Log("Time's up!");
                timeRemaining = 0; // lock the timer so it doesn't turn negative
                timerIsRunning = false;
                IsActive = false;

                moneyText.text = "" + moneyVal + "M";
                scienceText.text = "" + scienceVal;
                globalCoopText.text = "" + globalCoopVal;
                educationText.text = "" + educationVal;

                int[] resourceValues = getResourceValues();
                updatedMoneyVal = resourceValues[0] + moneyVal;
                updatedScienceVal = resourceValues[1] + scienceVal;
                updatedGlobalCoopVal = resourceValues[2] + globalCoopVal;
                updatedEducationVal = resourceValues[3] + educationVal;
                
                Serializer.updateXml(updatedMoneyVal, updatedScienceVal, updatedGlobalCoopVal, updatedEducationVal);
            }
        }
    }

    // Display time left in minutes and seconds
    void DisplayTimeMS(float time) {
        time += 1;
        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);

        timeText.text = "Time: " + string.Format("{0:00}:{1:00}", minutes, seconds);
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
        
        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                GameObject newTile = Instantiate(tile, new Vector3(startX + (deltaX * x), startY + (deltaY * y), 0), tile.transform.rotation);
                tiles[x, y] = newTile;
                newTile.transform.parent = transform; // parents all tiles
                Sprite newSprite = characters[Random.Range(0, characters.Count)]; // randomly choose one of the sprites
                newTile.GetComponent<SpriteRenderer>().sprite = newSprite; // sets newly created sprite to the randomly chosen sprite
            }
        }
        
    }

    public Sprite getRandomSprite()
    {
        return characters[Random.Range(0, characters.Count)];
    }

    //Retrieve resource quantity from xml file
    private int getResourceValues(string resourceName) {
        XDocument doc = XDocument.Load(Application.persistentDataPath + LOCAL_PATH);
        List<XElement> allResources = doc.Root.Descendants().ToList();
        
        var result = allResources.Elements("Resource").
            First(x => x.Element("Name").Value.Equals(resourceName));

        string amountText = result.Element("Amount").Value;
        int parsedResourceAmount = Int32.Parse(amountText);
        
        return parsedResourceAmount;
    }

    private int[] getResourceValues()
    {
        XDocument doc = XDocument.Load(Application.persistentDataPath + LOCAL_PATH);
        List<XElement> allResources = doc.Root.Descendants().ToList();
        int[] resourceValues = new int[4];
        string[] resourceNames = { "Money", "Science", "Global Cooperation", "Education" };
        for (int i = 0; i < resourceValues.Length; i++)
        {
            var result = allResources.Elements("Resource").
                First(x => x.Element("Name").Value.Equals(resourceNames[i]));

            string amountText = result.Element("Amount").Value;
            int parsedResourceAmount = Int32.Parse(amountText);

            //print("GET Resource: " + resourceNames[i] + " Amount: " + parsedResourceAmount);
            resourceValues[i] = parsedResourceAmount;
        }
        return resourceValues;
    }
}
