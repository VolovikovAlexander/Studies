

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
        for field in fields:
            object = getattr(source.__class__, field)
            if isinstance(object, property):
                value = object.__get__(source, source.__class__)
                type_value = type(value)
                yes_json = hasattr(type_value, "toJSON")
                
                if yes_json:
                    result = helper.toDict(value)
                    if len(result) == 0:
                        attributes[field] = value.toJSON()
                    else:    
                        attributes[field] = helper.toDict(value)
                else:
                    yes_list = isinstance(value, list)
                    if yes_list:
                        items = []
                        for item in value:
                            items.append(helper.toDict(item))

                        attributes[field] = items
                    else:                                  
                        attributes[field] = value    

        return attributes        