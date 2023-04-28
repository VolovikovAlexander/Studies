from clickhouse_driver import Client


"""
    Прокси класс для работы с базой данных
"""
class db_proxy():
    __client = None
    __is_open = False
    __error_text = ""
    __data = []

    def create(self):
        """
        Создать новое подключение
        """
        self.__error_text = ""
        if self.__is_open == True:
            return self.__client
        
        try:
            self.__client = Client(host='rc1a-7ut3ob6t69958voj.mdb.yandexcloud.net', user='user', password='useruser', port = 9440, database = "dbDashboard", secure = True)
            return self.__client
        except Exception as ex:
            self.__error_text = "Невозможно открыть подключение к базе данных! " + ex.args[0]
            self.__client = None
            return None

    
    @property
    def error_text(self):
        """
        Свойство: Получить сообщение об ошибке
        """
        return self.__error_text
    

    @property
    def is_error(self):
        """
        Свойство: Получить флаг о последнем состоянии
        """
        return not self.__error_text == "" 

    @property    
    def get_last_data(self):
        """
        Свойство: Получить данные по последней успешной выборке данных
        """
        return self.__data
    

    def get_rows(self, sql, map_type):
        """
        Выполнить SQL запрос и сформировать массив из структур <map_type>.
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
            rows = self.__client.execute(query = sql,  with_column_types=True)
        except Exception as ex:
            self.__error_text = "Ошибка при выполнении SQL запроса (" + sql + "): " + ex.args[0]
            return []
        
        return self.__prepare_rows(map_type, rows)
        

    def __prepare_rows(self, map_type, rows):
        """
        Провести обработку полученных данных при выполнении SQL запроса от ClickHouse
        """
        if rows is None:
            raise Exception("Некорректно передан параметр rows!")
        
        try:
            # Формируем словарь: поле / значение
            data = []
            columns = list(map(lambda x: x[0], rows[-1]))
            for row in rows[0]:
                dict = {}
                column_index = 0
                for column in columns:
                    dict[column] = str(row[column_index])
                    column_index += 1

                data.append(dict)

            # Сконвертируем словарь в массив типа map_type
            self.__data = []
            for item in data:
                object = map_type()
                is_yes_data = False
                for column in columns:
                    yes_field = hasattr(map_type, column)
                    if yes_field:
                        setattr(object, column, item[column])
                        is_yes_data = True
                    
                if is_yes_data == True:
                    self.__data.append(object)

            return self.__data

        except Exception as ex:
            self.__error_text = "Ошибка при обработке данных от SQL запроса: " + ex.args[0]
            return []


    def execute(self, sql, params = None):
        """
        Выполнить произвольный SQL запрос
        """
        if sql == "":
            raise Exception("Некорректно передан параметр sql!")
        
        if self.__client is None:
            self.create()

        if self.is_error:
            raise Exception("Невозможно выполнить SQL запрос. " + self.__error_text)
        
        try:
            self.__client.execute(sql, params)
            return True
        except Exception as ex:
            self.__error_text = "Ошибка при выполнении SQL запроса (" + sql + "): " + ex.args[0]            
            return False
        


        

        




    



    



        