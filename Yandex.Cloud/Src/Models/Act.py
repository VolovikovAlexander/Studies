from Src.Models.Statuses import progress_status
from Src.Models.Executor import executor
from Src.Models.Contractor import contractor
from Src.Models.Period import period
from Src.Services.Helper import helper
from Src.Models.Guid import guid

import json


"""
# Модель документа /Акт проверки/
"""
class act():
    __amount = 0
    __executor = None
    __contractors = {}
    __progress = None
    __period = None
    __guid = ""

    def __init__(self):
        self.__progress = progress_status.start
        self.__period = period()


    @property
    def amount(self):
        """
        Свойство: Сумма штрафа    
        """
        return self.__amount
    
    @amount.setter
    def amount(self, value):
        """
        Свойство: Сумма штрафа    
        """
        if value is None:
            raise Exception("ОШИБКА! Параметр amount - не указан!")
        
        self.__amount = value

    @property
    def progress(self):
        """
        Свойство: Статус - прогресс
        """
        return self.__progress

    @progress.setter
    def progress(self, value):
        if value is None:
            raise Exception("ОШИБКА! Параметр progress - не указан!")

        if not isinstance(value, progress_status):
            raise Exception("ОШИБКА! Параметр progress, некорректно указан!")    

        self.__progress = value    


    @property
    def uid(self):
        """
        Свойство. Уникальный номер документа
        """
        return self.__guid    

    @property
    def executor(self):
        """
        Свойство: Исполнитель    
        """
        return self.__executor

    @executor.setter
    def executor(self, value):
        """
        Свойство: Исполнитель    
        """
        if value is None:
            raise Exception("ОШИБКА! Параметр executor - не указан!")
        
        if not isinstance(value, executor):
            raise Exception("ОШИБКА! Параметр executor - должен быть типом executor!")
        
        self.__executor = value
        

    @property    
    def period(self):
        """
        Дата и время создания документа
        """
        return self.__period    
        
    @property
    def contractors(self):
        """
        Свойство: Список застройщиков   
        """
        return list(self.__contractors.values())
        

    def create(_executor):
        """
        Фабричный метод    
        """
        result = act()
        result.executor = _executor
        result.add(_executor.contraсtor)

        result.__guid = guid()

        return result
        

    def add(self, _contractor):
        """
         Добавить в документ исполнителей
        """
        if _contractor is None:
            return
        
        if not isinstance(_contractor, contractor):
            raise Exception("ОШИБКА! Параметр _contractor - должен быть типом contractor!")
        
        self.__contractors[_contractor.id] = _contractor
        self.add(_contractor.parent)


    def toJSON(self):
        """
        Сериализовать объект в Json
        """
        items = helper.toDict(self)
        return json.dumps(items, sort_keys = True, indent = 4)   

    
    
    



        
        
