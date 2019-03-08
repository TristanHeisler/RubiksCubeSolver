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
        private GameObject Corner_OBW;
        private GameObject Corner_OBY;
        private GameObject Corner_OGW;
        private GameObject Corner_OGY;
        private GameObject Corner_RBW;
        private GameObject Corner_RBY;
        private GameObject Corner_RGW;
        private GameObject Corner_RGY;

        //12 Edge Cubes
        private GameObject Edge_BW;
        private GameObject Edge_BY;
        private GameObject Edge_GW;
        private GameObject Edge_GY;
        private GameObject Edge_OB;
        private GameObject Edge_OG;
        private GameObject Edge_OW;
        private GameObject Edge_OY;
        private GameObject Edge_RB;
        private GameObject Edge_RG;
        private GameObject Edge_RW;
        private GameObject Edge_RY;

        //6 Middle Cubes
        private GameObject Middle_B;
        private GameObject Middle_G;
        private GameObject Middle_O;
        private GameObject Middle_R;
        private GameObject Middle_W;
        private GameObject Middle_Y;

        //Objects to represent the current state of each cube face
        private Face blueFace, greenFace, orangeFace, redFace, whiteFace, yellowFace;

        //Variables for rotating the cube faces
        public FaceColor rotatingFaceColor;
        private Face rotatingFace;
        public RotationDirection rotationDirection;

        //Represent the internal state of the cube
        private CubeState state;

        public void Initialize()
        {
            initializeCubits();
            initializeFaces();
            initializeState();
        }

        private void initializeCubits()
        {
            Corner_OBW = GameObject.Find("Corner_OBW");
            Corner_OBY = GameObject.Find("Corner_OBY");
            Corner_OGW = GameObject.Find("Corner_OGW");
            Corner_OGY = GameObject.Find("Corner_OGY");
            Corner_RBW = GameObject.Find("Corner_RBW");
            Corner_RBY = GameObject.Find("Corner_RBY");
            Corner_RGW = GameObject.Find("Corner_RGW");
            Corner_RGY = GameObject.Find("Corner_RGY");
            Edge_BW = GameObject.Find("Edge_BW");
            Edge_BY = GameObject.Find("Edge_BY");
            Edge_GW = GameObject.Find("Edge_GW");
            Edge_GY = GameObject.Find("Edge_GY");
            Edge_OB = GameObject.Find("Edge_OB");
            Edge_OG = GameObject.Find("Edge_OG");
            Edge_OW = GameObject.Find("Edge_OW");
            Edge_OY = GameObject.Find("Edge_OY");
            Edge_RB = GameObject.Find("Edge_RB");
            Edge_RG = GameObject.Find("Edge_RG");
            Edge_RW = GameObject.Find("Edge_RW");
            Edge_RY = GameObject.Find("Edge_RY");
            Middle_B = GameObject.Find("Middle_B");
            Middle_G = GameObject.Find("Middle_G");
            Middle_O = GameObject.Find("Middle_O");
            Middle_R = GameObject.Find("Middle_R");
            Middle_W = GameObject.Find("Middle_W");
            Middle_Y = GameObject.Find("Middle_Y");
        }

        private void initializeFaces()
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

        private void initializeState()
        {
            state = new CubeState();
        }

        public bool IsSolved()
        {
            return state.IsSolved();
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

        public void UpdateState(FaceColor rotatingFace, RotationDirection direction)
        {
            state.Rotate(rotatingFace, direction);
        }

        public CubeState GetState()
        {
            return state;
        }

        public void RotateCubeFace()
        {
            int rotationDegrees = (rotationDirection == RotationDirection.Clockwise) ? ROTATION_SPEED : -ROTATION_SPEED;

            //Rotate all cubes associated with the rotating face around the appropriate axis
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    rotatingFace.cubes[i, j].transform.RotateAround(Vector3.zero, rotatingFace.rotationAxis, rotationDegrees);
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
