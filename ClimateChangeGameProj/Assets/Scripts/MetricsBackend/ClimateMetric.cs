using System;
using System.Xml.Serialization;
using UnityEngine;

public class ClimateMetric
{
    [XmlElement("Name")]
    public string name;

    [XmlElement("Value")]
    public float value;
}
