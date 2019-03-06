using Rubiks.Enums;
using System.Collections.Generic;
using UnityEngine;

namespace Rubiks.Solvers
{
    public class HumanSolver
    {
        private Queue<Rotation> currentStepRotations;
        private CubeState cubeState;

        public Queue<Rotation> Solve(CubeState state)
        {
            cubeState = state;
            Queue<Rotation> solutionPath = new Queue<Rotation>();

            //Add the steps required to solve the white cross to the solution path
            Queue<Rotation> whiteCrossSteps = solveWhiteCross();
            foreach(Rotation step in whiteCrossSteps)
            {
                solutionPath.Enqueue(step);
            }

            //Add the steps required to solve the white face to the solution path


            //Add the steps required to solve the middle layer to the solution path


            //Add the steps required to solve the yellow cross to the solution path


            //Add the steps required to solve the yellow corners to the solution path


            //Add the steps required to solve the yellow edges to the solution path



            //Return the sequence of operators that solves the cube
            return solutionPath;
        }

        private Queue<Rotation> solveWhiteCross()
        {
            currentStepRotations = new Queue<Rotation>();

            currentStepRotations.Enqueue(new Rotation(FaceColor.Blue, RotationDirection.Counterclockwise));
            //currentStepRotations.Enqueue(new Rotation(FaceColor.Red, RotationDirection.Counterclockwise));
            //currentStepRotations.Enqueue(new Rotation(FaceColor.White, RotationDirection.Counterclockwise));

            //Solve the blue-white edge

            //Solve the red-white edge

            //Solve the green-white edge

            //Solve the orange-white edge

            return currentStepRotations;
        }

        private Queue<Rotation> solveWhiteFace(CubeState cube)
        {
            currentStepRotations = new Queue<Rotation>();

            currentStepRotations.Enqueue(new Rotation(FaceColor.Yellow, RotationDirection.Clockwise));

            return currentStepRotations;
        }
    }
}
