using System;
using System.Collections.Generic;
using System.Xml.Serialization;

[XmlRoot("ClimateMetricList")]
public class ClimateMetricContainer
{
    [XmlArray("ClimateMetrics")]
    public List<ClimateMetric> ClimateMetricContainers = new List<ClimateMetric>();
}