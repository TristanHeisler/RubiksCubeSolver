using Rubiks;
using Rubiks.Enums;
using System.Collections.Generic;
using UnityEngine;

public class RubiksCube : MonoBehaviour
{
    private const int FRAMES_PER_ROTATION = 90 / Cube.ROTATION_SPEED;
    private const int ROTATIONS_PER_SCRAMBLE = 50;

    //The cube to be manipulated
    private Cube rubiksCube;

    //Variables concerning cube face rotation and cube interaction
    private bool isRotating = false;
    private int remainingRotationFrames;

    //Variables for scrambling the cube
    private bool isScrambling = false;
    Queue<Rotation> randomRotations;

    // Use this for initialization
    void Start()
    {
        rubiksCube = GetComponent<Cube>();
        rubiksCube.InitializeFaces();
        randomRotations = new Queue<Rotation>();
    }
	
	// Update is called once per frame
	void Update()
    {
        //If the cube is in use, continue performing the appropriate action
        if(cubeIsInUse())
        {
            if (isRotating)
            {
                //Continue rotating
                rubiksCube.GetComponent<Cube>().RotateCubeFace();

                //Decrement the number of frames remaining to complete the rotation
                remainingRotationFrames--;

                //If the rotation has completed, set the rotation flag to false to allow cube interaction
                if (remainingRotationFrames == 0)
                {
                    isRotating = false;

                    //Additionally, perform the appropriate cube face remappings
                    rubiksCube.HandleRotationRemapping();
                }
            }
            else if (isScrambling)
            {
                //Retrieve the next element from the queue of random rotations if needed
                if (remainingRotationFrames == 0)
                {
                    Rotation nextRotation = randomRotations.Dequeue();
                    rubiksCube.SetRotatingFace(nextRotation.FaceColor);
                    rubiksCube.SetRotationDirection(nextRotation.Direction);
                    remainingRotationFrames = FRAMES_PER_ROTATION;
                }

                //Continue rotating
                rubiksCube.GetComponent<Cube>().RotateCubeFace();

                //Decrement the number of frames remaining to complete the rotation
                remainingRotationFrames--;

                //If the current rotation has completed, handle the remapping
                if (remainingRotationFrames == 0)
                {
                    rubiksCube.HandleRotationRemapping();

                    //If the scramble has completed, set the scrambling flag to false to allow cube interaction
                    if (randomRotations.Count == 0)
                    {
                        isScrambling = false;
                    }
                }
            }
        }
        //Otherwise, handle any potential rotation inputs
        else if(rotationKeyWasPressed())
        {
            //Indicate that a rotation is now occuring
            isRotating = true;
            remainingRotationFrames = FRAMES_PER_ROTATION;

            //If the shift key is pressed, the turn is counterclockwise. Otherwise, it is clockwise
            var rotationDirection = Input.GetKey(KeyCode.LeftShift) ? RotationDirection.Counterclockwise : RotationDirection.Clockwise;
            rubiksCube.SetRotationDirection(rotationDirection);

            //Blue Face Rotation
            if (Input.GetKeyDown(KeyCode.B))
            {
                rubiksCube.SetRotatingFace(FaceColor.Blue);
            }
            //Green Face Rotation
            else if (Input.GetKeyDown(KeyCode.G))
            {
                rubiksCube.SetRotatingFace(FaceColor.Green);
            }
            //Red Face Rotation
            else if (Input.GetKeyDown(KeyCode.R))
            {
                rubiksCube.SetRotatingFace(FaceColor.Red);
            }
            //Orange Face Rotation
            else if (Input.GetKeyDown(KeyCode.O))
            {
                rubiksCube.SetRotatingFace(FaceColor.Orange);
            }
            //White Face Rotation
            else if (Input.GetKeyDown(KeyCode.W))
            {
                rubiksCube.SetRotatingFace(FaceColor.White);
            }
            //Yellow Face Rotation
            else if (Input.GetKeyDown(KeyCode.Y))
            {
                rubiksCube.SetRotatingFace(FaceColor.Yellow);
            }
        }     
	}

    private bool rotationKeyWasPressed()
    {
        return Input.GetKeyDown(KeyCode.B) || Input.GetKeyDown(KeyCode.G) || Input.GetKeyDown(KeyCode.R) ||
            Input.GetKeyDown(KeyCode.O) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Y);
    }

    public void ScrambleCube()
    {
        isScrambling = true;

        for (int i = 0; i < ROTATIONS_PER_SCRAMBLE; i++)
        {
            //Select a random face
            FaceColor selectedFace = (FaceColor)Random.Range(0, 6);

            //Select a random direction
            RotationDirection selectedDirection = Random.Range(0, 2) == 1 ? RotationDirection.Clockwise : RotationDirection.Counterclockwise;

            //Add the random rotation to the queue
            randomRotations.Enqueue(new Rotation(selectedFace, selectedDirection));
        }

        remainingRotationFrames = 0;
    }

    private bool cubeIsInUse()
    {
        return isRotating || isScrambling;
    }
}
