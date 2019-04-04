using System;
using Rubiks.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Rubiks.Solvers
{
    public class BreadthFirstSolver : ISolver
    {
        private const byte MAX_DEPTH = 6;

        private Queue<Rotation> _currentStepRotations;
        private readonly CubeState _givenState;
        private RotationDirection _direction;
        private FaceColor _face;

        public BreadthFirstSolver(CubeState initialState)
        {
            _givenState = initialState.Clone();
        }

        public async Task<Stack<Rotation>> Solve()
        {
            return await Task.Run(() =>
            {
                var solutionPath = new Stack<Rotation>();

                //Create open and closed lists
                var open = new Queue<CubeState>();
                var closed = new Queue<CubeState>();

                //Add the initial state to the open list
                open.Enqueue(_givenState);

                //Retrieve the possible rotations
                var possibleRotations = CubeState.GetPossibleRotations();

                //Loop as long as states remain in the open list
                while (open.Any())
                {
                    //Retrieve the first state in the open list
                    var currentState = open.Dequeue();

                    //If the current state is the goal, return the list of rotations leading to the solution
                    if (currentState.IsSolved())
                    {
                        var state = currentState;
                        while (state.rotation != null)
                        {
                            solutionPath.Push(state.rotation);
                            state = state.parentState;
                        }

                        return solutionPath;
                    }

                    //Generate the children of the current state if the maximum search depth has not been reached
                    if (currentState.depth <= MAX_DEPTH)
                    {
                        foreach (var rotation in possibleRotations)
                        {
                            //Generate the child state produced by the current rotation
                            var childState = currentState.Clone();
                            childState.Rotate(rotation.FaceColor, rotation.Direction);

                            //Check if the state is already on the open or closed list
                            var alreadyExists = false;
                            foreach (var existingState in open)
                            {
                                if (childState.EqualsState(existingState))
                                {
                                    alreadyExists = true;
                                    break;
                                }
                            }

                            if (!alreadyExists)
                            {
                                foreach (var existingState in closed)
                                {
                                    if (childState.EqualsState(existingState))
                                    {
                                        alreadyExists = true;
                                        break;
                                    }
                                }
                            }

                            //Add the child state to the open list if it does not already exist
                            if (!alreadyExists)
                            {
                                childState.parentState = currentState;
                                childState.rotation = rotation;
                                open.Enqueue(childState);
                            }
                        }
                    }

                    //Put the current state on the closed list
                    closed.Enqueue(currentState);
                }

                //If no solution was found, return the empty list
                return solutionPath;
            });
        }
    }
}