﻿using UnityEngine;
using System.Collections;

public class Hazards : MonoBehaviour {
    public GameObject shotPrefab;
    public float InstantiationTimer = 1f;
    public int movementDimension;
    public float maxPolar;
    public float minPolar;
    public float maxElevation;
    public float minElevation;
    public float angularSpeed;
    public float radius;

    private float copy;
    private SphericalCoordinates curPos;
    private int sign;

    // Use this for initialization
    void Start () {
        copy = InstantiationTimer;
        minPolar = minPolar *Mathf.Deg2Rad;
        maxPolar = maxPolar *Mathf.Deg2Rad;
        minElevation = minElevation * Mathf.Deg2Rad;
        maxElevation *= Mathf.Deg2Rad;

        curPos = new SphericalCoordinates(radius, 0, 0, 1, radius, minPolar, maxPolar, minElevation, maxElevation);
        //curPos.FromCartesian(gameObject.transform.position);
        sign = 1;
        Debug.Log(transform.position);
        curPos.FromCartesian(transform.position);
        Debug.Log(curPos.toCartesian);
        //Debug.Log(minPolar);
        //Debug.Log(maxPolar);
        //Debug.Log(curPos.ToString());
    }

    // Update is called once per frame
    void Update () {
        InstantiationTimer -= Time.deltaTime;
        if (InstantiationTimer <= 0)
        {
            GameObject shot = Instantiate(shotPrefab, gameObject.transform.position, Quaternion.identity) as GameObject;
            LaserBehaivor b = shot.GetComponent<LaserBehaivor>();
            b.direction = 0;
            b.sign = 1;
            b.radius = radius - 1;
            InstantiationTimer = copy;
        }

        curPos.FromCartesian(gameObject.transform.position);
        Debug.Log(curPos.ToString());

        if (curPos.polar >= maxPolar)
        {
            sign = -1;
            //Debug.Log("sign changed to -1");
        }
        else if( curPos.polar <= minPolar)
        {
            sign = 1;
            //Debug.Log("sign changed to 1");
        }

        float radians = angularSpeed * Time.deltaTime * sign;
        Debug.Log(radians);
        curPos.RotatePolarAngle(radians);
        Debug.Log(curPos.ToString());
        //Debug.Log(transform.position);
        transform.position = curPos.toCartesian;
        //Debug.Log(transform.position);
    }


}