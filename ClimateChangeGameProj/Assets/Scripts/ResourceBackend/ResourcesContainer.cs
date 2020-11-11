using System.Collections.Generic;
using System.Xml.Serialization;

public class ResourcesContainer
{
    [XmlArray("Resources")]
    public List<Resource> resourcesContainers = new List<Resource>();
}