from Src.Models.Statuses import progress_status
from Src.Models.Executor import executor
from Src.Models.Contractor import contractor


#
# Модель документа /Акт проверки/
#
class act():
    __amount = 0
    __executor = None
    __contractors = {}
    __progress = None

    def __init__(self):
        self.__progress = progress_status.start


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
        

    def contractors(self):
        # Свойство: Список застройщиков   
        items = list(self.__contractors)
        return items
        

    def create(self_executor):
        # Фабричный метод    
        result = act()
        result.executor = self_executor
        result.add(self_executor.contraсtor)

        return result
        

    def add(self, self_contractor):
        # Добавить в документ исполнителей
        if self_contractor is None:
            return
        
        if not isinstance(self_contractor, contractor):
            raise Exception("ОШИБКА! Параметр self_contractor - должен быть типом contractor!")
        
        self.__contractors[self_contractor.guid] = self_contractor
        self.add(self_contractor.parent)

        
        





        
        
