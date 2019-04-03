using System.Collections.Generic;

namespace Rubiks.Solvers
{
    public interface ISolver
    {
        Stack<Rotation> Solve();
    }
}
