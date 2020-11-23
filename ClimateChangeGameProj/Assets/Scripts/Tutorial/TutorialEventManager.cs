using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Xml.Serialization;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using UnityEngine.UI;

public class TutorialEventManager : MonoBehaviour
{
    public static TutorialEventManager instance;
    public TextMeshProUGUI title;
    public TextMeshProUGUI prompt;
    public GameObject[] buttons;
    public const string path = "/Resources/EventFlags.xml";
    
    // Start is called before the first frame update
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        if (!Directory.Exists(Application.persistentDataPath + "/Resources"))
            Directory.CreateDirectory(Application.persistentDataPath + "/Resources");
        if (!File.Exists(Application.persistentDataPath + path))
            initializeXml();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void initializeXml()
    {
        EventFlagContainer eventFlagContainer = new EventFlagContainer();
        int max = ClimateEventManager.MAX_NUMBER_OF_EVENTS;
        for (int i = 0; i < max; i++)
        {
            eventFlagContainer.EventFlagContainers.Add(new EventFlag
            {
                eventIndex = i,
                triggered = false
            });
        }

        print("File Created" + Application.persistentDataPath + path);
        XmlOperation.Serialize(eventFlagContainer, Application.persistentDataPath + path);
    }
}
