import unittest
from Src.Services.Proxy import db_proxy
from Src.Models.Statuses import progress_status

class proxy_tests(unittest.TestCase):

     
    # 
    # Проверить подключение к базе данных
    #
    def test_connect_database(self):
        # Подготовка
        proxy = db_proxy()

        # Действие
        proxy.open()

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
        proxy.open()
        _map_type = type(progress_status)

        # Действие
        data = proxy.get_rows("select * from statuses", _map_type)

        # Проверки
        print(proxy.error_text)
        assert proxy.error_text == ""
        assert proxy.is_error == False



