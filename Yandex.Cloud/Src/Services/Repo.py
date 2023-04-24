from Src.Models.Act import act
from Src.Models.Executor import executor
from Src.Models.Statuses import progress_status
from Src.Models.Contractor import contractor
from datetime import datetime

"""
#  Класс репозиторий
"""
class repo():
    __acts = []

    def get_acts(self):
        """
        Получить список актов
        """
        return self.__acts


    def load(self):
        """
        Загрузить акты и базы данных
        """
        self.__acts = []

        contractor_parent = contractor.create(name="test1", parent=None)
        contractor_act = contractor.create(name="test2", parent=contractor_parent)
        executor_act = executor.create(name="test3", _contractor=contractor_act)
        current_act = act.create(_executor=executor_act)

        self.__acts.append(current_act)


    # Статические общие методы

    def toDict(source):
        """
        Сформировать набор ключ / значение из произвольного объекта
        """
        if source is None:
            raise Exception("ОШИБКА! Параметр source - пустой!")
        
        attributes = {}
        fields = list(filter(lambda x: not x.startswith("_"), dir(source.__class__)))
        for name in fields:
            object = getattr(source.__class__, name)
            if isinstance(object, property):
                value = object.__get__(source, source.__class__)
                type_value = type(value)
                yes_json = hasattr(type_value, "toJSON")
                if yes_json:
                    attributes[name] = value.toJSON()
                else:
                    attributes[name] = value    

        return attributes        


    def create():
        """
        Фабричный метод
        """
        main = repo()
        main.load()

        return main




        
