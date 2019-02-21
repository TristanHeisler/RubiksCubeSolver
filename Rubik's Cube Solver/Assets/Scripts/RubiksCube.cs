using Rubiks;
using Rubiks.Enums;
using UnityEngine;

public class RubiksCube : MonoBehaviour
{
    public const int FRAMES_PER_ROTATION = 90 / Cube.ROTATION_SPEED;

    //The cube to be manipulated
    private Cube rubiksCube;

    //Variables concerning cube face rotation
    private bool isRotating = false;
    private int remainingRotationFrames;

    // Use this for initialization
    void Start()
    {
        rubiksCube = GetComponent<Cube>();
        rubiksCube.InitializeFaces();        
    }
	
	// Update is called once per frame
	void Update()
    {
        //If a rotation is already occuring, simply continue rotating the appropriate face
        if(isRotating)
        {
            //Continue rotating
            rubiksCube.GetComponent<Cube>().RotateCubeFace();

            //Decrement the number of frames remaining to complete the rotation
            remainingRotationFrames--;

            //If the rotation has completed, set the rotation flag to false to allow another rotation to begin
            if (remainingRotationFrames == 0)
            {
                isRotating = false;

                //Additionally, perform the appropriate cube face remappings
                rubiksCube.HandleRotationRemapping();
            }
        }
        //Otherwise, handle any potential rotation inputs
        else if(RotationKeyWasPressed())
        {
            //Indicate that a rotation is now occuring
            isRotating = true;
            remainingRotationFrames = FRAMES_PER_ROTATION;

            //If the shift key is pressed, the turn is counterclockwise. Otherwise, it is clockwise
            var rotationDirection = Input.GetKey(KeyCode.LeftShift) ? RotationDirection.Counterclockwise : RotationDirection.Clockwise;
            rubiksCube.SetRotationDirection(rotationDirection);

            //Blue Face Rotation
            if (Input.GetKeyDown(KeyCode.B))
            {
                rubiksCube.SetRotatingFace(FaceColor.Blue);
            }
            //Green Face Rotation
            else if (Input.GetKeyDown(KeyCode.G))
            {
                rubiksCube.SetRotatingFace(FaceColor.Green);
            }
            //Red Face Rotation
            else if (Input.GetKeyDown(KeyCode.R))
            {
                rubiksCube.SetRotatingFace(FaceColor.Red);
            }
            //Orange Face Rotation
            else if (Input.GetKeyDown(KeyCode.O))
            {
                rubiksCube.SetRotatingFace(FaceColor.Orange);
            }
            //White Face Rotation
            else if (Input.GetKeyDown(KeyCode.W))
            {
                rubiksCube.SetRotatingFace(FaceColor.White);
            }
            //Yellow Face Rotation
            else if (Input.GetKeyDown(KeyCode.Y))
            {
                rubiksCube.SetRotatingFace(FaceColor.Yellow);
            }
        }     
	}

    private bool RotationKeyWasPressed()
    {
        return Input.GetKeyDown(KeyCode.B) || Input.GetKeyDown(KeyCode.G) || Input.GetKeyDown(KeyCode.R) ||
            Input.GetKeyDown(KeyCode.O) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Y);
    }
}
