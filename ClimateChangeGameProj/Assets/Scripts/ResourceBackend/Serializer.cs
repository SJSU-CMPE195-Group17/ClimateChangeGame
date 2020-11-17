using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System.Xml.Serialization;

//To reset values, uncomment initializeXml method
public class Serializer : MonoBehaviour
{
    //Change path name to your ProjDir/Assets/Resources
    private const string path = "/users/isabellelow/Desktop/ClimateChangeGame/ClimateChangeGameProj/Assets/Resources/Resources.xml";

    void Start()
    {  
        //initializeXml();
    }

    void initializeXml() {
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
        XmlOperation.Serialize(resourcesContainer, Path.Combine(Application.persistentDataPath, path));
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
        XmlOperation.Serialize(resourcesContainer, Path.Combine(Application.persistentDataPath, path));
    }

    void loadXml() {
        // ResourcesContainer readResources = XmlOperation.Deserialize(path);
        // foreach(Resource resource in readResources.resourcesContainers) {
        //     Debug.Log(resource.totalAmount);
        // }
    }
}