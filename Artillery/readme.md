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

## Занятия
###  Подготовка (`Занятие 1`)
> Цель: Ознакомить слушателей с основными элементами SQL. 

1. [Запускаем Postgres в Docker среде](./_Infra)
- Принципы ACID (Принципы ACID предоставили разработчикам и пользователям стандартизированный подход к пониманию и обеспечению надёжности транзакций)

**Atomicity (Атомарность)** - Транзакция должна выполняться целиком либо не выполняться вовсе.
**Consistency (Целостность)** - После завершения транзакции данные должны быть согласованными.
**Isolation (Изолированность)** - Одновременно выполняющиеся транзакции не должны влиять друг на друга.
**Durability (Долговечность)** - После завершения транзакции результаты должны быть сохранены и иметь возможность восстановиться после сбоя

2. Краткий теоретический материал
```
DDL, DML, DCL, TCL
```

**DDL** - Data Definition Language, управление структурой базы данных
**DML** - Data Manipulation Language, управление данными
**DCL** - Data Control Language. управление доступом
**TCL** - Transaction Control Language, управление транзакциями

3. Создаем пробные таблицы. Учимся работыть с DDL командами
```sql
create table public.table1
(
    id bigint NOT NULL ,
    foreign_key bigint NOT NULL,
	CONSTRAINT table1_pkey PRIMARY KEY (id)
);

create table public.table2
(
	id bigint NOT NULL,
	description text,
	CONSTRAINT table2_pkey PRIMARY KEY (id)
);


```

4. Добавляем пробные записи. Учимся работать с DML командами

**Задание**
> Самостоятельно при прмощи команд DDL создать структуру базы данных для текущего решения. <br>
> История измерений, Набор параметров, Типы измерителей <br>

5. Создаем локально базу данных `Meteo11`
	* measurment_params
	* measurment_types
	* measurement_batch
 

6. Начало работы с PgSQL. Пакетные скрипты 

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


### Интерполяция (`Занятие 2`)
> Цель: Ознакомить слушателей с основными приемами разработки pgSQL и реализовать функцию линейной интерполяции
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
Пример:
```sql
select public.fn_calc_temperatures_correction(8, 5, 10, 0.5, 1);
```

**Задание**
> Добавить в функцию проверку аргументов. $X > X_{0} < X_{1}$ <br>
> $D$ - должно быть больше > 0 <br>
> Провери должны формировать [raise error](https://www.postgresql.org/docs/current/plpgsql-errors-and-messages.html)

5. Создаем собственный тип данных. 
```sql
CREATE TYPE public.interpolation_batch AS
(
	x0 numeric(8,2),
	x1 numeric(8,2),
	y0 numeric(8,2),
	y1 numeric(8,2)
);
```

**Задание:**
> Создать функцию `fn_calc_temperatures_correction` с использованием типа `interpolation_batch`  <br>
> Изменить текущую реализацию текущей функции `fn_calc_temperatures_correction` <br>

#### Домашнее задание
1. Создать таблицу с настройками для проверки входных данных. В рамках данной таблицы нужно хранить **все** `константы`
- `Температура`. Минимальное значение **-58**, максимальное **58**, указывается в цельсиях
- `Давления`.  Минимальное значение **500**, максимальное **900**, указывается в мм рт ст
- `Направление ветра`. Минимальное значение **0**,максимальное значение **59** <br>
  и т.д.

2. Создать собственный тип данных для передахи входных параметров 
3. Написать собственную функцию на вход должны подаваться входные параметры, а на выходе собственный тип данных.
4. Функция должна проверять входные параметры. При нарушении граничных параметров формировать [raise error](https://www.postgresql.org/docs/current/plpgsql-errors-and-messages.html)

###  Приближенный (`Занятие 3`)
> Цель: Реализовать расчет данных заголовка Метео-11
1. Создаем SQL функцию для преобразования даты времени в строковое значение `ДДЧЧММ` 
```sql

CREATE OR REPLACE FUNCTION public.fn_calc_period(
	par_period timestamp with time zone)
    RETURNS text
    LANGUAGE 'sql'
    COST 100
    VOLATILE PARALLEL UNSAFE

RETURN  extract(day from par_period)::text || extract(hour from par_period)::text ||  extract(minute from par_period):: text;

ALTER FUNCTION public.fn_calc_period(timestamp with time zone)
    OWNER TO admin;
```

Пример
```sql
select fn_calc_period(CURRENT_TIMESTAMP);
select public.fn_calc_period(now());
```

**Задание:**
> Измеить функцию. Добавить проверку по дню < 10 и минуты < 10.

2. Создаем SQL функция для расчета `ВВВВ` - высота расположения метеопоста над уровнем моря. Добавить все проверки и ограничения.
```sql
select LPAD(40::TEXT, 4, '0')
```
3. Создаем таблицу для хранения констант для расчетов `measurment_settings`
```sql
CREATE TABLE IF NOT EXISTS public.measurment_settings
(
    key character varying(100) COLLATE pg_catalog."default" NOT NULL,
    value character varying(255) COLLATE pg_catalog."default",
    description text COLLATE pg_catalog."default",
    CONSTRAINT measurment_settings_pkey PRIMARY KEY (key)
)

TABLESPACE pg_default;
```
4. Добавляем данные в таблицу `measurment_settings`
```sql
truncate table public.measurment_settings;
insert into public.measurment_settings(key, value, description)
values('temperature_15','15.9','Табличное значение наземной температуры'),
('pressure_750','750','Табличное значения наземного давления');
```
5. Создаем таблицу `ref_devices_type` - список устройств измерения метрологических данных
```sql
insert into public.ref_devices_type(short_name, description)
values('ВР','Ветровое ружье'),
('ДМК','Десантно метео комплекс');
```
4. Создаем тип данных `input_data_batch`

```sql
CREATE TYPE public.input_data_batch AS
(
	device_type_id bigint,
	height numeric(8,2),
	temperature numeric(8,2),
	pressure numeric(8,2),
	wind_speed numeric(8,2)
);
```

5. Создаем функцию для расчета `Отклонение наземной виртуальной температуры от табличного` ($ΔT_{0}^{мп}$) - fn_calc_temperature

```sql
CREATE OR REPLACE FUNCTION public.fn_calc_temperature(
	input_data input_data_batch)
    RETURNS numeric
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE PARALLEL UNSAFE
AS $BODY$
DECLARE
	default_temperature numeric(8,2) default 15.9;
	default_temperature_key character varying default 'temperature_15' ;
	virtual_temperature numeric(8,2) default 0;
	temperature_batch interpolation_batch;
	deltaTv numeric(8,2) default 0;
	t0 numeric(8,2) default 0;
	"result" numeric(8,2) default 0;
BEGIN	

	-- Определим табличное значение температуры
	Select coalesce(value::numeric(8,2), default_temperature) 
	from public.measurment_settings 
	into virtual_temperature
	where 
		key = default_temperature_key;

	RAISE NOTICE 'virtual_temperature, %', virtual_temperature;	

	-- Определяем виртуальную поправку 
	-- 1. Определяем левый диаппазон
	select
		temperature,
		correction
	into 	
		temperature_batch.x0, 
		temperature_batch.y0
	from public.calc_temperatures_correction
	where 
		temperature <= input_data.temperature
	order by 	temperature desc
	limit 1;

	
	-- 2. Определяем правый диаппазон
	select
		temperature,
		correction
	into 	
		temperature_batch.x1, 
		temperature_batch.y1
	from public.calc_temperatures_correction
	where 
		temperature >= input_data.temperature
	order by 	temperature 
	limit 1;

	RAISE NOTICE 'temperature_batch batch, %', temperature_batch;
	RAISE NOTICE 'Correct %', public.fn_calc_temperatures_correction(par_x => input_data.temperature, par_batch => temperature_batch);
	
	
    -- Вирутальная поправка
	deltaTv := input_data.temperature + 
		public.fn_calc_temperatures_correction(par_x => input_data.temperature, par_batch => temperature_batch);


	-- Отклонение приземной виртуальной температуры
	"result" := deltaTv - virtual_temperature;
	
	RETURN "result";
END	
$BODY$;
```

**Задание:**
> Создать функцию для расчета отклонения наземного давления $ΔНо$ - `fn_calc_pressure`<br>

**Входные данные*
```sql
do $$
declare
	input_data public.input_data_batch;
	"result" numeric(8,2) default 0;
begin
	input_data.device_type_id := 1;
	input_data.height := 2300;
	input_data.temperature := 23;
	input_data.pressure := 760;
	input_data.wind_speed := 20;


	select public.fn_calc_temperature(input_data => input_data)
	into "result";
	
	raise notice 'result %', "result";
	
end $$
```


#### Домашнее задание
1. Создать функцию которая сформирует заголовок в формте `ДДЧЧМ ВВВВ БББТТ` согласно техническому заданию <br>
2. Написать `pgSQL` скрипт для проверки работы данной функции. Примеры брать из [технического задания](./_Docs/TechnicalTask.md)
3. Перенести все настройки в таблицу `measurment_settings`. Написать скрипт переноса и сделать рефакторинг <br> 
> 1. Создать таблицу с настройками для проверки входных данных. В рамках данной таблицы нужно хранить **все** `константы`

###  Подготовка к расчетам (`Занятие 4`)
> Цель: Подготовить таблицы и структуры для дальнейшей реализации расчетов Метео-11
1. Создадим таблицу для описания списка высот 'default_calc_heights'

```sql

CREATE TABLE IF NOT EXISTS public.default_calc_heights
(
    id bigint NOT NULL DEFAULT nextval('seq_default_calc_heights'::regclass),
    height bigint NOT NULL,
    CONSTRAINT default_calc_heights_pkey PRIMARY KEY (id)
)

```

2. Создадим таблицу `default_calc_temperature_corrections` (**Таблица 1**) для хранения информации о [среднем отклонении температуры воздуха](./_Docs/AlgoritmDmk.md)
```sql
CREATE TABLE IF NOT EXISTS public.default_calc_temperature_corrections
(
    default_calc_height_id bigint NOT NULL,
    data bigint[] NOT NULL,
    is_positive boolean NOT NULL DEFAULT true
)
```
3. Добавим связь между таблицами и ограничения по первичному ключу - **составной ключ** (`default_calc_height_id`, `is_positive`)
4. Создадим скрипт для наполнения данных в таблицы: `default_calc_heights`, `default_calc_temperature_corrections`
```sql
-- Список табличных высот
insert into public.default_calc_heights(height) values(200), (400), (800), (1200), (1600), (2000), (2400), (3000), (4000);
-- select * from default_calc_heights

-- Положительные значения для высоты 200
insert into public.default_calc_temperature_corrections(default_calc_height_id, data, is_positive)
values(1, array[0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 20, 30, 30, 30], True);
-- select * from default_calc_temperature_corrections
```

**Задание:**
> Дописать скрипт для добавления записей в таблицу `default_calc_temperature_corrections`. Положительные и отрицательные значения. <br>
> Написать SQL запрос на удаление любой высоты. Продемонстрировать результат.<br>

5. Добавим связь с типом измерительного оборудования, таблица `ref_devices_type` и обновим текущие записи в таблице `default_calc_temperature_corrections`

#### Домашнее задание
1. Дописать pgSQL скрипт. Включить в него создание таблиц и наполнения для всех типов устройств
2. Создать таблицу для учета [`Скорости среднего ветра`](./Docs/AlgoritmBp.md)(**Таблица 3**) для расчета с помощью ветрового ружья
 

 






 
 

