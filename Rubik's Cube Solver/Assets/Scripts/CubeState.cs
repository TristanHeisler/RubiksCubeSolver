using Rubiks.Enums;
using UnityEngine;

namespace Rubiks
{
    public class CubeState
    {
        public const byte NUMBER_OF_FACES = 6;
        public const byte CUBITS_PER_FACE = 8;

        private const byte BLUE = (byte)FaceColor.Blue;
        private const byte GREEN = (byte)FaceColor.Green;
        private const byte ORANGE = (byte)FaceColor.Orange;
        private const byte RED = (byte)FaceColor.Red;
        private const byte WHITE = (byte)FaceColor.White;
        private const byte YELLOW = (byte)FaceColor.Yellow;

        private const byte TOP_LEFT = 0;
        private const byte TOP = 1;
        private const byte TOP_RIGHT = 2;
        private const byte RIGHT = 3;
        private const byte BOTTOM_RIGHT = 4;
        private const byte BOTTOM = 5;
        private const byte BOTTOM_LEFT = 6;
        private const byte LEFT = 7;

        private FaceColor[][] squares = new FaceColor[NUMBER_OF_FACES][];

        public CubeState()
        {
            for(byte currentFace = 0; currentFace < NUMBER_OF_FACES; currentFace++)
            {
                squares[currentFace] = new FaceColor[CUBITS_PER_FACE];

                for(byte currentSquare = 0; currentSquare < CUBITS_PER_FACE; currentSquare++)
                {
                    squares[currentFace][currentSquare] = (FaceColor)currentFace;
                }
            }           
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
            FaceColor[] face = squares[(byte)rotatingFace];

            FaceColor tempCorner = face[0];
            FaceColor tempEdge = face[1];

            if(direction == RotationDirection.Clockwise)
            {
                //Remap the corners
                face[0] = face[6];
                face[6] = face[4];
                face[4] = face[2];
                face[2] = tempCorner;

                //Remap the edges
                face[1] = face[7];
                face[7] = face[5];
                face[5] = face[3];
                face[3] = tempEdge;
            }
            else
            {
                //Remap the corners
                face[0] = face[2];
                face[2] = face[4];
                face[4] = face[6];
                face[6] = tempCorner;

                //Remap the edges
                face[1] = face[3];
                face[3] = face[5];
                face[5] = face[7];
                face[7] = tempEdge;
            }
        }

        private void rotateBlueAdjacentFaces(RotationDirection direction)
        {
            FaceColor tempFirstCorner = squares[YELLOW][TOP_LEFT];
            FaceColor tempEdge = squares[YELLOW][LEFT];
            FaceColor tempSecondCorner = squares[YELLOW][BOTTOM_LEFT];

            if (direction == RotationDirection.Clockwise)
            {
                //Remap the first corner piece
                squares[YELLOW][TOP_LEFT] = squares[ORANGE][BOTTOM_RIGHT];
                squares[ORANGE][BOTTOM_RIGHT] = squares[WHITE][TOP_LEFT];
                squares[WHITE][TOP_LEFT] = squares[RED][TOP_LEFT];
                squares[RED][TOP_LEFT] = tempFirstCorner;

                //Remap the edge piece
                squares[YELLOW][LEFT] = squares[ORANGE][RIGHT];
                squares[ORANGE][RIGHT] = squares[WHITE][LEFT];
                squares[WHITE][LEFT] = squares[RED][LEFT];
                squares[RED][LEFT] = tempEdge;

                //Remap the second corner piece
                squares[YELLOW][BOTTOM_LEFT] = squares[ORANGE][TOP_RIGHT];
                squares[ORANGE][TOP_RIGHT] = squares[WHITE][BOTTOM_LEFT];
                squares[WHITE][BOTTOM_LEFT] = squares[RED][BOTTOM_LEFT];
                squares[RED][BOTTOM_LEFT] = tempSecondCorner;
            }
            else
            {
                //Remap the first corner piece
                squares[YELLOW][TOP_LEFT] = squares[RED][TOP_LEFT];
                squares[RED][TOP_LEFT] = squares[WHITE][TOP_LEFT];
                squares[WHITE][TOP_LEFT] = squares[ORANGE][BOTTOM_RIGHT];
                squares[ORANGE][BOTTOM_RIGHT] = tempFirstCorner;

                //Remap the edge piece
                squares[YELLOW][LEFT] = squares[RED][LEFT];
                squares[RED][LEFT] = squares[WHITE][LEFT];
                squares[WHITE][LEFT] = squares[ORANGE][RIGHT];
                squares[ORANGE][RIGHT] = tempEdge;

                //Remap the second corner piece
                squares[YELLOW][BOTTOM_LEFT] = squares[RED][BOTTOM_LEFT];
                squares[RED][BOTTOM_LEFT] = squares[WHITE][BOTTOM_LEFT];
                squares[WHITE][BOTTOM_LEFT] = squares[ORANGE][TOP_RIGHT];
                squares[ORANGE][TOP_RIGHT] = tempSecondCorner;
            }
        }

        private void rotateGreenAdjacentFaces(RotationDirection direction)
        {
            FaceColor tempFirstCorner = squares[YELLOW][BOTTOM_RIGHT];
            FaceColor tempEdge = squares[YELLOW][RIGHT];
            FaceColor tempSecondCorner = squares[YELLOW][TOP_RIGHT];

            if (direction == RotationDirection.Clockwise)
            {
                //Remap the first corner piece
                squares[YELLOW][BOTTOM_RIGHT] = squares[RED][BOTTOM_RIGHT];
                squares[RED][BOTTOM_RIGHT] = squares[WHITE][BOTTOM_RIGHT];
                squares[WHITE][BOTTOM_RIGHT] = squares[ORANGE][TOP_LEFT];
                squares[ORANGE][TOP_LEFT] = tempFirstCorner;

                //Remap the edge piece
                squares[YELLOW][RIGHT] = squares[RED][RIGHT];
                squares[RED][RIGHT] = squares[WHITE][RIGHT];
                squares[WHITE][RIGHT] = squares[ORANGE][LEFT];
                squares[ORANGE][LEFT] = tempEdge;

                //Remap the second corner piece
                squares[YELLOW][TOP_RIGHT] = squares[RED][TOP_RIGHT];
                squares[RED][TOP_RIGHT] = squares[WHITE][TOP_RIGHT];
                squares[WHITE][TOP_RIGHT] = squares[ORANGE][BOTTOM_LEFT];
                squares[ORANGE][BOTTOM_LEFT] = tempSecondCorner;
            }
            else
            {
                //Remap the first corner piece
                squares[YELLOW][BOTTOM_RIGHT] = squares[ORANGE][TOP_LEFT];
                squares[ORANGE][TOP_LEFT] = squares[WHITE][BOTTOM_RIGHT];
                squares[WHITE][BOTTOM_RIGHT] = squares[RED][BOTTOM_RIGHT];
                squares[RED][BOTTOM_RIGHT] = tempFirstCorner;

                //Remap the edge piece
                squares[YELLOW][RIGHT] = squares[ORANGE][LEFT];
                squares[ORANGE][LEFT] = squares[WHITE][RIGHT];
                squares[WHITE][RIGHT] = squares[RED][RIGHT];
                squares[RED][RIGHT] = tempEdge;

                //Remap the second corner piece
                squares[YELLOW][TOP_RIGHT] = squares[ORANGE][BOTTOM_LEFT];
                squares[ORANGE][BOTTOM_LEFT] = squares[WHITE][TOP_RIGHT];
                squares[WHITE][TOP_RIGHT] = squares[RED][TOP_RIGHT];
                squares[RED][TOP_RIGHT] = tempSecondCorner;
            }
        }

        private void rotateOrangeAdjacentFaces(RotationDirection direction)
        {
            FaceColor tempFirstCorner = squares[YELLOW][TOP_RIGHT];
            FaceColor tempEdge = squares[YELLOW][TOP];
            FaceColor tempSecondCorner = squares[YELLOW][TOP_LEFT];

            if (direction == RotationDirection.Clockwise)
            {
                //Remap the first corner piece
                squares[YELLOW][TOP_RIGHT] = squares[GREEN][BOTTOM_RIGHT];
                squares[GREEN][BOTTOM_RIGHT] = squares[WHITE][BOTTOM_LEFT];
                squares[WHITE][BOTTOM_LEFT] = squares[BLUE][TOP_LEFT];
                squares[BLUE][TOP_LEFT] = tempFirstCorner;

                //Remap the edge piece
                squares[YELLOW][TOP] = squares[GREEN][RIGHT];
                squares[GREEN][RIGHT] = squares[WHITE][BOTTOM];
                squares[WHITE][BOTTOM] = squares[BLUE][LEFT];
                squares[BLUE][LEFT] = tempEdge;

                //Remap the second corner piece
                squares[YELLOW][TOP_LEFT] = squares[GREEN][TOP_RIGHT];
                squares[GREEN][TOP_RIGHT] = squares[WHITE][BOTTOM_RIGHT];
                squares[WHITE][BOTTOM_RIGHT] = squares[BLUE][BOTTOM_LEFT];
                squares[BLUE][BOTTOM_LEFT] = tempSecondCorner;
            }
            else
            {
                //Remap the first corner piece
                squares[YELLOW][TOP_RIGHT] = squares[BLUE][TOP_LEFT];
                squares[BLUE][TOP_LEFT] = squares[WHITE][BOTTOM_LEFT];
                squares[WHITE][BOTTOM_LEFT] = squares[GREEN][BOTTOM_RIGHT];
                squares[GREEN][BOTTOM_RIGHT] = tempFirstCorner;

                //Remap the edge piece
                squares[YELLOW][TOP] = squares[BLUE][LEFT];
                squares[BLUE][LEFT] = squares[WHITE][BOTTOM];
                squares[WHITE][BOTTOM] = squares[GREEN][RIGHT];
                squares[GREEN][RIGHT] = tempEdge;

                //Remap the second corner piece
                squares[YELLOW][TOP_LEFT] = squares[BLUE][BOTTOM_LEFT];
                squares[BLUE][BOTTOM_LEFT] = squares[WHITE][BOTTOM_RIGHT];
                squares[WHITE][BOTTOM_RIGHT] = squares[GREEN][TOP_RIGHT];
                squares[GREEN][TOP_RIGHT] = tempSecondCorner;
            }
        }

        private void rotateRedAdjacentFaces(RotationDirection direction)
        {
            FaceColor tempFirstCorner = squares[YELLOW][BOTTOM_LEFT];
            FaceColor tempEdge = squares[YELLOW][BOTTOM];
            FaceColor tempSecondCorner = squares[YELLOW][BOTTOM_RIGHT];

            if (direction == RotationDirection.Clockwise)
            {
                //Remap the first corner piece
                squares[YELLOW][BOTTOM_LEFT] = squares[BLUE][BOTTOM_RIGHT];
                squares[BLUE][BOTTOM_RIGHT] = squares[WHITE][TOP_RIGHT];
                squares[WHITE][TOP_RIGHT] = squares[GREEN][TOP_LEFT];
                squares[GREEN][TOP_LEFT] = tempFirstCorner;

                //Remap the edge piece
                squares[YELLOW][BOTTOM] = squares[BLUE][RIGHT];
                squares[BLUE][RIGHT] = squares[WHITE][TOP];
                squares[WHITE][TOP] = squares[GREEN][LEFT];
                squares[GREEN][LEFT] = tempEdge;

                //Remap the second corner piece
                squares[YELLOW][BOTTOM_RIGHT] = squares[BLUE][TOP_RIGHT];
                squares[BLUE][TOP_RIGHT] = squares[WHITE][TOP_LEFT];
                squares[WHITE][TOP_LEFT] = squares[GREEN][BOTTOM_LEFT];
                squares[GREEN][BOTTOM_LEFT] = tempSecondCorner;
            }
            else
            {
                //Remap the first corner piece
                squares[YELLOW][BOTTOM_LEFT] = squares[GREEN][TOP_LEFT];
                squares[GREEN][TOP_LEFT] = squares[WHITE][TOP_RIGHT];
                squares[WHITE][TOP_RIGHT] = squares[BLUE][BOTTOM_RIGHT];
                squares[BLUE][BOTTOM_RIGHT] = tempFirstCorner;

                //Remap the edge piece
                squares[YELLOW][BOTTOM] = squares[GREEN][LEFT];
                squares[GREEN][LEFT] = squares[WHITE][TOP];
                squares[WHITE][TOP] = squares[BLUE][RIGHT];
                squares[BLUE][RIGHT] = tempEdge;

                //Remap the second corner piece
                squares[YELLOW][BOTTOM_RIGHT] = squares[GREEN][BOTTOM_LEFT];
                squares[GREEN][BOTTOM_LEFT] = squares[WHITE][TOP_LEFT];
                squares[WHITE][TOP_LEFT] = squares[BLUE][TOP_RIGHT];
                squares[BLUE][TOP_RIGHT] = tempSecondCorner;
            }
        }

        private void rotateWhiteAdjacentFaces(RotationDirection direction)
        {
            FaceColor tempFirstCorner = squares[RED][BOTTOM_LEFT];
            FaceColor tempEdge = squares[RED][BOTTOM];
            FaceColor tempSecondCorner = squares[RED][BOTTOM_RIGHT];

            if (direction == RotationDirection.Clockwise)
            {
                //Remap the first corner piece
                squares[RED][BOTTOM_LEFT] = squares[BLUE][BOTTOM_LEFT];
                squares[BLUE][BOTTOM_LEFT] = squares[ORANGE][BOTTOM_LEFT];
                squares[ORANGE][BOTTOM_LEFT] = squares[GREEN][BOTTOM_LEFT];
                squares[GREEN][BOTTOM_LEFT] = tempFirstCorner;

                //Remap the edge piece
                squares[RED][BOTTOM] = squares[BLUE][BOTTOM];
                squares[BLUE][BOTTOM] = squares[ORANGE][BOTTOM];
                squares[ORANGE][BOTTOM] = squares[GREEN][BOTTOM];
                squares[GREEN][BOTTOM] = tempEdge;

                //Remap the second corner piece
                squares[RED][BOTTOM_RIGHT] = squares[BLUE][BOTTOM_RIGHT];
                squares[BLUE][BOTTOM_RIGHT] = squares[ORANGE][BOTTOM_RIGHT];
                squares[ORANGE][BOTTOM_RIGHT] = squares[GREEN][BOTTOM_RIGHT];
                squares[GREEN][BOTTOM_RIGHT] = tempSecondCorner;
            }
            else
            {
                //Remap the first corner piece
                squares[RED][BOTTOM_LEFT] = squares[GREEN][BOTTOM_LEFT];
                squares[GREEN][BOTTOM_LEFT] = squares[ORANGE][BOTTOM_LEFT];
                squares[ORANGE][BOTTOM_LEFT] = squares[BLUE][BOTTOM_LEFT];
                squares[BLUE][BOTTOM_LEFT] = tempFirstCorner;

                //Remap the edge piece
                squares[RED][BOTTOM] = squares[GREEN][BOTTOM];
                squares[GREEN][BOTTOM] = squares[ORANGE][BOTTOM];
                squares[ORANGE][BOTTOM] = squares[BLUE][BOTTOM];
                squares[BLUE][BOTTOM] = tempEdge;

                //Remap the second corner piece
                squares[RED][BOTTOM_RIGHT] = squares[GREEN][BOTTOM_RIGHT];
                squares[GREEN][BOTTOM_RIGHT] = squares[ORANGE][BOTTOM_RIGHT];
                squares[ORANGE][BOTTOM_RIGHT] = squares[BLUE][BOTTOM_RIGHT];
                squares[BLUE][BOTTOM_RIGHT] = tempSecondCorner;
            }
        }

        private void rotateYellowAdjacentFaces(RotationDirection direction)
        {
            FaceColor tempFirstCorner = squares[ORANGE][TOP_RIGHT];
            FaceColor tempEdge = squares[ORANGE][TOP];
            FaceColor tempSecondCorner = squares[ORANGE][TOP_LEFT];

            if (direction == RotationDirection.Clockwise)
            {
                //Remap the first corner piece
                squares[ORANGE][TOP_RIGHT] = squares[BLUE][TOP_RIGHT];
                squares[BLUE][TOP_RIGHT] = squares[RED][TOP_RIGHT];
                squares[RED][TOP_RIGHT] = squares[GREEN][TOP_RIGHT];
                squares[GREEN][TOP_RIGHT] = tempFirstCorner;

                //Remap the edge piece
                squares[ORANGE][TOP] = squares[BLUE][TOP];
                squares[BLUE][TOP] = squares[RED][TOP];
                squares[RED][TOP] = squares[GREEN][TOP];
                squares[GREEN][TOP] = tempEdge;

                //Remap the second corner piece
                squares[ORANGE][TOP_LEFT] = squares[BLUE][TOP_LEFT];
                squares[BLUE][TOP_LEFT] = squares[RED][TOP_LEFT];
                squares[RED][TOP_LEFT] = squares[GREEN][TOP_LEFT];
                squares[GREEN][TOP_LEFT] = tempSecondCorner;
            }
            else
            {
                //Remap the first corner piece
                squares[ORANGE][TOP_RIGHT] = squares[GREEN][TOP_RIGHT];
                squares[GREEN][TOP_RIGHT] = squares[RED][TOP_RIGHT];
                squares[RED][TOP_RIGHT] = squares[BLUE][TOP_RIGHT];
                squares[BLUE][TOP_RIGHT] = tempFirstCorner;

                //Remap the edge piece
                squares[ORANGE][TOP] = squares[GREEN][TOP];
                squares[GREEN][TOP] = squares[RED][TOP];
                squares[RED][TOP] = squares[BLUE][TOP];
                squares[BLUE][TOP] = tempEdge;

                //Remap the second corner piece
                squares[ORANGE][TOP_LEFT] = squares[GREEN][TOP_LEFT];
                squares[GREEN][TOP_LEFT] = squares[RED][TOP_LEFT];
                squares[RED][TOP_LEFT] = squares[BLUE][TOP_LEFT];
                squares[BLUE][TOP_LEFT] = tempSecondCorner;
            }
        }

        public void PrintCubeState()
        {
            for (byte currentFace = 0; currentFace < NUMBER_OF_FACES; currentFace++)
            {
                var faceSquares = squares[currentFace];

                Debug.Log((FaceColor)currentFace + " Face: " + faceSquares[0] + " " + faceSquares[1] + " " + faceSquares[2] + " "
                    + faceSquares[3] + " " + faceSquares[4] + " " + faceSquares[5] + " " + faceSquares[6] + " " + faceSquares[7]);
            }
        }

        public FaceColor[][] GetAllFaces()
        {
            return squares;
        }
    }
}
