using Gpsd.Core.Models;

namespace Gpsd.Core;

/// <summary>
/// Класс обвертка для работы с оповещением для модели <see cref="IGpsDataModel"/>
/// </summary>
public class GpsdObserverVersion: IObserver<IGpsDataModel>
{
    private GpsdVersionModel _model;
 
    public void OnCompleted()
    {
    }

    public void OnError(Exception error)
    {
    }

    public void OnNext(IGpsDataModel value)
    {
        if(value is GpsdVersionModel)
            _model = (value as GpsdVersionModel)!;
    }
}