from clickhouse_driver import Client


"""
    Прокси класс для работы с базой данных
"""
class db_proxy():
    __client = None
    __is_open = False
    __error_text = ""

    def open(self):
        """
        Открыть новое подключение к базе данных
        """
        self.__error_text = ""
        if self.__is_open == True:
            return
        
        try:
            self.__client = Client(host='rc1a-7ut3ob6t69958voj.mdb.yandexcloud.net', user='user', password='useruser', port = 9440, database = "dbDashboard")
        except Exception as ex:
            self.__error_text = "Невозможно открыть подключение к базе данных! " + ex.args[0]
        finally:
            self.__client = None


    
    @property
    def error_text(self):
        """
        Получить сообщение об ошибке
        """
        return self.__error_text
    

    @property
    def is_error(self):
        """
        Получить флаг о последнем состоянии
        """
        return not self.__error_text == "" 
    

    def get_rows(self, sql, map_type):
        """
        Выполнить SQL запрос и сформировать массив из структур map_type
        """

        if sql == "":
            raise Exception("Некорректно передан параметр sql!")
        
        if map_type is None:
            raise Exception("Некорректно передан параметр map_type!")

        if self.__client is None:
            self.open()

        if self.is_error:
            return
        
        self.__error_text = ""
        try:
            rows = Client.execute(query = sql)
        except Exception as ex:
            self.__error_text = "Ошибка при выполнении SQL запроса (" + sql + ")  " + ex.args[0]



        

        




    



    



        