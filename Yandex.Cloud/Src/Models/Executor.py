import uuid
import json
from Src.Models.Contractor import contractor

#
# Класс модель - исполнитель
#
class executor():
    __name = ""
    __guid = ""
    __contractor = None


    @property
    def name(self):
        # Свойство: Наименование
        return self.__name
    
    def name(self, value):
        # Свойство: Наименование
        if not isinstance(value, str):
            raise Exception("ОШИБКА! Параметр name - должен быть типом str!")
        
        if value == "":
            raise Exception("ОШИБКА! Параметр name должен быть указан!")
        
        self.__name = value


    @property
    def guid(self):
        # Свойство: Уникальный код объекта строительства
        return self.__guid    
    
    @property
    def contraсtor(self):
        # Свойство: Организация исполнителя
        return self.__contractor
    

    def create(name, _contractor):
         # Фабричный метод. Создать объект типа executor
        result = executor()
        result.name = name
        result.__guid = uuid.uuid4()

        if _contractor is None:
            raise Exception("ОШИБКА! Параметр _contractor должен быть указан!")
        
        if not isinstance(_contractor, contractor):
            raise Exception("ОШИБКА! Параметр _contractor - должен быть типом contractor!")
        
        result.__contractor = _contractor
        return result


    def toJSON(self):
        # Сериализовать объект в Json
        return json.dumps(self, default=lambda o: o.__dict__, 
            sort_keys=True, indent=4)  