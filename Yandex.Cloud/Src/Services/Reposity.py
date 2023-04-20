from Src.Models.Act import act
from Src.Models.Executor import executor
from Src.Models.Statuses import progress_status
from Src.Models.Contractor import contractor

#
#  Класс репозиторий
#
class reposity():
    __acts = []


    def acts(self):
        # Получить список актов
        self.__acts = []

        contractor_parent = contractor.create(name="test1", parent=None)
        contractor_act = contractor.create(name="test2", parent=contractor_parent)
        executor_act = executor.create(name="test3", _contractor=contractor_act)
        current_act = act.create(_executor=executor_act)

        self.__acts.append(current_act)
        return self.__acts



        
