from Src.Services.Proxy import db_proxy

"""
Класс для генерации тестовх данных 
"""
class generator():
    __proxy = None

    def __init__(self):
        self.__proxy = db_proxy()

    def clear(self):
        """
        Очистить базу данных
        """    
        result = self.__proxy.execute("alter table buildings delete where 1 = 1")
        if not result:
            raise Exception("Ошибка при удалении данных из таблицы buildings! " + self.__proxy.error_text )
        result = self.__proxy.execute("alter table executors delete where 1 = 1")
        if not result:
            raise Exception("Ошибка при удалении данных из таблицы executors! " + self.__proxy.error_text )
        result = self.__proxy.execute("alter table contractors delete where 1 = 1")
        if not result:
            raise Exception("Ошибка при удалении данных из таблицы contractors! " + self.__proxy.error_text )
        result = self.__proxy.execute("alter table acts_status_links delete where 1 = 1")
        if not result:
            raise Exception("Ошибка при удалении данных из таблицы acts_status_links! " + self.__proxy.error_text )
        result = self.__proxy.execute("alter table acts_contractors_links delete where 1 = 1")
        if not result:
            raise Exception("Ошибка при удалении данных из таблицы acts_contractors_links! " + self.__proxy.error_text )
        
        return True






