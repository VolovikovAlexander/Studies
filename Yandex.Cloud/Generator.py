from Src.Models.Statuses import progress_status
from Src.Models.Building import building
from Src.Models.Contractor import contractor
from Src.Models.Executor import executor
from Src.Models.Act import act
from Src.Services.Proxy import db_proxy
from Src.Models.Period import period

import random

# Набор операций для создания произвольных данных 

class generator():
    __buildings = []
    __executors = []
    __contractors= []
    __proxy = None

    def __init__(self):
        self.__proxy = db_proxy()
        self.__proxy.create()
        self.__proxy.clear()

    def create_buildings(self, quantity):
        '''
        Сформировать в базе данных объекты капитального строительства (ОКС)
        '''
        if quantity is None:
            raise Exception("ОШИБКА! Не указано количество записей которое нужно сформировать!")
        
        if quantity < 1:
            raise Exception("ОШИБКА! Некорректно указано количество записей!")
        
        start_period = period()
        print("-> Генерация записей: buildings")
        print("Старт: %s" % start_period.toJSON())
        
        for position in range(quantity):
            _name = "building № %s" %  (position + 1)
            _building = building.create(name = _name)

            result = self.__proxy.execute(str(_building))
            if not result:
                print("ОШИБКА! %s" % (self.__proxy.error_text))
                return
            
            self.__buildings.append(_building)
            
        print("Сформировано успешно %s записей за %s сек." % (quantity, start_period.diff()))    

    def create_contractors(self, quantity):
        """
        Сформировать застройщиков в виде дерева
        """
        if quantity is None:
            raise Exception("ОШИБКА! Не указано количество записей которое нужно сформировать!")
        
        if quantity < 1:
            raise Exception("ОШИБКА! Некорректно указано количество записей!")
        
        start_period = period()
        print("-> Генерация записей: contractors")
        print("Старт: %s" % start_period.toJSON())

        for position in range(quantity):
            _name = "contractor № %s" %  (position + 1)
            _contractor = contractor.create(_name)

            if len(self.__contractors) > 0:
                _number = int(random.random() * len(self.__contractors))

                if _number > 1 and _number <=len(self.__contractors):
                    _parent = self.__contractors[_number]
                    _contractor = contractor.create(_name, parent=_parent)

            result = self.__proxy.execute(str(_contractor))
            if not result:
                print("ОШИБКА! %s" % (self.__proxy.error_text))
                return
            
            self.__contractors.append(_contractor)

        print("Сформировано успешно %s записей за %s сек." % (quantity, start_period.diff()))        

    def create_executors(self, quantity):
        """
        Сформировать исполнителей
        """
        if quantity is None:
            raise Exception("ОШИБКА! Не указано количество записей которое нужно сформировать!")
        
        if quantity < 1:
            raise Exception("ОШИБКА! Некорректно указано количество записей!")
        
        if len(self.__contractors) == 0:
            raise Exception("ОШИБКА! Необходимо сперва сформировать застройщиков")
        
        start_period = period()
        print("-> Генерация записей: executors")
        print("Старт: %s" % start_period.toJSON())

        for position in range(quantity):
            _name = "executor № %s" %  (position + 1)
            _number = int(random.random() * len(self.__contractors))
            if _number > 1 and _number <=len(self.__contractors):
                _contractor = self.__contractors[_number]
                _executor = executor.create(_name,_contractor)

                result = self.__proxy.execute(str(_executor))
                if not result:
                    print("ОШИБКА! %s" % (self.__proxy.error_text))
                    return
            
                self.__executors.append(_executor)

        print("Сформировано успешно %s записей за %s сек." % (quantity, start_period.diff()))                





main = generator()
main.create_buildings(100)
main.create_contractors(100)
main.create_executors(100)

