from Src.Models.Act import act
from Src.Models.Executor import executor
from Src.Models.Statuses import progress_status
from Src.Models.Contractor import contractor
from datetime import datetime

#
#  Класс репозиторий
#
class repo():
    __acts = []

    def get_acts(self):
        # Получить список актов
        return self.__acts


    def load(self):
        # Загрузить акты и базы данных
        self.__acts = []

        contractor_parent = contractor.create(name="test1", parent=None)
        contractor_act = contractor.create(name="test2", parent=contractor_parent)
        executor_act = executor.create(name="test3", _contractor=contractor_act)
        current_act = act.create(_executor=executor_act)

        self.__acts.append(current_act)


    def create():
        # Фабричный метод
        main = repo()
        main.load()

        return main




        
