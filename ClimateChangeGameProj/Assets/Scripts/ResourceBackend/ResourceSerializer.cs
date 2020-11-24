using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System.Xml.Serialization;

public class ResourceSerializer : MonoBehaviour
{
    public static ResourceSerializer instance;
    public const string path = "/Resources/Resources.xml";

    void Awake()
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

    public void initializeXml() {
        ResourcesContainer resourcesContainer = new ResourcesContainer();
        resourcesContainer.resourcesContainers.Add(new Resource
        {
            name = "Money",
            totalAmount = 40
        });

        resourcesContainer.resourcesContainers.Add(new Resource
        {
            name = "Science",
            totalAmount = 40
        });

        resourcesContainer.resourcesContainers.Add(new Resource
        {
            name = "Global Cooperation",
            totalAmount = 40
        });   

        resourcesContainer.resourcesContainers.Add(new Resource
        {
            name = "Education",
            totalAmount = 40
        });

        /*
        File is saved as Resources.xml under Assets/Resources directory in project
        Initialization of XML file
        */
        print("File Created" + Application.persistentDataPath + path);
        XmlOperation.Serialize(resourcesContainer, Application.persistentDataPath + path);
        
    }

    public static void updateXml(int moneyAmt, int scienceAmt, int globalCoopAmt, int educationAmt) {
        ResourcesContainer resourcesContainer = new ResourcesContainer();
        resourcesContainer.resourcesContainers.Add(new Resource
        {
            name = "Money",
            totalAmount = moneyAmt
        });

        resourcesContainer.resourcesContainers.Add(new Resource
        {
            name = "Science",
            totalAmount = scienceAmt
        });

        resourcesContainer.resourcesContainers.Add(new Resource
        {
            name = "Global Cooperation",
            totalAmount = globalCoopAmt
        });   

        resourcesContainer.resourcesContainers.Add(new Resource
        {
            name = "Education",
            totalAmount = educationAmt
        });

        /*
        File is saved as Resources.xml under Assets/Resources directory in project
        Initialization of XML file
        */
        if (!File.Exists(Application.persistentDataPath + path))
            Debug.LogWarning("File " + Application.persistentDataPath + path + " not found");
        XmlOperation.Serialize(resourcesContainer, Application.persistentDataPath + path);
    }
}