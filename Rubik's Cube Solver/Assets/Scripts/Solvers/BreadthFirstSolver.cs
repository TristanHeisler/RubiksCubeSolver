using Rubiks.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Rubiks.Solvers
{
    public class BreadthFirstSolver : ISolver
    {
        private const byte MAX_DEPTH = 3;

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
                Debug.Log("Start");
                
                var solutionPath = new Stack<Rotation>();

                //Create open and closed lists
                var open = new Queue<CubeState>();
                var closed = new Queue<CubeState>();

                //Add the initial state to the open list
                open.Enqueue(_givenState);

                //Retrieve the possible rotations
                var possibleRotations = CubeState.GetPossibleRotations();
                var count = 1;
                
                //Loop as long as states remain in the open list
                while (open.Any())
                {
                    //Retrieve the first state in the open list
                    var currentState = open.Dequeue();
                    Debug.Log("Dequeueing " + count++ +". Depth is " + currentState.depth);
                    
                    if (count == 1000)
                    {
                        return solutionPath;
                    }
                    
                    //Generate the children of the current state if the maximum search depth has not been reached
                    var enumerable = possibleRotations as Rotation[] ?? possibleRotations.ToArray();
                    if (currentState.depth <= MAX_DEPTH)
                    {
                        foreach (var rotation in enumerable)
                        {
                            //Generate the child state produced by the current rotation
                            var childState = currentState.Clone();
                            childState.Rotate(rotation.FaceColor, rotation.Direction);
                            
                            //If the generated state is the goal, return the list of rotations leading to the solution
                            if (childState.IsSolved())
                            {
                                solutionPath.Push(rotation);
                                
                                var previousState = currentState;
                                while (previousState.rotation != null)
                                {
                                    solutionPath.Push(previousState.rotation);
                                    previousState = previousState.parentState;
                                }
                                
                                Debug.Log("End");

                                return solutionPath;
                            }

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
                                childState.depth = (byte)(currentState.depth + 1);
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