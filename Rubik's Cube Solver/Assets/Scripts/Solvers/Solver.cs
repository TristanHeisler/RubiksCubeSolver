using System.Collections.Generic;

namespace Rubiks.Solvers
{
    public interface Solver
    {
        Stack<Rotation> Solve();
    }
}
