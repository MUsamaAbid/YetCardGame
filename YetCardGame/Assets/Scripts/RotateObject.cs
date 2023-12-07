using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectType
{
    General,
    Card
}
public class RotateObject : MonoBehaviour
{
    public float targetAngle = 0; // Set your target angle here
    public float rotationSpeed = 2f; // Set the rotation speed

    bool rotate = false;

    [SerializeField] ObjectType objectType;

    public void Rotate(float speed, ObjectType type)
    {
        rotationSpeed = speed;
        objectType = type;

        rotate = true;
    }

    private bool actionPerformed = false;

    void Update()
    {
        if (!rotate) return;
        // Calculate the current y-axis rotation in the range [0, 360]
        float currentRotation = (transform.eulerAngles.y + 360) % 360;

        // Check if the rotation exceeds 270 or goes below 90
        if ((currentRotation > 270 || currentRotation < 90) && !actionPerformed)
        {
            // Perform your action here
            Debug.Log("Action performed!");
            GetComponent<Card>().DeleteBackside();
            // Set a flag to ensure the action is performed only once
            actionPerformed = true;
        }
        else
        {
            // Reset the flag when the rotation is not within the specified range
            actionPerformed = false;
        }

        // Rotate the object
        PerformRotation();
    }

    void PerformRotation()
    {
        // Calculate the target rotation using Quaternion.Euler
        Quaternion targetRotation = Quaternion.Euler(0, 360, 0); // Rotate continuously

        // Gradually rotate towards the target rotation
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        if(transform.localRotation == targetRotation)
        {
            rotate = false;
        }
    }
}
