from Src.Models.Statuses import progress_status
from Src.Models.Executor import executor
from Src.Models.Contractor import contractor
from Src.Models.Period import period
from Src.Services.Helper import helper
from Src.Models.Guid import guid
from Src.Models.Building import building

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
    __building = None
    __comments = ""

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
    def period(self):
        """
        Свойство: Дата документа
        """
        return self.__period    

    @period.setter    
    def period(self, value):
        """
        Свойство: Дата документа
        """
        if value is None:
            raise Exception("ОШИБКА! параметр period - не указан!")
        
        if not isinstance(value, period):
            raise Exception("ОШИБКА! Некорректно указан параметр period!")
        
        self.__period = value

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
    def id(self):
        """
        Свойство. Уникальный номер документа
        """
        return self.__guid    

    # Данное свойство не меняется в акте
    @property
    def executor(self):
        """
        Свойство: Исполнитель    
        """
        return self.__executor


    @property    
    def period(self):
        """
        Свойство: Дата и время создания документа
        """
        return self.__period   

    @period.setter
    def period(self, value):
        """
        Свойство: Дата и время создания документа
        """
        if value is None:
            raise Exception("ОШИБКА! Не указано поле period!") 
        
        if not isinstance(value, period):
            raise Exception("ОШИБКА! Не указано поле period!") 
        
        self.__period = value
        
    @property
    def contractors(self):
        """
        Свойство: Список застройщиков   
        """
        return list(self.__contractors.values())
    
    # Данное свойство не меняется в акте 
    @property
    def building(self):
        """
        Свойство: Объект капитального строительства
        """
        return self.__building
    
    @property
    def comments(self):
        """
        Свойство: Краткий комментарий к акту
        """
        return self.__comments
    
    @comments.setter
    def comments(self, value):
        """
        Свойство: Краткий комментарий к акту
        """
        if value is None:
            return
        
        self.__comments = value
        

    def create(_executor, _building):
        """
        Фабричный метод    
        """

        if _building is None:
            raise Exception("ОШИБКА! Параметр _building - не указан!")
        
        if not isinstance(_building, building):
            raise Exception("ОШИБКА! Параметр _building - долже быть типом  building!")
        
        if _executor is None:
            raise Exception("ОШИБКА! Параметр executor - не указан!")
        
        if not isinstance(_executor, executor):
            raise Exception("ОШИБКА! Параметр executor - должен быть типом executor!")

        result = act()
        result.__executor = _executor
        result.add(_executor.contraсtor)
        result.__building = _building

        result.__guid = guid()

        return result
        

    def add(self, _contractor):
        """
         Добавить в документ застройщиков
        """
        if _contractor is None:
            return
        
        if not isinstance(_contractor, contractor):
            raise Exception("ОШИБКА! Параметр _contractor - должен быть типом contractor!")
        
        find_result =  list(filter( lambda x:x == _contractor.id, self.__contractors))
        if len(find_result) == 0:
            self.__contractors[_contractor.id] = _contractor
            
        if not _contractor.parent is None:
            self.add(_contractor.parent)


    def toJSON(self):
        """
        Сериализовать объект в Json
        """
        items = helper.toDict(self)
        return json.dumps(items, sort_keys = True, indent = 4)   
    
    def __str__(self):
        """
        Сформировать SQL запрос на вставку данных
        """

        # Заголовок документа
        sql = "insert into acts(id, building_id, executor_id, period) values('%s', '%s', '%s', '%s');\n" % (self.id.toJSON(), self.building.id.toJSON(), self.executor.id.toJSON(), self.period.toJSON())

        currect_period = period()

        # Прогресс
        sql += "insert into acts_status_links (id , period, status_code, comments, amount) values('%s', '%s', %s, '%s', %s);\n" % (self.id.toJSON(), currect_period.toJSON(), self.__progress, self.__comments, self.amount)

        # Связка с застройщиками
        for key in self.__contractors:
            sql += "insert into acts_contractors_links(id, period, contractor_id) values('%s', '%s', '%s');\n" % (self.id.toJSON(), currect_period.toJSON(), key.toJSON())

        return sql




    
    
    



        
        
