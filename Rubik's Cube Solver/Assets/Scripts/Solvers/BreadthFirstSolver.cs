using Rubiks.Constants;
using Rubiks.Enums;
using System.Collections.Generic;
using UnityEngine;

namespace Rubiks.Solvers
{
    public class BreadthFirstSolver : Solver
    {
        private Queue<Rotation> currentStepRotations;
        private CubeState givenState;

        private RotationDirection direction;
        private FaceColor face;

        public BreadthFirstSolver(CubeState initialState)
        {
            givenState = CubeStateHelper.Clone(initialState);
        }

        public Queue<Rotation> Solve()
        {
            Queue<Rotation> solutionPath = new Queue<Rotation>();

            //Get possible rotations
            IEnumerable<Rotation> possibleRotations = GetPossibleRotations();

            //Loop through the available rotations
            foreach(var rotation in possibleRotations)
            {
                //Generate the child state produced by the current rotation
                CubeState childState = CubeStateHelper.Clone(givenState);
                childState.Rotate(rotation.FaceColor, rotation.Direction);

                //If the child state is the goal, return the current rotation
                if (childState.IsSolved())
                {
                   solutionPath.Enqueue(new Rotation(rotation.FaceColor, rotation.Direction));
                }
            }            

            return solutionPath;
        }

        private IEnumerable<Rotation> GetPossibleRotations()
        {
            //Blue face rotations
            yield return new Rotation(FaceColor.Blue, RotationDirection.Clockwise);
            yield return new Rotation(FaceColor.Blue, RotationDirection.Counterclockwise);

            //Green face rotations
            yield return new Rotation(FaceColor.Green, RotationDirection.Clockwise);
            yield return new Rotation(FaceColor.Green, RotationDirection.Counterclockwise);

            //Orange face rotations
            yield return new Rotation(FaceColor.Orange, RotationDirection.Clockwise);
            yield return new Rotation(FaceColor.Orange, RotationDirection.Counterclockwise);

            //Red face rotations
            yield return new Rotation(FaceColor.Red, RotationDirection.Clockwise);
            yield return new Rotation(FaceColor.Red, RotationDirection.Counterclockwise);

            //White face rotations
            yield return new Rotation(FaceColor.White, RotationDirection.Clockwise);
            yield return new Rotation(FaceColor.White, RotationDirection.Counterclockwise);

            //Yellow face rotations
            yield return new Rotation(FaceColor.Yellow, RotationDirection.Clockwise);
            yield return new Rotation(FaceColor.Yellow, RotationDirection.Counterclockwise);
        }
    }
}
