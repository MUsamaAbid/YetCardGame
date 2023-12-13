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
    public float rotationSpeed = 2f; // Set the rotation speed

    bool rotateInHand = false;
    bool rotateUpsideDown = false;
    bool rotateDownsideUp = false;

    [SerializeField] ObjectType objectType;

    public void RotateInHand(float speed, ObjectType type)
    {
        rotationSpeed = speed;
        objectType = type;

        rotateInHand = true;
    }
    public void RotateUpsideDown(float speed, ObjectType type)
    {
        rotationSpeed = speed;
        objectType = type;

        rotateUpsideDown = true;
    }
    public void RotateDownsideUp(float speed, ObjectType type)
    {
        rotationSpeed = speed;
        objectType = type;

        rotateDownsideUp = true;
    }
    private bool actionPerformed = false;

    void Update()
    {
        if (rotateInHand)
        {
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
            PerformRotationInHand();
        }
        if (rotateUpsideDown) //Change it to rorate for upside down
        {
            // Calculate the current y-axis rotation in the range [0, 360]
            float currentRotation = (transform.eulerAngles.z + 360) % 360;

            // Rotate the object
            PerformRotationUpsideDown();
        }
        if (rotateDownsideUp) //Change it to rorate for upside down
        {
            // Calculate the current y-axis rotation in the range [0, 360]
            float currentRotation = (transform.eulerAngles.x + 360) % 360;
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
            PerformRotationDownSideUp();
        }
    }

    void PerformRotationInHand()
    {
        // Calculate the target rotation using Quaternion.Euler
        Quaternion targetRotation = Quaternion.Euler(0, 360, 0); // Rotate continuously

        // Gradually rotateInHand towards the target rotation
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        if(transform.localRotation == targetRotation)
        {
            rotateInHand = false;
        }
    }
    void PerformRotationUpsideDown()
    {
        // Calculate the target rotation using Quaternion.Euler
        Quaternion targetRotation = Quaternion.Euler(0, 0, 180); // Rotate continuously

        // Gradually rotateInHand towards the target rotation
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        if (transform.localRotation == targetRotation)
        {
            rotateUpsideDown = false;
        }
    }
    void PerformRotationDownSideUp()
    {
        // Calculate the target rotation using Quaternion.Euler
        Quaternion targetRotation = Quaternion.Euler(0, 0, 0); // Rotate continuously

        // Gradually rotateInHand towards the target rotation
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        if (transform.localRotation == targetRotation)
        {
            rotateDownsideUp = false;
        }
    }
}
