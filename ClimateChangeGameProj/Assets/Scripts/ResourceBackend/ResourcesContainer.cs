using System;
using System.Collections.Generic;
using System.Xml.Serialization;

[XmlRoot("ResourcesList")]
public class ResourcesContainer
{
    [XmlArray("Resources")]
    public List<Resource> resourcesContainers = new List<Resource>();
}