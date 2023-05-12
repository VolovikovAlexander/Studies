from Src.Models.Act import act
from Src.Models.Executor import executor
from Src.Models.Contractor import contractor
from Src.Services.Proxy import db_proxy
from Src.Models.Building import building

from Src.Data.BadContractor import bad_contractor

"""
#  Класс репозиторий
"""
class repo():
    __acts = []
    __buildings = []
    __executors = []
    __contractors = []
    __proxy = None

    def __init__(self):
        self.__proxy = db_proxy()
        self.__proxy.create()

    # Методы данных    

    def get_acts(self):
        """
        Получить список всех актов
        """
        return self.__acts
    
    def get_buildings(self):
        """
        Получить список всех строений
        """
        return self.__buildings
    
    def get_executors(self):
        """
        Получить список всех исполнителей
        """
        return self.__executors
    
    def get_contractors(self):
        """
        Получить список всех подрядчиков
        """
        return self.__contractors
        

    # Методы класса
    def load(self):
        """
        Сформировать тестовые данные
        """    
        self.__acts = []
        self.__buildings = []
        self.__executors = []
        self.__contractors = []

        # Сформировать тестовый набор данных
        contractor_parent = contractor.create(name="test1", parent=None)
        contractor_act = contractor.create(name="test2", parent=contractor_parent)
        executor_act = executor.create(name="test3", _contractor=contractor_act)
        building_act = building.create(name="test4")
        current_act = act.create(_executor=executor_act, _building=building_act)

        self.__acts.append(current_act)


    # Dashboards
    def get_bad_contractors(self):
        """
        Получить Dashboad: Самые плохие застройщики
        """
        return bad_contractor.create(self.__proxy)

    # Статические методы
    def create():
        """
        Фабричный метод
        """
        main = repo()
        main.load()

        return main

    

        
