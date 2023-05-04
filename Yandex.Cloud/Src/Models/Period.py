from datetime import timedelta, date, datetime
"""
# Класс обвертка над типом datetime
"""
class period():
    __period = None

    def __init__(self, value = None, days = None):
      """
      value - значение типа datetime
      """
      if value is None and days is None:
        self.__period = datetime.now()      

      if not value is None:  
        if not isinstance(value, datetime):
          raise Exception("ОШИБКА! Некорректно  указан паметр data!")

        self.__period = value  

      if not days is None:
        if not isinstance(days, int):
          raise Exception("ОШИБКА! Некорректно  указан паметр int!")
        
        self.__period = date.today() + timedelta(days)      
        
            

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
    



