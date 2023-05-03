import json

from Src.Models.Guid import guid
from Src.Services.Helper import helper

"""
# Класс модели с описанием параметров объекта капитального строительства
"""
class building():
    __description = ""
    __name = ""
    __guid = ""

    
    @property
    def description(self):
        """
        Свойство. Описание
        """
        return self.__description
    
    @description.setter
    def description(self, value):
        """
        Свойство. Описание
        """
        if not isinstance(value, str):
            raise Exception("ОШИБКА! Параметр description - должен быть типом str!")

        self.__description = value

    
    @property
    def name(self):
        """
        Свойство: Наименование
        """
        return self.__name
    

    @name.setter
    def name(self, value):
        """
        Свойство: Наименование
        """
        if not isinstance(value, str):
            raise Exception("ОШИБКА! Параметр name - должен быть типом str!")
        
        if value == "":
            raise Exception("ОШИБКА! Параметр name должен быть указан!")
        
        self.__name = value


    @property
    def id(self):
        """
        Свойство: Уникальный код объекта строительства
        """
        return self.__guid    
    
    @id.setter
    def id(self, value):
        """
        Свойство: Уникальный код объекта строительства
        """
        if value is None:
            raise Exception("Некорректно передан параметр id!")
        
        self._guid = value
    

    def create( name):
        """
        Фабричный метод. Создать объект типа building
        """
        result = building()
        result.name = name
        result.__guid = guid()

        return result


    def toJSON(self):
        """
        Сериализовать объект в Json
        """
        items = helper.toDict(self)
        return json.dumps(items, sort_keys = True, indent = 4)   


    def __str__(self):
        """
        Сформировать SQL запрос на вставку данных
        """
        sql = "insert into buildings (id, name, description) values ('%s','%s','%s')" % (self.id.toJSON(), self.name, self.description)
        return sql

        
    



    

        
        
    


