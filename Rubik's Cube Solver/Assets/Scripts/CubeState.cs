using Rubiks.Constants;
using Rubiks.Enums;
using UnityEngine;

namespace Rubiks
{
    public class CubeState
    {
        public const byte NUMBER_OF_FACES = 6;
        public const byte CUBITS_PER_FACE = 8;          

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

            FaceColor tempCorner = face[Locations.TOP_LEFT];
            FaceColor tempEdge = face[Locations.TOP];

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
            FaceColor tempFirstCorner = squares[Colors.YELLOW][Locations.TOP_LEFT];
            FaceColor tempEdge = squares[Colors.YELLOW][Locations.LEFT];
            FaceColor tempSecondCorner = squares[Colors.YELLOW][Locations.BOTTOM_LEFT];

            if (direction == RotationDirection.Clockwise)
            {
                //Remap the first corner piece
                squares[Colors.YELLOW][Locations.TOP_LEFT] = squares[Colors.ORANGE][Locations.BOTTOM_RIGHT];
                squares[Colors.ORANGE][Locations.BOTTOM_RIGHT] = squares[Colors.WHITE][Locations.TOP_LEFT];
                squares[Colors.WHITE][Locations.TOP_LEFT] = squares[Colors.RED][Locations.TOP_LEFT];
                squares[Colors.RED][Locations.TOP_LEFT] = tempFirstCorner;

                //Remap the edge piece
                squares[Colors.YELLOW][Locations.LEFT] = squares[Colors.ORANGE][Locations.RIGHT];
                squares[Colors.ORANGE][Locations.RIGHT] = squares[Colors.WHITE][Locations.LEFT];
                squares[Colors.WHITE][Locations.LEFT] = squares[Colors.RED][Locations.LEFT];
                squares[Colors.RED][Locations.LEFT] = tempEdge;

                //Remap the second corner piece
                squares[Colors.YELLOW][Locations.BOTTOM_LEFT] = squares[Colors.ORANGE][Locations.TOP_RIGHT];
                squares[Colors.ORANGE][Locations.TOP_RIGHT] = squares[Colors.WHITE][Locations.BOTTOM_LEFT];
                squares[Colors.WHITE][Locations.BOTTOM_LEFT] = squares[Colors.RED][Locations.BOTTOM_LEFT];
                squares[Colors.RED][Locations.BOTTOM_LEFT] = tempSecondCorner;
            }
            else
            {
                //Remap the first corner piece
                squares[Colors.YELLOW][Locations.TOP_LEFT] = squares[Colors.RED][Locations.TOP_LEFT];
                squares[Colors.RED][Locations.TOP_LEFT] = squares[Colors.WHITE][Locations.TOP_LEFT];
                squares[Colors.WHITE][Locations.TOP_LEFT] = squares[Colors.ORANGE][Locations.BOTTOM_RIGHT];
                squares[Colors.ORANGE][Locations.BOTTOM_RIGHT] = tempFirstCorner;

                //Remap the edge piece
                squares[Colors.YELLOW][Locations.LEFT] = squares[Colors.RED][Locations.LEFT];
                squares[Colors.RED][Locations.LEFT] = squares[Colors.WHITE][Locations.LEFT];
                squares[Colors.WHITE][Locations.LEFT] = squares[Colors.ORANGE][Locations.RIGHT];
                squares[Colors.ORANGE][Locations.RIGHT] = tempEdge;

                //Remap the second corner piece
                squares[Colors.YELLOW][Locations.BOTTOM_LEFT] = squares[Colors.RED][Locations.BOTTOM_LEFT];
                squares[Colors.RED][Locations.BOTTOM_LEFT] = squares[Colors.WHITE][Locations.BOTTOM_LEFT];
                squares[Colors.WHITE][Locations.BOTTOM_LEFT] = squares[Colors.ORANGE][Locations.TOP_RIGHT];
                squares[Colors.ORANGE][Locations.TOP_RIGHT] = tempSecondCorner;
            }
        }

        private void rotateGreenAdjacentFaces(RotationDirection direction)
        {
            FaceColor tempFirstCorner = squares[Colors.YELLOW][Locations.BOTTOM_RIGHT];
            FaceColor tempEdge = squares[Colors.YELLOW][Locations.RIGHT];
            FaceColor tempSecondCorner = squares[Colors.YELLOW][Locations.TOP_RIGHT];

            if (direction == RotationDirection.Clockwise)
            {
                //Remap the first corner piece
                squares[Colors.YELLOW][Locations.BOTTOM_RIGHT] = squares[Colors.RED][Locations.BOTTOM_RIGHT];
                squares[Colors.RED][Locations.BOTTOM_RIGHT] = squares[Colors.WHITE][Locations.BOTTOM_RIGHT];
                squares[Colors.WHITE][Locations.BOTTOM_RIGHT] = squares[Colors.ORANGE][Locations.TOP_LEFT];
                squares[Colors.ORANGE][Locations.TOP_LEFT] = tempFirstCorner;

                //Remap the edge piece
                squares[Colors.YELLOW][Locations.RIGHT] = squares[Colors.RED][Locations.RIGHT];
                squares[Colors.RED][Locations.RIGHT] = squares[Colors.WHITE][Locations.RIGHT];
                squares[Colors.WHITE][Locations.RIGHT] = squares[Colors.ORANGE][Locations.LEFT];
                squares[Colors.ORANGE][Locations.LEFT] = tempEdge;

                //Remap the second corner piece
                squares[Colors.YELLOW][Locations.TOP_RIGHT] = squares[Colors.RED][Locations.TOP_RIGHT];
                squares[Colors.RED][Locations.TOP_RIGHT] = squares[Colors.WHITE][Locations.TOP_RIGHT];
                squares[Colors.WHITE][Locations.TOP_RIGHT] = squares[Colors.ORANGE][Locations.BOTTOM_LEFT];
                squares[Colors.ORANGE][Locations.BOTTOM_LEFT] = tempSecondCorner;
            }
            else
            {
                //Remap the first corner piece
                squares[Colors.YELLOW][Locations.BOTTOM_RIGHT] = squares[Colors.ORANGE][Locations.TOP_LEFT];
                squares[Colors.ORANGE][Locations.TOP_LEFT] = squares[Colors.WHITE][Locations.BOTTOM_RIGHT];
                squares[Colors.WHITE][Locations.BOTTOM_RIGHT] = squares[Colors.RED][Locations.BOTTOM_RIGHT];
                squares[Colors.RED][Locations.BOTTOM_RIGHT] = tempFirstCorner;

                //Remap the edge piece
                squares[Colors.YELLOW][Locations.RIGHT] = squares[Colors.ORANGE][Locations.LEFT];
                squares[Colors.ORANGE][Locations.LEFT] = squares[Colors.WHITE][Locations.RIGHT];
                squares[Colors.WHITE][Locations.RIGHT] = squares[Colors.RED][Locations.RIGHT];
                squares[Colors.RED][Locations.RIGHT] = tempEdge;

                //Remap the second corner piece
                squares[Colors.YELLOW][Locations.TOP_RIGHT] = squares[Colors.ORANGE][Locations.BOTTOM_LEFT];
                squares[Colors.ORANGE][Locations.BOTTOM_LEFT] = squares[Colors.WHITE][Locations.TOP_RIGHT];
                squares[Colors.WHITE][Locations.TOP_RIGHT] = squares[Colors.RED][Locations.TOP_RIGHT];
                squares[Colors.RED][Locations.TOP_RIGHT] = tempSecondCorner;
            }
        }

        private void rotateOrangeAdjacentFaces(RotationDirection direction)
        {
            FaceColor tempFirstCorner = squares[Colors.YELLOW][Locations.TOP_RIGHT];
            FaceColor tempEdge = squares[Colors.YELLOW][Locations.TOP];
            FaceColor tempSecondCorner = squares[Colors.YELLOW][Locations.TOP_LEFT];

            if (direction == RotationDirection.Clockwise)
            {
                //Remap the first corner piece
                squares[Colors.YELLOW][Locations.TOP_RIGHT] = squares[Colors.GREEN][Locations.BOTTOM_RIGHT];
                squares[Colors.GREEN][Locations.BOTTOM_RIGHT] = squares[Colors.WHITE][Locations.BOTTOM_LEFT];
                squares[Colors.WHITE][Locations.BOTTOM_LEFT] = squares[Colors.BLUE][Locations.TOP_LEFT];
                squares[Colors.BLUE][Locations.TOP_LEFT] = tempFirstCorner;

                //Remap the edge piece
                squares[Colors.YELLOW][Locations.TOP] = squares[Colors.GREEN][Locations.RIGHT];
                squares[Colors.GREEN][Locations.RIGHT] = squares[Colors.WHITE][Locations.BOTTOM];
                squares[Colors.WHITE][Locations.BOTTOM] = squares[Colors.BLUE][Locations.LEFT];
                squares[Colors.BLUE][Locations.LEFT] = tempEdge;

                //Remap the second corner piece
                squares[Colors.YELLOW][Locations.TOP_LEFT] = squares[Colors.GREEN][Locations.TOP_RIGHT];
                squares[Colors.GREEN][Locations.TOP_RIGHT] = squares[Colors.WHITE][Locations.BOTTOM_RIGHT];
                squares[Colors.WHITE][Locations.BOTTOM_RIGHT] = squares[Colors.BLUE][Locations.BOTTOM_LEFT];
                squares[Colors.BLUE][Locations.BOTTOM_LEFT] = tempSecondCorner;
            }
            else
            {
                //Remap the first corner piece
                squares[Colors.YELLOW][Locations.TOP_RIGHT] = squares[Colors.BLUE][Locations.TOP_LEFT];
                squares[Colors.BLUE][Locations.TOP_LEFT] = squares[Colors.WHITE][Locations.BOTTOM_LEFT];
                squares[Colors.WHITE][Locations.BOTTOM_LEFT] = squares[Colors.GREEN][Locations.BOTTOM_RIGHT];
                squares[Colors.GREEN][Locations.BOTTOM_RIGHT] = tempFirstCorner;

                //Remap the edge piece
                squares[Colors.YELLOW][Locations.TOP] = squares[Colors.BLUE][Locations.LEFT];
                squares[Colors.BLUE][Locations.LEFT] = squares[Colors.WHITE][Locations.BOTTOM];
                squares[Colors.WHITE][Locations.BOTTOM] = squares[Colors.GREEN][Locations.RIGHT];
                squares[Colors.GREEN][Locations.RIGHT] = tempEdge;

                //Remap the second corner piece
                squares[Colors.YELLOW][Locations.TOP_LEFT] = squares[Colors.BLUE][Locations.BOTTOM_LEFT];
                squares[Colors.BLUE][Locations.BOTTOM_LEFT] = squares[Colors.WHITE][Locations.BOTTOM_RIGHT];
                squares[Colors.WHITE][Locations.BOTTOM_RIGHT] = squares[Colors.GREEN][Locations.TOP_RIGHT];
                squares[Colors.GREEN][Locations.TOP_RIGHT] = tempSecondCorner;
            }
        }

        private void rotateRedAdjacentFaces(RotationDirection direction)
        {
            FaceColor tempFirstCorner = squares[Colors.YELLOW][Locations.BOTTOM_LEFT];
            FaceColor tempEdge = squares[Colors.YELLOW][Locations.BOTTOM];
            FaceColor tempSecondCorner = squares[Colors.YELLOW][Locations.BOTTOM_RIGHT];

            if (direction == RotationDirection.Clockwise)
            {
                //Remap the first corner piece
                squares[Colors.YELLOW][Locations.BOTTOM_LEFT] = squares[Colors.BLUE][Locations.BOTTOM_RIGHT];
                squares[Colors.BLUE][Locations.BOTTOM_RIGHT] = squares[Colors.WHITE][Locations.TOP_RIGHT];
                squares[Colors.WHITE][Locations.TOP_RIGHT] = squares[Colors.GREEN][Locations.TOP_LEFT];
                squares[Colors.GREEN][Locations.TOP_LEFT] = tempFirstCorner;

                //Remap the edge piece
                squares[Colors.YELLOW][Locations.BOTTOM] = squares[Colors.BLUE][Locations.RIGHT];
                squares[Colors.BLUE][Locations.RIGHT] = squares[Colors.WHITE][Locations.TOP];
                squares[Colors.WHITE][Locations.TOP] = squares[Colors.GREEN][Locations.LEFT];
                squares[Colors.GREEN][Locations.LEFT] = tempEdge;

                //Remap the second corner piece
                squares[Colors.YELLOW][Locations.BOTTOM_RIGHT] = squares[Colors.BLUE][Locations.TOP_RIGHT];
                squares[Colors.BLUE][Locations.TOP_RIGHT] = squares[Colors.WHITE][Locations.TOP_LEFT];
                squares[Colors.WHITE][Locations.TOP_LEFT] = squares[Colors.GREEN][Locations.BOTTOM_LEFT];
                squares[Colors.GREEN][Locations.BOTTOM_LEFT] = tempSecondCorner;
            }
            else
            {
                //Remap the first corner piece
                squares[Colors.YELLOW][Locations.BOTTOM_LEFT] = squares[Colors.GREEN][Locations.TOP_LEFT];
                squares[Colors.GREEN][Locations.TOP_LEFT] = squares[Colors.WHITE][Locations.TOP_RIGHT];
                squares[Colors.WHITE][Locations.TOP_RIGHT] = squares[Colors.BLUE][Locations.BOTTOM_RIGHT];
                squares[Colors.BLUE][Locations.BOTTOM_RIGHT] = tempFirstCorner;

                //Remap the edge piece
                squares[Colors.YELLOW][Locations.BOTTOM] = squares[Colors.GREEN][Locations.LEFT];
                squares[Colors.GREEN][Locations.LEFT] = squares[Colors.WHITE][Locations.TOP];
                squares[Colors.WHITE][Locations.TOP] = squares[Colors.BLUE][Locations.RIGHT];
                squares[Colors.BLUE][Locations.RIGHT] = tempEdge;

                //Remap the second corner piece
                squares[Colors.YELLOW][Locations.BOTTOM_RIGHT] = squares[Colors.GREEN][Locations.BOTTOM_LEFT];
                squares[Colors.GREEN][Locations.BOTTOM_LEFT] = squares[Colors.WHITE][Locations.TOP_LEFT];
                squares[Colors.WHITE][Locations.TOP_LEFT] = squares[Colors.BLUE][Locations.TOP_RIGHT];
                squares[Colors.BLUE][Locations.TOP_RIGHT] = tempSecondCorner;
            }
        }

        private void rotateWhiteAdjacentFaces(RotationDirection direction)
        {
            FaceColor tempFirstCorner = squares[Colors.RED][Locations.BOTTOM_LEFT];
            FaceColor tempEdge = squares[Colors.RED][Locations.BOTTOM];
            FaceColor tempSecondCorner = squares[Colors.RED][Locations.BOTTOM_RIGHT];

            if (direction == RotationDirection.Clockwise)
            {
                //Remap the first corner piece
                squares[Colors.RED][Locations.BOTTOM_LEFT] = squares[Colors.BLUE][Locations.BOTTOM_LEFT];
                squares[Colors.BLUE][Locations.BOTTOM_LEFT] = squares[Colors.ORANGE][Locations.BOTTOM_LEFT];
                squares[Colors.ORANGE][Locations.BOTTOM_LEFT] = squares[Colors.GREEN][Locations.BOTTOM_LEFT];
                squares[Colors.GREEN][Locations.BOTTOM_LEFT] = tempFirstCorner;

                //Remap the edge piece
                squares[Colors.RED][Locations.BOTTOM] = squares[Colors.BLUE][Locations.BOTTOM];
                squares[Colors.BLUE][Locations.BOTTOM] = squares[Colors.ORANGE][Locations.BOTTOM];
                squares[Colors.ORANGE][Locations.BOTTOM] = squares[Colors.GREEN][Locations.BOTTOM];
                squares[Colors.GREEN][Locations.BOTTOM] = tempEdge;

                //Remap the second corner piece
                squares[Colors.RED][Locations.BOTTOM_RIGHT] = squares[Colors.BLUE][Locations.BOTTOM_RIGHT];
                squares[Colors.BLUE][Locations.BOTTOM_RIGHT] = squares[Colors.ORANGE][Locations.BOTTOM_RIGHT];
                squares[Colors.ORANGE][Locations.BOTTOM_RIGHT] = squares[Colors.GREEN][Locations.BOTTOM_RIGHT];
                squares[Colors.GREEN][Locations.BOTTOM_RIGHT] = tempSecondCorner;
            }
            else
            {
                //Remap the first corner piece
                squares[Colors.RED][Locations.BOTTOM_LEFT] = squares[Colors.GREEN][Locations.BOTTOM_LEFT];
                squares[Colors.GREEN][Locations.BOTTOM_LEFT] = squares[Colors.ORANGE][Locations.BOTTOM_LEFT];
                squares[Colors.ORANGE][Locations.BOTTOM_LEFT] = squares[Colors.BLUE][Locations.BOTTOM_LEFT];
                squares[Colors.BLUE][Locations.BOTTOM_LEFT] = tempFirstCorner;

                //Remap the edge piece
                squares[Colors.RED][Locations.BOTTOM] = squares[Colors.GREEN][Locations.BOTTOM];
                squares[Colors.GREEN][Locations.BOTTOM] = squares[Colors.ORANGE][Locations.BOTTOM];
                squares[Colors.ORANGE][Locations.BOTTOM] = squares[Colors.BLUE][Locations.BOTTOM];
                squares[Colors.BLUE][Locations.BOTTOM] = tempEdge;

                //Remap the second corner piece
                squares[Colors.RED][Locations.BOTTOM_RIGHT] = squares[Colors.GREEN][Locations.BOTTOM_RIGHT];
                squares[Colors.GREEN][Locations.BOTTOM_RIGHT] = squares[Colors.ORANGE][Locations.BOTTOM_RIGHT];
                squares[Colors.ORANGE][Locations.BOTTOM_RIGHT] = squares[Colors.BLUE][Locations.BOTTOM_RIGHT];
                squares[Colors.BLUE][Locations.BOTTOM_RIGHT] = tempSecondCorner;
            }
        }

        private void rotateYellowAdjacentFaces(RotationDirection direction)
        {
            FaceColor tempFirstCorner = squares[Colors.ORANGE][Locations.TOP_RIGHT];
            FaceColor tempEdge = squares[Colors.ORANGE][Locations.TOP];
            FaceColor tempSecondCorner = squares[Colors.ORANGE][Locations.TOP_LEFT];

            if (direction == RotationDirection.Clockwise)
            {
                //Remap the first corner piece
                squares[Colors.ORANGE][Locations.TOP_RIGHT] = squares[Colors.BLUE][Locations.TOP_RIGHT];
                squares[Colors.BLUE][Locations.TOP_RIGHT] = squares[Colors.RED][Locations.TOP_RIGHT];
                squares[Colors.RED][Locations.TOP_RIGHT] = squares[Colors.GREEN][Locations.TOP_RIGHT];
                squares[Colors.GREEN][Locations.TOP_RIGHT] = tempFirstCorner;

                //Remap the edge piece
                squares[Colors.ORANGE][Locations.TOP] = squares[Colors.BLUE][Locations.TOP];
                squares[Colors.BLUE][Locations.TOP] = squares[Colors.RED][Locations.TOP];
                squares[Colors.RED][Locations.TOP] = squares[Colors.GREEN][Locations.TOP];
                squares[Colors.GREEN][Locations.TOP] = tempEdge;

                //Remap the second corner piece
                squares[Colors.ORANGE][Locations.TOP_LEFT] = squares[Colors.BLUE][Locations.TOP_LEFT];
                squares[Colors.BLUE][Locations.TOP_LEFT] = squares[Colors.RED][Locations.TOP_LEFT];
                squares[Colors.RED][Locations.TOP_LEFT] = squares[Colors.GREEN][Locations.TOP_LEFT];
                squares[Colors.GREEN][Locations.TOP_LEFT] = tempSecondCorner;
            }
            else
            {
                //Remap the first corner piece
                squares[Colors.ORANGE][Locations.TOP_RIGHT] = squares[Colors.GREEN][Locations.TOP_RIGHT];
                squares[Colors.GREEN][Locations.TOP_RIGHT] = squares[Colors.RED][Locations.TOP_RIGHT];
                squares[Colors.RED][Locations.TOP_RIGHT] = squares[Colors.BLUE][Locations.TOP_RIGHT];
                squares[Colors.BLUE][Locations.TOP_RIGHT] = tempFirstCorner;

                //Remap the edge piece
                squares[Colors.ORANGE][Locations.TOP] = squares[Colors.GREEN][Locations.TOP];
                squares[Colors.GREEN][Locations.TOP] = squares[Colors.RED][Locations.TOP];
                squares[Colors.RED][Locations.TOP] = squares[Colors.BLUE][Locations.TOP];
                squares[Colors.BLUE][Locations.TOP] = tempEdge;

                //Remap the second corner piece
                squares[Colors.ORANGE][Locations.TOP_LEFT] = squares[Colors.GREEN][Locations.TOP_LEFT];
                squares[Colors.GREEN][Locations.TOP_LEFT] = squares[Colors.RED][Locations.TOP_LEFT];
                squares[Colors.RED][Locations.TOP_LEFT] = squares[Colors.BLUE][Locations.TOP_LEFT];
                squares[Colors.BLUE][Locations.TOP_LEFT] = tempSecondCorner;
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

        public FaceColor[] GetBlueFace()
        {
            return squares[Colors.BLUE];
        }

        public FaceColor[] GetGreenFace()
        {
            return squares[Colors.GREEN];
        }

        public FaceColor[] GetOrangeFace()
        {
            return squares[Colors.ORANGE];
        }

        public FaceColor[] GetRedFace()
        {
            return squares[Colors.RED];
        }

        public FaceColor[] GetWhiteFace()
        {
            return squares[Colors.WHITE];
        }

        public FaceColor[] GetYellowFace()
        {
            return squares[Colors.YELLOW];
        }
    }
}
