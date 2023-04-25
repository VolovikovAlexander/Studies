

"""
Класс с набором вспомогаьельных методов
"""
class helper():

    # Статические общие методы

    def toDict(source):
        """
        Сформировать набор ключ / значение из произвольного объекта
        """
        if source is None:
            raise Exception("ОШИБКА! Параметр source - пустой!")
        
        attributes = {}
        fields = list(filter(lambda x: not x.startswith("_"), dir(source.__class__)))
        for name in fields:
            object = getattr(source.__class__, name)
            if isinstance(object, property):
                value = object.__get__(source, source.__class__)
                type_value = type(value)
                yes_json = hasattr(type_value, "toJSON")
                if yes_json:
                    attributes[name] = value.toJSON()
                else:
                    attributes[name] = value    

        return attributes        