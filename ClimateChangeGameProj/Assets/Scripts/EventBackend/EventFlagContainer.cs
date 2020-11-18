using System;
using System.Collections.Generic;
using System.Xml.Serialization;

[XmlRoot("EventFlagList")]
public class EventFlagContainer
{
    [XmlArray("EventFlags")]
    public List<EventFlag> EventFlagContainers = new List<EventFlag>();
}
