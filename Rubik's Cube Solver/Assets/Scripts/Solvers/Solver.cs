using System.Collections.Generic;

namespace Rubiks.Solvers
{
    public interface Solver
    {
        Queue<Rotation> Solve();
    }
}
