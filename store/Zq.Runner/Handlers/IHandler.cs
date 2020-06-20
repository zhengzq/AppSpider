using System;
using System.Threading.Tasks;

namespace Zq.Runner.Handlers
{
    public interface IHandler :  IDisposable
    {
        Task Handle();
    }
}
