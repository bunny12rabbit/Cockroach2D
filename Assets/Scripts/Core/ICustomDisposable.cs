using System;

namespace Core
{
    public interface ICustomDisposable : IDisposable
    {
        Action OnDispose { get; set; }
    }
}