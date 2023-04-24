from datetime import datetime

"""
# Класс обвертка над типом datetime
"""
class period():
    __period = None

    def __init__(self, value = None):
      if value is None:
        self.__period = datetime.now()      
      else:
        if not isinstance(value, datetime):
          raise Exception("ОШИБКА! Некорректно  указан паметр data!")

        self.__period = value  

    def toJSON(self):
        """
        Преобразовать в Json
        """
        return str(self.__period)
