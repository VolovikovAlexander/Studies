import unittest
from Src.Services.Proxy import db_proxy
from Src.Data.BadContractor import bad_contractor

#
# Набор интеграционных тестов для проверки выборки данных по различным Dashbord
# 
class dashboard_tests(unittest.TestCase):

    def  test_get_bad_contractors(self):
        """
        Проверить получение данных по Dasboard ->  bad_contractor
        """

        # Подготовка
        proxy = db_proxy()
        proxy.create()

        # Действие
        result = bad_contractor.create(proxy)

        # Проверки
        assert result is not None
        assert len(result) > 0




if __name__ == '__main__':
    unittest.main()
