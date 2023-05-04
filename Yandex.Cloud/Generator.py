from Src.Models.Statuses import progress_status
from Src.Models.Building import building
from Src.Models.Contractor import contractor
from Src.Models.Executor import executor
from Src.Models.Act import act
from Src.Services.Proxy import db_proxy
from Src.Models.Period import period

import random
from datetime import datetime
import datetime

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

    def create_acts(self, quantity):
        """
        Создать набор актов с разными статусами
        """
        if quantity is None:
            raise Exception("ОШИБКА! Не указано количество записей которое нужно сформировать!")
        
        if quantity < 1:
            raise Exception("ОШИБКА! Некорректно указано количество записей!")
        
        if len(self.__contractors) == 0:
            raise Exception("ОШИБКА! Необходимо сперва сформировать застройщиков")
        
        if len(self.__buildings) == 0:
            raise Exception("ОШИБКА! Необходимо сперва сформировать ОКС")

        if len(self.__executors) == 0:
            raise Exception("ОШИБКА! Необходимо сперва сформировать исполнителей")
        
        start_period = period()
        print("-> Генерация записей: acts")
        print("Старт: %s" % start_period.toJSON())

        for position in range(quantity):
            # Создадим базовый акт            
            _number_building = int(random.random() * len(self.__buildings))
            _number_executor = int(random.random() * len(self.__executors))
            _act = act.create(self.__executors[_number_executor], self.__buildings[_number_building])

            # Добавим 10 разных застройщиков к акту
            for item in range(10):
                _number_contractor = int(random.random() * len(self.__contractors))
                _act.add(self.__contractors[_number_contractor])

            # Произвольный период (- 365 дней назал)
            _days_period =  int(random.random() * 365)
            _act_period = period( days=_days_period * (-1))
            _act.period = _act_period

            # Сохраним результат
            result = self.__proxy.execute(str(_act))
            if not result:
                    print("ОШИБКА! %s" % (self.__proxy.error_text))
                    return

            # Заменим статусы
                
        print("Сформировано успешно %s записей за %s сек." % (quantity, start_period.diff()))         



# Запускаем генерацию данных
start_period = period()
main = generator()
main.create_buildings(100)
main.create_contractors(100)
main.create_executors(100)
main.create_acts(500)
print("Генерация данных завершена за %s сек." % (start_period.diff()))      


