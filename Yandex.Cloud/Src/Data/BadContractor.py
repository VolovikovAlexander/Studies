from Src.Data.Abstract import abstract

"""
Модель Dashboard - плохой застройщик
"""
class bad_contractor(abstract):
    __link = ""
    __quantity = 0
    __failure = 0
    __amount = 0 

    def __init__(self):
        super().__init__()
        self.sql = """
        with cte_acts as
        (
            -- Список проблемных актов
            select id as act_id from ( select id, argMax(status_code, period) as status_code from acts_status_links group by id) as tt where tt.status_code = 4 
        ),
        cte_buildings  as 
        (
            -- Список проблемных застройщиков
            select t2.id as building_id, t2.name as building_name
            from acts as t1 
            inner join cte_acts as tt on tt.act_id = t1.id 
            inner join buildings as t2 on t1.building_id = t2.id
            group by t2.id, t2.name
        ),
        cte_quantity_acts as
        (
            -- Количество актов в работе по каждому застройщику
            select t1.building_id, count(*) as cnt_all from acts as t1 where t1.building_id in ( select building_id from cte_buildings)
            group by t1.building_id
        ),
        cte_quantity_failure_acts as
        (
            -- Количество актов по застройщикам, которые имеют проблемы
            select t1.building_id as building_id, count(*) as cnt_failure, sum(tt.amount) as amount from acts as t1
            left join ( select id as act_id, argMax(amount, period) as amount from acts_status_links where status_code = 4 group by id ) as tt on tt.act_id = t1.id
            where t1.id in (select act_id from cte_acts) 
            group by t1.building_id 
        )

        select concat('http://localhost:8080/api/contractors/',  toString(t1.building_id)) as link, t1.building_name as name,  t2.cnt_all as qauntity, t3.cnt_failure as failure, t3.amount from cte_buildings as t1
        left join cte_quantity_acts as t2 on t1.building_id = t2.building_id
        left join cte_quantity_failure_acts as t3 on t3.building_id = t1.building_id
        order by t2.cnt_all, t3.cnt_failure, t3.amount desc;
        
        """
        
     
    @property
    def link(self):
        """
        Ссылка на застройщика
        """
        return self.__link
    
    @link.setter
    def link(self, value):
        self.__link = value


    @property
    def quantity(self):
        """
        Количество актов в работе
        """
        return self.__quantity
    
    @quantity.setter
    def quantity(self, value):
        self.__quantity = value    

    
    @property
    def failure(self):
        """
        Количество актов с замечаниями 
        """
        return self.__failure
    
    @failure.setter
    def link(self, value):
        self.__failure = value     

    @property
    def amount(self):
        """
        Сумма предполагаемого штрафа,руб
        """
        return self.__amount
    
    @amount.setter
    def link(self, value):
        self.__amount = value     
               