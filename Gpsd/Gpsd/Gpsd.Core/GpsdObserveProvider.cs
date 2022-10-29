using Gpsd.Core.Models;

namespace Gpsd.Core;

/// <summary>
/// Класс - провайдер для работы с подписками на обновление
/// </summary>
public class GpsdObserveProvider: IObservable<IGpsDataModel>
{
    private readonly List<IObserver<IGpsDataModel>> _observers = new List<IObserver<IGpsDataModel>>();

    #region Private
    
    private class Unsubscriber : IDisposable
    {
        private List<IObserver<IGpsDataModel>> _observers;
        private IObserver<IGpsDataModel> _observer;
        public Unsubscriber(List<IObserver<IGpsDataModel>> observers, IObserver<IGpsDataModel> observer)
        {
            this._observers = observers;
            this._observer = observer;
        }

        public void Dispose()
        {
            if (_observer != null && _observers.Contains(_observer))
                _observers.Remove(_observer);
        }
    }

    #endregion
    
    /// <summary>
    /// Добавить объект в подписку на обновление
    /// </summary>
    /// <param name="observer"></param>
    /// <returns></returns>
    public IDisposable Subscribe(IObserver<IGpsDataModel> observer)
    {
        if (! _observers.Contains(observer))
            _observers.Add(observer);
        return new Unsubscriber(_observers, observer);
    }
    

    /// <summary>
    /// Выполнить оповещение всех связанных подписщиков для обработки данных новой сущности
    /// </summary>
    /// <param name="model"> Новая модель </param>
    public void Track(IGpsDataModel? model)
    {
        foreach (var observer in _observers)
            if (model != null) observer.OnNext(model);
    }
}