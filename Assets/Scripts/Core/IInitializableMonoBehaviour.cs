using System;

namespace Core
{
    public interface IInitializableMonoBehaviour<in TInputParams> : ICustomDisposable
    {
        IDisposable Init(TInputParams inputParams);
    }
}