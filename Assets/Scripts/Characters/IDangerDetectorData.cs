using UniRx;

namespace Characters
{
    public interface IDangerDetectorData
    {
        IReactiveProperty<float> Radius { get; }
    }
}