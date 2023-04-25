from Src.Models.Act import act
from Src.Models.Executor import executor
from Src.Models.Contractor import contractor

"""
#  Класс репозиторий
"""
class repo():
    __acts = []
    __buildings = []
    __executors = []
    __contractors = []

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

    def load_demo(self):
        """
        Сформировать тестовые данные
        """    
        self.__acts = []

        contractor_parent = contractor.create(name="test1", parent=None)
        contractor_act = contractor.create(name="test2", parent=contractor_parent)
        executor_act = executor.create(name="test3", _contractor=contractor_act)
        current_act = act.create(_executor=executor_act)

        self.__acts.append(current_act)


    # Статические методы

    def create(is_demo = False):
        """
        Фабричный метод
        """
        main = repo()
        if is_demo == True:
            main.load_demo()
        else:    
            main.load()

        return main




        
