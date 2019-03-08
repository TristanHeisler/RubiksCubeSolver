using Rubiks.Constants;
using Rubiks.Enums;
using System.Collections.Generic;

namespace Rubiks.Solvers
{
    public class HumanSolver : Solver
    {
        private Queue<Rotation> currentStepRotations;
        private CubeState givenState;

        private RotationDirection direction;
        private FaceColor face;

        public HumanSolver(CubeState initialState)
        {
            givenState = CubeStateHelper.Clone(initialState);
        }

        public Queue<Rotation> Solve()
        {
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
                givenState.Rotate(FaceColor.Yellow, RotationDirection.Clockwise);
                solutionPath.Enqueue(new Rotation(FaceColor.Yellow, RotationDirection.Clockwise));
            }

            //Return the sequence of operators that solves the cube
            return solutionPath;
        }

        private Queue<Rotation> solveWhiteCross()
        {
            currentStepRotations = new Queue<Rotation>();

            //Up to four loops are needed to solve each of the four white edges
            if (givenState.GetBlueFace()[Locations.RIGHT] == FaceColor.Blue && givenState.GetRedFace()[Locations.LEFT] == FaceColor.White)
            {
                currentStepRotations.Enqueue(new Rotation(FaceColor.Blue, RotationDirection.Clockwise));
            }
            else if (givenState.GetBlueFace()[Locations.TOP] == FaceColor.Blue && givenState.GetYellowFace()[Locations.LEFT] == FaceColor.White)
            {

                currentStepRotations.Enqueue(new Rotation(FaceColor.Blue, RotationDirection.Clockwise));
                currentStepRotations.Enqueue(new Rotation(FaceColor.Blue, RotationDirection.Clockwise));
            }
            else if (givenState.GetBlueFace()[Locations.LEFT] == FaceColor.Blue && givenState.GetOrangeFace()[Locations.RIGHT] == FaceColor.White)
            {
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
