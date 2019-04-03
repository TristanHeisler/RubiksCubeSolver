﻿using Rubiks;
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
    private Cube _rubiksCube;

    //Variables for rotating cube faces
    private bool _isRotating = false;
    private int _remainingRotationFrames;

    //Variables for scrambling the cube
    private bool _isScrambling = false;
    private Queue<Rotation> _randomRotations;

    //Variables for solving the cube
    private bool _isSolving = false;
    private Stack<Rotation> _solveRotations;

    //Initialization
    void Start()
    {
        _rubiksCube = GetComponent<Cube>();
        _rubiksCube.Initialize();

        _randomRotations = new Queue<Rotation>();
        _solveRotations = new Stack<Rotation>();
    }

    // Update is called once per frame
    void Update()
    {
        //If the cube is in use, continue performing the appropriate action
        if (cubeIsInUse())
        {
            if (_isRotating)
            {
                //Continue rotating
                _rubiksCube.GetComponent<Cube>().RotateCubeFace();

                //Decrement the number of frames remaining to complete the rotation
                _remainingRotationFrames--;

                //If the rotation has completed, set the rotation flag to false to allow cube interaction
                if (_remainingRotationFrames == 0)
                {
                    _isRotating = false;

                    //Additionally, perform the appropriate cube face remappings
                    _rubiksCube.HandleRotationRemapping();

                    //Display a message if the user solved the cube
                    if (_rubiksCube.IsSolved())
                    {
                        AlertText.text = "You solved the Rubik's Cube!";
                    }
                }
            }
            else if (_isScrambling)
            {
                //Retrieve the next element from the queue of random rotations if needed
                if (_remainingRotationFrames == 0)
                {
                    Rotation nextRotation = _randomRotations.Dequeue();
                    _rubiksCube.SetRotatingFace(nextRotation.FaceColor);
                    _rubiksCube.SetRotationDirection(nextRotation.Direction);
                    _rubiksCube.UpdateState(nextRotation.FaceColor, nextRotation.Direction);
                    _remainingRotationFrames = FRAMES_PER_ROTATION;
                }

                //Continue rotating
                _rubiksCube.GetComponent<Cube>().RotateCubeFace();

                //Decrement the number of frames remaining to complete the rotation
                _remainingRotationFrames--;

                //If the current rotation has completed, handle the remapping
                if (_remainingRotationFrames == 0)
                {
                    _rubiksCube.HandleRotationRemapping();

                    //If the rotations have completed, set the scrambling flag to false to allow cube interaction
                    if (_randomRotations.Count == 0)
                    {
                        _isScrambling = false;
                    }
                }
            }
            else if (_isSolving)
            {
                //Retrieve the next element from the queue of solving rotations if needed
                if (_remainingRotationFrames == 0)
                {
                    Rotation nextRotation = _solveRotations.Pop();
                    _rubiksCube.SetRotatingFace(nextRotation.FaceColor);
                    _rubiksCube.SetRotationDirection(nextRotation.Direction);
                    _rubiksCube.UpdateState(nextRotation.FaceColor, nextRotation.Direction);
                    _remainingRotationFrames = FRAMES_PER_ROTATION;
                }

                //Continue rotating
                _rubiksCube.GetComponent<Cube>().RotateCubeFace();

                //Decrement the number of frames remaining to complete the rotation
                _remainingRotationFrames--;

                //If the current rotation has completed, handle the remapping
                if (_remainingRotationFrames == 0)
                {
                    _rubiksCube.HandleRotationRemapping();

                    //If the rotations have completed, set the scrambling flag to false to allow cube interaction
                    if (_solveRotations.Count == 0)
                    {
                        _isSolving = false;

                        //Additionally, ensure that the cube has actually been solved
                        AlertText.text = _rubiksCube.IsSolved()
                            ? "The Rubik's Cube has been solved!"
                            : "The Rubik's Cube was not successfully solved.";
                    }
                }
            }
        }
        //Otherwise, handle any potential rotation inputs
        else if (rotationKeyWasPressed())
        {
            RotationDirection rotationDirection;
            FaceColor rotatingFace;

            //Reset the alert text
            AlertText.text = "";

            //Indicate that a rotation is now occuring
            _isRotating = true;
            _remainingRotationFrames = FRAMES_PER_ROTATION;

            //If the shift key is pressed, the turn is counterclockwise. Otherwise, it is clockwise
            rotationDirection = Input.GetKey(KeyCode.LeftShift)
                ? RotationDirection.Counterclockwise
                : RotationDirection.Clockwise;

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
            _rubiksCube.SetRotationDirection(rotationDirection);
            _rubiksCube.SetRotatingFace(rotatingFace);

            //Adjust the internal state of the cube
            _rubiksCube.UpdateState(rotatingFace, rotationDirection);
        }
    }

    private static bool rotationKeyWasPressed()
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

            _isScrambling = true;

            for (int i = 0; i < ROTATIONS_PER_SCRAMBLE; i++)
            {
                //Select a random face
                FaceColor selectedFace = (FaceColor) Random.Range(0, 6);

                //Select a random direction
                RotationDirection selectedDirection = Random.Range(0, 2) == 1
                    ? RotationDirection.Clockwise
                    : RotationDirection.Counterclockwise;

                //Add the random rotation to the queue
                _randomRotations.Enqueue(new Rotation(selectedFace, selectedDirection));
            }

            _remainingRotationFrames = 0;
        }
    }

    public void BreadthFirstSolve()
    {
        Solver breadthFirstSolver = new BreadthFirstSolver(_rubiksCube.GetState());
        Solve(breadthFirstSolver);
    }

    public void DepthFirstSolve()
    {
        Solver depthFirstSolver = new DepthFirstSolver(_rubiksCube.GetState());
        Solve(depthFirstSolver);
    }

    public void HumanSolve()
    {
        Solver humanSolver = new HumanSolver(_rubiksCube.GetState());
        Solve(humanSolver);
    }

    private void Solve(Solver solver)
    {
        //If the cube is in use, ignore the button click
        if (!cubeIsInUse())
        {
            //If the cube is already solved, no further work needs to be done
            if (_rubiksCube.IsSolved())
            {
                AlertText.text = "The Rubik's Cube is already solved.";
            }
            else
            {
                //Reset the alert text
                AlertText.text = "";

                //Determine the path required to solve the cube
                _solveRotations = solver.Solve();
                //If a path was returned, set the variables for solving the cube
                if (_solveRotations.Count > 0)
                {
                    _isSolving = true;
                    _remainingRotationFrames = 0;
                }
                else
                {
                    AlertText.text = "The selected solver was unable to find a solution.";
                }
            }
        }
    }

    private bool cubeIsInUse()
    {
        return _isRotating || _isScrambling || _isSolving;
    }
}