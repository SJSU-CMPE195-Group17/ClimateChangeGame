using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    
    //regions based off of https://upload.wikimedia.org/wikipedia/commons/0/08/United_Nations_geographical_subregions.png
    public const int NORTHERN_AMERICA = 0;
    public const int CENTRAL_AMERICA = 1;
    public const int CARIBBEAN = 2;
    public const int SOUTH_AMERICA = 3;
    public const int NORTHERN_EUROPE = 4;
    public const int WESTERN_EUROPE = 5;
    public const int SOUTHERN_EUROPE = 6;
    public const int EASTERN_EUROPE = 7;
    public const int NOTHERN_AFRICA = 8;
    public const int WESTERN_AFRICA = 9;
    public const int MIDDLE_AFRICA = 10;
    public const int EASTERN_AFRICA = 11;
    public const int SOUTHERN_AFRICA = 12;
    public const int WESTERN_ASIA = 13;
    public const int CENTRAL_ASIA = 14;
    public const int SOUTHERN_ASIA = 15;
    public const int EASTERN_ASIA = 16;
    public const int SOUTHEASTERN_ASIA = 17;
    public const int MELANESIA = 18;
    public const int POLYNESIA = 19;
    public const int AUSTRALIA_NZ = 20;
    public const int HAWAII = 21;

    private Vector3[] regionRotations;
    public Transform camTarget;
    public float moveSpeed = 2.0f;
    public float dist = 240;

    public int rotateDestination;

    private void Start()
    {
        regionRotations = new Vector3[22];
        //Northern America
        regionRotations[0] = new Vector3(0, 240, 45);
        //Central America
        regionRotations[1] = new Vector3(0, 250, 70);
        //Caribbean
        regionRotations[2] = new Vector3(0, 270, 70);
        //South America
        regionRotations[3] = new Vector3(0, 285, 110);
        //Northern Europe
        regionRotations[4] = new Vector3(0, 355, 30);
        //Western Europe
        regionRotations[5] = new Vector3(0, 350, 42);
        //Southern Europe
        regionRotations[6] = new Vector3(0, 355, 50);
        //Eastern Europe
        regionRotations[7] = new Vector3(0, 45, 35);
        //Northern Africa
        regionRotations[8] = new Vector3(0, 0, 60);
        //Western Africa
        regionRotations[9] = new Vector3(0, 345, 70);
        //Middle Africa
        regionRotations[10]= new Vector3(0, 5, 90);
        //Eastern Africa
        regionRotations[11] = new Vector3(0, 23, 90);
        //Southern Africa
        regionRotations[12] = new Vector3(0, 10, 115);
        //Western Asia
        regionRotations[13] = new Vector3(0, 30, 60);
        //Central Asia
        regionRotations[14] = new Vector3(0, 52, 47);
        //Southern Asia
        regionRotations[15] = new Vector3(0, 60, 64);
        //Eastern Asia
        regionRotations[16] = new Vector3(0, 90, 53);
        //Southeastern Asia
        regionRotations[17] = new Vector3(0, 95, 87);
        //Melanesia
        regionRotations[18] = new Vector3(0, 140, 95);
        //Polynesia
        regionRotations[19] = new Vector3(0, 172, 102);
        //Australia and NZ
        regionRotations[20] = new Vector3(0, 130, 118);
        //Hawaii
        regionRotations[21] = new Vector3(0, 188, 70);
    }

    // Start is called before the first frame update
    private void Update()
    {
        if (rotateDestination == -1)
        {
            transform.RotateAround(camTarget.position, Vector3.up, moveSpeed * Time.deltaTime);
            transform.LookAt(camTarget);
        }
        else
        {
            // from https://math.stackexchange.com/questions/989900/calculate-x-y-z-from-two-specific-degrees-on-a-sphere
            // and https://stackoverflow.com/questions/43187019/moving-camera-around-an-object-in-unity
            float angleH = regionRotations[rotateDestination].y;
            float angleV = regionRotations[rotateDestination].z;
            Vector3 tmp;
            tmp.x = Mathf.Sin(angleV * (Mathf.PI / 180)) * Mathf.Cos(angleH * (Mathf.PI / 180)) * dist + camTarget.position.x;
            tmp.z = Mathf.Sin(angleV * (Mathf.PI / 180)) * Mathf.Sin(angleH * (Mathf.PI / 180)) * dist + camTarget.position.z;
            tmp.y = Mathf.Cos(angleV * (Mathf.PI / 180)) * dist + camTarget.position.y;
            transform.position = Vector3.Slerp(transform.position, tmp, moveSpeed * Time.deltaTime);
            transform.LookAt(camTarget);
        }

    }
}

