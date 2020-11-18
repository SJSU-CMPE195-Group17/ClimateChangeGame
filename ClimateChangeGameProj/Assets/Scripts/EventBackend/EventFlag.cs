using System;
using System.Xml.Serialization;
using UnityEngine;

public class EventFlag
{
    [XmlElement("Index")]
    public int eventIndex;

    [XmlElement("Triggered")]
    public bool triggered;
}
