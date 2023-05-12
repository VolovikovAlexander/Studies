
"""
Абстрактный класс для наследования. 
Содержит в себе инкапсуляцию Proxy для работы с базой данных и основные свойства.
"""
class abstract():
    __name = ""
    __description = ""
    __sql = ""


    @property
    def name(self):
        """
        Наименование модели данных
        """
        return self.__name
    
    @name.setter
    def name(self, value):
        """
        Наименование модели данных
        """
        if value is None:
            raise Exception("ОШИБКА! Некорректно передан параметр name!")
        
        if not isinstance(value, str):
            raise Exception("ОШИБКА! Некорректно передан параметр name!")
        
        self.__name = value


    @property
    def description(self):
        """
        Подробное описание модели данных
        """
        return self.__description
    
    @description.setter
    def description(self, value):
        """
        Подробное описание модели данных
        """
        if value is None:
            raise Exception("ОШИБКА! Некорректно передан параметр description!")
        
        if not isinstance(value, str):
            raise Exception("ОШИБКА! Некорректно передан параметр description!")
        
        self.__description = value



    @property
    def sql(self):
        """
        SQL запрос на получение данных
        """
        return self.__sql
    
    @sql.setter
    def sql(self, value):
        """
        SQL запрос на получение данных
        """
        if value is None:
            raise Exception("ОШИБКА! Некорректно передан параметр sql!")
        
        if not isinstance(value, str):
            raise Exception("ОШИБКА! Некорректно передан параметр sql!")
        
        self.__sql = value    

        

 
