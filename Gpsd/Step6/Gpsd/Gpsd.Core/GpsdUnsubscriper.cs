using Gpsd.Core.Models;

namespace Gpsd.Core;

/// <summary>
/// Класс для управления отпиской от оповещения
/// </summary>
public class GpsdUnsubscriper : IDisposable
{
    private List<IObserver<IGpsDataModel>> _observers;
    private IObserver<IGpsDataModel> _observer;
    
    public GpsdUnsubscriper(List<IObserver<IGpsDataModel>> observers, IObserver<IGpsDataModel> observer)
    {
        _observers = observers;
        _observer = observer;
    }

    public void Dispose()
    {
        if (_observer is not null && _observers.Contains(_observer))
            _observers.Remove(_observer);
    }
}

