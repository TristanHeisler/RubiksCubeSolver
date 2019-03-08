using Rubiks.Enums;

namespace Rubiks
{
    public class Rotation
    {
        public FaceColor FaceColor;
        public RotationDirection Direction;

        public Rotation(FaceColor faceColor, RotationDirection direction)
        {
            FaceColor = faceColor;
            Direction = direction;
        }        
    }
}
