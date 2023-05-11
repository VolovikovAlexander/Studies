from Src.Data.Abstract import abstract

"""
Модель Dashboard - плохой застройщик
"""
class bad_contractor(abstract):
    def __init__(self):
        super().__init__()
        self.sql = "select * from acts as t1 inner join ( select id from ( select id, argMax(status_code, period) as status_code from acts_status_links group by id) as tt where tt.status_code = 4 ) as tt on tt.id = t1.id inner join building as t2 on t1.building_id = t2.id"
