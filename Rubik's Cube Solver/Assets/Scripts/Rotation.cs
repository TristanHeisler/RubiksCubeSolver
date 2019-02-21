using Rubiks.Enums;

namespace Rubiks
{
    public class Rotation
    {
        public Rotation(FaceColor faceColor, RotationDirection direction)
        {
            FaceColor = faceColor;
            Direction = direction;
        }

        public FaceColor FaceColor;
        public RotationDirection Direction;
    }
}
