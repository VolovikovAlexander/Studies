from Src.Models.Statuses import progress_status
from Src.Models.Executor import executor
from Src.Models.Contractor import contractor
from datetime import datetime


#
# Модель документа /Акт проверки/
#
class act():
    __amount = 0
    __executor = None
    __contractors = {}
    __progress = None
    __period = None

    def __init__(self):
        self.__progress = progress_status.start
        self.__period = datetime.now()


    @property
    def amount(self):
        # Свойство: Сумма штрафа    
        return self.__amount
    
    @amount.setter
    def amount(self, value):
        # Свойство: Сумма штрафа    
        if value is None:
            raise Exception("ОШИБКА! Параметр amount - не указан!")
        
        self.__amount = value


    @property
    def executor(self):
        # Свойство: Исполнитель    
        return self.__executor

    def executor(self, value):
        # Свойство: Исполнитель    
        if value is None:
            raise Exception("ОШИБКА! Параметр executor - не указан!")
        
        if not isinstance(value, executor):
            raise Exception("ОШИБКА! Параметр executor - должен быть типом executor!")
        

    @property    
    def period(self):
        # Дата и время создания документа
        return self.__period    
        

    def contractors(self):
        # Свойство: Список застройщиков   
        items = list(self.__contractors)
        return items
        

    def create(_executor):
        # Фабричный метод    
        result = act()
        result.executor = _executor
        result.add(_executor.contraсtor)

        return result
        

    def add(self, _contractor):
        # Добавить в документ исполнителей
        if _contractor is None:
            return
        
        if not isinstance(_contractor, contractor):
            raise Exception("ОШИБКА! Параметр _contractor - должен быть типом contractor!")
        
        self.__contractors[_contractor.guid] = _contractor
        self.add(_contractor.parent)

        
        





        
        
