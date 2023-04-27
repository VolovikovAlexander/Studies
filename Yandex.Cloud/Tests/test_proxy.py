import unittest
from Src.Services.Proxy import db_proxy

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
