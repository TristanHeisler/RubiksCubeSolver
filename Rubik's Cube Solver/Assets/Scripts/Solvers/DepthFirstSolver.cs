using Rubiks.Constants;
using Rubiks.Enums;
using System.Collections.Generic;

namespace Rubiks.Solvers
{
    public class DepthFirstSolver : Solver
    {
        private Queue<Rotation> _currentStepRotations;
        private readonly CubeState _givenState;
        private RotationDirection _direction;
        private FaceColor _face;

        public DepthFirstSolver(CubeState initialState)
        {
            _givenState = initialState.Clone();
        }

        public Queue<Rotation> Solve()
        {
            var solutionPath = new Queue<Rotation>();

            //TODO: Implement depth-first search.

            return solutionPath;
        }
    }
}
