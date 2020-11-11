using System.IO;
using UnityEngine;


public class Serializer : MonoBehaviour
{
    void Start()
    {
        ResourcesContainer resourcesContainer = new ResourcesContainer();
        resourcesContainer.resourcesContainers.Add(new Resource
        {
            Name = "Money",
            TotalAmount = 0
        });

        resourcesContainer.resourcesContainers.Add(new Resource
        {
            Name = "Scientific Resources",
            TotalAmount = 0
        });

        resourcesContainer.resourcesContainers.Add(new Resource
        {
            Name = "Earth Resources",
            TotalAmount = 0
        });   

        resourcesContainer.resourcesContainers.Add(new Resource
        {
            Name = "Knowledge",
            TotalAmount = 0
        });

        XmlOperation.Serialize(resourcesContainer, Path.Combine(Application.persistentDataPath, "Resources.xml"));
    }
}