import uuid

"""
# Класс обвертка над типом uuid
"""
class guid():
    __guid = None

    def __init__(self, value = None):
        if value is None:
            self.__guid = uuid.uuid4()
        else:
            if isinstance(value, uuid):
                raise Exception("Ошибка! Некорректный тип данных value!")    
            self.__guid = value


    def toJSON(self):
        """
        Преобразовать в Json
        """
        return str(self.__guid)