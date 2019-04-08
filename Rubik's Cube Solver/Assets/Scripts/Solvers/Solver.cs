using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rubiks.Solvers
{
    public interface ISolver
    {
        Task<Tuple<Stack<Rotation>, int, TimeSpan>> Solve();
    }
}
