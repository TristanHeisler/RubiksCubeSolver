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
                    { Corner_RGW, Edge_GW, Corner_OGW},
                    { Edge_RW, Middle_W, Edge_OW},
                    { Corner_RBW, Edge_BW, Corner_OBW}
                },
                rotationAxis = Vector3.down
            };

            yellowFace = new Face
            {
                cubes = new GameObject[,]
                {
                    { Corner_RBY, Edge_BY, Corner_OBY},
                    { Edge_RY, Middle_Y, Edge_OY},
                    { Corner_RGY, Edge_GY, Corner_OGY}
                },
                rotationAxis = Vector3.up
            };
        }

        public void SetRotationDirection(RotationDirection direction)
        {
            rotationDirection = direction;
        }

        public void SetRotatingFace(FaceColor rotatingFaceColor)
        {
            switch(rotatingFaceColor)
            {
                case FaceColor.Blue:
                    rotatingFace = blueFace;
                    break;
                case FaceColor.Green:
                    rotatingFace = greenFace;
                    break;
                case FaceColor.Red:
                    rotatingFace = redFace;
                    break;
                case FaceColor.Orange:
                    rotatingFace = orangeFace;
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
    }
}
