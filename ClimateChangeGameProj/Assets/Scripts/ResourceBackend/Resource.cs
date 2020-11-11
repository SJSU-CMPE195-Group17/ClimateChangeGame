﻿using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Resource
{
    [XmlElement("ResourceName")]
    public string Name;
    public double TotalAmount;
}
