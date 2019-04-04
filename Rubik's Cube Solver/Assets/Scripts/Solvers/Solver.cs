using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rubiks.Solvers
{
    public interface ISolver
    {
        Task<Stack<Rotation>> Solve();
    }
}
