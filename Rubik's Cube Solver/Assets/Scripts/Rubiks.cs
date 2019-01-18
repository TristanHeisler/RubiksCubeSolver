using UnityEngine;

public class Rubiks : MonoBehaviour {
    //Constants
    private const int ROTATION_SPEED = 10;
    private const int FRAMES_PER_ROTATION = 90 / ROTATION_SPEED;

    //Enums
    private enum Face
    {
        Blue,
        Green,
        Orange,
        Red,
        White,
        Yellow
    }

    private enum Rotation
    {
        Clockwise = 1,
        Counterclockwise = -1
    }    

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

    //Variables concerning cube face rotation
    private Face currentFace;
    private Rotation currentRotation;
    private bool isRotating = false;
    private int remainingRotationFrames;

    //Variables to record the current state of each cube face
    private GameObject[,] BlueFace, GreenFace, OrangeFace, RedFace, WhiteFace, YellowFace;

    // Use this for initialization
    void Start ()
    {
        InitializeCubeFaces();        
    }

    private void InitializeCubeFaces()
    {
        BlueFace = new GameObject[3, 3] {
            { Corner_OBY, Edge_BY, Corner_RBY},
            { Edge_OB, Middle_B, Edge_RB},
            { Corner_OBW, Edge_BW, Corner_RBW}
        };

        GreenFace = new GameObject[3, 3] {
            { Corner_RGY, Edge_GY, Corner_OGY},
            { Edge_RG, Middle_G, Edge_OG},
            { Corner_RGW, Edge_GW, Corner_OGW}
        };

        OrangeFace = new GameObject[3, 3] {
            { Corner_OGY, Edge_OY, Corner_OBY},
            { Edge_OG, Middle_O, Edge_OB},
            { Corner_OGW, Edge_OW, Corner_OBW}
        };

        RedFace = new GameObject[3, 3] {
            { Corner_RBY, Edge_RY, Corner_RGY},
            { Edge_RB, Middle_R, Edge_RG},
            { Corner_RBW, Edge_RW, Corner_RGW}
        };

        WhiteFace = new GameObject[3, 3] {
            { Corner_RGW, Edge_GW, Corner_OGW},
            { Edge_RW, Middle_W, Edge_OW},
            { Corner_RBW, Edge_BW, Corner_OBW}
        };

        YellowFace = new GameObject[3, 3] {
            { Corner_RBY, Edge_BY, Corner_OBY},
            { Edge_RY, Middle_Y, Edge_OY},
            { Corner_RGY, Edge_GY, Corner_OGY}
        };
    }
	
	// Update is called once per frame
	void Update ()
    {
        //Handle rotation inputs if no current rotations are happening
        if (!isRotating)
        {
            //Blue
            if (Input.GetKeyDown(KeyCode.B))
            {
                isRotating = true;
                remainingRotationFrames = FRAMES_PER_ROTATION;
                currentFace = Face.Blue;

                if (Input.GetKey(KeyCode.LeftShift))
                {
                    currentRotation = Rotation.Counterclockwise;
                }
                else
                {
                    currentRotation = Rotation.Clockwise;
                }
            }

            //Green
            if (Input.GetKeyDown(KeyCode.G))
            {
                isRotating = true;
                remainingRotationFrames = FRAMES_PER_ROTATION;
                currentFace = Face.Green;

                if (Input.GetKey(KeyCode.LeftShift))
                {
                    currentRotation = Rotation.Counterclockwise;
                }
                else
                {
                    currentRotation = Rotation.Clockwise;
                }
            }

            //Orange
            if (Input.GetKeyDown(KeyCode.O))
            {
                isRotating = true;
                remainingRotationFrames = FRAMES_PER_ROTATION;
                currentFace = Face.Orange;

                if (Input.GetKey(KeyCode.LeftShift))
                {
                    currentRotation = Rotation.Counterclockwise;
                }
                else
                {
                    currentRotation = Rotation.Clockwise;
                }
            }

            //Red
            if (Input.GetKeyDown(KeyCode.R))
            {
                isRotating = true;
                remainingRotationFrames = FRAMES_PER_ROTATION;
                currentFace = Face.Red;

                if (Input.GetKey(KeyCode.LeftShift))
                {
                    currentRotation = Rotation.Counterclockwise;
                }
                else
                {
                    currentRotation = Rotation.Clockwise;
                }
            }

            //White
            if (Input.GetKeyDown(KeyCode.W))
            {
                isRotating = true;
                remainingRotationFrames = FRAMES_PER_ROTATION;
                currentFace = Face.White;

                if (Input.GetKey(KeyCode.LeftShift))
                {
                    currentRotation = Rotation.Counterclockwise;
                }
                else
                {
                    currentRotation = Rotation.Clockwise;
                }
            }

            //Yellow
            if (Input.GetKeyDown(KeyCode.Y))
            {
                isRotating = true;
                remainingRotationFrames = FRAMES_PER_ROTATION;
                currentFace = Face.Yellow;

                if(Input.GetKey(KeyCode.LeftShift))
                {
                    currentRotation = Rotation.Counterclockwise;
                }
                else
                {
                    currentRotation = Rotation.Clockwise;
                }                
            }
        }        

        //Otherwise, continue the current rotation
        else
        {
            Rotate(currentFace, currentRotation);
        }        
	}

    private void Rotate(Face face, Rotation rotationDirection)
    {
        //Blue
        if (face == Face.Blue)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    BlueFace[i, j].transform.RotateAround(Vector3.zero, Vector3.left, ROTATION_SPEED * (int)rotationDirection);
                }
            }
        }

        //Green
        if (face == Face.Green)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    GreenFace[i, j].transform.RotateAround(Vector3.zero, Vector3.right, ROTATION_SPEED * (int)rotationDirection);
                }
            }
        }

        //Orange
        if (face == Face.Orange)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    OrangeFace[i, j].transform.RotateAround(Vector3.zero, Vector3.forward, ROTATION_SPEED * (int)rotationDirection);
                }
            }
        }

        //Red
        if (face == Face.Red)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    RedFace[i, j].transform.RotateAround(Vector3.zero, Vector3.back, ROTATION_SPEED * (int)rotationDirection);
                }
            }
        }

        //White
        if (face == Face.White)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    WhiteFace[i, j].transform.RotateAround(Vector3.zero, Vector3.down, ROTATION_SPEED * (int)rotationDirection);
                }
            }
        }

        //Yellow
        if (face == Face.Yellow)
        {
            for(int i = 0; i < 3; i++)
            {
                for(int j = 0; j < 3; j++)
                {
                    YellowFace[i, j].transform.RotateAround(Vector3.zero, Vector3.up, ROTATION_SPEED * (int)rotationDirection);
                }
            }            
        }        

        remainingRotationFrames--;

        if(remainingRotationFrames == 0)
        {
            isRotating = false;
        }
    }
}
