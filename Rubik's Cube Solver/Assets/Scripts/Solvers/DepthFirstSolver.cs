using System;
using System.Collections;
using Rubiks.Enums;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEditor;
using Debug = UnityEngine.Debug;

namespace Rubiks.Solvers
{
    public class DepthFirstSolver : ISolver
    {
        private const byte MAX_DEPTH = 3;

        private Queue<Rotation> _currentStepRotations;
        private readonly CubeState _givenState;
        private RotationDirection _direction;
        private FaceColor _face;
        private readonly Stopwatch _stopwatch;
        private int _exploredStates;
        private int _generatedStates;

        public DepthFirstSolver(CubeState initialState)
        {
            _givenState = initialState.Clone();
            _givenState.depth = 0;
            _generatedStates = 0;
            _exploredStates = 0;
            _stopwatch = new Stopwatch();
        }

        public async Task<Tuple<Stack<Rotation>, int, TimeSpan>> Solve()
        {
            return await Task.Run(() =>
            {          
                Debug.Log("Start");
                _stopwatch.Start();
                
                var solutionPath = new Stack<Rotation>();

                //Create open and closed lists
                var open = new Stack<CubeState>();
                var closed = new Queue<CubeState>();

                //Add the initial state to the open list
                open.Push(_givenState);

                //Retrieve the possible rotations
                var possibleRotations = CubeState.GetPossibleRotations();

                //Loop as long as states remain in the open list
                while (open.Any() && _exploredStates < 25000)
                {
                    //Retrieve the first state in the open list
                    var currentState = open.Pop();
                    _exploredStates++;
                    
                    Debug.Log("Dequeueing state " + _exploredStates +". Depth is " + currentState.depth);

                    //Generate the children of the current state if the maximum search depth has not been reached
                    if (currentState.depth <= MAX_DEPTH)
                    {
                        foreach (var rotation in possibleRotations)
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
                                _stopwatch.Stop();
                                return new Tuple<Stack<Rotation>, int, TimeSpan>(solutionPath, _generatedStates, _stopwatch.Elapsed);
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
                                childState.depth = (byte) (currentState.depth + 1);
                                open.Push(childState);
                                _generatedStates++;
                            }
                        }
                    }

                    //Put the current state on the closed list
                    closed.Enqueue(currentState);
                }

                //If no solution was found, return the empty list
                _stopwatch.Stop();
                return new Tuple<Stack<Rotation>, int, TimeSpan>(solutionPath, _generatedStates, _stopwatch.Elapsed);
            });
        }
    }
}