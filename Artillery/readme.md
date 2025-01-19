\usepackage{amsmath}

# Разработка систем на основе PostgreSQL

#### Стек
1. Операционная система: [`Linux`](https://ubuntu.com/download/desktop), [`Docker`](https://docs.docker.com/get-started/get-docker/)
2. СУБД: [`PostgreSQL`](https://www.postgresql.org/download/) , [`postgrest`](https://docs.postgrest.org/en/v12/)


## Легенда
> Вы включены в команду целью которой состоит в разработке базы данных и **Backend** для системы подготовки метрологических данных в рамках разработки ПАК (Программно-аппаратного комплекса) согласно [**Технического задания**](./_Docs/TechnicalTask.md).<br><br>
> `Основаня задача команды` - это разработать решение которое должно соответствовать следующим критериям:<br>
>   * Решение должно работать в режиме как `SINGLE-USER` так и `MULTI-USER`
>   * В режиме `MULTI-USER`, количество одновременных пользователей до **50 000**
>   * Требуется обеспечить максимальную производительность при минимально производительном оборудовании
>   * Необходимо обеспечить максимально гибкий и простой вариант загрузки справочников и таблиц стрельбы

### Занятия
####  Подготовка (`Занятие 1`)
> Цель: Ознакомить слушателей с основными элементами SQL. 

1. [Запускаем Postgres в Docker среде](./_Infra)
2. Создаем локально базу данных `Meteo11`
3. Создаем первые справочники. Изучаем синтаксис запросов SQL

```sql
DO $$
BEGIN

-- Скрипт наполнения основных параметров
if not exists (select 1 from public.measurment_types where id = 1) then
	insert into public.measurment_types(id, short_name, description) values(1, 'ДМК', 'Десантный метео комплект');
end if;


if not exists (select 1 from public.measurment_types where id = 2) then
	insert into public.measurment_types(id, short_name, description) values(2, 'ВР', 'Ветровое ружье');
end if;

--select * from public.measurment_types

END$$;
```

**Задание:**
> Необходимо по аналогии добавить различные данные в таблицу `measurment_types` в том числе с одинаковым кодом.
> Удалить лишние данные. Оставить только две исходные записи.
> Скрипт с заданием сохранить в виде файла.

4. Подключаем `sequences`
```sql
DO $$
BEGIN
	
	-- Подключаем инкремент в таблицы справочники
	alter table public.measurement_batch alter column id set default nextval('public.measurment_batch_seq');
	alter table public.measurment_params alter column id set default nextval('public.measurment_params_seq');

END$$;
```

**Задание:**
> Необходимо выполнить вставку данных в таблицы `measurement_batch`, `measurment_params`. Далее, удалить и убедится, что `sequences` назначили новый код.
> Убрать `DEFAULT` значение в таблицах. Удалить `sequences`, создать заново и привязать
> Скрипт с заданием сохранить в виде файла.

5. Подключаем внешние ключи
```sql
DO $$
BEGIN

 -- Свяжем две таблицы measurment_params и measurement_batch
 alter table public.measurment_params 
 add constraint fk_measurment_param_measurement_batch_id foreign key (measurment_batch_id)
 references public.measurement_batch(id);

 -- Свяжем две таблицы measurment_params и measurment_types
 alter table public.measurment_params 
 add constraint fk_measurment_param_measurment_types foreign key (measurment_type_id)
 references public.measurment_types(id);
 
END$$;
```

**Задание:**
> Необходимо заполнить данными таблицу `measurment_params` с учетом связей.
> Необходимо через UI удалить связь между таблицей `fk_measurment_param_measurement_batch_id`. Далее, вставить некорректные данные в таблицу `measurment_params`
> Заново попробовать создать связь. Заменить некорреткные данные на `null` и создать связь.

#### Домашнее задание
1. Создать таблицу для учета пользователей системы. Предусмотреть справочник военных должностей. 
2. Изменить таблицу `measurement_batch` изменить поле `metrologist`. Сделать связь со справочником пользователей.
3. Оформить решение в виде скрипта и приложить `Screenshot` решения.


#### Интерполяция (`Занятие 2`)
> Цель: Ознакомить слушателей с основными приемами разработки pgSQL
1. Согласно [технического задания](./_Docs/TechnicalTask.md) необходимо разработать разработать алгоритм линейной интерполяции.
2. Добавим новую таблицу со справочником виртуальным поправок на температуру `calc_temperatures_correction` - **Таблица 1**
```sql
DO $$
BEGIN

	truncate table public.calc_temperatures_correction;
	insert into public.calc_temperatures_correction(temperature, correction)
	Values(0, 0.5),(5, 0.5),(10, 1), (20,1), (25, 2), (30, 3.5), (40, 4.5);

	-- select * from  public.calc_temperatures_correction;
END$$;
```
3. Создадим SQL запрос для расчета интерполяции по таблице `calc_temperatures_correction`. 
Формула:
> $D = X_{1} - X_{0}$ <br>
> $Result = (X - X_{0}) * (Y_{1} - Y_{0}) / D + Y_{0}$ <br>

Где:
 - $X_{1}$ - правая часть значения
 - $X_{0}$ - левая часть значения 
 - $Y_{1}$ - правая чась поправки
 - $Y_{0}$ - левая часть поправки

```sql
	-- Получим 5 градусов
	select Temperature from  public.calc_temperatures_correction
	where  Temperature < 8 order by Temperature desc limit 1;
```

**Задание:**
> Написать SQL запрос для получение $X_{1}$,  $X_{0}$,  $Y_{1}$ ,  $Y_{0}$ <br>
> Написать SQL запрос расчета линейной интерполяции <br>

3. Добавим функцию для расчета линейной интерполяции.
```sql
CREATE OR REPLACE FUNCTION public.fn_calc_temperatures_correction(
    -- Значение для которого нужно посчитать интерполяцию
	par_x numeric(8,2),
	par_x0 numeric(8,2),
	par_x1 numeric(8,2),
	par_y0 numeric(8,2),
	par_y1 numeric(8,2))
    RETURNS numeric
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE PARALLEL UNSAFE
AS $BODY$
declare
	denominator numeric(8,2) default 0;
	correction numeric(8,2) default 0;
begin
	denominator := par_x1 - par_x0;
	correction := (par_x - par_x0) * (par_y1 - par_y0) / denominator + par_y0;
	
	return correction;

end
$BODY$;
```
4. Меняем SQL запрос для получения  $X_{1}$,  $X_{0}$,  $Y_{1}$ ,  $Y_{0}$ с использованием функции.
Пример
```
select public.fn_calc_temperatures_correction(8, 5, 10, 0.5, 1);
```

**Задание**
> Добавить в функцию проверку аргументов. $X > X_{0} < X_{1}$
> $D$ - должно быть больше > 0

 
 

