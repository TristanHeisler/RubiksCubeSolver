using Rubiks.Enums;
using UnityEngine;

namespace Rubiks
{
    public class CubeState
    {
        public const int NUMBER_OF_FACES = 6;
        public const int CUBITS_PER_FACE = 8;

        private const int BLUE = (int)FaceColor.Blue;
        private const int GREEN = (int)FaceColor.Green;
        private const int ORANGE = (int)FaceColor.Orange;
        private const int RED = (int)FaceColor.Red;
        private const int WHITE = (int)FaceColor.White;
        private const int YELLOW = (int)FaceColor.Yellow;

        private const int TOP_LEFT = 0;
        private const int TOP = 1;
        private const int TOP_RIGHT = 2;
        private const int RIGHT = 3;
        private const int BOTTOM_RIGHT = 4;
        private const int BOTTOM = 5;
        private const int BOTTOM_LEFT = 6;
        private const int LEFT = 7;

        private FaceColor[][] squares = new FaceColor[NUMBER_OF_FACES][];

        public CubeState()
        {
            for(int currentFace = 0; currentFace < NUMBER_OF_FACES; currentFace++)
            {
                squares[currentFace] = new FaceColor[CUBITS_PER_FACE];

                for(int currentSquare = 0; currentSquare < CUBITS_PER_FACE; currentSquare++)
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
            FaceColor[] face = squares[(int)rotatingFace];

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
        }

        private void rotateGreenAdjacentFaces(RotationDirection direction)
        {
        }

        private void rotateOrangeAdjacentFaces(RotationDirection direction)
        {
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
        }

        private void rotateYellowAdjacentFaces(RotationDirection direction)
        {
        }

        public void PrintCubeState()
        {
            for (int currentFace = 0; currentFace < NUMBER_OF_FACES; currentFace++)
            {
                Debug.Log("Printing face: " + (FaceColor)currentFace);

                for (int currentSquare = 0; currentSquare < CUBITS_PER_FACE; currentSquare++)
                {
                    Debug.Log("Position " + currentSquare + " is " + squares[currentFace][currentSquare]);
                }
            }
        }

        public FaceColor[][] GetAllFaces()
        {
            return squares;
        }
    }
}
