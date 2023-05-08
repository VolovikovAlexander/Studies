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
    __acts= []
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
                _number = random.randint(0, len(self.__contractors) - 1)

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
            _number = random.randint(0, len(self.__contractors) - 1)

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
        
        if quantity < 25:
            raise Exception("ОШИБКА! Необходимо указать актов > 25")
        
        start_period = period()
        print("-> Генерация записей: acts")
        print("Старт: %s" % start_period.toJSON())

        for position in range(quantity):
            # Создадим базовый акт            
            _number_building = random.randint(0, len(self.__buildings) - 1) 
            _number_executor = random.randint(0, len(self.__executors) - 1)
            _act = act.create(self.__executors[_number_executor], self.__buildings[_number_building])

            # Добавим разных застройщиков к акту
            _number_contractors= random.randint(1,5) 
            for item in range(_number_contractors):
                _number_contractor = random.randint(0, len(self.__contractors) - 1)
                _act.add(self.__contractors[_number_contractor])

            # Произвольный период (- 365 дней назал)
            _days_period =  random.randint(1, 365)
            _act_period = period( days=_days_period * (-1))
            _act.period = _act_period
            self.__acts.append(_act)

            # Сохраним результат

            sql = str(_act)
            result = self.__proxy.execute(sql)
            if not result:
                    print("ОШИБКА! %s" % (self.__proxy.error_text))
                    return
            
        print("Сформировано успешно %s записей за %s сек." % (quantity, start_period.diff()))     
            
    def create_status_history(self, quantity):        
        """
        Создать история статусов
        """
        if quantity is None:
            raise Exception("ОШИБКА! Не указано количество записей которое нужно сформировать!")
        
        if quantity < 1:
            raise Exception("ОШИБКА! Некорректно указано количество записей!")
        
        start_period = period()
        print("-> Генерация записей: acts_status_links")
        print("Старт: %s" % start_period.toJSON())
        
        for _act_number in range(quantity):
            _act = self.__acts[_act_number]
            _status = random.randint(1,5)
            _act.progress = _status

            sql = str(_act)
            result = self.__proxy.execute(sql)
            if not result:
                    print("ОШИБКА! %s" % (self.__proxy.error_text))
                    return

                
        print("Сформировано успешно %s записей за %s сек." % (quantity, start_period.diff()))         



# Запускаем генерацию данных
start_period = period()
main = generator()
main.create_buildings(100)
main.create_contractors(100)
main.create_executors(100)
main.create_acts(50)
main.create_status_history(25)
print("Генерация данных завершена за %s сек." % (start_period.diff()))      


