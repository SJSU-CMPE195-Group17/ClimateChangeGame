using System;
using System.Xml.Serialization;
using UnityEngine;

public class Resource
{
    [XmlElement("Name")]
    public string name;

    [XmlElement("Amount")]
    public int totalAmount;
}
