using Rubiks.Constants;
using Rubiks.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rubiks.Solvers
{
    public class HumanSolver : ISolver
    {
        private Queue<Rotation> _currentStepRotations;
        private readonly CubeState _givenState;
        private RotationDirection _direction;
        private FaceColor _face;

        public HumanSolver(CubeState initialState)
        {
            _givenState = initialState.Clone();
        }

        public async Task<Stack<Rotation>> Solve()
        {
            return await Task.Run(() =>
            {
                var solutionPath = new Stack<Rotation>();

                //Add the steps required to solve the white cross to the solution path
                var whiteCrossSteps = solveWhiteCross();
                foreach (var step in whiteCrossSteps)
                {
                    solutionPath.Push(step);
                }

                //Add the steps required to solve the white face to the solution path


                //Add the steps required to solve the middle layer to the solution path


                //Add the steps required to solve the yellow cross to the solution path


                //Add the steps required to solve the yellow corners to the solution path


                //Add the steps required to solve the yellow edges to the solution path

                //For debugging purposes only: Ensure the returned queue is not empty
                if (solutionPath.Count == 0)
                {
                    _givenState.Rotate(FaceColor.Yellow, RotationDirection.Clockwise);
                    solutionPath.Push(new Rotation(FaceColor.Yellow, RotationDirection.Clockwise));
                }

                //Return the sequence of operators that solves the cube
                return solutionPath;
            });
        }

        private Queue<Rotation> solveWhiteCross()
        {
            _currentStepRotations = new Queue<Rotation>();

            //Up to four loops are needed to solve each of the four white edges
            if (_givenState.GetBlueFace()[Locations.RIGHT] == FaceColor.Blue &&
                _givenState.GetRedFace()[Locations.LEFT] == FaceColor.White)
            {
                _currentStepRotations.Enqueue(new Rotation(FaceColor.Blue, RotationDirection.Clockwise));
            }
            else if (_givenState.GetBlueFace()[Locations.TOP] == FaceColor.Blue &&
                     _givenState.GetYellowFace()[Locations.LEFT] == FaceColor.White)
            {
                _currentStepRotations.Enqueue(new Rotation(FaceColor.Blue, RotationDirection.Clockwise));
                _currentStepRotations.Enqueue(new Rotation(FaceColor.Blue, RotationDirection.Clockwise));
            }
            else if (_givenState.GetBlueFace()[Locations.LEFT] == FaceColor.Blue &&
                     _givenState.GetOrangeFace()[Locations.RIGHT] == FaceColor.White)
            {
                _currentStepRotations.Enqueue(new Rotation(FaceColor.Blue, RotationDirection.Counterclockwise));
            }

            return _currentStepRotations;
        }

        private Queue<Rotation> solveWhiteFace(CubeState cube)
        {
            _currentStepRotations = new Queue<Rotation>();
            return _currentStepRotations;
        }
    }
}