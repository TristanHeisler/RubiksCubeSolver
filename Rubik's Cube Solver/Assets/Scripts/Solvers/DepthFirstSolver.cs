using Rubiks.Constants;
using Rubiks.Enums;
using System.Collections.Generic;

namespace Rubiks.Solvers
{
    public class DepthFirstSolver : Solver
    {
        private Queue<Rotation> currentStepRotations;
        private CubeState givenState;

        private RotationDirection direction;
        private FaceColor face;

        public DepthFirstSolver(CubeState initialState)
        {
            givenState = CubeStateHelper.Clone(initialState);
        }

        public Queue<Rotation> Solve()
        {
            Queue<Rotation> solutionPath = new Queue<Rotation>();

            //TODO: Implement depth-first search.

            return solutionPath;
        }
    }
}
