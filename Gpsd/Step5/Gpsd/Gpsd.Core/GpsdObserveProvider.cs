using Gpsd.Core.Models;

namespace Gpsd.Core;

/// <summary>
/// Класс - провайдер для работы с подписками на оповещение
/// </summary>
public class GpsdObserveProvider: IObservable<IGpsDataModel>
{
    // Список объектов с которым требуется вести оповещение
    private readonly List<IObserver<IGpsDataModel>> _observers = new List<IObserver<IGpsDataModel>>();
    
    /// <summary>
    /// Добавить объект в подписку на обновление
    /// </summary>
    /// <param name="observer"></param>
    /// <returns></returns>
    public IDisposable Subscribe(IObserver<IGpsDataModel> observer)
    {
        if (! _observers.Contains(observer))
            _observers.Add(observer);
        
        return new GpsdUnsubscriper(_observers, observer);
    }
    

    /// <summary>
    /// Выполнить оповещение всех связанных подписщиков для обработки данных новой сущности
    /// </summary>
    /// <param name="model"> Новая модель </param>
    public void Push(IGpsDataModel? model)
    {
        foreach (var observer in _observers)
            if (model != null) observer.OnNext(model);
    }
}