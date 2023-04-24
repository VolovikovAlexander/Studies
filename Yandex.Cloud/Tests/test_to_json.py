from datetime import datetime
from Src.Models.Period import period
from Src.Models.Building import building

import unittest

#
# Набор модульных для проверки конвертиции структур в Json
#
class json_convert_tests(unittest.TestCase):

    #
    # Проверить конвертацию в Json объекта типа period
    #
    def test_period_to_json(self):
        # Подготовка
        test_period = datetime(2020,1,1)
        object = period(test_period)

        # Действие
        result = object.toJSON()

        # Проверки
        assert result is not None
        assert result == "2020-01-01 00:00:00"

    #
    # Проверить конвертаци. в Json объекта типа builder
    #
    def test_building_to_json(self):
        # Подготовка
        object = building.create("test")

        # Действие
        result = object.toJSON()

        # Проверки
        assert result is not None
        print(result)




if __name__ == '__main__':
    unittest.main()
