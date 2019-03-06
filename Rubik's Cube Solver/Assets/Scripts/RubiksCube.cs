using Rubiks;
using Rubiks.Enums;
using Rubiks.Solvers;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RubiksCube : MonoBehaviour
{
    //Constants
    private const int FRAMES_PER_ROTATION = 90 / Cube.ROTATION_SPEED;
    private const int ROTATIONS_PER_SCRAMBLE = 50;

    //User interface elements
    public Text AlertText;

    //The cube to be manipulated
    private Cube rubiksCube;

    //Variables for rotating cube faces
    private bool isRotating = false;
    private int remainingRotationFrames;

    //Variables for scrambling the cube
    private bool isScrambling = false;
    Queue<Rotation> randomRotations;

    //Variables for solving the cube
    private bool isSolving = false;
    Queue<Rotation> solveRotations;

    //Initialization
    void Start()
    {
        rubiksCube = GetComponent<Cube>();
        rubiksCube.Initialize();

        randomRotations = new Queue<Rotation>();
        solveRotations = new Queue<Rotation>();
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

                    //Display a message if the user solved the cube
                    if (rubiksCube.IsSolved())
                    {
                        AlertText.text = "You solved the Rubik's Cube!";
                    }
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
                    rubiksCube.UpdateState(nextRotation.FaceColor, nextRotation.Direction);
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

                    //If the rotations have completed, set the scrambling flag to false to allow cube interaction
                    if (randomRotations.Count == 0)
                    {
                        isScrambling = false;
                    }
                }
            }
            else if(isSolving)
            {
                //Retrieve the next element from the queue of solving rotations if needed
                if (remainingRotationFrames == 0)
                {
                    Rotation nextRotation = solveRotations.Dequeue();
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

                    //If the rotations have completed, set the scrambling flag to false to allow cube interaction
                    if (solveRotations.Count == 0)
                    {
                        isSolving = false;

                        //Additionally, ensure that the cube has actually been solved
                        AlertText.text = rubiksCube.IsSolved() ? "The Rubik's Cube has been solved!" : "The Rubik's Cube was not successfully solved.";
                    }
                }
            }
        }
        //Otherwise, handle any potential rotation inputs
        else if(rotationKeyWasPressed())
        {
            RotationDirection rotationDirection;
            FaceColor rotatingFace;

            //Reset the alert text
            AlertText.text = "";

            //Indicate that a rotation is now occuring
            isRotating = true;
            remainingRotationFrames = FRAMES_PER_ROTATION;

            //If the shift key is pressed, the turn is counterclockwise. Otherwise, it is clockwise
            rotationDirection = Input.GetKey(KeyCode.LeftShift) ? RotationDirection.Counterclockwise : RotationDirection.Clockwise;          

            //Blue Face Rotation
            if (Input.GetKeyDown(KeyCode.B))
            {
                rotatingFace = FaceColor.Blue;                
            }
            //Green Face Rotation
            else if (Input.GetKeyDown(KeyCode.G))
            {
                rotatingFace = FaceColor.Green;
            }
            //Red Face Rotation
            else if (Input.GetKeyDown(KeyCode.R))
            {
                rotatingFace = FaceColor.Red;
            }
            //Orange Face Rotation
            else if (Input.GetKeyDown(KeyCode.O))
            {
                rotatingFace = FaceColor.Orange;
            }
            //White Face Rotation
            else if (Input.GetKeyDown(KeyCode.W))
            {
                rotatingFace = FaceColor.White;
            }
            //Yellow Face Rotation
            else
            {
                rotatingFace = FaceColor.Yellow;
            }

            //Set the parameters needed to graphically update the cube
            rubiksCube.SetRotationDirection(rotationDirection);
            rubiksCube.SetRotatingFace(rotatingFace);

            //Adjust the internal state of the cube
            rubiksCube.UpdateState(rotatingFace, rotationDirection);
        }     
	}

    private bool rotationKeyWasPressed()
    {
        return Input.GetKeyDown(KeyCode.B) || Input.GetKeyDown(KeyCode.G) || Input.GetKeyDown(KeyCode.R) ||
            Input.GetKeyDown(KeyCode.O) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Y);
    }

    public void ScrambleCube()
    {
        //If the cube is in use, ignore the button click
        if (!cubeIsInUse())
        {
            //Reset the alert text
            AlertText.text = "";

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
    }

    public void HumanSolve()
    {
        //If the cube is in use, ignore the button click
        if(!cubeIsInUse())
        {
            //If the cube is already solved, no further work needs to be done
            if (rubiksCube.IsSolved())
            {
                AlertText.text = "The Rubik's Cube is already solved.";
            }
            else
            {
                //Reset the alert text
                AlertText.text = "";

                isSolving = true;
                remainingRotationFrames = 0;

                HumanSolver solver = new HumanSolver();
                solveRotations = solver.Solve(rubiksCube.GetState());
            }
        }           
    }

    private bool cubeIsInUse()
    {
        return isRotating || isScrambling || isSolving;
    }
}
