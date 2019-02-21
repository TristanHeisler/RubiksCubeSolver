using Rubiks.Enums;
using System;
using UnityEngine;

namespace Rubiks
{
    public class Cube : MonoBehaviour
    {
        public const int ROTATION_SPEED = 10;

        //Objects to represent the physical cube in the game
        //8 Corner Cubes
        public GameObject Corner_OBW;
        public GameObject Corner_OBY;
        public GameObject Corner_OGW;
        public GameObject Corner_OGY;
        public GameObject Corner_RBW;
        public GameObject Corner_RBY;
        public GameObject Corner_RGW;
        public GameObject Corner_RGY;

        //12 Edge Cubes
        public GameObject Edge_BW;
        public GameObject Edge_BY;
        public GameObject Edge_GW;
        public GameObject Edge_GY;
        public GameObject Edge_OB;
        public GameObject Edge_OG;
        public GameObject Edge_OW;
        public GameObject Edge_OY;
        public GameObject Edge_RB;
        public GameObject Edge_RG;
        public GameObject Edge_RW;
        public GameObject Edge_RY;

        //6 Middle Cubes
        public GameObject Middle_B;
        public GameObject Middle_G;
        public GameObject Middle_O;
        public GameObject Middle_R;
        public GameObject Middle_W;
        public GameObject Middle_Y;

        //Objects to represent the current state of each cube face
        private Face blueFace, greenFace, orangeFace, redFace, whiteFace, yellowFace;

        private FaceColor rotatingFaceColor;
        private Face rotatingFace;
        private RotationDirection rotationDirection;

        public void InitializeFaces()
        {
            blueFace = new Face
            {
                cubes = new GameObject[,]
                {
                    { Corner_OBY, Edge_BY, Corner_RBY},
                    { Edge_OB, Middle_B, Edge_RB},
                    { Corner_OBW, Edge_BW, Corner_RBW}
                },
                rotationAxis = Vector3.left
            };

            greenFace = new Face
            {
                cubes = new GameObject[,]
                {
                    { Corner_RGY, Edge_GY, Corner_OGY},
                    { Edge_RG, Middle_G, Edge_OG},
                    { Corner_RGW, Edge_GW, Corner_OGW}
                },
                rotationAxis = Vector3.right
            };

            orangeFace = new Face
            {
                cubes = new GameObject[,]
                {
                    { Corner_OGY, Edge_OY, Corner_OBY},
                    { Edge_OG, Middle_O, Edge_OB},
                    { Corner_OGW, Edge_OW, Corner_OBW}
                },
                rotationAxis = Vector3.forward
            };

            redFace = new Face
            {
                cubes = new GameObject[,]
                {
                    { Corner_RBY, Edge_RY, Corner_RGY},
                    { Edge_RB, Middle_R, Edge_RG},
                    { Corner_RBW, Edge_RW, Corner_RGW}
                },
                rotationAxis = Vector3.back
            };

            whiteFace = new Face
            {
                cubes = new GameObject[,]
                {
                    { Corner_RBW, Edge_RW, Corner_RGW},
                    { Edge_BW, Middle_W, Edge_GW},
                    { Corner_OBW, Edge_OW, Corner_OGW}
                },
                rotationAxis = Vector3.down
            };

            yellowFace = new Face
            {
                cubes = new GameObject[,]
                {
                    { Corner_OBY, Edge_OY, Corner_OGY},
                    { Edge_BY, Middle_Y, Edge_GY},
                    { Corner_RBY, Edge_RY, Corner_RGY}
                },
                rotationAxis = Vector3.up
            };
        }

        public void SetRotationDirection(RotationDirection direction)
        {
            rotationDirection = direction;
        }

        public void SetRotatingFace(FaceColor faceColor)
        {
            rotatingFaceColor = faceColor;

            switch(faceColor)
            {
                case FaceColor.Blue:
                    rotatingFace = blueFace;
                    break;
                case FaceColor.Green:
                    rotatingFace = greenFace;
                    break;
                case FaceColor.Orange:
                    rotatingFace = orangeFace;
                    break;
                case FaceColor.Red:
                    rotatingFace = redFace;
                    break;                
                case FaceColor.White:
                    rotatingFace = whiteFace;
                    break;
                case FaceColor.Yellow:
                    rotatingFace = yellowFace;
                    break;
            }
        }

        public void RotateCubeFace()
        {
            //Rotate all cubes associated with the rotating face around the appropriate axis
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    rotatingFace.cubes[i, j].transform.RotateAround(Vector3.zero, rotatingFace.rotationAxis, ROTATION_SPEED * (int)rotationDirection);
                }
            }          
        }

        public void HandleRotationRemapping()
        {
            //Perform the correct remappings
            switch (rotatingFaceColor)
            {
                case FaceColor.Blue:
                    handleBlueRotationRemapping();
                    break;
                case FaceColor.Green:
                    handleGreenRotationRemapping();
                    break;
                case FaceColor.Orange:
                    handleOrangeRotationRemapping();
                    break;
                case FaceColor.Red:
                    handleRedRotationRemapping();
                    break;
                case FaceColor.White:
                    handleWhiteRotationRemapping();
                    break;
                case FaceColor.Yellow:
                    handleYellowRotationRemapping();
                    break;
            }
        }

        private void handleBlueRotationRemapping()
        {
            var tempCorner = blueFace.cubes[0, 0];
            var tempEdge = blueFace.cubes[0, 1];

            if (rotationDirection == RotationDirection.Clockwise)
            {
                //Corners
                setOBYCorner(blueFace.cubes[2, 0]);
                setOBWCorner(blueFace.cubes[2, 2]);
                setRBWCorner(blueFace.cubes[0, 2]);
                setRBYCorner(tempCorner);

                //Edges
                setBYEdge(blueFace.cubes[1, 0]);
                setOBEdge(blueFace.cubes[2, 1]);
                setBWEdge(blueFace.cubes[1, 2]);
                setRBEdge(tempEdge);
            }
            else
            {
                //Corners
                setOBYCorner(blueFace.cubes[0, 2]);
                setRBYCorner(blueFace.cubes[2, 2]);
                setRBWCorner(blueFace.cubes[2, 0]);
                setOBWCorner(tempCorner);

                //Edges
                setBYEdge(blueFace.cubes[1, 2]);
                setRBEdge(blueFace.cubes[2, 1]);
                setBWEdge(blueFace.cubes[1, 0]);
                setOBEdge(tempEdge);
            }
        }

        private void handleGreenRotationRemapping()
        {
            var tempCorner = greenFace.cubes[0, 0];
            var tempEdge = greenFace.cubes[0, 1];

            if (rotationDirection == RotationDirection.Clockwise)
            {
                //Corners
                setRGYCorner(greenFace.cubes[2, 0]);
                setRGWCorner(greenFace.cubes[2, 2]);
                setOGWCorner(greenFace.cubes[0, 2]);
                setOGYCorner(tempCorner);

                //Edges
                setGYEdge(greenFace.cubes[1, 0]);
                setRGEdge(greenFace.cubes[2, 1]);
                setGWEdge(greenFace.cubes[1, 2]);
                setOGEdge(tempEdge);
            }
            else
            {
                //Corners
                setRGYCorner(greenFace.cubes[0, 2]);
                setOGYCorner(greenFace.cubes[2, 2]);
                setOGWCorner(greenFace.cubes[2, 0]);
                setRGWCorner(tempCorner);

                //Edges
                setGYEdge(greenFace.cubes[1, 2]);
                setOGEdge(greenFace.cubes[2, 1]);
                setGWEdge(greenFace.cubes[1, 0]);
                setRGEdge(tempEdge);
            }
        }

        private void handleOrangeRotationRemapping()
        {
            var tempCorner = orangeFace.cubes[0, 0];
            var tempEdge = orangeFace.cubes[0, 1];

            if (rotationDirection == RotationDirection.Clockwise)
            {
                //Corners
                setOGYCorner(orangeFace.cubes[2, 0]);
                setOGWCorner(orangeFace.cubes[2, 2]);
                setOBWCorner(orangeFace.cubes[0, 2]);
                setOBYCorner(tempCorner);

                //Edges
                setOYEdge(orangeFace.cubes[1, 0]);
                setOGEdge(orangeFace.cubes[2, 1]);
                setOWEdge(orangeFace.cubes[1, 2]);
                setOBEdge(tempEdge);
            }
            else
            {
                //Corners
                setOGYCorner(orangeFace.cubes[0, 2]);
                setOBYCorner(orangeFace.cubes[2, 2]);
                setOBWCorner(orangeFace.cubes[2, 0]);
                setOGWCorner(tempCorner);

                //Edges
                setOYEdge(orangeFace.cubes[1, 2]);
                setOBEdge(orangeFace.cubes[2, 1]);
                setOWEdge(orangeFace.cubes[1, 0]);
                setOGEdge(tempEdge);
            }
        }

        private void handleRedRotationRemapping()
        {
            var tempCorner = redFace.cubes[0, 0];
            var tempEdge = redFace.cubes[0, 1];

            if (rotationDirection == RotationDirection.Clockwise)
            {
                //Corners
                setRBYCorner(redFace.cubes[2, 0]);
                setRBWCorner(redFace.cubes[2, 2]);
                setRGWCorner(redFace.cubes[0, 2]);
                setRGYCorner(tempCorner);

                //Edges
                setRYEdge(redFace.cubes[1, 0]);
                setRBEdge(redFace.cubes[2, 1]);
                setRWEdge(redFace.cubes[1, 2]);
                setRGEdge(tempEdge);
            }
            else
            {
                //Corners
                setRBYCorner(redFace.cubes[0, 2]);
                setRGYCorner(redFace.cubes[2, 2]);
                setRGWCorner(redFace.cubes[2, 0]);
                setRBWCorner(tempCorner);

                //Edges               
                setRYEdge(redFace.cubes[1, 2]);
                setRGEdge(redFace.cubes[2, 1]);
                setRWEdge(redFace.cubes[1, 0]);
                setRBEdge(tempEdge);
            }
        }

        private void handleWhiteRotationRemapping()
        {
            var tempCorner = whiteFace.cubes[0, 0];
            var tempEdge = whiteFace.cubes[0, 1];

            if (rotationDirection == RotationDirection.Clockwise)
            {
                //Corners
                setRBWCorner(whiteFace.cubes[2, 0]);
                setOBWCorner(whiteFace.cubes[2, 2]);
                setOGWCorner(whiteFace.cubes[0, 2]);
                setRGWCorner(tempCorner);

                //Edges
                setRWEdge(whiteFace.cubes[1, 0]);
                setBWEdge(whiteFace.cubes[2, 1]);
                setOWEdge(whiteFace.cubes[1, 2]);
                setGWEdge(tempEdge);
            }
            else
            {
                //Corners
                setRBWCorner(whiteFace.cubes[0, 2]);
                setRGWCorner(whiteFace.cubes[2, 2]);
                setOGWCorner(whiteFace.cubes[2, 0]);
                setOBWCorner(tempCorner);

                //Edges
                setRWEdge(whiteFace.cubes[1, 2]);
                setGWEdge(whiteFace.cubes[2, 1]);
                setOWEdge(whiteFace.cubes[1, 0]);
                setBWEdge(tempEdge);
            }
        }

        private void handleYellowRotationRemapping()
        {
            var tempCorner = yellowFace.cubes[0, 0];
            var tempEdge = yellowFace.cubes[0, 1];

            if (rotationDirection == RotationDirection.Clockwise)
            {
                //Corners
                setOBYCorner(yellowFace.cubes[2, 0]);
                setRBYCorner(yellowFace.cubes[2, 2]);
                setRGYCorner(yellowFace.cubes[0, 2]);
                setOGYCorner(tempCorner);

                //Edges
                setOYEdge(yellowFace.cubes[1, 0]);
                setBYEdge(yellowFace.cubes[2, 1]);
                setRYEdge(yellowFace.cubes[1, 2]);
                setGYEdge(tempEdge);
            }
            else
            {
                //Corners
                setOBYCorner(yellowFace.cubes[0, 2]);
                setOGYCorner(yellowFace.cubes[2, 2]);
                setRGYCorner(yellowFace.cubes[2, 0]);
                setRBYCorner(tempCorner);

                //Edges
                setOYEdge(yellowFace.cubes[1, 2]);
                setGYEdge(yellowFace.cubes[2, 1]);
                setRYEdge(yellowFace.cubes[1, 0]);
                setBYEdge(tempEdge);
            }
        }

        private void setOBWCorner(GameObject corner)
        {
            orangeFace.cubes[2, 2] = blueFace.cubes[2, 0] = whiteFace.cubes[2, 0] = corner;
        }

        private void setOBYCorner(GameObject corner)
        {
            orangeFace.cubes[0, 2] = blueFace.cubes[0, 0] = yellowFace.cubes[0, 0] = corner;
        }

        private void setOGWCorner(GameObject corner)
        {
            orangeFace.cubes[2, 0] = greenFace.cubes[2, 2] = whiteFace.cubes[2, 2] = corner;          
        }

        private void setOGYCorner(GameObject corner)
        {
            orangeFace.cubes[0, 0] = greenFace.cubes[0, 2] = yellowFace.cubes[0, 2] = corner;
        }

        private void setRBWCorner(GameObject corner)
        {
            redFace.cubes[2, 0] = blueFace.cubes[2, 2] = whiteFace.cubes[0, 0] = corner;
        }

        private void setRBYCorner(GameObject corner)
        {
            redFace.cubes[0, 0] = blueFace.cubes[0, 2] = yellowFace.cubes[2, 0] = corner;
        }

        private void setRGWCorner(GameObject corner)
        {
            redFace.cubes[2, 2] = greenFace.cubes[2, 0] = whiteFace.cubes[0, 2] = corner;
        }

        private void setRGYCorner(GameObject corner)
        {
            redFace.cubes[0, 2] = greenFace.cubes[0, 0] = yellowFace.cubes[2, 2] = corner;
        }

        private void setBWEdge(GameObject edge)
        {
            blueFace.cubes[2, 1] = whiteFace.cubes[1, 0] = edge;
        }

        private void setBYEdge(GameObject edge)
        {
            blueFace.cubes[0, 1] = yellowFace.cubes[1, 0] = edge;
        }

        private void setGWEdge(GameObject edge)
        {
            greenFace.cubes[2, 1] = whiteFace.cubes[1, 2] = edge;
        }

        private void setGYEdge(GameObject edge)
        {
            greenFace.cubes[0, 1] = yellowFace.cubes[1, 2] = edge;
        }

        private void setOBEdge(GameObject edge)
        {
            orangeFace.cubes[1, 2] = blueFace.cubes[1, 0] = edge;
        }

        private void setOGEdge(GameObject edge)
        {
            orangeFace.cubes[1, 0] = greenFace.cubes[1, 2] = edge;
        }

        private void setOWEdge(GameObject edge)
        {
            orangeFace.cubes[2, 1] = whiteFace.cubes[2, 1] = edge;
        }

        private void setOYEdge(GameObject edge)
        {
            orangeFace.cubes[0, 1] = yellowFace.cubes[0, 1] = edge;
        }

        private void setRBEdge(GameObject edge)
        {
            redFace.cubes[1, 0] = blueFace.cubes[1, 2] = edge;
        }

        private void setRGEdge(GameObject edge)
        {
            redFace.cubes[1, 2] = greenFace.cubes[1, 0] = edge;
        }

        private void setRWEdge(GameObject edge)
        {
            redFace.cubes[2, 1] = whiteFace.cubes[0, 1] = edge;
        }

        private void setRYEdge(GameObject edge)
        {
            redFace.cubes[0, 1] = yellowFace.cubes[2, 1] = edge;
        }
    }
}
