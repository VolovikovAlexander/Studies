from Src.Models.Statuses import progress_status
from Src.Models.Building import building
from Src.Models.Contractor import contractor
from Src.Models.Executor import executor
from Src.Models.Act import act

# Набор операций для создания произвольных данных 

class generator():
    __buildings = {}
    __executors = {}
    __contractors= {}


    def create_buildings(self, quantity):
        '''
        Сформировать в базе данных объекты капитального строительства (ОКС)
        '''
        if quantity is None:
            raise Exception("ОШИБКА! Не указано количество записей которое нужно сформировать!")
        
        if quantity < 1:
            raise Exception("ОШИБКА! Некорректно указано количество записей!")
        
        for position in range[quantity]:
            _name = "building № %s" %  (position)
            _building = building.create(name = _name)
            
        

