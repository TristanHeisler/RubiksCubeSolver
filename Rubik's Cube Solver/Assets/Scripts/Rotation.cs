using Rubiks.Enums;
using System;

namespace Rubiks
{
    [Serializable]
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
