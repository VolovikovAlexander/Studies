
import unittest
from Src.Models.Building  import building
from Src.Models.Contractor import contractor
from Src.Models.Executor import executor
from Src.Models.Act import act

#
# Набор модульных тестов для проверки работы моделей данных
#
class models_tests(unittest.TestCase):

    # Проверить создание объекта типа building
    def test_create_builder(self):
        # Подготовка

        # Действие
        result = building.create(name="test")

        # Проверки
        assert result is not None
        print(result.guid)


    # Проверить создание объекта типа contractor
    def test_create_contractor_common(self):
        # Подготовка

        # Действие
        result = contractor.create(name="test", parent=None)

        # Проверки
        assert result is not None
        print(result.guid)

    # Проверить создание объекта типа contractor
    def test_create_contractor_with_parent(self):
        # Подготовка
        parent = contractor.create(name="test", parent=None)

        # Действие
        result = contractor.create(name="test2", parent=parent)

        # Проверки
        assert result is not None
        print(result.guid)
        assert result.parent is not None

    # Проверить создание типа executor
    def test_create_executor(self):
        # Подготовка
        parent = contractor.create(name="test", parent=None)

        # Действие
        result = executor.create(name="test", self_contractor=parent)

        # Проверки
        assert result is not None
        print(result.guid)


    # Проверить создание документа /Акт/
    def test_create_act(self):
        # Подготовка
        contractor_parent = contractor.create(name="test1", parent=None)
        contractor_act = contractor.create(name="test2", parent=contractor_parent)
        executor_act = executor.create(name="test3", self_contractor=contractor_act)

        # Действие
        result = act.create(self_executor=executor_act)
        #result.add(contractor_act)

        # Проверки
        assert result is not None
        assert len(result.contractors()) == 2





if __name__ == '__main__':
    unittest.main()
