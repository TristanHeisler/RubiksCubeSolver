using System;
using Rubiks.Constants;
using Rubiks.Enums;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Experimental.PlayerLoop;

namespace Rubiks.Solvers
{
    public class HumanSolver : ISolver
    {
        private const FaceColor BLUE = FaceColor.Blue;
        private const FaceColor GREEN = FaceColor.Green;
        private const FaceColor ORANGE = FaceColor.Orange;
        private const FaceColor RED = FaceColor.Red;
        private const FaceColor WHITE = FaceColor.White;
        private const FaceColor YELLOW = FaceColor.Yellow;

        private const byte TOP_LEFT = Locations.TOP_LEFT;
        private const byte TOP = Locations.TOP;
        private const byte TOP_RIGHT = Locations.TOP_RIGHT;
        private const byte RIGHT = Locations.RIGHT;
        private const byte BOTTOM_RIGHT = Locations.BOTTOM_RIGHT;
        private const byte BOTTOM = Locations.BOTTOM;
        private const byte BOTTOM_LEFT = Locations.BOTTOM_LEFT;
        private const byte LEFT = Locations.LEFT;

        private const RotationDirection CLOCKWISE = RotationDirection.Clockwise;
        private const RotationDirection COUNTER_CLOCKWISE = RotationDirection.Counterclockwise;

        private Stack<Rotation> _currentStepRotations;
        private readonly CubeState _state;
        private RotationDirection _direction;
        private FaceColor _face;

        public HumanSolver(CubeState initialState)
        {
            _state = initialState.Clone();
        }

        public async Task<Stack<Rotation>> Solve()
        {
            return await Task.Run(() =>
            {
                var solutionPath = new Stack<Rotation>();

                //Determine the steps required to solve the white cross
                var whiteCrossSteps = solveWhiteCross();

                //Determine the steps required to solve the white face
                var whiteFaceSteps = solveWhiteFace();

                //Determine the steps required to solve the middle layer


                //Add the steps required to solve the yellow cross


                //Add the steps required to solve the yellow corners


                //Add the steps required to solve the yellow edges


                //Return the sequence of operators that solves the cube
//                foreach (var step in whiteFaceSteps)
//                {
//                    solutionPath.Push(step);
//                }
//                
                foreach (var step in whiteCrossSteps)
                {
                    solutionPath.Push(step);
                }
                
                return solutionPath;
            });
        }

        private void rotateAndAdd(FaceColor faceColor, RotationDirection direction)
        {
            _state.Rotate(faceColor, direction);
            _currentStepRotations.Push(new Rotation(faceColor, direction));
        }

        private bool whiteCrossIsSolved()
        {
            return (_state.GetWhiteFace()[TOP] == WHITE && _state.GetRedFace()[BOTTOM] == RED)
                   && (_state.GetWhiteFace()[RIGHT] == WHITE && _state.GetGreenFace()[BOTTOM] == GREEN)
                   && (_state.GetWhiteFace()[BOTTOM] == WHITE && _state.GetOrangeFace()[BOTTOM] == ORANGE)
                   && (_state.GetWhiteFace()[LEFT] == WHITE && _state.GetBlueFace()[BOTTOM] == BLUE);
        }

        private IEnumerable<Rotation> solveWhiteCross()
        {
            _currentStepRotations = new Stack<Rotation>();

            //Up to four loops are required to place every white edge
            var edgesToSolve = 4;

            //Decrement the number of remaining edges if some are already solved
            if (_state.GetWhiteFace()[TOP] == WHITE && _state.GetRedFace()[BOTTOM] == RED)
            {
                edgesToSolve--;
            }

            if (_state.GetWhiteFace()[RIGHT] == WHITE && _state.GetGreenFace()[BOTTOM] == GREEN)
            {
                edgesToSolve--;
            }

            if (_state.GetWhiteFace()[BOTTOM] == WHITE && _state.GetOrangeFace()[BOTTOM] == ORANGE)
            {
                edgesToSolve--;
            }

            if (_state.GetWhiteFace()[LEFT] == WHITE && _state.GetBlueFace()[BOTTOM] == BLUE)
            {
                edgesToSolve--;
            }

            for (; edgesToSolve > 0; edgesToSolve--)
            {
                if (_state.GetWhiteFace()[TOP] == WHITE && _state.GetRedFace()[BOTTOM] != RED)
                {
                    switch (_state.GetRedFace()[BOTTOM])
                    {
                        case BLUE:
                            rotateAndAdd(RED, CLOCKWISE);
                            rotateAndAdd(WHITE, CLOCKWISE);
                            rotateAndAdd(RED, COUNTER_CLOCKWISE);
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            Debug.Log("Solved blue. White top");
                            break;
                        case GREEN:
                            rotateAndAdd(RED, CLOCKWISE);
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            rotateAndAdd(RED, COUNTER_CLOCKWISE);
                            rotateAndAdd(WHITE, CLOCKWISE);
                            Debug.Log("Solved green. White top");
                            break;
                        case ORANGE:
                            rotateAndAdd(RED, CLOCKWISE);
                            rotateAndAdd(WHITE, CLOCKWISE);
                            rotateAndAdd(WHITE, CLOCKWISE);
                            rotateAndAdd(RED, COUNTER_CLOCKWISE);
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            Debug.Log("Solved orange. White top");
                            break;
                    }

                    continue;
                }

                if (_state.GetWhiteFace()[RIGHT] == WHITE && _state.GetGreenFace()[BOTTOM] != GREEN)
                {
                    switch (_state.GetGreenFace()[BOTTOM])
                    {
                        case BLUE:
                            rotateAndAdd(GREEN, CLOCKWISE);
                            rotateAndAdd(WHITE, CLOCKWISE);
                            rotateAndAdd(WHITE, CLOCKWISE);
                            rotateAndAdd(GREEN, COUNTER_CLOCKWISE);
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            Debug.Log("Solved blue. White right");
                            break;
                        case ORANGE:
                            rotateAndAdd(GREEN, CLOCKWISE);
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            rotateAndAdd(GREEN, COUNTER_CLOCKWISE);
                            rotateAndAdd(WHITE, CLOCKWISE);
                            Debug.Log("Solved orange. White right");
                            break;
                        case RED:
                            rotateAndAdd(GREEN, CLOCKWISE);
                            rotateAndAdd(WHITE, CLOCKWISE);
                            rotateAndAdd(GREEN, COUNTER_CLOCKWISE);
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            Debug.Log("Solved red. White right");
                            break;
                    }

                    continue;
                }

                if (_state.GetWhiteFace()[BOTTOM] == WHITE && _state.GetOrangeFace()[BOTTOM] != ORANGE)
                {
                    switch (_state.GetOrangeFace()[BOTTOM])
                    {
                        case BLUE:
                            rotateAndAdd(ORANGE, CLOCKWISE);
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            rotateAndAdd(ORANGE, COUNTER_CLOCKWISE);
                            rotateAndAdd(WHITE, CLOCKWISE);
                            Debug.Log("Solved blue. White bottom");
                            break;
                        case GREEN:
                            rotateAndAdd(ORANGE, CLOCKWISE);
                            rotateAndAdd(WHITE, CLOCKWISE);
                            rotateAndAdd(ORANGE, COUNTER_CLOCKWISE);
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            Debug.Log("Solved green. White bottom");
                            break;
                        case RED:
                            rotateAndAdd(ORANGE, CLOCKWISE);
                            rotateAndAdd(WHITE, CLOCKWISE);
                            rotateAndAdd(WHITE, CLOCKWISE);
                            rotateAndAdd(ORANGE, COUNTER_CLOCKWISE);
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            Debug.Log("Solved red. White bottom");
                            break;
                    }

                    continue;
                }

                if (_state.GetWhiteFace()[LEFT] == WHITE && _state.GetBlueFace()[BOTTOM] != BLUE)
                {
                    switch (_state.GetBlueFace()[BOTTOM])
                    {
                        case GREEN:
                            rotateAndAdd(BLUE, CLOCKWISE);
                            rotateAndAdd(WHITE, CLOCKWISE);
                            rotateAndAdd(WHITE, CLOCKWISE);
                            rotateAndAdd(BLUE, COUNTER_CLOCKWISE);
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            Debug.Log("Solved green. White left");
                            break;
                        case ORANGE:
                            rotateAndAdd(BLUE, CLOCKWISE);
                            rotateAndAdd(WHITE, CLOCKWISE);
                            rotateAndAdd(BLUE, COUNTER_CLOCKWISE);
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            Debug.Log("Solved orange. White left");
                            break;
                        case RED:
                            rotateAndAdd(BLUE, CLOCKWISE);
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            rotateAndAdd(BLUE, COUNTER_CLOCKWISE);
                            rotateAndAdd(WHITE, CLOCKWISE);
                            Debug.Log("Solved red. White left");
                            break;
                    }

                    continue;
                }

                if (_state.GetRedFace()[BOTTOM] == WHITE)
                {
                    switch (_state.GetWhiteFace()[TOP])
                    {
                        case BLUE:
                            rotateAndAdd(RED, CLOCKWISE);
                            rotateAndAdd(BLUE, CLOCKWISE);
                            Debug.Log("Solved blue. Red bottom");
                            break;
                        case GREEN:
                            rotateAndAdd(RED, COUNTER_CLOCKWISE);
                            rotateAndAdd(GREEN, COUNTER_CLOCKWISE);
                            Debug.Log("Solved green. Red bottom");
                            break;
                        case ORANGE:
                            rotateAndAdd(RED, CLOCKWISE);
                            rotateAndAdd(WHITE, CLOCKWISE);
                            rotateAndAdd(BLUE, CLOCKWISE);
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            Debug.Log("Solved orange. Red bottom");
                            break;
                        case RED:
                            rotateAndAdd(RED, CLOCKWISE);
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            rotateAndAdd(BLUE, CLOCKWISE);
                            rotateAndAdd(WHITE, CLOCKWISE);
                            Debug.Log("Solved red. Red bottom");
                            break;
                    }

                    continue;
                }

                if (_state.GetGreenFace()[BOTTOM] == WHITE)
                {
                    switch (_state.GetWhiteFace()[RIGHT])
                    {
                        case BLUE:
                            rotateAndAdd(GREEN, CLOCKWISE);
                            rotateAndAdd(WHITE, CLOCKWISE);
                            rotateAndAdd(RED, CLOCKWISE);
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            Debug.Log("Solved blue. Green bottom");
                            break;
                        case GREEN:
                            rotateAndAdd(GREEN, CLOCKWISE);
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            rotateAndAdd(RED, CLOCKWISE);
                            rotateAndAdd(WHITE, CLOCKWISE);
                            Debug.Log("Solved green. Green bottom");
                            break;
                        case ORANGE:
                            rotateAndAdd(GREEN, COUNTER_CLOCKWISE);
                            rotateAndAdd(ORANGE, COUNTER_CLOCKWISE);
                            Debug.Log("Solved orange. Green bottom");
                            break;
                        case RED:
                            rotateAndAdd(GREEN, CLOCKWISE);
                            rotateAndAdd(RED, CLOCKWISE);
                            Debug.Log("Solved red. Green bottom");
                            break;
                    }

                    continue;
                }

                if (_state.GetOrangeFace()[BOTTOM] == WHITE)
                {
                    switch (_state.GetWhiteFace()[BOTTOM])
                    {
                        case BLUE:
                            rotateAndAdd(ORANGE, COUNTER_CLOCKWISE);
                            rotateAndAdd(BLUE, COUNTER_CLOCKWISE);
                            Debug.Log("Solved blue. Orange bottom");
                            break;
                        case GREEN:
                            rotateAndAdd(ORANGE, CLOCKWISE);
                            rotateAndAdd(GREEN, CLOCKWISE);
                            Debug.Log("Solved green. Orange bottom");
                            break;
                        case ORANGE:
                            rotateAndAdd(ORANGE, CLOCKWISE);
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            rotateAndAdd(GREEN, CLOCKWISE);
                            rotateAndAdd(WHITE, CLOCKWISE);
                            Debug.Log("Solved orange. Orange bottom");
                            break;
                        case RED:
                            rotateAndAdd(ORANGE, CLOCKWISE);
                            rotateAndAdd(WHITE, CLOCKWISE);
                            rotateAndAdd(GREEN, CLOCKWISE);
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            Debug.Log("Solved red. Orange bottom");
                            break;
                    }

                    continue;
                }

                if (_state.GetBlueFace()[BOTTOM] == WHITE)
                {
                    switch (_state.GetWhiteFace()[LEFT])
                    {
                        case BLUE:
                            rotateAndAdd(BLUE, CLOCKWISE);
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            rotateAndAdd(ORANGE, CLOCKWISE);
                            rotateAndAdd(WHITE, CLOCKWISE);
                            Debug.Log("Solved blue. Blue bottom");
                            break;
                        case GREEN:
                            rotateAndAdd(BLUE, CLOCKWISE);
                            rotateAndAdd(WHITE, CLOCKWISE);
                            rotateAndAdd(ORANGE, CLOCKWISE);
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            Debug.Log("Solved green. Blue bottom");
                            break;
                        case ORANGE:
                            rotateAndAdd(BLUE, CLOCKWISE);
                            rotateAndAdd(ORANGE, CLOCKWISE);
                            Debug.Log("Solved orange. Blue bottom");
                            break;
                        case RED:
                            rotateAndAdd(BLUE, COUNTER_CLOCKWISE);
                            rotateAndAdd(RED, COUNTER_CLOCKWISE);
                            Debug.Log("Solved red. Blue bottom");
                            break;
                    }

                    continue;
                }

                if (_state.GetRedFace()[LEFT] == WHITE)
                {
                    switch (_state.GetBlueFace()[RIGHT])
                    {
                        case BLUE:
                            rotateAndAdd(BLUE, CLOCKWISE);
                            Debug.Log("Solved blue. Red left");
                            break;
                        case GREEN:
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            rotateAndAdd(BLUE, CLOCKWISE);
                            rotateAndAdd(WHITE, CLOCKWISE);
                            rotateAndAdd(WHITE, CLOCKWISE);
                            Debug.Log("Solved green. Red left");
                            break;
                        case ORANGE:
                            rotateAndAdd(WHITE, CLOCKWISE);
                            rotateAndAdd(BLUE, CLOCKWISE);
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            Debug.Log("Solved white. Red left");
                            break;
                        case RED:
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            rotateAndAdd(BLUE, CLOCKWISE);
                            rotateAndAdd(WHITE, CLOCKWISE);
                            Debug.Log("Solved red. Red left");
                            break;
                    }

                    continue;
                }

                if (_state.GetBlueFace()[LEFT] == WHITE)
                {
                    switch (_state.GetOrangeFace()[RIGHT])
                    {
                        case BLUE:
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            rotateAndAdd(ORANGE, CLOCKWISE);
                            rotateAndAdd(WHITE, CLOCKWISE);
                            Debug.Log("Solved blue. Blue left");
                            break;
                        case GREEN:
                            rotateAndAdd(WHITE, CLOCKWISE);
                            rotateAndAdd(ORANGE, CLOCKWISE);
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            Debug.Log("Solved green. Blue left");
                            break;
                        case ORANGE:
                            rotateAndAdd(ORANGE, CLOCKWISE);
                            Debug.Log("Solved orange. Blue left");
                            break;
                        case RED:
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            rotateAndAdd(ORANGE, CLOCKWISE);
                            rotateAndAdd(WHITE, CLOCKWISE);
                            rotateAndAdd(WHITE, CLOCKWISE);
                            Debug.Log("Solved red. Blue left");
                            break;
                    }

                    continue;
                }

                if (_state.GetOrangeFace()[LEFT] == WHITE)
                {
                    switch (_state.GetGreenFace()[RIGHT])
                    {
                        case BLUE:
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            rotateAndAdd(GREEN, CLOCKWISE);
                            rotateAndAdd(WHITE, CLOCKWISE);
                            rotateAndAdd(WHITE, CLOCKWISE);
                            Debug.Log("Solved blue. Orange left");
                            break;
                        case GREEN:
                            rotateAndAdd(GREEN, CLOCKWISE);
                            Debug.Log("Solved green. Orange left");
                            break;
                        case ORANGE:
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            rotateAndAdd(GREEN, CLOCKWISE);
                            rotateAndAdd(WHITE, CLOCKWISE);
                            Debug.Log("Solved orange. Orange left");
                            break;
                        case RED:
                            rotateAndAdd(WHITE, CLOCKWISE);
                            rotateAndAdd(GREEN, CLOCKWISE);
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            Debug.Log("Solved red. Orange left");
                            break;
                    }

                    continue;
                }

                if (_state.GetGreenFace()[LEFT] == WHITE)
                {
                    switch (_state.GetRedFace()[RIGHT])
                    {
                        case BLUE:
                            rotateAndAdd(WHITE, CLOCKWISE);
                            rotateAndAdd(RED, CLOCKWISE);
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            Debug.Log("Solved blue. Green left");
                            break;
                        case GREEN:
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            rotateAndAdd(RED, CLOCKWISE);
                            rotateAndAdd(WHITE, CLOCKWISE);
                            Debug.Log("Solved green. Green left");
                            break;
                        case ORANGE:
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            rotateAndAdd(RED, CLOCKWISE);
                            rotateAndAdd(WHITE, CLOCKWISE);
                            rotateAndAdd(WHITE, CLOCKWISE);
                            Debug.Log("Solved orange. Green left");
                            break;
                        case RED:
                            rotateAndAdd(RED, CLOCKWISE);
                            Debug.Log("Solved red. Green left");
                            break;
                    }

                    continue;
                }

                if (_state.GetRedFace()[RIGHT] == WHITE)
                {
                    switch (_state.GetGreenFace()[LEFT])
                    {
                        case BLUE:
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            rotateAndAdd(GREEN, COUNTER_CLOCKWISE);
                            rotateAndAdd(WHITE, CLOCKWISE);
                            rotateAndAdd(WHITE, CLOCKWISE);
                            Debug.Log("Solved blue. Red right");
                            break;
                        case GREEN:
                            rotateAndAdd(GREEN, COUNTER_CLOCKWISE);
                            Debug.Log("Solved green. Red right");
                            break;
                        case ORANGE:
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            rotateAndAdd(GREEN, COUNTER_CLOCKWISE);
                            rotateAndAdd(WHITE, CLOCKWISE);
                            Debug.Log("Solved orange. Red right");
                            break;
                        case RED:
                            rotateAndAdd(WHITE, CLOCKWISE);
                            rotateAndAdd(GREEN, COUNTER_CLOCKWISE);
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            Debug.Log("Solved red. Red right");
                            break;
                    }

                    continue;
                }

                if (_state.GetBlueFace()[RIGHT] == WHITE)
                {
                    switch (_state.GetRedFace()[LEFT])
                    {
                        case BLUE:
                            rotateAndAdd(WHITE, CLOCKWISE);
                            rotateAndAdd(RED, COUNTER_CLOCKWISE);
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            Debug.Log("Solved blue. Blue right");
                            break;
                        case GREEN:
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            rotateAndAdd(RED, COUNTER_CLOCKWISE);
                            rotateAndAdd(WHITE, CLOCKWISE);
                            Debug.Log("Solved green. Blue right");
                            break;
                        case ORANGE:
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            rotateAndAdd(RED, COUNTER_CLOCKWISE);
                            rotateAndAdd(WHITE, CLOCKWISE);
                            rotateAndAdd(WHITE, CLOCKWISE);
                            Debug.Log("Solved orange. Blue right");
                            break;
                        case RED:
                            rotateAndAdd(RED, COUNTER_CLOCKWISE);
                            Debug.Log("Solved red. Blue right");
                            break;
                    }

                    continue;
                }

                if (_state.GetOrangeFace()[RIGHT] == WHITE)
                {
                    switch (_state.GetBlueFace()[LEFT])
                    {
                        case BLUE:
                            rotateAndAdd(BLUE, COUNTER_CLOCKWISE);
                            Debug.Log("Solved blue. Orange right");
                            break;
                        case GREEN:
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            rotateAndAdd(BLUE, COUNTER_CLOCKWISE);
                            rotateAndAdd(WHITE, CLOCKWISE);
                            rotateAndAdd(WHITE, CLOCKWISE);
                            Debug.Log("Solved green. Orange right");
                            break;
                        case ORANGE:
                            rotateAndAdd(WHITE, CLOCKWISE);
                            rotateAndAdd(BLUE, COUNTER_CLOCKWISE);
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            Debug.Log("Solved orange. Orange right");
                            break;
                        case RED:
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            rotateAndAdd(BLUE, COUNTER_CLOCKWISE);
                            rotateAndAdd(WHITE, CLOCKWISE);
                            Debug.Log("Solved red. Orange right");
                            break;
                    }

                    continue;
                }

                if (_state.GetGreenFace()[RIGHT] == WHITE)
                {
                    switch (_state.GetOrangeFace()[LEFT])
                    {
                        case BLUE:
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            rotateAndAdd(ORANGE, COUNTER_CLOCKWISE);
                            rotateAndAdd(WHITE, CLOCKWISE);
                            Debug.Log("Solved blue. Green right");
                            break;
                        case GREEN:
                            rotateAndAdd(WHITE, CLOCKWISE);
                            rotateAndAdd(ORANGE, COUNTER_CLOCKWISE);
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            Debug.Log("Solved green. Green right");
                            break;
                        case ORANGE:
                            rotateAndAdd(ORANGE, COUNTER_CLOCKWISE);
                            Debug.Log("Solved orange. Green right");
                            break;
                        case RED:
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            rotateAndAdd(ORANGE, COUNTER_CLOCKWISE);
                            rotateAndAdd(WHITE, CLOCKWISE);
                            rotateAndAdd(WHITE, CLOCKWISE);
                            Debug.Log("Solved red. Green right");
                            break;
                    }

                    continue;
                }

                if (_state.GetRedFace()[TOP] == WHITE)
                {
                    switch (_state.GetYellowFace()[BOTTOM])
                    {
                        case BLUE:
                            rotateAndAdd(RED, COUNTER_CLOCKWISE);
                            rotateAndAdd(BLUE, CLOCKWISE);
                            rotateAndAdd(RED, CLOCKWISE);
                            Debug.Log("Solved blue. Red top");
                            break;
                        case GREEN:
                            rotateAndAdd(RED, CLOCKWISE);
                            rotateAndAdd(GREEN, COUNTER_CLOCKWISE);
                            rotateAndAdd(RED, COUNTER_CLOCKWISE);
                            Debug.Log("Solved green. Red top");
                            break;
                        case ORANGE:
                            rotateAndAdd(YELLOW, COUNTER_CLOCKWISE);
                            rotateAndAdd(GREEN, CLOCKWISE);
                            rotateAndAdd(ORANGE, COUNTER_CLOCKWISE);
                            rotateAndAdd(GREEN, COUNTER_CLOCKWISE);
                            Debug.Log("Solved orange. Red top");
                            break;
                        case RED:
                            rotateAndAdd(YELLOW, COUNTER_CLOCKWISE);
                            rotateAndAdd(GREEN, COUNTER_CLOCKWISE);
                            rotateAndAdd(RED, CLOCKWISE);
                            rotateAndAdd(GREEN, CLOCKWISE);
                            Debug.Log("Solved red. Red top");
                            break;
                    }
                }
                else if (_state.GetYellowFace()[BOTTOM] == WHITE)
                {
                    switch (_state.GetRedFace()[TOP])
                    {
                        case BLUE:
                            rotateAndAdd(YELLOW, CLOCKWISE);
                            rotateAndAdd(BLUE, CLOCKWISE);
                            rotateAndAdd(BLUE, CLOCKWISE);
                            Debug.Log("Solved blue. Yellow bottom");
                            break;
                        case GREEN:
                            rotateAndAdd(YELLOW, COUNTER_CLOCKWISE);
                            rotateAndAdd(GREEN, CLOCKWISE);
                            rotateAndAdd(GREEN, CLOCKWISE);
                            Debug.Log("Solved green. Yellow bottom");
                            break;
                        case ORANGE:
                            rotateAndAdd(YELLOW, CLOCKWISE);
                            rotateAndAdd(YELLOW, CLOCKWISE);
                            rotateAndAdd(ORANGE, CLOCKWISE);
                            rotateAndAdd(ORANGE, CLOCKWISE);
                            Debug.Log("Solved orange. Yellow bottom");
                            break;
                        case RED:
                            rotateAndAdd(RED, CLOCKWISE);
                            rotateAndAdd(RED, CLOCKWISE);
                            Debug.Log("Solved red. Yellow bottom");
                            break;
                    }
                }
                else if (!whiteCrossIsSolved())
                {
                    Debug.Log("Rotating yellow clockwise");
                    rotateAndAdd(YELLOW, CLOCKWISE);
                    edgesToSolve++;
                }
            }

            return _currentStepRotations;
        }
        
        private bool whiteFaceIsSolved()
        {
            return (_state.GetWhiteFace()[TOP_RIGHT] == WHITE && _state.GetRedFace()[BOTTOM_RIGHT] == RED)
                   && (_state.GetWhiteFace()[BOTTOM_RIGHT] == WHITE && _state.GetGreenFace()[BOTTOM_RIGHT] == GREEN)
                   && (_state.GetWhiteFace()[BOTTOM_LEFT] == WHITE && _state.GetOrangeFace()[BOTTOM_RIGHT] == ORANGE)
                   && (_state.GetWhiteFace()[TOP_LEFT] == WHITE && _state.GetBlueFace()[BOTTOM_RIGHT] == BLUE);
        }

        private IEnumerable<Rotation> solveWhiteFace()
        {
            _currentStepRotations = new Stack<Rotation>();

            //Up to four loops are required to place every white corner
            var cornersToSolve = 4;
            
            //Decrement the number of remaining corners if some are already solved
            if (_state.GetWhiteFace()[TOP_RIGHT] == WHITE && _state.GetRedFace()[BOTTOM_RIGHT] == RED)
            {
                cornersToSolve--;
            }

            if (_state.GetWhiteFace()[BOTTOM_RIGHT] == WHITE && _state.GetGreenFace()[BOTTOM_RIGHT] == GREEN)
            {
                cornersToSolve--;
            }

            if (_state.GetWhiteFace()[BOTTOM_LEFT] == WHITE && _state.GetOrangeFace()[BOTTOM_RIGHT] == ORANGE)
            {
                cornersToSolve--;
            }

            if (_state.GetWhiteFace()[TOP_LEFT] == WHITE && _state.GetBlueFace()[BOTTOM_RIGHT] == BLUE)
            {
                cornersToSolve--;
            }

            var max = 50;
            for (; cornersToSolve > 0 || max > 0; cornersToSolve--, max--)
            {
                Debug.Log(_state.GetRedFace()[TOP_RIGHT]);
                
                if (_state.GetRedFace()[TOP_RIGHT] == WHITE)
                {
                    switch (_state.GetYellowFace()[BOTTOM_RIGHT])
                    {
                        case BLUE:
                            rotateAndAdd(YELLOW, CLOCKWISE);
                            rotateAndAdd(BLUE, COUNTER_CLOCKWISE);
                            rotateAndAdd(YELLOW, COUNTER_CLOCKWISE);
                            rotateAndAdd(BLUE, CLOCKWISE);
                            break;
                        case GREEN:
                            rotateAndAdd(YELLOW, COUNTER_CLOCKWISE);
                            rotateAndAdd(GREEN, COUNTER_CLOCKWISE);
                            rotateAndAdd(YELLOW, COUNTER_CLOCKWISE);
                            rotateAndAdd(GREEN, CLOCKWISE);
                            break;
                        case ORANGE:
                            rotateAndAdd(YELLOW, CLOCKWISE);
                            rotateAndAdd(YELLOW, CLOCKWISE);
                            rotateAndAdd(ORANGE, COUNTER_CLOCKWISE);
                            rotateAndAdd(YELLOW, COUNTER_CLOCKWISE);
                            rotateAndAdd(ORANGE, CLOCKWISE);
                            break;
                        case RED:
                            rotateAndAdd(RED, COUNTER_CLOCKWISE);
                            rotateAndAdd(YELLOW, COUNTER_CLOCKWISE);
                            rotateAndAdd(RED, CLOCKWISE);
                            break;
                    }

                    continue;
                }
                
                if (_state.GetRedFace()[TOP_LEFT] == WHITE)
                {
                    switch (_state.GetYellowFace()[BOTTOM_LEFT])
                    {
                        case BLUE:
                            rotateAndAdd(YELLOW, CLOCKWISE);
                            rotateAndAdd(BLUE, CLOCKWISE);
                            rotateAndAdd(YELLOW, CLOCKWISE);
                            rotateAndAdd(BLUE, COUNTER_CLOCKWISE);
                            break;
                        case GREEN:
                            rotateAndAdd(GREEN, COUNTER_CLOCKWISE);
                            rotateAndAdd(ORANGE, CLOCKWISE);
                            rotateAndAdd(YELLOW, CLOCKWISE);
                            rotateAndAdd(GREEN, COUNTER_CLOCKWISE);
                            break;
                        case ORANGE:
                            rotateAndAdd(YELLOW, CLOCKWISE);
                            rotateAndAdd(YELLOW, CLOCKWISE);
                            rotateAndAdd(ORANGE, CLOCKWISE);
                            rotateAndAdd(YELLOW, CLOCKWISE);
                            rotateAndAdd(ORANGE, COUNTER_CLOCKWISE);
                            break;
                        case RED:
                            rotateAndAdd(RED, CLOCKWISE);
                            rotateAndAdd(YELLOW, CLOCKWISE);
                            rotateAndAdd(RED, COUNTER_CLOCKWISE);
                            break;
                    }
                    
                    continue;
                }

                if (_state.GetWhiteFace()[TOP_RIGHT] == WHITE && _state.GetRedFace()[BOTTOM_RIGHT] != RED)
                {
                    rotateAndAdd(RED, COUNTER_CLOCKWISE);
                    rotateAndAdd(YELLOW, COUNTER_CLOCKWISE);
                    rotateAndAdd(RED, CLOCKWISE);
                    cornersToSolve++;
                    continue;
                }
                
                if (_state.GetWhiteFace()[BOTTOM_RIGHT] == WHITE && _state.GetGreenFace()[BOTTOM_RIGHT] != GREEN)
                {
                    rotateAndAdd(GREEN, COUNTER_CLOCKWISE);
                    rotateAndAdd(YELLOW, COUNTER_CLOCKWISE);
                    rotateAndAdd(GREEN, CLOCKWISE);
                    cornersToSolve++;
                    continue;
                }
                
                if (_state.GetWhiteFace()[BOTTOM_LEFT] == WHITE && _state.GetOrangeFace()[BOTTOM_RIGHT] != ORANGE)
                {
                    rotateAndAdd(ORANGE, COUNTER_CLOCKWISE);
                    rotateAndAdd(YELLOW, COUNTER_CLOCKWISE);
                    rotateAndAdd(ORANGE, CLOCKWISE);
                    cornersToSolve++;
                    continue;
                }
                
                if (_state.GetWhiteFace()[TOP_LEFT] == WHITE && _state.GetBlueFace()[BOTTOM_RIGHT] != BLUE)
                {
                    rotateAndAdd(BLUE, COUNTER_CLOCKWISE);
                    rotateAndAdd(YELLOW, COUNTER_CLOCKWISE);
                    rotateAndAdd(BLUE, CLOCKWISE);
                    cornersToSolve++;
                    continue;
                }

                if (_state.GetYellowFace()[BOTTOM_RIGHT] == WHITE)
                {
                    rotateAndAdd(RED, COUNTER_CLOCKWISE);
                    rotateAndAdd(YELLOW, CLOCKWISE);
                    rotateAndAdd(RED, CLOCKWISE);
                    cornersToSolve++;
                    continue;
                }
                
                if (!whiteFaceIsSolved())
                {
                    Debug.Log("Rotating yellow clockwise");
                    rotateAndAdd(YELLOW, CLOCKWISE);
                    cornersToSolve++;
                }
            }

            return _currentStepRotations;
        }
    }
}