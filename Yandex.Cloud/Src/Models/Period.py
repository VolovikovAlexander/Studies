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
        result = str(self.__period)
        return result[0:19]
    
    def diff(self):
      """
       Сверить даты друг с другом
      """
      value = datetime.now() 
      result = (value - self.__period).total_seconds()
      return result
    



