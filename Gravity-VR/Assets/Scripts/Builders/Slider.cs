﻿using UnityEngine;
using System.Collections;
using VolumetricLines;

public class Slider : MonoBehaviour {
    //public Vector3 movement;
    public int movementDimension = 0; // 0 = x, 1 = y
    public float radius = 30f;
    public float minPolar = -15;
    public float maxPolar = 15;
    public float minElevation = -15;
    public float maxElevation = 15;
    public float angularSpeed = .1f;

    public bool selected = false;

    public Color startColor;
    public Color lookAtColor;
    public Color selectedColor;

    private bool seen = false;
    private int sign = 0;


    //public float curDegreesMoved = 0;
    //Color original;
    public SphericalCoordinates startPosition;
    public SphericalCoordinates curPosition;
    static GameObject[] sliders;

	// Use this for initialization
	void Start () {
        startPosition = new SphericalCoordinates(radius, 0, 0, 1, radius + .3f, minPolar*Mathf.Deg2Rad, maxPolar * Mathf.Deg2Rad, minElevation * Mathf.Deg2Rad, maxElevation * Mathf.Deg2Rad);
        startPosition.loopPolar = false;
        startPosition.FromCartesian(transform.position);
        curPosition = new SphericalCoordinates(radius, 0, 0, 1, radius + .3f, minPolar * Mathf.Deg2Rad, maxPolar * Mathf.Deg2Rad, minElevation * Mathf.Deg2Rad, maxElevation * Mathf.Deg2Rad);
        curPosition.loopPolar = false;
        curPosition.FromCartesian(transform.position);
        sliders = GameObject.FindGameObjectsWithTag("Slider");
    }

    void Update()
    {
        if (Input.GetButtonDown("X")) {
            //Debug.Log("X pressed");
            Select();
        } else if (Input.GetButtonDown("Circle"))
        {
            //Debug.Log("Circle pressed");
            Deselect();
        }

        if (selected)
            ProcessMovement();
    }

    void ProcessMovement()
    {
        float horizontal = -Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (movementDimension == 0 && horizontal != 0)
        {
            sign = horizontal > 0 ? 1 : -1;
            float degrees = angularSpeed * Time.deltaTime * sign;
            //Debug.Log(degrees);
            //startPostion.polar - curPosition.polar + degrees;
            curPosition.RotatePolarAngle(degrees);
            //Debug.Log("ENTERED MOVEMENT CONDITION");
            //Debug.Log(curPosition.toCartesian);
            transform.position = curPosition.toCartesian;

        } else if (movementDimension == 1 && vertical !=0)
        {
            sign = vertical > 0 ? 1 : -1;
            float degrees = angularSpeed * Time.deltaTime * sign;
            curPosition.RotateElevationAngle(degrees);
            transform.position = curPosition.toCartesian;
        } else
        {
            sign = 0;
        }

        
    }

    /*void OnMouseDown()
    {
        selected = !selected;
    }
    */

    public void Select()
    {
        if (seen)
        {
            for (int i = 0; i < sliders.Length; i++)
            {
                sliders[i].GetComponent<Slider>().Deselect();
            }
            selected = true;
            setLineColors(selectedColor);
        }
    }

    public void Deselect()
    {
        selected = false;
        Color color = seen ? lookAtColor : startColor;
        setLineColors(color);
    }

    public void OnPointer()
    {
        seen = !seen;
        if (!selected)
        {
            Color color = seen ? lookAtColor : startColor;
            setLineColors(color);
        }
    }

    private void setLineColors (Color color)
    {
        VolumetricLineBehavior[] lines = GetComponentsInChildren<VolumetricLineBehavior>();
        for (int i = 0; i < lines.Length; i++)
        {
            lines[i].LineColor = color;
        }
    }

    public int getSign()
    {
        return sign;
    }
}
