﻿using System;
using Rubiks.Constants;
using Rubiks.Enums;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Debug = UnityEngine.Debug;

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
        private bool _isError;
        private readonly Stopwatch _stopwatch;
        private int _generatedStates;

        public HumanSolver(CubeState initialState)
        {
            _state = initialState.Clone();
            _generatedStates = 0;
            _stopwatch = new Stopwatch();
        }

        public async Task<Tuple<Stack<Rotation>, int, TimeSpan>> Solve()
        {
            return await Task.Run(() =>
            {
                _stopwatch.Start();
                
                var solutionPath = new Stack<Rotation>();

                //Determine the steps required to solve the white cross
                var whiteCrossSteps = solveWhiteCross();

                //Determine the steps required to solve the white face
                var whiteFaceSteps = !_isError ? solveWhiteFace() : new Stack<Rotation>();

                //Determine the steps required to solve the middle layer
                var middleLayerSteps = !_isError ? solveMiddleLayer() : new Stack<Rotation>();

                //Add the steps required to solve the yellow cross
                var yellowCrossSteps = !_isError ? solveYellowCross() : new Stack<Rotation>();

                //Add the steps required to solve the yellow face
                var yellowFaceSteps = !_isError ? solveYellowFace() : new Stack<Rotation>();

                //Add the steps required to solve the yellow corners
                var yellowCornerSteps = !_isError ? solveYellowCorners() : new Stack<Rotation>();
                
                //Add the steps required to solve the yellow edges
                var yellowEdgeSteps = !_isError ? solveYellowEdges() : new Stack<Rotation>();

                //Return the sequence of operators that solves the cube
                foreach (var step in yellowEdgeSteps)
                {
                    solutionPath.Push(step);
                }
                
                foreach (var step in yellowCornerSteps)
                {
                    solutionPath.Push(step);
                }
                
                foreach (var step in yellowFaceSteps)
                {
                    solutionPath.Push(step);
                }
                
                foreach (var step in yellowCrossSteps)
                {
                    solutionPath.Push(step);
                }
                
                foreach (var step in middleLayerSteps)
                {
                    solutionPath.Push(step);
                }

                foreach (var step in whiteFaceSteps)
                {
                    solutionPath.Push(step);
                }

                foreach (var step in whiteCrossSteps)
                {
                    solutionPath.Push(step);
                }
                
                _stopwatch.Stop();

                return new Tuple<Stack<Rotation>, int, TimeSpan>(solutionPath, _generatedStates, _stopwatch.Elapsed);
            });
        }

        private void rotateAndAdd(FaceColor faceColor, RotationDirection direction)
        {
            _state.Rotate(faceColor, direction);
            _currentStepRotations.Push(new Rotation(faceColor, direction));
            _generatedStates++;
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

            var count = 0;
            while (!whiteCrossIsSolved() && count < 20)
            {
                count--;
                
                if (_state.GetWhiteFace()[TOP] == WHITE && _state.GetRedFace()[BOTTOM] != RED)
                {
                    switch (_state.GetRedFace()[BOTTOM])
                    {
                        case BLUE:
                            rotateAndAdd(RED, CLOCKWISE);
                            rotateAndAdd(WHITE, CLOCKWISE);
                            rotateAndAdd(RED, COUNTER_CLOCKWISE);
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            break;
                        case GREEN:
                            rotateAndAdd(RED, CLOCKWISE);
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            rotateAndAdd(RED, COUNTER_CLOCKWISE);
                            rotateAndAdd(WHITE, CLOCKWISE);
                            break;
                        case ORANGE:
                            rotateAndAdd(RED, CLOCKWISE);
                            rotateAndAdd(WHITE, CLOCKWISE);
                            rotateAndAdd(WHITE, CLOCKWISE);
                            rotateAndAdd(RED, COUNTER_CLOCKWISE);
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
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
                            break;
                        case ORANGE:
                            rotateAndAdd(GREEN, CLOCKWISE);
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            rotateAndAdd(GREEN, COUNTER_CLOCKWISE);
                            rotateAndAdd(WHITE, CLOCKWISE);
                            break;
                        case RED:
                            rotateAndAdd(GREEN, CLOCKWISE);
                            rotateAndAdd(WHITE, CLOCKWISE);
                            rotateAndAdd(GREEN, COUNTER_CLOCKWISE);
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
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
                            break;
                        case GREEN:
                            rotateAndAdd(ORANGE, CLOCKWISE);
                            rotateAndAdd(WHITE, CLOCKWISE);
                            rotateAndAdd(ORANGE, COUNTER_CLOCKWISE);
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            break;
                        case RED:
                            rotateAndAdd(ORANGE, CLOCKWISE);
                            rotateAndAdd(WHITE, CLOCKWISE);
                            rotateAndAdd(WHITE, CLOCKWISE);
                            rotateAndAdd(ORANGE, COUNTER_CLOCKWISE);
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
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
                            break;
                        case ORANGE:
                            rotateAndAdd(BLUE, CLOCKWISE);
                            rotateAndAdd(WHITE, CLOCKWISE);
                            rotateAndAdd(BLUE, COUNTER_CLOCKWISE);
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            break;
                        case RED:
                            rotateAndAdd(BLUE, CLOCKWISE);
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            rotateAndAdd(BLUE, COUNTER_CLOCKWISE);
                            rotateAndAdd(WHITE, CLOCKWISE);
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
                            break;
                        case GREEN:
                            rotateAndAdd(RED, COUNTER_CLOCKWISE);
                            rotateAndAdd(GREEN, COUNTER_CLOCKWISE);
                            break;
                        case ORANGE:
                            rotateAndAdd(RED, CLOCKWISE);
                            rotateAndAdd(WHITE, CLOCKWISE);
                            rotateAndAdd(BLUE, CLOCKWISE);
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            break;
                        case RED:
                            rotateAndAdd(RED, CLOCKWISE);
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            rotateAndAdd(BLUE, CLOCKWISE);
                            rotateAndAdd(WHITE, CLOCKWISE);
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
                            break;
                        case GREEN:
                            rotateAndAdd(GREEN, CLOCKWISE);
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            rotateAndAdd(RED, CLOCKWISE);
                            rotateAndAdd(WHITE, CLOCKWISE);
                            break;
                        case ORANGE:
                            rotateAndAdd(GREEN, COUNTER_CLOCKWISE);
                            rotateAndAdd(ORANGE, COUNTER_CLOCKWISE);
                            break;
                        case RED:
                            rotateAndAdd(GREEN, CLOCKWISE);
                            rotateAndAdd(RED, CLOCKWISE);
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
                            break;
                        case GREEN:
                            rotateAndAdd(ORANGE, CLOCKWISE);
                            rotateAndAdd(GREEN, CLOCKWISE);
                            break;
                        case ORANGE:
                            rotateAndAdd(ORANGE, CLOCKWISE);
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            rotateAndAdd(GREEN, CLOCKWISE);
                            rotateAndAdd(WHITE, CLOCKWISE);
                            break;
                        case RED:
                            rotateAndAdd(ORANGE, CLOCKWISE);
                            rotateAndAdd(WHITE, CLOCKWISE);
                            rotateAndAdd(GREEN, CLOCKWISE);
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
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
                            break;
                        case GREEN:
                            rotateAndAdd(BLUE, CLOCKWISE);
                            rotateAndAdd(WHITE, CLOCKWISE);
                            rotateAndAdd(ORANGE, CLOCKWISE);
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            break;
                        case ORANGE:
                            rotateAndAdd(BLUE, CLOCKWISE);
                            rotateAndAdd(ORANGE, CLOCKWISE);
                            break;
                        case RED:
                            rotateAndAdd(BLUE, COUNTER_CLOCKWISE);
                            rotateAndAdd(RED, COUNTER_CLOCKWISE);
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
                            break;
                        case GREEN:
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            rotateAndAdd(BLUE, CLOCKWISE);
                            rotateAndAdd(WHITE, CLOCKWISE);
                            rotateAndAdd(WHITE, CLOCKWISE);
                            break;
                        case ORANGE:
                            rotateAndAdd(WHITE, CLOCKWISE);
                            rotateAndAdd(BLUE, CLOCKWISE);
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            break;
                        case RED:
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            rotateAndAdd(BLUE, CLOCKWISE);
                            rotateAndAdd(WHITE, CLOCKWISE);
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
                            break;
                        case GREEN:
                            rotateAndAdd(WHITE, CLOCKWISE);
                            rotateAndAdd(ORANGE, CLOCKWISE);
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            break;
                        case ORANGE:
                            rotateAndAdd(ORANGE, CLOCKWISE);
                            break;
                        case RED:
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            rotateAndAdd(ORANGE, CLOCKWISE);
                            rotateAndAdd(WHITE, CLOCKWISE);
                            rotateAndAdd(WHITE, CLOCKWISE);
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
                            break;
                        case GREEN:
                            rotateAndAdd(GREEN, CLOCKWISE);
                            break;
                        case ORANGE:
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            rotateAndAdd(GREEN, CLOCKWISE);
                            rotateAndAdd(WHITE, CLOCKWISE);
                            break;
                        case RED:
                            rotateAndAdd(WHITE, CLOCKWISE);
                            rotateAndAdd(GREEN, CLOCKWISE);
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
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
                            break;
                        case GREEN:
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            rotateAndAdd(RED, CLOCKWISE);
                            rotateAndAdd(WHITE, CLOCKWISE);
                            break;
                        case ORANGE:
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            rotateAndAdd(RED, CLOCKWISE);
                            rotateAndAdd(WHITE, CLOCKWISE);
                            rotateAndAdd(WHITE, CLOCKWISE);
                            break;
                        case RED:
                            rotateAndAdd(RED, CLOCKWISE);
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
                            break;
                        case GREEN:
                            rotateAndAdd(GREEN, COUNTER_CLOCKWISE);
                            break;
                        case ORANGE:
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            rotateAndAdd(GREEN, COUNTER_CLOCKWISE);
                            rotateAndAdd(WHITE, CLOCKWISE);
                            break;
                        case RED:
                            rotateAndAdd(WHITE, CLOCKWISE);
                            rotateAndAdd(GREEN, COUNTER_CLOCKWISE);
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
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
                            break;
                        case GREEN:
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            rotateAndAdd(RED, COUNTER_CLOCKWISE);
                            rotateAndAdd(WHITE, CLOCKWISE);
                            break;
                        case ORANGE:
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            rotateAndAdd(RED, COUNTER_CLOCKWISE);
                            rotateAndAdd(WHITE, CLOCKWISE);
                            rotateAndAdd(WHITE, CLOCKWISE);
                            break;
                        case RED:
                            rotateAndAdd(RED, COUNTER_CLOCKWISE);
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
                            break;
                        case GREEN:
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            rotateAndAdd(BLUE, COUNTER_CLOCKWISE);
                            rotateAndAdd(WHITE, CLOCKWISE);
                            rotateAndAdd(WHITE, CLOCKWISE);
                            break;
                        case ORANGE:
                            rotateAndAdd(WHITE, CLOCKWISE);
                            rotateAndAdd(BLUE, COUNTER_CLOCKWISE);
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            break;
                        case RED:
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            rotateAndAdd(BLUE, COUNTER_CLOCKWISE);
                            rotateAndAdd(WHITE, CLOCKWISE);
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
                            break;
                        case GREEN:
                            rotateAndAdd(WHITE, CLOCKWISE);
                            rotateAndAdd(ORANGE, COUNTER_CLOCKWISE);
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            break;
                        case ORANGE:
                            rotateAndAdd(ORANGE, COUNTER_CLOCKWISE);
                            break;
                        case RED:
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            rotateAndAdd(WHITE, COUNTER_CLOCKWISE);
                            rotateAndAdd(ORANGE, COUNTER_CLOCKWISE);
                            rotateAndAdd(WHITE, CLOCKWISE);
                            rotateAndAdd(WHITE, CLOCKWISE);
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
                            break;
                        case GREEN:
                            rotateAndAdd(RED, CLOCKWISE);
                            rotateAndAdd(GREEN, COUNTER_CLOCKWISE);
                            rotateAndAdd(RED, COUNTER_CLOCKWISE);
                            break;
                        case ORANGE:
                            rotateAndAdd(YELLOW, COUNTER_CLOCKWISE);
                            rotateAndAdd(GREEN, CLOCKWISE);
                            rotateAndAdd(ORANGE, COUNTER_CLOCKWISE);
                            rotateAndAdd(GREEN, COUNTER_CLOCKWISE);
                            break;
                        case RED:
                            rotateAndAdd(YELLOW, COUNTER_CLOCKWISE);
                            rotateAndAdd(GREEN, COUNTER_CLOCKWISE);
                            rotateAndAdd(RED, CLOCKWISE);
                            rotateAndAdd(GREEN, CLOCKWISE);
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
                            break;
                        case GREEN:
                            rotateAndAdd(YELLOW, COUNTER_CLOCKWISE);
                            rotateAndAdd(GREEN, CLOCKWISE);
                            rotateAndAdd(GREEN, CLOCKWISE);
                            break;
                        case ORANGE:
                            rotateAndAdd(YELLOW, CLOCKWISE);
                            rotateAndAdd(YELLOW, CLOCKWISE);
                            rotateAndAdd(ORANGE, CLOCKWISE);
                            rotateAndAdd(ORANGE, CLOCKWISE);
                            break;
                        case RED:
                            rotateAndAdd(RED, CLOCKWISE);
                            rotateAndAdd(RED, CLOCKWISE);
                            break;
                    }
                }
                else
                {
                    rotateAndAdd(YELLOW, CLOCKWISE);
                }
            }

            if (!whiteCrossIsSolved())
            {
                Debug.Log("Error solving white cross.");
                _isError = true;
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

            var count = 0;
            while (!whiteFaceIsSolved() && count < 20)
            {
                count++;
                
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
                            rotateAndAdd(YELLOW, COUNTER_CLOCKWISE);
                            rotateAndAdd(GREEN, CLOCKWISE);
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

                if ((_state.GetWhiteFace()[TOP_RIGHT] == WHITE && _state.GetRedFace()[BOTTOM_RIGHT] != RED)
                    || (_state.GetRedFace()[BOTTOM_RIGHT] == WHITE && _state.GetOrangeFace()[TOP_RIGHT] != WHITE))
                {
                    rotateAndAdd(RED, COUNTER_CLOCKWISE);
                    rotateAndAdd(YELLOW, COUNTER_CLOCKWISE);
                    rotateAndAdd(RED, CLOCKWISE);
                    continue;
                }

                if ((_state.GetWhiteFace()[BOTTOM_RIGHT] == WHITE && _state.GetGreenFace()[BOTTOM_RIGHT] != GREEN)
                    || (_state.GetGreenFace()[BOTTOM_RIGHT] == WHITE && _state.GetBlueFace()[TOP_RIGHT] != WHITE))
                {
                    rotateAndAdd(GREEN, COUNTER_CLOCKWISE);
                    rotateAndAdd(YELLOW, COUNTER_CLOCKWISE);
                    rotateAndAdd(GREEN, CLOCKWISE);
                    continue;
                }

                if ((_state.GetWhiteFace()[BOTTOM_LEFT] == WHITE && _state.GetOrangeFace()[BOTTOM_RIGHT] != ORANGE)
                    || (_state.GetOrangeFace()[BOTTOM_RIGHT] == WHITE && _state.GetRedFace()[TOP_RIGHT] != WHITE))
                {
                    rotateAndAdd(ORANGE, COUNTER_CLOCKWISE);
                    rotateAndAdd(YELLOW, COUNTER_CLOCKWISE);
                    rotateAndAdd(ORANGE, CLOCKWISE);
                    continue;
                }

                if ((_state.GetWhiteFace()[TOP_LEFT] == WHITE && _state.GetBlueFace()[BOTTOM_RIGHT] != BLUE)
                    || (_state.GetBlueFace()[BOTTOM_RIGHT] == WHITE && _state.GetGreenFace()[TOP_RIGHT] != WHITE))
                {
                    rotateAndAdd(BLUE, COUNTER_CLOCKWISE);
                    rotateAndAdd(YELLOW, COUNTER_CLOCKWISE);
                    rotateAndAdd(BLUE, CLOCKWISE);
                    continue;
                }

                if (_state.GetRedFace()[BOTTOM_LEFT] == WHITE)
                {
                    rotateAndAdd(RED, CLOCKWISE);
                    rotateAndAdd(YELLOW, CLOCKWISE);
                    rotateAndAdd(RED, COUNTER_CLOCKWISE);
                    continue;
                }

                if (_state.GetGreenFace()[BOTTOM_LEFT] == WHITE)
                {
                    rotateAndAdd(GREEN, CLOCKWISE);
                    rotateAndAdd(YELLOW, CLOCKWISE);
                    rotateAndAdd(GREEN, COUNTER_CLOCKWISE);
                    continue;
                }

                if (_state.GetOrangeFace()[BOTTOM_LEFT] == WHITE)
                {
                    rotateAndAdd(ORANGE, CLOCKWISE);
                    rotateAndAdd(YELLOW, CLOCKWISE);
                    rotateAndAdd(ORANGE, COUNTER_CLOCKWISE);
                    continue;
                }

                if (_state.GetBlueFace()[BOTTOM_LEFT] == WHITE)
                {
                    rotateAndAdd(BLUE, CLOCKWISE);
                    rotateAndAdd(YELLOW, CLOCKWISE);
                    rotateAndAdd(BLUE, COUNTER_CLOCKWISE);
                    continue;
                }

                if (_state.GetYellowFace()[BOTTOM_RIGHT] == WHITE && _state.GetWhiteFace()[TOP_RIGHT] != WHITE)
                {
                    rotateAndAdd(RED, COUNTER_CLOCKWISE);
                    rotateAndAdd(YELLOW, COUNTER_CLOCKWISE);
                    rotateAndAdd(RED, CLOCKWISE);
                    continue;
                }

                if (_state.GetYellowFace()[BOTTOM_LEFT] == WHITE && _state.GetWhiteFace()[TOP_LEFT] != WHITE)
                {
                    rotateAndAdd(BLUE, COUNTER_CLOCKWISE);
                    rotateAndAdd(YELLOW, COUNTER_CLOCKWISE);
                    rotateAndAdd(BLUE, CLOCKWISE);
                    continue;
                }

                if (_state.GetYellowFace()[TOP_LEFT] == WHITE && _state.GetWhiteFace()[BOTTOM_LEFT] != WHITE)
                {
                    rotateAndAdd(ORANGE, COUNTER_CLOCKWISE);
                    rotateAndAdd(YELLOW, COUNTER_CLOCKWISE);
                    rotateAndAdd(ORANGE, CLOCKWISE);
                    continue;
                }

                if (_state.GetYellowFace()[TOP_RIGHT] == WHITE && _state.GetWhiteFace()[BOTTOM_RIGHT] != WHITE)
                {
                    rotateAndAdd(GREEN, COUNTER_CLOCKWISE);
                    rotateAndAdd(YELLOW, COUNTER_CLOCKWISE);
                    rotateAndAdd(GREEN, CLOCKWISE);
                    continue;
                }

                if (!whiteFaceIsSolved())
                {
                    rotateAndAdd(YELLOW, CLOCKWISE);
                }
            }

            if (!whiteFaceIsSolved())
            {
                Debug.Log("Error solving white face.");
                _isError = true;
            }

            return _currentStepRotations;
        }

        private bool middleLayerIsSolved()
        {
            return (_state.GetRedFace()[LEFT] == RED && _state.GetRedFace()[RIGHT] == RED)
                   && (_state.GetGreenFace()[LEFT] == GREEN && _state.GetGreenFace()[RIGHT] == GREEN)
                   && (_state.GetOrangeFace()[LEFT] == ORANGE && _state.GetOrangeFace()[RIGHT] == ORANGE)
                   && (_state.GetBlueFace()[LEFT] == BLUE && _state.GetBlueFace()[RIGHT] == BLUE);
        }

        private IEnumerable<Rotation> solveMiddleLayer()
        {
            _currentStepRotations = new Stack<Rotation>();

            var count = 0;
            while (!middleLayerIsSolved() && count < 20)
            {
                count++;
                
                if (_state.GetRedFace()[TOP] == RED && _state.GetYellowFace()[BOTTOM] != YELLOW)
                {
                    if (_state.GetYellowFace()[BOTTOM] == BLUE)
                    {
                        rotateAndAdd(YELLOW, COUNTER_CLOCKWISE);
                        rotateAndAdd(BLUE, COUNTER_CLOCKWISE);
                        rotateAndAdd(YELLOW, CLOCKWISE);
                        rotateAndAdd(BLUE, CLOCKWISE);
                        rotateAndAdd(YELLOW, CLOCKWISE);
                        rotateAndAdd(RED, CLOCKWISE);
                        rotateAndAdd(YELLOW, COUNTER_CLOCKWISE);
                        rotateAndAdd(RED, COUNTER_CLOCKWISE);
                    }
                    else
                    {
                        rotateAndAdd(YELLOW, CLOCKWISE);
                        rotateAndAdd(GREEN, CLOCKWISE);
                        rotateAndAdd(YELLOW, COUNTER_CLOCKWISE);
                        rotateAndAdd(GREEN, COUNTER_CLOCKWISE);
                        rotateAndAdd(YELLOW, COUNTER_CLOCKWISE);
                        rotateAndAdd(RED, COUNTER_CLOCKWISE);
                        rotateAndAdd(YELLOW, CLOCKWISE);
                        rotateAndAdd(RED, CLOCKWISE);
                    }

                    continue;
                }

                if (_state.GetGreenFace()[TOP] == GREEN && _state.GetYellowFace()[RIGHT] != YELLOW)
                {
                    if (_state.GetYellowFace()[RIGHT] == RED)
                    {
                        rotateAndAdd(YELLOW, COUNTER_CLOCKWISE);
                        rotateAndAdd(RED, COUNTER_CLOCKWISE);
                        rotateAndAdd(YELLOW, CLOCKWISE);
                        rotateAndAdd(RED, CLOCKWISE);
                        rotateAndAdd(YELLOW, CLOCKWISE);
                        rotateAndAdd(GREEN, CLOCKWISE);
                        rotateAndAdd(YELLOW, COUNTER_CLOCKWISE);
                        rotateAndAdd(GREEN, COUNTER_CLOCKWISE);
                    }
                    else
                    {
                        rotateAndAdd(YELLOW, CLOCKWISE);
                        rotateAndAdd(ORANGE, CLOCKWISE);
                        rotateAndAdd(YELLOW, COUNTER_CLOCKWISE);
                        rotateAndAdd(ORANGE, COUNTER_CLOCKWISE);
                        rotateAndAdd(YELLOW, COUNTER_CLOCKWISE);
                        rotateAndAdd(GREEN, COUNTER_CLOCKWISE);
                        rotateAndAdd(YELLOW, CLOCKWISE);
                        rotateAndAdd(GREEN, CLOCKWISE);
                    }

                    continue;
                }

                if (_state.GetOrangeFace()[TOP] == ORANGE && _state.GetYellowFace()[TOP] != YELLOW)
                {
                    if (_state.GetYellowFace()[TOP] == GREEN)
                    {
                        rotateAndAdd(YELLOW, COUNTER_CLOCKWISE);
                        rotateAndAdd(GREEN, COUNTER_CLOCKWISE);
                        rotateAndAdd(YELLOW, CLOCKWISE);
                        rotateAndAdd(GREEN, CLOCKWISE);
                        rotateAndAdd(YELLOW, CLOCKWISE);
                        rotateAndAdd(ORANGE, CLOCKWISE);
                        rotateAndAdd(YELLOW, COUNTER_CLOCKWISE);
                        rotateAndAdd(ORANGE, COUNTER_CLOCKWISE);
                    }
                    else
                    {
                        rotateAndAdd(YELLOW, CLOCKWISE);
                        rotateAndAdd(BLUE, CLOCKWISE);
                        rotateAndAdd(YELLOW, COUNTER_CLOCKWISE);
                        rotateAndAdd(BLUE, COUNTER_CLOCKWISE);
                        rotateAndAdd(YELLOW, COUNTER_CLOCKWISE);
                        rotateAndAdd(ORANGE, COUNTER_CLOCKWISE);
                        rotateAndAdd(YELLOW, CLOCKWISE);
                        rotateAndAdd(ORANGE, CLOCKWISE);
                    }

                    continue;
                }

                if (_state.GetBlueFace()[TOP] == BLUE && _state.GetYellowFace()[LEFT] != YELLOW)
                {
                    if (_state.GetYellowFace()[LEFT] == ORANGE)
                    {
                        rotateAndAdd(YELLOW, COUNTER_CLOCKWISE);
                        rotateAndAdd(ORANGE, COUNTER_CLOCKWISE);
                        rotateAndAdd(YELLOW, CLOCKWISE);
                        rotateAndAdd(ORANGE, CLOCKWISE);
                        rotateAndAdd(YELLOW, CLOCKWISE);
                        rotateAndAdd(BLUE, CLOCKWISE);
                        rotateAndAdd(YELLOW, COUNTER_CLOCKWISE);
                        rotateAndAdd(BLUE, COUNTER_CLOCKWISE);
                    }
                    else
                    {
                        rotateAndAdd(YELLOW, CLOCKWISE);
                        rotateAndAdd(RED, CLOCKWISE);
                        rotateAndAdd(YELLOW, COUNTER_CLOCKWISE);
                        rotateAndAdd(RED, COUNTER_CLOCKWISE);
                        rotateAndAdd(YELLOW, COUNTER_CLOCKWISE);
                        rotateAndAdd(BLUE, COUNTER_CLOCKWISE);
                        rotateAndAdd(YELLOW, CLOCKWISE);
                        rotateAndAdd(BLUE, CLOCKWISE);
                    }

                    continue;
                }

                if ((_state.GetRedFace()[TOP] != YELLOW && _state.GetYellowFace()[BOTTOM] != YELLOW)
                    || (_state.GetGreenFace()[TOP] != YELLOW && _state.GetYellowFace()[RIGHT] != YELLOW)
                    || (_state.GetOrangeFace()[TOP] != YELLOW && _state.GetYellowFace()[TOP] != YELLOW)
                    || (_state.GetBlueFace()[TOP] != YELLOW && _state.GetYellowFace()[LEFT] != YELLOW))
                {
                    rotateAndAdd(YELLOW, CLOCKWISE);
                    continue;
                }

                if ((_state.GetRedFace()[RIGHT] != RED && _state.GetRedFace()[RIGHT] != YELLOW)
                    || (_state.GetGreenFace()[LEFT] != GREEN && _state.GetGreenFace()[LEFT] != YELLOW))
                {
                    rotateAndAdd(YELLOW, CLOCKWISE);
                    rotateAndAdd(GREEN, CLOCKWISE);
                    rotateAndAdd(YELLOW, COUNTER_CLOCKWISE);
                    rotateAndAdd(GREEN, COUNTER_CLOCKWISE);
                    rotateAndAdd(YELLOW, COUNTER_CLOCKWISE);
                    rotateAndAdd(RED, COUNTER_CLOCKWISE);
                    rotateAndAdd(YELLOW, CLOCKWISE);
                    rotateAndAdd(RED, CLOCKWISE);
                    continue;
                }

                if (_state.GetGreenFace()[RIGHT] != GREEN && _state.GetGreenFace()[RIGHT] != YELLOW
                    || _state.GetOrangeFace()[LEFT] != ORANGE && _state.GetOrangeFace()[LEFT] != YELLOW)
                {
                    rotateAndAdd(YELLOW, CLOCKWISE);
                    rotateAndAdd(ORANGE, CLOCKWISE);
                    rotateAndAdd(YELLOW, COUNTER_CLOCKWISE);
                    rotateAndAdd(ORANGE, COUNTER_CLOCKWISE);
                    rotateAndAdd(YELLOW, COUNTER_CLOCKWISE);
                    rotateAndAdd(GREEN, COUNTER_CLOCKWISE);
                    rotateAndAdd(YELLOW, CLOCKWISE);
                    rotateAndAdd(GREEN, CLOCKWISE);
                    continue;
                }

                if (_state.GetOrangeFace()[RIGHT] != ORANGE && _state.GetOrangeFace()[RIGHT] != YELLOW
                    || _state.GetBlueFace()[LEFT] != BLUE && _state.GetBlueFace()[LEFT] != YELLOW)
                {
                    rotateAndAdd(YELLOW, CLOCKWISE);
                    rotateAndAdd(BLUE, CLOCKWISE);
                    rotateAndAdd(YELLOW, COUNTER_CLOCKWISE);
                    rotateAndAdd(BLUE, COUNTER_CLOCKWISE);
                    rotateAndAdd(YELLOW, COUNTER_CLOCKWISE);
                    rotateAndAdd(ORANGE, COUNTER_CLOCKWISE);
                    rotateAndAdd(YELLOW, CLOCKWISE);
                    rotateAndAdd(ORANGE, CLOCKWISE);
                    continue;
                }

                if (_state.GetBlueFace()[RIGHT] != BLUE && _state.GetBlueFace()[RIGHT] != YELLOW
                    || _state.GetRedFace()[LEFT] != RED && _state.GetRedFace()[LEFT] != YELLOW)
                {
                    rotateAndAdd(YELLOW, CLOCKWISE);
                    rotateAndAdd(RED, CLOCKWISE);
                    rotateAndAdd(YELLOW, COUNTER_CLOCKWISE);
                    rotateAndAdd(RED, COUNTER_CLOCKWISE);
                    rotateAndAdd(YELLOW, COUNTER_CLOCKWISE);
                    rotateAndAdd(BLUE, COUNTER_CLOCKWISE);
                    rotateAndAdd(YELLOW, CLOCKWISE);
                    rotateAndAdd(BLUE, CLOCKWISE);
                }
            }

            if (!middleLayerIsSolved())
            {
                Debug.Log("Error solving middle layer.");
                _isError = true;
            }
            
            return _currentStepRotations;
        }

        private bool yellowCrossIsSolved()
        {
            return _state.GetYellowFace()[TOP] == YELLOW
                   && _state.GetYellowFace()[RIGHT] == YELLOW
                   && _state.GetYellowFace()[BOTTOM] == YELLOW
                   && _state.GetYellowFace()[LEFT] == YELLOW;
        }

        private IEnumerable<Rotation> solveYellowCross()
        {
            _currentStepRotations = new Stack<Rotation>();

            var count = 0;
            while (!yellowCrossIsSolved() && count < 20)
            {
                count++;
                
                if (_state.GetYellowFace()[BOTTOM] == YELLOW)
                {
                    if (_state.GetYellowFace()[LEFT] == YELLOW || _state.GetYellowFace()[TOP] == YELLOW)
                    {
                        rotateAndAdd(YELLOW, CLOCKWISE);
                    }
                    else if (_state.GetYellowFace()[RIGHT] == YELLOW)
                    {
                        rotateAndAdd(YELLOW, CLOCKWISE);
                        rotateAndAdd(YELLOW, CLOCKWISE);
                    }
                }

                if (_state.GetYellowFace()[TOP] == YELLOW && _state.GetYellowFace()[RIGHT] == YELLOW)
                {
                    rotateAndAdd(YELLOW, COUNTER_CLOCKWISE);
                }

                rotateAndAdd(RED, CLOCKWISE);
                rotateAndAdd(GREEN, CLOCKWISE);
                rotateAndAdd(YELLOW, CLOCKWISE);
                rotateAndAdd(GREEN, COUNTER_CLOCKWISE);
                rotateAndAdd(YELLOW, COUNTER_CLOCKWISE);
                rotateAndAdd(RED, COUNTER_CLOCKWISE);
            }
            
            if (!yellowCrossIsSolved())
            {
                Debug.Log("Error solving yellow cross.");
                _isError = true;
            }

            return _currentStepRotations;
        }

        private bool yellowFaceIsSolved()
        {
            return _state.GetYellowFace()[TOP_RIGHT] == YELLOW
                   && _state.GetYellowFace()[BOTTOM_RIGHT] == YELLOW
                   && _state.GetYellowFace()[BOTTOM_LEFT] == YELLOW
                   && _state.GetYellowFace()[TOP_LEFT] == YELLOW;
        }
        
        private IEnumerable<Rotation> solveYellowFace()
        {
            _currentStepRotations = new Stack<Rotation>();

            var count = 0;
            while (!yellowFaceIsSolved() && count < 20)
            {
                count++;

                var cornerMatches = 0;
                if (_state.GetYellowFace()[TOP_LEFT] == YELLOW)
                {
                    cornerMatches++;
                }
                if (_state.GetYellowFace()[TOP_RIGHT] == YELLOW)
                {
                    cornerMatches++;
                }
                if (_state.GetYellowFace()[BOTTOM_LEFT] == YELLOW)
                {
                    cornerMatches++;
                }
                if (_state.GetYellowFace()[BOTTOM_RIGHT] == YELLOW)
                {
                    cornerMatches++;
                }
                
                if (cornerMatches == 0)
                {
                    while (_state.GetBlueFace()[TOP_RIGHT] != YELLOW)
                    {
                        rotateAndAdd(YELLOW, CLOCKWISE);
                    }
                }
                else if (cornerMatches == 1)
                {
                    while(_state.GetYellowFace()[BOTTOM_LEFT] != YELLOW)
                    {
                        rotateAndAdd(YELLOW, CLOCKWISE);
                    }
                }
                else
                {
                    while (_state.GetRedFace()[TOP_LEFT] != YELLOW)
                    {
                        rotateAndAdd(YELLOW, CLOCKWISE);
                    }
                }

                rotateAndAdd(GREEN, CLOCKWISE);
                rotateAndAdd(YELLOW, CLOCKWISE);
                rotateAndAdd(GREEN, COUNTER_CLOCKWISE);
                rotateAndAdd(YELLOW, CLOCKWISE);
                rotateAndAdd(GREEN, CLOCKWISE);
                rotateAndAdd(YELLOW, CLOCKWISE);
                rotateAndAdd(YELLOW, CLOCKWISE);
                rotateAndAdd(GREEN, COUNTER_CLOCKWISE);
            }
            
            if (!yellowFaceIsSolved())
            {
                Debug.Log("Error solving yellow face.");
                _isError = true;
            }

            return _currentStepRotations;
        }

        private bool yellowCornersAreSolved()
        {
            return (_state.GetRedFace()[TOP_LEFT] == RED && _state.GetRedFace()[TOP_RIGHT] == RED)
                   && (_state.GetGreenFace()[TOP_LEFT] == GREEN && _state.GetGreenFace()[TOP_RIGHT] == GREEN)
                   && (_state.GetOrangeFace()[TOP_LEFT] == ORANGE && _state.GetOrangeFace()[TOP_RIGHT] == ORANGE)
                   && (_state.GetBlueFace()[TOP_LEFT] == BLUE && _state.GetBlueFace()[TOP_RIGHT] == BLUE);
        }

        private IEnumerable<Rotation> solveYellowCorners()
        {
            _currentStepRotations = new Stack<Rotation>();

            var count = 0;
            while (!yellowCornersAreSolved() && count < 20)
            {
                count++;

                if (_state.GetRedFace()[TOP_LEFT] == RED 
                    && (_state.GetRedFace()[TOP_RIGHT] == RED || _state.GetOrangeFace()[TOP_LEFT] == ORANGE))
                {
                    rotateAndAdd(BLUE, COUNTER_CLOCKWISE);
                    rotateAndAdd(ORANGE, CLOCKWISE);
                    rotateAndAdd(BLUE, COUNTER_CLOCKWISE);
                    rotateAndAdd(RED, CLOCKWISE);
                    rotateAndAdd(RED, CLOCKWISE);
                    rotateAndAdd(BLUE, CLOCKWISE);
                    rotateAndAdd(ORANGE, COUNTER_CLOCKWISE);
                    rotateAndAdd(BLUE, COUNTER_CLOCKWISE);
                    rotateAndAdd(RED, CLOCKWISE);
                    rotateAndAdd(RED, CLOCKWISE);
                    rotateAndAdd(BLUE, CLOCKWISE);
                    rotateAndAdd(BLUE, CLOCKWISE);
                    continue;
                }

                if (_state.GetGreenFace()[TOP_LEFT] == GREEN 
                    && (_state.GetGreenFace()[TOP_RIGHT] == GREEN || _state.GetBlueFace()[TOP_LEFT] == BLUE))
                {
                    rotateAndAdd(RED, COUNTER_CLOCKWISE);
                    rotateAndAdd(BLUE, CLOCKWISE);
                    rotateAndAdd(RED, COUNTER_CLOCKWISE);
                    rotateAndAdd(GREEN, CLOCKWISE);
                    rotateAndAdd(GREEN, CLOCKWISE);
                    rotateAndAdd(RED, CLOCKWISE);
                    rotateAndAdd(BLUE, COUNTER_CLOCKWISE);
                    rotateAndAdd(RED, COUNTER_CLOCKWISE);
                    rotateAndAdd(GREEN, CLOCKWISE);
                    rotateAndAdd(GREEN, CLOCKWISE);
                    rotateAndAdd(RED, CLOCKWISE);
                    rotateAndAdd(RED, CLOCKWISE);
                    continue;
                }
                
                if (_state.GetOrangeFace()[TOP_LEFT] == ORANGE 
                    && (_state.GetOrangeFace()[TOP_RIGHT] == ORANGE || _state.GetRedFace()[TOP_LEFT] == RED))
                {
                    rotateAndAdd(GREEN, COUNTER_CLOCKWISE);
                    rotateAndAdd(RED, CLOCKWISE);
                    rotateAndAdd(GREEN, COUNTER_CLOCKWISE);
                    rotateAndAdd(ORANGE, CLOCKWISE);
                    rotateAndAdd(ORANGE, CLOCKWISE);
                    rotateAndAdd(GREEN, CLOCKWISE);
                    rotateAndAdd(RED, COUNTER_CLOCKWISE);
                    rotateAndAdd(GREEN, COUNTER_CLOCKWISE);
                    rotateAndAdd(ORANGE, CLOCKWISE);
                    rotateAndAdd(ORANGE, CLOCKWISE);
                    rotateAndAdd(GREEN, CLOCKWISE);
                    rotateAndAdd(GREEN, CLOCKWISE);
                    continue;
                }
                
                if (_state.GetBlueFace()[TOP_LEFT] == BLUE 
                    && (_state.GetBlueFace()[TOP_RIGHT] == BLUE || _state.GetGreenFace()[TOP_LEFT] == GREEN))
                {
                    rotateAndAdd(ORANGE, COUNTER_CLOCKWISE);
                    rotateAndAdd(GREEN, CLOCKWISE);
                    rotateAndAdd(ORANGE, COUNTER_CLOCKWISE);
                    rotateAndAdd(BLUE, CLOCKWISE);
                    rotateAndAdd(BLUE, CLOCKWISE);
                    rotateAndAdd(ORANGE, CLOCKWISE);
                    rotateAndAdd(GREEN, COUNTER_CLOCKWISE);
                    rotateAndAdd(ORANGE, COUNTER_CLOCKWISE);
                    rotateAndAdd(BLUE, CLOCKWISE);
                    rotateAndAdd(BLUE, CLOCKWISE);
                    rotateAndAdd(ORANGE, CLOCKWISE);
                    rotateAndAdd(ORANGE, CLOCKWISE);
                    continue;
                }
                
                rotateAndAdd(YELLOW, CLOCKWISE);
            }
            
            if (!yellowCornersAreSolved())
            {
                Debug.Log("Error solving yellow corners.");
                _isError = true;
            }

            return _currentStepRotations;
        }

        private bool yellowEdgesAreSolved()
        {
            return _state.GetRedFace()[TOP] == RED
                   && _state.GetGreenFace()[TOP] == GREEN
                   && _state.GetOrangeFace()[TOP] == ORANGE
                   && _state.GetBlueFace()[TOP] == BLUE;
        }

        private IEnumerable<Rotation> solveYellowEdges()
        {
            _currentStepRotations = new Stack<Rotation>();

            var count = 0;
            while (!yellowEdgesAreSolved() && count < 20)
            {
                count++;

                if (_state.GetRedFace()[TOP] == RED && (_state.GetBlueFace()[TOP] == ORANGE || _state.GetGreenFace()[TOP] == ORANGE))
                {
                    var yellowRotation = _state.GetBlueFace()[TOP] == ORANGE ? CLOCKWISE : COUNTER_CLOCKWISE;
                    
                    rotateAndAdd(ORANGE, CLOCKWISE);
                    rotateAndAdd(ORANGE, CLOCKWISE);
                    rotateAndAdd(YELLOW, yellowRotation);
                    rotateAndAdd(GREEN , CLOCKWISE);
                    rotateAndAdd(BLUE , COUNTER_CLOCKWISE);
                    rotateAndAdd(ORANGE, CLOCKWISE);
                    rotateAndAdd(ORANGE, CLOCKWISE);
                    rotateAndAdd(GREEN , COUNTER_CLOCKWISE);
                    rotateAndAdd(BLUE , CLOCKWISE);
                    rotateAndAdd(YELLOW, yellowRotation);
                    rotateAndAdd(ORANGE , CLOCKWISE);
                    rotateAndAdd(ORANGE , CLOCKWISE);
                    
                    continue;
                }
                
                if (_state.GetGreenFace()[TOP] == GREEN && (_state.GetRedFace()[TOP] == BLUE || _state.GetOrangeFace()[TOP] == BLUE))
                {
                    var yellowRotation = _state.GetRedFace()[TOP] == BLUE ? CLOCKWISE : COUNTER_CLOCKWISE;
                    
                    rotateAndAdd(BLUE, CLOCKWISE);
                    rotateAndAdd(BLUE, CLOCKWISE);
                    rotateAndAdd(YELLOW, yellowRotation);
                    rotateAndAdd(ORANGE, CLOCKWISE);
                    rotateAndAdd(RED, COUNTER_CLOCKWISE);
                    rotateAndAdd(BLUE, CLOCKWISE);
                    rotateAndAdd(BLUE, CLOCKWISE);
                    rotateAndAdd(ORANGE, COUNTER_CLOCKWISE);
                    rotateAndAdd(RED, CLOCKWISE);
                    rotateAndAdd(YELLOW, yellowRotation);
                    rotateAndAdd(BLUE , CLOCKWISE);
                    rotateAndAdd(BLUE , CLOCKWISE);
                    
                    continue;
                }
                
                if (_state.GetOrangeFace()[TOP] == ORANGE && (_state.GetGreenFace()[TOP] == RED || _state.GetBlueFace()[TOP] == RED))
                {
                    var yellowRotation = _state.GetGreenFace()[TOP] == RED ? CLOCKWISE : COUNTER_CLOCKWISE;
                    
                    rotateAndAdd(RED, CLOCKWISE);
                    rotateAndAdd(RED, CLOCKWISE);
                    rotateAndAdd(YELLOW, yellowRotation);
                    rotateAndAdd(BLUE, CLOCKWISE);
                    rotateAndAdd(GREEN, COUNTER_CLOCKWISE);
                    rotateAndAdd(RED, CLOCKWISE);
                    rotateAndAdd(RED, CLOCKWISE);
                    rotateAndAdd(BLUE, COUNTER_CLOCKWISE);
                    rotateAndAdd(GREEN, CLOCKWISE);
                    rotateAndAdd(YELLOW, yellowRotation);
                    rotateAndAdd(RED, CLOCKWISE);
                    rotateAndAdd(RED, CLOCKWISE);
                    
                    continue;
                }
                
                if (_state.GetBlueFace()[TOP] == BLUE && (_state.GetOrangeFace()[TOP] == GREEN || _state.GetRedFace()[TOP] == GREEN))
                {
                    var yellowRotation = _state.GetOrangeFace()[TOP] == GREEN ? CLOCKWISE : COUNTER_CLOCKWISE;
                    
                    rotateAndAdd(GREEN, CLOCKWISE);
                    rotateAndAdd(GREEN, CLOCKWISE);
                    rotateAndAdd(YELLOW, yellowRotation);
                    rotateAndAdd(RED, CLOCKWISE);
                    rotateAndAdd(ORANGE, COUNTER_CLOCKWISE);
                    rotateAndAdd(GREEN, CLOCKWISE);
                    rotateAndAdd(GREEN, CLOCKWISE);
                    rotateAndAdd(RED, COUNTER_CLOCKWISE);
                    rotateAndAdd(ORANGE, CLOCKWISE);
                    rotateAndAdd(YELLOW, yellowRotation);
                    rotateAndAdd(GREEN, CLOCKWISE);
                    rotateAndAdd(GREEN, CLOCKWISE);

                    continue;
                }
                
                rotateAndAdd(ORANGE, CLOCKWISE);
                rotateAndAdd(ORANGE, CLOCKWISE);
                rotateAndAdd(YELLOW, CLOCKWISE);
                rotateAndAdd(GREEN , CLOCKWISE);
                rotateAndAdd(BLUE , COUNTER_CLOCKWISE);
                rotateAndAdd(ORANGE, CLOCKWISE);
                rotateAndAdd(ORANGE, CLOCKWISE);
                rotateAndAdd(GREEN , COUNTER_CLOCKWISE);
                rotateAndAdd(BLUE , CLOCKWISE);
                rotateAndAdd(YELLOW, CLOCKWISE);
                rotateAndAdd(ORANGE , CLOCKWISE);
                rotateAndAdd(ORANGE , CLOCKWISE);
            }
            
            if (!yellowEdgesAreSolved())
            {
                Debug.Log("Error solving yellow edges.");
                _isError = true;
            }

            return _currentStepRotations;
        }
    }
}