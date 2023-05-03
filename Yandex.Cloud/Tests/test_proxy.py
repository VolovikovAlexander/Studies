import unittest
from Src.Services.Proxy import db_proxy
from Src.Models.Statuses import progress_status
from Src.Models.Building import building
from Src.Models.Executor import executor
from Src.Models.Contractor import contractor
from Src.Models.Act import act


class proxy_tests(unittest.TestCase):
    # 
    # Проверить подключение к базе данных
    #
    def test_connect_database(self):
        # Подготовка
        proxy = db_proxy()

        # Действие
        proxy.create()

        # Проверки
        print(proxy.error_text)
        assert proxy.error_text == ""
        assert proxy.is_error == False

    #
    # Проверить простую выборку данных
    #
    def test_get_rows_statuses(self):
        # Подготовка
        proxy = db_proxy()
        proxy.create()

        # Действие
        data = proxy.get_rows("select * from statuses", progress_status)

        # Проверки
        print(proxy.error_text)
        assert proxy.error_text == ""
        assert proxy.is_error == False
        assert len(data) > 0

    #
    # Проверить выполнение SQL запросов без выборки
    #
    def test_exec_queries(self):
        # Подготовка
        proxy = db_proxy()
        proxy.create()

        # Действие
        proxy.execute("alter table buildings delete where 1 = 1")

        # Проверки
        print(proxy.error_text)
        assert proxy.error_text == ""
        assert proxy.is_error == False

    #
    # Проверить вставку данных по типу building
    #
    def test_insert_building(self):
        # Подготовка
        proxy = db_proxy()
        proxy.create()
        object = building.create(name = "test" )
        sql = str(object)

        # Действие
        proxy.execute(sql)

        # Проверки
        print(proxy.error_text)
        assert proxy.error_text == ""
        assert proxy.is_error == False

    #
    # Проверить вставку данных по типу building
    #
    def test_insert_executor(self):
         # Подготовка
        proxy = db_proxy()
        proxy.create()
        _parent = contractor.create(name="test", parent=None)
        object = executor.create(name = "test", _contractor = _parent)
        sql = str(object)

        # Действие
        proxy.execute(sql)

        # Проверки
        print(proxy.error_text)
        assert proxy.error_text == ""
        assert proxy.is_error == False

    #
    # Проверить вставку данных типа act
    #
    def test_insert_act(self):
        # Подготовка
        proxy = db_proxy()
        proxy.create()
        contractor_parent = contractor.create(name="test1", parent=None)
        contractor_act = contractor.create(name="test2", parent=contractor_parent)
        executor_act = executor.create(name="test3", _contractor=contractor_act)
        building_act = building.create(name="test4" )
        object = act.create(_executor=executor_act, _building=building_act)
        object.amount = 220.2
        sql = str(object)

        # Действие
        print(sql)
        proxy.execute(sql)

        # Проверки
        print(proxy.error_text)
        assert proxy.error_text == ""
        assert proxy.is_error == False









