
import unittest
from Src.Models.Building  import building

#
# Набор модульных тестов для проверки работы моделей данных
#
class models_tests(unittest.TestCase):

    def test_create_builder(self):
        # Подготовка

        # Действие
        result = building.create(name="test")

        # Проверки
        assert result is not None
        print(result.guid)


if __name__ == '__main__':
    unittest.main()
