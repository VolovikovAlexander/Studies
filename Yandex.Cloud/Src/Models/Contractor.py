import json

from Src.Services.Helper import helper
from Src.Models.Guid import guid


"""
# Модель объекта - подрядчик / субподрядчик
"""
class contractor():
    __parent = None
    __name = ""
    __description = ""
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
        Cвойство: Наименование
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
    def guid(self):
        """
        Свойство: Уникальный код объекта строительства
        """
        return self.__guid    
    

    @property
    def parent(self):
        """
        Свойство: Объект владелец
        """
        return self.__parent

    def create( name, parent):
        """
        Фабричный метод. Создать объект типа building
        """
        result = contractor()
        result.name = name
        result.__guid = guid()

        if not parent is None:
            if not isinstance(parent, contractor):
                raise Exception("ОШИБКА! Параметр parent - должен быть типом contractor!")
            
            result.__parent = parent

        return result



    def toJSON(self):
        """
        Сериализовать объект в Json
        """
        items = helper.toDict(self)
        return json.dumps(items, sort_keys = True, indent = 4)  