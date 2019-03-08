using Rubiks.Constants;
using Rubiks.Enums;
using System.Collections.Generic;

namespace Rubiks.Solvers
{
    public class HumanSolver : Solver
    {
        private Queue<Rotation> currentStepRotations;
        private CubeState cube;

        private RotationDirection direction;
        private FaceColor face;

        public Queue<Rotation> Solve(CubeState initialState)
        {
            cube = initialState;
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

            //For debugging purposes only: Ensure the returned queue is not empty
            if(solutionPath.Count == 0)
            {
                cube.Rotate(FaceColor.Yellow, RotationDirection.Clockwise);
                solutionPath.Enqueue(new Rotation(FaceColor.Yellow, RotationDirection.Clockwise));
            }

            //Return the sequence of operators that solves the cube
            return solutionPath;
        }

        private Queue<Rotation> solveWhiteCross()
        {
            currentStepRotations = new Queue<Rotation>();

            //Up to four loops are needed to solve each of the four white edges
            if (cube.GetBlueFace()[Locations.RIGHT] == FaceColor.Blue && cube.GetRedFace()[Locations.LEFT] == FaceColor.White)
            {
                cube.Rotate(FaceColor.Blue, RotationDirection.Clockwise);
                currentStepRotations.Enqueue(new Rotation(FaceColor.Blue, RotationDirection.Clockwise));
            }
            else if (cube.GetBlueFace()[Locations.TOP] == FaceColor.Blue && cube.GetYellowFace()[Locations.LEFT] == FaceColor.White)
            {
                cube.Rotate(FaceColor.Blue, RotationDirection.Clockwise);
                cube.Rotate(FaceColor.Blue, RotationDirection.Clockwise);
                currentStepRotations.Enqueue(new Rotation(FaceColor.Blue, RotationDirection.Clockwise));
                currentStepRotations.Enqueue(new Rotation(FaceColor.Blue, RotationDirection.Clockwise));
            }
            else if (cube.GetBlueFace()[Locations.LEFT] == FaceColor.Blue && cube.GetOrangeFace()[Locations.RIGHT] == FaceColor.White)
            {
                cube.Rotate(FaceColor.Blue, RotationDirection.Counterclockwise);
                currentStepRotations.Enqueue(new Rotation(FaceColor.Blue, RotationDirection.Counterclockwise));
            }

            return currentStepRotations;
        }

        private Queue<Rotation> solveWhiteFace(CubeState cube)
        {
            currentStepRotations = new Queue<Rotation>();
            return currentStepRotations;
        }
    }
}
