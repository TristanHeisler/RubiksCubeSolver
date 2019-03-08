using Rubiks.Constants;
using Rubiks.Enums;
using System.Collections.Generic;

namespace Rubiks.Solvers
{
    public class DepthFirstSolver : Solver
    {
        private Queue<Rotation> currentStepRotations;
        private CubeState cube;

        private RotationDirection direction;
        private FaceColor face;

        public Queue<Rotation> Solve(CubeState initialState)
        {
            cube = initialState;
            Queue<Rotation> solutionPath = new Queue<Rotation>();

            //TODO: Implement depth-first search.

            return solutionPath;
        }
    }
}
