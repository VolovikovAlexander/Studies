from datetime import datetime

#
# Класс - текущая дата с сериализацией в Json
#
class period():
    __period = None

    def __init__(self):
      self.__period = datetime.now()      

    def toJSON(self):
        return str(self.__period)
