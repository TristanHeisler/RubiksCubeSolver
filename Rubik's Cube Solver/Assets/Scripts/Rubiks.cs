using UnityEngine;

public class Rubiks : MonoBehaviour
{
    //Constants
    private const int ROTATION_SPEED = 10;
    private const int FRAMES_PER_ROTATION = 90 / ROTATION_SPEED;

    //Enums
    private enum RotationDirection
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

    //Class to represent a face of the cube
    private class CubeFace
    {
        public GameObject[,] cubes;
        public Vector3 rotationAxis;
    }

    //Variables to record the current state of each cube face
    private CubeFace BlueFace, GreenFace, OrangeFace, RedFace, WhiteFace, YellowFace;

    //Variables concerning cube face rotation
    private bool isRotating = false;
    private CubeFace rotatingFace;
    private RotationDirection rotationDirection;
    private int remainingRotationFrames;

    // Use this for initialization
    void Start()
    {
        InitializeCubeFaces();        
    }

    private void InitializeCubeFaces()
    {
        BlueFace = new CubeFace
        {
            cubes = new GameObject[,]
            {
                { Corner_OBY, Edge_BY, Corner_RBY},
                { Edge_OB, Middle_B, Edge_RB},
                { Corner_OBW, Edge_BW, Corner_RBW}
            },
            rotationAxis = Vector3.left
        };

        GreenFace = new CubeFace
        {
            cubes = new GameObject[,]
            {
                { Corner_RGY, Edge_GY, Corner_OGY},
                { Edge_RG, Middle_G, Edge_OG},
                { Corner_RGW, Edge_GW, Corner_OGW}
            },
            rotationAxis = Vector3.right
        };

        OrangeFace = new CubeFace
        {
            cubes = new GameObject[,]
            {
                { Corner_OGY, Edge_OY, Corner_OBY},
                { Edge_OG, Middle_O, Edge_OB},
                { Corner_OGW, Edge_OW, Corner_OBW}
            },
            rotationAxis = Vector3.forward
        };

        RedFace = new CubeFace
        {
            cubes = new GameObject[,]
            {
                { Corner_RBY, Edge_RY, Corner_RGY},
                { Edge_RB, Middle_R, Edge_RG},
                { Corner_RBW, Edge_RW, Corner_RGW}
            },
            rotationAxis = Vector3.back
        };

        WhiteFace = new CubeFace
        {
            cubes = new GameObject[,]
            {
                { Corner_RGW, Edge_GW, Corner_OGW},
                { Edge_RW, Middle_W, Edge_OW},
                { Corner_RBW, Edge_BW, Corner_OBW}
            },
            rotationAxis = Vector3.down
        };

        YellowFace = new CubeFace
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
	
	// Update is called once per frame
	void Update()
    {
        //If a rotation is already occuring, simply continue rotating the appropriate face
        if(isRotating)
        {
            RotateCubeFace();
        }
        //Otherwise, handle any potential rotation inputs
        else if(RotationKeyWasPressed())
        {
            //Indicate that a rotation is now occuring
            isRotating = true;
            remainingRotationFrames = FRAMES_PER_ROTATION;

            //If the shift key is pressed, the turn is counterclockwise. Otherwise, it is clockwise
            rotationDirection = Input.GetKey(KeyCode.LeftShift) ? RotationDirection.Counterclockwise : RotationDirection.Clockwise;

            //Blue Face Rotation
            if (Input.GetKeyDown(KeyCode.B))
            {
                rotatingFace = BlueFace;
            }
            //Green Face Rotation
            else if (Input.GetKeyDown(KeyCode.G))
            {
                rotatingFace = GreenFace;
            }
            //Red Face Rotation
            else if (Input.GetKeyDown(KeyCode.R))
            {
                rotatingFace = RedFace;
            }
            //Orange Face Rotation
            else if (Input.GetKeyDown(KeyCode.O))
            {
                rotatingFace = OrangeFace;
            }
            //White Face Rotation
            else if (Input.GetKeyDown(KeyCode.W))
            {
                rotatingFace = WhiteFace;
            }
            //Yellow Face Rotation
            else if (Input.GetKeyDown(KeyCode.Y))
            {
                rotatingFace = YellowFace;
            }
        }     
	}

    private bool RotationKeyWasPressed()
    {
        return Input.GetKeyDown(KeyCode.B) || Input.GetKeyDown(KeyCode.G) || Input.GetKeyDown(KeyCode.R) ||
            Input.GetKeyDown(KeyCode.O) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Y);
    }

    private void RotateCubeFace()
    {
        //Rotate all cubes associated with the rotating face around the appropriate axis
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                rotatingFace.cubes[i, j].transform.RotateAround(Vector3.zero, rotatingFace.rotationAxis, ROTATION_SPEED * (int)rotationDirection);
            }
        }   

        //Decrement the number of frames remaining to complete the rotation
        remainingRotationFrames--;

        //If the rotation has completed, set the rotation flag to false to allow another rotation to begin
        if(remainingRotationFrames == 0)
        {
            isRotating = false;
        }
    }
}
