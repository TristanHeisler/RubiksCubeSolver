using Rubiks.Constants;
using Rubiks.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Rubiks
{
    [Serializable]
    public class CubeState
    {
        public const byte NUMBER_OF_FACES = 6;
        public const byte CUBITS_PER_FACE = 8;          

        private readonly FaceColor[][] _squares = new FaceColor[NUMBER_OF_FACES][];
        public CubeState parentState;
        public Rotation rotation;

        public CubeState()
        {
            for(byte currentFace = 0; currentFace < NUMBER_OF_FACES; currentFace++)
            {
                _squares[currentFace] = new FaceColor[CUBITS_PER_FACE];

                for(byte currentSquare = 0; currentSquare < CUBITS_PER_FACE; currentSquare++)
                {
                    _squares[currentFace][currentSquare] = (FaceColor)currentFace;
                }
            }           
        }
        
        public static IEnumerable<Rotation> GetPossibleRotations()
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

        public bool IsSolved()
        {
            FaceColor[][] cubeFaces = GetAllFaces();

            //If any square does not match the color of that face's center square, then the cube is not solved
            for (int currentFace = 0; currentFace < NUMBER_OF_FACES; currentFace++)
            {
                for (int currentSquare = 0; currentSquare < CUBITS_PER_FACE; currentSquare++)
                {
                    if (cubeFaces[currentFace][currentSquare] != (FaceColor)currentFace)
                    {
                        return false;                        
                    }
                }
            }

            return true;
        }

        public void Rotate(FaceColor rotatingFace, RotationDirection direction)
        {
            //Handle the remapping of the rotating face
            rotateFace(rotatingFace, direction);

            //Handle the remapping of the adjacent faces
            switch(rotatingFace)
            {
                case FaceColor.Blue:
                    rotateBlueAdjacentFaces(direction);
                    break;
                case FaceColor.Green:
                    rotateGreenAdjacentFaces(direction);
                    break;
                case FaceColor.Orange:
                    rotateOrangeAdjacentFaces(direction);
                    break;
                case FaceColor.Red:
                    rotateRedAdjacentFaces(direction);
                    break;
                case FaceColor.White:
                    rotateWhiteAdjacentFaces(direction);
                    break;
                case FaceColor.Yellow:
                    rotateYellowAdjacentFaces(direction);
                    break;
            }
        }

        private void rotateFace(FaceColor rotatingFace, RotationDirection direction)
        {
            var face = _squares[(byte)rotatingFace];

            var tempCorner = face[Locations.TOP_LEFT];
            var tempEdge = face[Locations.TOP];

            if(direction == RotationDirection.Clockwise)
            {
                //Remap the corners
                face[Locations.TOP_LEFT] = face[Locations.BOTTOM_LEFT];
                face[Locations.BOTTOM_LEFT] = face[Locations.BOTTOM_RIGHT];
                face[Locations.BOTTOM_RIGHT] = face[Locations.TOP_RIGHT];
                face[Locations.TOP_RIGHT] = tempCorner;

                //Remap the edges
                face[Locations.TOP] = face[Locations.LEFT];
                face[Locations.LEFT] = face[Locations.BOTTOM];
                face[Locations.BOTTOM] = face[Locations.RIGHT];
                face[Locations.RIGHT] = tempEdge;
            }
            else
            {
                //Remap the corners
                face[Locations.TOP_LEFT] = face[Locations.TOP_RIGHT];
                face[Locations.TOP_RIGHT] = face[Locations.BOTTOM_RIGHT];
                face[Locations.BOTTOM_RIGHT] = face[Locations.BOTTOM_LEFT];
                face[Locations.BOTTOM_LEFT] = tempCorner;

                //Remap the edges
                face[Locations.TOP] = face[Locations.RIGHT];
                face[Locations.RIGHT] = face[Locations.BOTTOM];
                face[Locations.BOTTOM] = face[Locations.LEFT];
                face[Locations.LEFT] = tempEdge;
            }
        }

        private void rotateBlueAdjacentFaces(RotationDirection direction)
        {
            var tempFirstCorner = _squares[Colors.YELLOW][Locations.TOP_LEFT];
            var tempEdge = _squares[Colors.YELLOW][Locations.LEFT];
            var tempSecondCorner = _squares[Colors.YELLOW][Locations.BOTTOM_LEFT];

            if (direction == RotationDirection.Clockwise)
            {
                //Remap the first corner piece
                _squares[Colors.YELLOW][Locations.TOP_LEFT] = _squares[Colors.ORANGE][Locations.BOTTOM_RIGHT];
                _squares[Colors.ORANGE][Locations.BOTTOM_RIGHT] = _squares[Colors.WHITE][Locations.TOP_LEFT];
                _squares[Colors.WHITE][Locations.TOP_LEFT] = _squares[Colors.RED][Locations.TOP_LEFT];
                _squares[Colors.RED][Locations.TOP_LEFT] = tempFirstCorner;

                //Remap the edge piece
                _squares[Colors.YELLOW][Locations.LEFT] = _squares[Colors.ORANGE][Locations.RIGHT];
                _squares[Colors.ORANGE][Locations.RIGHT] = _squares[Colors.WHITE][Locations.LEFT];
                _squares[Colors.WHITE][Locations.LEFT] = _squares[Colors.RED][Locations.LEFT];
                _squares[Colors.RED][Locations.LEFT] = tempEdge;

                //Remap the second corner piece
                _squares[Colors.YELLOW][Locations.BOTTOM_LEFT] = _squares[Colors.ORANGE][Locations.TOP_RIGHT];
                _squares[Colors.ORANGE][Locations.TOP_RIGHT] = _squares[Colors.WHITE][Locations.BOTTOM_LEFT];
                _squares[Colors.WHITE][Locations.BOTTOM_LEFT] = _squares[Colors.RED][Locations.BOTTOM_LEFT];
                _squares[Colors.RED][Locations.BOTTOM_LEFT] = tempSecondCorner;
            }
            else
            {
                //Remap the first corner piece
                _squares[Colors.YELLOW][Locations.TOP_LEFT] = _squares[Colors.RED][Locations.TOP_LEFT];
                _squares[Colors.RED][Locations.TOP_LEFT] = _squares[Colors.WHITE][Locations.TOP_LEFT];
                _squares[Colors.WHITE][Locations.TOP_LEFT] = _squares[Colors.ORANGE][Locations.BOTTOM_RIGHT];
                _squares[Colors.ORANGE][Locations.BOTTOM_RIGHT] = tempFirstCorner;

                //Remap the edge piece
                _squares[Colors.YELLOW][Locations.LEFT] = _squares[Colors.RED][Locations.LEFT];
                _squares[Colors.RED][Locations.LEFT] = _squares[Colors.WHITE][Locations.LEFT];
                _squares[Colors.WHITE][Locations.LEFT] = _squares[Colors.ORANGE][Locations.RIGHT];
                _squares[Colors.ORANGE][Locations.RIGHT] = tempEdge;

                //Remap the second corner piece
                _squares[Colors.YELLOW][Locations.BOTTOM_LEFT] = _squares[Colors.RED][Locations.BOTTOM_LEFT];
                _squares[Colors.RED][Locations.BOTTOM_LEFT] = _squares[Colors.WHITE][Locations.BOTTOM_LEFT];
                _squares[Colors.WHITE][Locations.BOTTOM_LEFT] = _squares[Colors.ORANGE][Locations.TOP_RIGHT];
                _squares[Colors.ORANGE][Locations.TOP_RIGHT] = tempSecondCorner;
            }
        }

        private void rotateGreenAdjacentFaces(RotationDirection direction)
        {
            var tempFirstCorner = _squares[Colors.YELLOW][Locations.BOTTOM_RIGHT];
            var tempEdge = _squares[Colors.YELLOW][Locations.RIGHT];
            var tempSecondCorner = _squares[Colors.YELLOW][Locations.TOP_RIGHT];

            if (direction == RotationDirection.Clockwise)
            {
                //Remap the first corner piece
                _squares[Colors.YELLOW][Locations.BOTTOM_RIGHT] = _squares[Colors.RED][Locations.BOTTOM_RIGHT];
                _squares[Colors.RED][Locations.BOTTOM_RIGHT] = _squares[Colors.WHITE][Locations.BOTTOM_RIGHT];
                _squares[Colors.WHITE][Locations.BOTTOM_RIGHT] = _squares[Colors.ORANGE][Locations.TOP_LEFT];
                _squares[Colors.ORANGE][Locations.TOP_LEFT] = tempFirstCorner;

                //Remap the edge piece
                _squares[Colors.YELLOW][Locations.RIGHT] = _squares[Colors.RED][Locations.RIGHT];
                _squares[Colors.RED][Locations.RIGHT] = _squares[Colors.WHITE][Locations.RIGHT];
                _squares[Colors.WHITE][Locations.RIGHT] = _squares[Colors.ORANGE][Locations.LEFT];
                _squares[Colors.ORANGE][Locations.LEFT] = tempEdge;

                //Remap the second corner piece
                _squares[Colors.YELLOW][Locations.TOP_RIGHT] = _squares[Colors.RED][Locations.TOP_RIGHT];
                _squares[Colors.RED][Locations.TOP_RIGHT] = _squares[Colors.WHITE][Locations.TOP_RIGHT];
                _squares[Colors.WHITE][Locations.TOP_RIGHT] = _squares[Colors.ORANGE][Locations.BOTTOM_LEFT];
                _squares[Colors.ORANGE][Locations.BOTTOM_LEFT] = tempSecondCorner;
            }
            else
            {
                //Remap the first corner piece
                _squares[Colors.YELLOW][Locations.BOTTOM_RIGHT] = _squares[Colors.ORANGE][Locations.TOP_LEFT];
                _squares[Colors.ORANGE][Locations.TOP_LEFT] = _squares[Colors.WHITE][Locations.BOTTOM_RIGHT];
                _squares[Colors.WHITE][Locations.BOTTOM_RIGHT] = _squares[Colors.RED][Locations.BOTTOM_RIGHT];
                _squares[Colors.RED][Locations.BOTTOM_RIGHT] = tempFirstCorner;

                //Remap the edge piece
                _squares[Colors.YELLOW][Locations.RIGHT] = _squares[Colors.ORANGE][Locations.LEFT];
                _squares[Colors.ORANGE][Locations.LEFT] = _squares[Colors.WHITE][Locations.RIGHT];
                _squares[Colors.WHITE][Locations.RIGHT] = _squares[Colors.RED][Locations.RIGHT];
                _squares[Colors.RED][Locations.RIGHT] = tempEdge;

                //Remap the second corner piece
                _squares[Colors.YELLOW][Locations.TOP_RIGHT] = _squares[Colors.ORANGE][Locations.BOTTOM_LEFT];
                _squares[Colors.ORANGE][Locations.BOTTOM_LEFT] = _squares[Colors.WHITE][Locations.TOP_RIGHT];
                _squares[Colors.WHITE][Locations.TOP_RIGHT] = _squares[Colors.RED][Locations.TOP_RIGHT];
                _squares[Colors.RED][Locations.TOP_RIGHT] = tempSecondCorner;
            }
        }

        private void rotateOrangeAdjacentFaces(RotationDirection direction)
        {
            var tempFirstCorner = _squares[Colors.YELLOW][Locations.TOP_RIGHT];
            var tempEdge = _squares[Colors.YELLOW][Locations.TOP];
            var tempSecondCorner = _squares[Colors.YELLOW][Locations.TOP_LEFT];

            if (direction == RotationDirection.Clockwise)
            {
                //Remap the first corner piece
                _squares[Colors.YELLOW][Locations.TOP_RIGHT] = _squares[Colors.GREEN][Locations.BOTTOM_RIGHT];
                _squares[Colors.GREEN][Locations.BOTTOM_RIGHT] = _squares[Colors.WHITE][Locations.BOTTOM_LEFT];
                _squares[Colors.WHITE][Locations.BOTTOM_LEFT] = _squares[Colors.BLUE][Locations.TOP_LEFT];
                _squares[Colors.BLUE][Locations.TOP_LEFT] = tempFirstCorner;

                //Remap the edge piece
                _squares[Colors.YELLOW][Locations.TOP] = _squares[Colors.GREEN][Locations.RIGHT];
                _squares[Colors.GREEN][Locations.RIGHT] = _squares[Colors.WHITE][Locations.BOTTOM];
                _squares[Colors.WHITE][Locations.BOTTOM] = _squares[Colors.BLUE][Locations.LEFT];
                _squares[Colors.BLUE][Locations.LEFT] = tempEdge;

                //Remap the second corner piece
                _squares[Colors.YELLOW][Locations.TOP_LEFT] = _squares[Colors.GREEN][Locations.TOP_RIGHT];
                _squares[Colors.GREEN][Locations.TOP_RIGHT] = _squares[Colors.WHITE][Locations.BOTTOM_RIGHT];
                _squares[Colors.WHITE][Locations.BOTTOM_RIGHT] = _squares[Colors.BLUE][Locations.BOTTOM_LEFT];
                _squares[Colors.BLUE][Locations.BOTTOM_LEFT] = tempSecondCorner;
            }
            else
            {
                //Remap the first corner piece
                _squares[Colors.YELLOW][Locations.TOP_RIGHT] = _squares[Colors.BLUE][Locations.TOP_LEFT];
                _squares[Colors.BLUE][Locations.TOP_LEFT] = _squares[Colors.WHITE][Locations.BOTTOM_LEFT];
                _squares[Colors.WHITE][Locations.BOTTOM_LEFT] = _squares[Colors.GREEN][Locations.BOTTOM_RIGHT];
                _squares[Colors.GREEN][Locations.BOTTOM_RIGHT] = tempFirstCorner;

                //Remap the edge piece
                _squares[Colors.YELLOW][Locations.TOP] = _squares[Colors.BLUE][Locations.LEFT];
                _squares[Colors.BLUE][Locations.LEFT] = _squares[Colors.WHITE][Locations.BOTTOM];
                _squares[Colors.WHITE][Locations.BOTTOM] = _squares[Colors.GREEN][Locations.RIGHT];
                _squares[Colors.GREEN][Locations.RIGHT] = tempEdge;

                //Remap the second corner piece
                _squares[Colors.YELLOW][Locations.TOP_LEFT] = _squares[Colors.BLUE][Locations.BOTTOM_LEFT];
                _squares[Colors.BLUE][Locations.BOTTOM_LEFT] = _squares[Colors.WHITE][Locations.BOTTOM_RIGHT];
                _squares[Colors.WHITE][Locations.BOTTOM_RIGHT] = _squares[Colors.GREEN][Locations.TOP_RIGHT];
                _squares[Colors.GREEN][Locations.TOP_RIGHT] = tempSecondCorner;
            }
        }

        private void rotateRedAdjacentFaces(RotationDirection direction)
        {
            var tempFirstCorner = _squares[Colors.YELLOW][Locations.BOTTOM_LEFT];
            var tempEdge = _squares[Colors.YELLOW][Locations.BOTTOM];
            var tempSecondCorner = _squares[Colors.YELLOW][Locations.BOTTOM_RIGHT];

            if (direction == RotationDirection.Clockwise)
            {
                //Remap the first corner piece
                _squares[Colors.YELLOW][Locations.BOTTOM_LEFT] = _squares[Colors.BLUE][Locations.BOTTOM_RIGHT];
                _squares[Colors.BLUE][Locations.BOTTOM_RIGHT] = _squares[Colors.WHITE][Locations.TOP_RIGHT];
                _squares[Colors.WHITE][Locations.TOP_RIGHT] = _squares[Colors.GREEN][Locations.TOP_LEFT];
                _squares[Colors.GREEN][Locations.TOP_LEFT] = tempFirstCorner;

                //Remap the edge piece
                _squares[Colors.YELLOW][Locations.BOTTOM] = _squares[Colors.BLUE][Locations.RIGHT];
                _squares[Colors.BLUE][Locations.RIGHT] = _squares[Colors.WHITE][Locations.TOP];
                _squares[Colors.WHITE][Locations.TOP] = _squares[Colors.GREEN][Locations.LEFT];
                _squares[Colors.GREEN][Locations.LEFT] = tempEdge;

                //Remap the second corner piece
                _squares[Colors.YELLOW][Locations.BOTTOM_RIGHT] = _squares[Colors.BLUE][Locations.TOP_RIGHT];
                _squares[Colors.BLUE][Locations.TOP_RIGHT] = _squares[Colors.WHITE][Locations.TOP_LEFT];
                _squares[Colors.WHITE][Locations.TOP_LEFT] = _squares[Colors.GREEN][Locations.BOTTOM_LEFT];
                _squares[Colors.GREEN][Locations.BOTTOM_LEFT] = tempSecondCorner;
            }
            else
            {
                //Remap the first corner piece
                _squares[Colors.YELLOW][Locations.BOTTOM_LEFT] = _squares[Colors.GREEN][Locations.TOP_LEFT];
                _squares[Colors.GREEN][Locations.TOP_LEFT] = _squares[Colors.WHITE][Locations.TOP_RIGHT];
                _squares[Colors.WHITE][Locations.TOP_RIGHT] = _squares[Colors.BLUE][Locations.BOTTOM_RIGHT];
                _squares[Colors.BLUE][Locations.BOTTOM_RIGHT] = tempFirstCorner;

                //Remap the edge piece
                _squares[Colors.YELLOW][Locations.BOTTOM] = _squares[Colors.GREEN][Locations.LEFT];
                _squares[Colors.GREEN][Locations.LEFT] = _squares[Colors.WHITE][Locations.TOP];
                _squares[Colors.WHITE][Locations.TOP] = _squares[Colors.BLUE][Locations.RIGHT];
                _squares[Colors.BLUE][Locations.RIGHT] = tempEdge;

                //Remap the second corner piece
                _squares[Colors.YELLOW][Locations.BOTTOM_RIGHT] = _squares[Colors.GREEN][Locations.BOTTOM_LEFT];
                _squares[Colors.GREEN][Locations.BOTTOM_LEFT] = _squares[Colors.WHITE][Locations.TOP_LEFT];
                _squares[Colors.WHITE][Locations.TOP_LEFT] = _squares[Colors.BLUE][Locations.TOP_RIGHT];
                _squares[Colors.BLUE][Locations.TOP_RIGHT] = tempSecondCorner;
            }
        }

        private void rotateWhiteAdjacentFaces(RotationDirection direction)
        {
            var tempFirstCorner = _squares[Colors.RED][Locations.BOTTOM_LEFT];
            var tempEdge = _squares[Colors.RED][Locations.BOTTOM];
            var tempSecondCorner = _squares[Colors.RED][Locations.BOTTOM_RIGHT];

            if (direction == RotationDirection.Clockwise)
            {
                //Remap the first corner piece
                _squares[Colors.RED][Locations.BOTTOM_LEFT] = _squares[Colors.BLUE][Locations.BOTTOM_LEFT];
                _squares[Colors.BLUE][Locations.BOTTOM_LEFT] = _squares[Colors.ORANGE][Locations.BOTTOM_LEFT];
                _squares[Colors.ORANGE][Locations.BOTTOM_LEFT] = _squares[Colors.GREEN][Locations.BOTTOM_LEFT];
                _squares[Colors.GREEN][Locations.BOTTOM_LEFT] = tempFirstCorner;

                //Remap the edge piece
                _squares[Colors.RED][Locations.BOTTOM] = _squares[Colors.BLUE][Locations.BOTTOM];
                _squares[Colors.BLUE][Locations.BOTTOM] = _squares[Colors.ORANGE][Locations.BOTTOM];
                _squares[Colors.ORANGE][Locations.BOTTOM] = _squares[Colors.GREEN][Locations.BOTTOM];
                _squares[Colors.GREEN][Locations.BOTTOM] = tempEdge;

                //Remap the second corner piece
                _squares[Colors.RED][Locations.BOTTOM_RIGHT] = _squares[Colors.BLUE][Locations.BOTTOM_RIGHT];
                _squares[Colors.BLUE][Locations.BOTTOM_RIGHT] = _squares[Colors.ORANGE][Locations.BOTTOM_RIGHT];
                _squares[Colors.ORANGE][Locations.BOTTOM_RIGHT] = _squares[Colors.GREEN][Locations.BOTTOM_RIGHT];
                _squares[Colors.GREEN][Locations.BOTTOM_RIGHT] = tempSecondCorner;
            }
            else
            {
                //Remap the first corner piece
                _squares[Colors.RED][Locations.BOTTOM_LEFT] = _squares[Colors.GREEN][Locations.BOTTOM_LEFT];
                _squares[Colors.GREEN][Locations.BOTTOM_LEFT] = _squares[Colors.ORANGE][Locations.BOTTOM_LEFT];
                _squares[Colors.ORANGE][Locations.BOTTOM_LEFT] = _squares[Colors.BLUE][Locations.BOTTOM_LEFT];
                _squares[Colors.BLUE][Locations.BOTTOM_LEFT] = tempFirstCorner;

                //Remap the edge piece
                _squares[Colors.RED][Locations.BOTTOM] = _squares[Colors.GREEN][Locations.BOTTOM];
                _squares[Colors.GREEN][Locations.BOTTOM] = _squares[Colors.ORANGE][Locations.BOTTOM];
                _squares[Colors.ORANGE][Locations.BOTTOM] = _squares[Colors.BLUE][Locations.BOTTOM];
                _squares[Colors.BLUE][Locations.BOTTOM] = tempEdge;

                //Remap the second corner piece
                _squares[Colors.RED][Locations.BOTTOM_RIGHT] = _squares[Colors.GREEN][Locations.BOTTOM_RIGHT];
                _squares[Colors.GREEN][Locations.BOTTOM_RIGHT] = _squares[Colors.ORANGE][Locations.BOTTOM_RIGHT];
                _squares[Colors.ORANGE][Locations.BOTTOM_RIGHT] = _squares[Colors.BLUE][Locations.BOTTOM_RIGHT];
                _squares[Colors.BLUE][Locations.BOTTOM_RIGHT] = tempSecondCorner;
            }
        }

        private void rotateYellowAdjacentFaces(RotationDirection direction)
        {
            var tempFirstCorner = _squares[Colors.ORANGE][Locations.TOP_RIGHT];
            var tempEdge = _squares[Colors.ORANGE][Locations.TOP];
            var tempSecondCorner = _squares[Colors.ORANGE][Locations.TOP_LEFT];

            if (direction == RotationDirection.Clockwise)
            {
                //Remap the first corner piece
                _squares[Colors.ORANGE][Locations.TOP_RIGHT] = _squares[Colors.BLUE][Locations.TOP_RIGHT];
                _squares[Colors.BLUE][Locations.TOP_RIGHT] = _squares[Colors.RED][Locations.TOP_RIGHT];
                _squares[Colors.RED][Locations.TOP_RIGHT] = _squares[Colors.GREEN][Locations.TOP_RIGHT];
                _squares[Colors.GREEN][Locations.TOP_RIGHT] = tempFirstCorner;

                //Remap the edge piece
                _squares[Colors.ORANGE][Locations.TOP] = _squares[Colors.BLUE][Locations.TOP];
                _squares[Colors.BLUE][Locations.TOP] = _squares[Colors.RED][Locations.TOP];
                _squares[Colors.RED][Locations.TOP] = _squares[Colors.GREEN][Locations.TOP];
                _squares[Colors.GREEN][Locations.TOP] = tempEdge;

                //Remap the second corner piece
                _squares[Colors.ORANGE][Locations.TOP_LEFT] = _squares[Colors.BLUE][Locations.TOP_LEFT];
                _squares[Colors.BLUE][Locations.TOP_LEFT] = _squares[Colors.RED][Locations.TOP_LEFT];
                _squares[Colors.RED][Locations.TOP_LEFT] = _squares[Colors.GREEN][Locations.TOP_LEFT];
                _squares[Colors.GREEN][Locations.TOP_LEFT] = tempSecondCorner;
            }
            else
            {
                //Remap the first corner piece
                _squares[Colors.ORANGE][Locations.TOP_RIGHT] = _squares[Colors.GREEN][Locations.TOP_RIGHT];
                _squares[Colors.GREEN][Locations.TOP_RIGHT] = _squares[Colors.RED][Locations.TOP_RIGHT];
                _squares[Colors.RED][Locations.TOP_RIGHT] = _squares[Colors.BLUE][Locations.TOP_RIGHT];
                _squares[Colors.BLUE][Locations.TOP_RIGHT] = tempFirstCorner;

                //Remap the edge piece
                _squares[Colors.ORANGE][Locations.TOP] = _squares[Colors.GREEN][Locations.TOP];
                _squares[Colors.GREEN][Locations.TOP] = _squares[Colors.RED][Locations.TOP];
                _squares[Colors.RED][Locations.TOP] = _squares[Colors.BLUE][Locations.TOP];
                _squares[Colors.BLUE][Locations.TOP] = tempEdge;

                //Remap the second corner piece
                _squares[Colors.ORANGE][Locations.TOP_LEFT] = _squares[Colors.GREEN][Locations.TOP_LEFT];
                _squares[Colors.GREEN][Locations.TOP_LEFT] = _squares[Colors.RED][Locations.TOP_LEFT];
                _squares[Colors.RED][Locations.TOP_LEFT] = _squares[Colors.BLUE][Locations.TOP_LEFT];
                _squares[Colors.BLUE][Locations.TOP_LEFT] = tempSecondCorner;
            }
        }

        public void PrintCubeState()
        {
            for (byte currentFace = 0; currentFace < NUMBER_OF_FACES; currentFace++)
            {
                var faceSquares = _squares[currentFace];

                Debug.Log((FaceColor)currentFace + " Face: " + faceSquares[0] + " " + faceSquares[1] + " " + faceSquares[2] + " "
                    + faceSquares[3] + " " + faceSquares[4] + " " + faceSquares[5] + " " + faceSquares[6] + " " + faceSquares[7]);
            }
        }

        public FaceColor[][] GetAllFaces()
        {
            return _squares;
        }

        public FaceColor[] GetBlueFace()
        {
            return _squares[Colors.BLUE];
        }

        public FaceColor[] GetGreenFace()
        {
            return _squares[Colors.GREEN];
        }

        public FaceColor[] GetOrangeFace()
        {
            return _squares[Colors.ORANGE];
        }

        public FaceColor[] GetRedFace()
        {
            return _squares[Colors.RED];
        }

        public FaceColor[] GetWhiteFace()
        {
            return _squares[Colors.WHITE];
        }

        public FaceColor[] GetYellowFace()
        {
            return _squares[Colors.YELLOW];
        }
    }

    public static class CubeStateHelper
    {
        public static CubeState Clone(this CubeState state)
        {
            // Don't serialize a null object, simply return the default for that object
            if (ReferenceEquals(state, null))
            {
                return default(CubeState);
            }

            IFormatter formatter = new BinaryFormatter();
            using (Stream stream = new MemoryStream())
            {
                formatter.Serialize(stream, state);
                stream.Seek(0, SeekOrigin.Begin);
                return (CubeState)formatter.Deserialize(stream);
            }
        }
        
        public static bool EqualsState(this CubeState stateOne, CubeState stateTwo)
        {
            var stateOneFaces = stateOne.GetAllFaces();
            var stateTwoFaces = stateTwo.GetAllFaces();

            //If any squares do not match, the cubes have differing states
            for (var currentFace = 0; currentFace < CubeState.NUMBER_OF_FACES; currentFace++)
            {
                for (var currentSquare = 0; currentSquare < CubeState.CUBITS_PER_FACE; currentSquare++)
                {
                    if (stateOneFaces[currentFace][currentSquare] != stateTwoFaces[currentFace][currentSquare])
                    {
                        return false;
                    }
                }
            }
            
            //Otherwise, return true
            return true;
        }
    }
}
