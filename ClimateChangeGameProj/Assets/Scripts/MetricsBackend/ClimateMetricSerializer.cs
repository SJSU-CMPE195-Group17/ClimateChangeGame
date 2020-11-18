using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System.Xml.Serialization;

public class ClimateMetricSerializer : MonoBehaviour
{
    public const string path = "/Resources/ClimateMetrics.xml";
    // Start is called before the first frame update
    void Awake()
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
            value = MainGameBackend.GLOBAL_TEMP_BASE
        });
        climateMetricContainer.ClimateMetricContainers.Add(new ClimateMetric
        {
            name = "OceanTemp",
            value = MainGameBackend.OCEAN_TEMP_BASE
        });
        climateMetricContainer.ClimateMetricContainers.Add(new ClimateMetric
        {
            name = "SeaLevel",
            value = MainGameBackend.SEA_LEVEL_BASE
        });
        climateMetricContainer.ClimateMetricContainers.Add(new ClimateMetric
        {
            name = "IceSheets",
            value = MainGameBackend.ICE_SHEET_BASE
        });
        climateMetricContainer.ClimateMetricContainers.Add(new ClimateMetric
        {
            name = "Co2",
            value = MainGameBackend.CO2_BASE
        });
        climateMetricContainer.ClimateMetricContainers.Add(new ClimateMetric
        {
            name = "GlobalTempDelta",
            value = MainGameBackend.GLOBAL_TEMP_DELTA_BASE
        });
        climateMetricContainer.ClimateMetricContainers.Add(new ClimateMetric
        {
            name = "OceanTempDelta",
            value = MainGameBackend.OCEAN_TEMP_DELTA_BASE
        });
        climateMetricContainer.ClimateMetricContainers.Add(new ClimateMetric
        {
            name = "SeaLevelDelta",
            value = MainGameBackend.SEA_LEVEL_DELTA_BASE
        });
        climateMetricContainer.ClimateMetricContainers.Add(new ClimateMetric
        {
            name = "IceSheetsDelta",
            value = MainGameBackend.ICE_SHEET_DELTA_BASE
        });
        climateMetricContainer.ClimateMetricContainers.Add(new ClimateMetric
        {
            name = "Co2Delta",
            value = MainGameBackend.CO2_DELTA_BASE
        });
        climateMetricContainer.ClimateMetricContainers.Add(new ClimateMetric
        {
            name = "Date",
            value = (float)MainGameBackend.START_YEAR + 0.25f*MainGameBackend.START_SEASON
        });
        /*
        File is saved as Resources.xml under Assets/Resources directory in project
        Initialization of XML file
        */
        print("File Created" + Application.persistentDataPath + path);
        XmlOperation.Serialize(climateMetricContainer, Application.persistentDataPath + path);

    }

    public static void updateXml(float glblTempVal, float oceanTempVal, float seaLvlVal, float iceSheetVal, float co2Val, 
        float glblTempValDelta, float oceanTempValDelta, float seaLvlValDelta, float iceSheetValDelta, float co2ValDelta, 
        int year, int season)
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
        climateMetricContainer.ClimateMetricContainers.Add(new ClimateMetric
        {
            name = "GlobalTempDelta",
            value = glblTempValDelta
        });
        climateMetricContainer.ClimateMetricContainers.Add(new ClimateMetric
        {
            name = "OceanTempDelta",
            value = oceanTempValDelta
        });
        climateMetricContainer.ClimateMetricContainers.Add(new ClimateMetric
        {
            name = "SeaLevelDelta",
            value = seaLvlValDelta
        });
        climateMetricContainer.ClimateMetricContainers.Add(new ClimateMetric
        {
            name = "IceSheetsDelta",
            value = iceSheetValDelta
        });
        climateMetricContainer.ClimateMetricContainers.Add(new ClimateMetric
        {
            name = "Co2Delta",
            value = co2ValDelta
        });

        float date = (float)year + 0.25f * season;
        climateMetricContainer.ClimateMetricContainers.Add(new ClimateMetric
        {
            name = "Date",
            value = date
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
