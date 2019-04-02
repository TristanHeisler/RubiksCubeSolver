using Rubiks.Enums;
using System.Collections.Generic;
using UnityEngine;

namespace Rubiks.Solvers
{
    public class BreadthFirstSolver : Solver
    {
        private Queue<Rotation> _currentStepRotations;
        private readonly CubeState _givenState;
        private RotationDirection _direction;
        private FaceColor _face;

        public BreadthFirstSolver(CubeState initialState)
        {
            _givenState = initialState.Clone();
        }

        public Queue<Rotation> Solve()
        {
            var solutionPath = new Queue<Rotation>();

            //Get possible rotations
            var possibleRotations = CubeState.GetPossibleRotations();

            //Loop through the available rotations
            foreach (var rotation in possibleRotations)
            {
                //Generate the child state produced by the current rotation
                var childState = _givenState.Clone();
                childState.Rotate(rotation.FaceColor, rotation.Direction);

                //If the child state is the goal, return the current rotation
                if (childState.IsSolved())
                {
                    solutionPath.Enqueue(new Rotation(rotation.FaceColor, rotation.Direction));
                }
            }

            var one = _givenState.Clone();
            one.Rotate(FaceColor.Blue, RotationDirection.Clockwise);

            var two = _givenState.Clone();
            two.Rotate(FaceColor.Blue, RotationDirection.Counterclockwise);

            Debug.Log(one.EqualsState(two));

            return solutionPath;
        }
    }
}