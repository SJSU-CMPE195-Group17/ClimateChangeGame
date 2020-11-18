using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System.Xml.Serialization;

public class ClimateMetricSerializer : MonoBehaviour
{
    public const string path = "/Resources/ClimateMetrics.xml";
    // Start is called before the first frame update
    void Start()
    {
        if (!Directory.Exists(Application.persistentDataPath + "/Resources"))
            Directory.CreateDirectory(Application.persistentDataPath + "/Resources");
        if (!File.Exists(Application.persistentDataPath + path))
            initializeXml();
    }

    void initializeXml()
    {
        ClimateMetricContainer climateMetricContainer = new ClimateMetricContainer();
        climateMetricContainer.ClimateMetricContainers.Add(new ClimateMetric
        {
            name = "GlobalTemp",
            value = 1.78f
        });
        climateMetricContainer.ClimateMetricContainers.Add(new ClimateMetric
        {
            name = "OceanTemp",
            value = 1.25f
        });
        climateMetricContainer.ClimateMetricContainers.Add(new ClimateMetric
        {
            name = "SeaLevel",
            value = 94f
        });
        climateMetricContainer.ClimateMetricContainers.Add(new ClimateMetric
        {
            name = "IceSheets",
            value = 7200f
        });
        climateMetricContainer.ClimateMetricContainers.Add(new ClimateMetric
        {
            name = "Co2",
            value = 1050f
        });

        /*
        File is saved as Resources.xml under Assets/Resources directory in project
        Initialization of XML file
        */
        print("File Created" + Application.persistentDataPath + path);
        XmlOperation.Serialize(climateMetricContainer, Application.persistentDataPath + path);

    }

    public static void updateXml(float glblTempVal, float oceanTempVal, float seaLvlVal, float iceSheetVal, float co2Val)
    {
        ClimateMetricContainer climateMetricContainer = new ClimateMetricContainer();
        climateMetricContainer.ClimateMetricContainers.Add(new ClimateMetric
        {
            name = "GlobalTemp",
            value = glblTempVal
        });
        climateMetricContainer.ClimateMetricContainers.Add(new ClimateMetric
        {
            name = "OceanTemp",
            value = oceanTempVal
        });
        climateMetricContainer.ClimateMetricContainers.Add(new ClimateMetric
        {
            name = "SeaLevel",
            value = seaLvlVal
        });
        climateMetricContainer.ClimateMetricContainers.Add(new ClimateMetric
        {
            name = "IceSheets",
            value = iceSheetVal
        });
        climateMetricContainer.ClimateMetricContainers.Add(new ClimateMetric
        {
            name = "Co2",
            value = co2Val
        });

        /*
        File is saved as Resources.xml under Assets/Resources directory in project
        Initialization of XML file
        */
        if (!File.Exists(Application.persistentDataPath + path))
            Debug.LogWarning("File " + Application.persistentDataPath + path + " not found");
        XmlOperation.Serialize(climateMetricContainer, Application.persistentDataPath + path);
    }
}
