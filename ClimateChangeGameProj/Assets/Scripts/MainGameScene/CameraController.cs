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
    public const int NORTHERN_AFRICA = 8;
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
    public const int NORTH_POLE = 22;
    public const int ANTARCTICA = 23;

    private Vector3[] regionRotations;
    public Transform camTarget;
    public float moveSpeed = 2.0f;
    public float dist = 240;

    public int rotateDestination;
    public static CameraController instance;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        //defined here because Vector3s can only be initialized at runtime
        //format:   y= rotation horizontally, 0 = vertical line intersecting Europe and Africa
        //          x= rotation vertically, 0 = North pole, 180 = South pole
        regionRotations = new Vector3[24];
        regionRotations[NORTHERN_AMERICA] =     new Vector3(0, 240, 45);
        regionRotations[CENTRAL_AMERICA] =      new Vector3(0, 250, 70);
        regionRotations[CARIBBEAN] =            new Vector3(0, 270, 70);
        regionRotations[SOUTH_AMERICA] =        new Vector3(0, 285, 110);

        regionRotations[NORTHERN_EUROPE] =      new Vector3(0, 355, 30);
        regionRotations[WESTERN_EUROPE] =       new Vector3(0, 350, 42);
        regionRotations[SOUTHERN_EUROPE] =      new Vector3(0, 355, 50);
        regionRotations[EASTERN_EUROPE] =       new Vector3(0, 45, 35);

        regionRotations[NORTHERN_AFRICA] =      new Vector3(0, 0, 60);
        regionRotations[WESTERN_AFRICA] =       new Vector3(0, 345, 70);
        regionRotations[MIDDLE_AFRICA]=         new Vector3(0, 5, 90);
        regionRotations[EASTERN_AFRICA] =       new Vector3(0, 23, 90);
        regionRotations[SOUTHERN_AFRICA] =      new Vector3(0, 10, 115);

        regionRotations[WESTERN_ASIA] =         new Vector3(0, 30, 60);
        regionRotations[CENTRAL_ASIA] =         new Vector3(0, 52, 47);
        regionRotations[SOUTHERN_ASIA] =        new Vector3(0, 60, 64);
        regionRotations[EASTERN_ASIA] =         new Vector3(0, 90, 53);
        regionRotations[SOUTHEASTERN_ASIA] =    new Vector3(0, 95, 87);

        regionRotations[MELANESIA] =            new Vector3(0, 140, 95);
        regionRotations[POLYNESIA] =            new Vector3(0, 172, 102);
        regionRotations[AUSTRALIA_NZ] =         new Vector3(0, 130, 118);
        regionRotations[HAWAII] =               new Vector3(0, 188, 70);

        regionRotations[NORTH_POLE] =           new Vector3(0, 90, 5);
        regionRotations[ANTARCTICA] =           new Vector3(0, 0, 175);
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

