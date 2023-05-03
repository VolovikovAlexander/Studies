from Src.Models.Statuses import progress_status
from Src.Models.Building import building
from Src.Models.Contractor import contractor
from Src.Models.Executor import executor
from Src.Models.Act import act
from Src.Services.Proxy import db_proxy
from Src.Models.Period import period

# Набор операций для создания произвольных данных 

class generator():
    __buildings = []
    __executors = {}
    __contractors= {}
    __proxy = None

    def __init__(self):
        self.__proxy = db_proxy()
        self.__proxy.create()

    def create_buildings(self, quantity):
        '''
        Сформировать в базе данных объекты капитального строительства (ОКС)
        '''
        if quantity is None:
            raise Exception("ОШИБКА! Не указано количество записей которое нужно сформировать!")
        
        if quantity < 1:
            raise Exception("ОШИБКА! Некорректно указано количество записей!")
        
        start_period = period()
        print("Генерация записей: buildings")
        print("Старт: %s" % start_period.toJSON())
        
        for position in range(quantity):
            _name = "building № %s" %  (position + 1)
            _building = building.create(name = _name)

            result = self.__proxy.execute(str(_building))
            if not result:
                print("ОШИБКА! %s" % (self.__proxy.error_text))
                return
            
            self.__buildings.append(_building)
            
        print("Сформировано успешно %s записей ОКС за %s сек." % (quantity, start_period.diff()))    

    def create_contractors(self, quantity):
        """
        Сформировать застройщиков в виде дерева
        """
        if quantity is None:
            raise Exception("ОШИБКА! Не указано количество записей которое нужно сформировать!")
        
        if quantity < 1:
            raise Exception("ОШИБКА! Некорректно указано количество записей!")
        
        start_period = period()
        print("Генерация записей: contractors")
        print("Старт: %s" % start_period.toJSON())

        for position in range(quantity):
            _name = "contractor № %s" %  (position + 1)
            





main = generator()
main.create_buildings(100)
main.create_contractors(100)

