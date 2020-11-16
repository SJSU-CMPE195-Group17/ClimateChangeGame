using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System.Xml.Serialization;

//To retrieve and update values, go to MainGameBackend.
public class Serializer : MonoBehaviour
{
    private const string path = "/users/isabellelow/Desktop/ClimateChangeGame/ClimateChangeGameProj/Assets/Resources/Resources.xml";

    void Start()
    {
        // updateXml(40, 40, 40, 40);
    }

    void initializeXml() {
        ResourcesContainer resourcesContainer = new ResourcesContainer();
        resourcesContainer.resourcesContainers.Add(new Resource
        {
            name = "Money",
            totalAmount = 0
        });

        resourcesContainer.resourcesContainers.Add(new Resource
        {
            name = "Science",
            totalAmount = 0
        });

        resourcesContainer.resourcesContainers.Add(new Resource
        {
            name = "Global Cooperation",
            totalAmount = 0
        });   

        resourcesContainer.resourcesContainers.Add(new Resource
        {
            name = "Education",
            totalAmount = 0
        });

        /*
        File is saved as Resources.xml under Assets/Resources directory in project
        Initialization of XML file
        */
        XmlOperation.Serialize(resourcesContainer, Path.Combine(Application.persistentDataPath, path));
    }

    void updateXml(int moneyAmt, int scienceAmt, int globalCoopAmt, int educationAmt) {
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
        XmlOperation.Serialize(resourcesContainer, Path.Combine(Application.persistentDataPath, path));
    }

    void loadXml() {
        // ResourcesContainer readResources = XmlOperation.Deserialize(path);
        // foreach(Resource resource in readResources.resourcesContainers) {
        //     Debug.Log(resource.totalAmount);
        // }
    }
}