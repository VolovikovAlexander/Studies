"""
# Класс с набором перечислений статусов для этапов
"""
class progress_status():
    # Все документы для начала строительства / этапа собраны и проверены. Разрешения выданы 
    preparation = 1
    # На объекте начаты строительные работы. Представители субподрядчика прибыли на объект
    start = 2
    # Очередная проверка пройдена без замечаний со стороны контроллирующих органов
    passing = 3
    # Очередная проверка пройдена с замечаниями. Необходимо устранить все замечания
    failure = 4
    # Все замечания устранены. Все проверки пройдены. Оплату можно проводить
    finish = 5

    __name = ""
    __descrption = ""
    __code = 1
    __id = None

    @property
    def id(self):
        '''
        Уникальный код записи
        '''
        return self.__id
    
    @id.setter
    def id(self, value):
        '''
        Уникальный код записи
        '''
        if value is None:
            raise Exception("Некорректно указан параметр id!")
        
        self.__id = value

    @property
    def name(self):
        '''
        Короткое наименование статуса
        '''
        return self.__name
    
    @name.setter
    def name(self, value):
        '''
        Короткое наименование статуса
        '''
        if value is None:
            raise Exception("Некорректно указан параметр name!")
        
        self.__name = value

    @property
    def description(self):
        '''
        Описание статуса
        '''
        return self.__descrption    
    
    @description.setter
    def description(self, value):
        '''
        Описание статуса
        '''
        if value is None:
            raise Exception("Некорректно указан параметр name!")
        
        self.__descrption = value
        
        