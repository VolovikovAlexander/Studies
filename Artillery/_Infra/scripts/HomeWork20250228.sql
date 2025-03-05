do $$
begin

/*
Скрипт создания информационной базы данных
Согласно технического задания https://git.hostfl.ru/VolovikovAlex/Study2025
Редакция 2025-02-28
Edit by valex
*/


/*
 1. Удаляем старые элементы
 ======================================
 */

 drop view if exists vw_report_fails_statistics;
 drop view if exists vw_report_fails_height_statistics;

raise notice 'Запускаем создание новой структуры базы данных meteo'; 
begin

	-- Связи
	alter table if exists public.measurment_input_params
	drop constraint if exists measurment_type_id_fk;

	alter table if exists public.employees
	drop constraint if exists military_rank_id_fk;

	alter table if exists public.measurment_baths
	drop constraint if exists measurment_input_param_id_fk;

	alter table if exists public.measurment_baths
	drop constraint if exists emploee_id_fk;

	alter table if exists public.calc_height_correction
	drop constraint if exists measurment_type_id_fk;

	alter table if exists public.calc_temperature_height_correction
	drop constraint if exists calc_temperature_header_id_fk;

	alter table if exists public.calc_temperature_height_correction
	drop constraint if exists calc_height_id_fk;

	alter table if exists public.calc_header_correction
	drop constraint  if exists measurment_type_id_fk;
	
	-- Таблицы
	drop table if exists public.measurment_input_params;
	drop table if exists public.measurment_baths;
	drop table if exists public.employees;
	drop table if exists public.measurment_types;
	drop table if exists public.military_ranks;
	drop table if exists public.measurment_settings;
	drop table if exists public.calc_height_correction;
	drop table if exists public.calc_temperature_correction;
	drop table if exists public.calc_temperature_height_correction;
	drop table if exists public.calc_header_correction;

	-- Нумераторы
	drop sequence if exists public.measurment_input_params_seq cascade;
	drop sequence if exists public.measurment_baths_seq cascade;
	drop sequence if exists public.employees_seq cascade;
	drop sequence if exists public.military_ranks_seq cascade;
	drop sequence if exists public.measurment_types_seq cascade;
	drop sequence if exists public.calc_height_correction_seq cascade;
	drop sequence if exists public.calc_temperature_height_correction_seq cascade;
	drop sequence if exists public.calc_header_correction_seq cascade;
end;

raise notice 'Удаление старых данных выполнено успешно';

/*
 2. Добавляем структуры данных 
 ================================================
 */

-- Справочник должностей
create table military_ranks
(
	id integer primary key not null,
	description character varying(255)
);

insert into military_ranks(id, description)
values(1,'Рядовой'),(2,'Лейтенант');

create sequence military_ranks_seq start 3;

alter table military_ranks alter column id set default nextval('public.military_ranks_seq');

-- Пользователя
create table employees
(
    id integer primary key not null,
	name text,
	birthday timestamp ,
	military_rank_id integer not null
);

insert into employees(id, name, birthday,military_rank_id )  
values(1, 'Воловиков Александр Сергеевич','1978-06-24', 2);

create sequence employees_seq start 2;

alter table employees alter column id set default nextval('public.employees_seq');


-- Устройства для измерения
create table measurment_types
(
   id integer primary key not null,
   short_name  character varying(50),
   description text 
);

insert into measurment_types(id, short_name, description)
values(1, 'ДМК', 'Десантный метео комплекс'),
(2,'ВР','Ветровое ружье');

create sequence measurment_types_seq start 3;

alter table measurment_types alter column id set default nextval('public.measurment_types_seq');

-- Таблица с параметрами
create table measurment_input_params 
(
    id integer primary key not null,
	measurment_type_id integer not null,
	height numeric(8,2) default 0,
	temperature numeric(8,2) default 0,
	pressure numeric(8,2) default 0,
	wind_direction numeric(8,2) default 0,
	wind_speed numeric(8,2) default 0,
	bullet_demolition_range numeric(8,2) default 0
);

insert into measurment_input_params(id, measurment_type_id, height, temperature, pressure, wind_direction,wind_speed )
values(1, 1, 100,12,34,0.2,45);

create sequence measurment_input_params_seq start 2;

alter table measurment_input_params alter column id set default nextval('public.measurment_input_params_seq');

-- Таблица с историей
create table measurment_baths
(
		id integer primary key not null,
		emploee_id integer not null,
		measurment_input_param_id integer not null,
		started timestamp default now()
);


insert into measurment_baths(id, emploee_id, measurment_input_param_id)
values(1, 1, 1);

create sequence measurment_baths_seq start 2;

alter table measurment_baths alter column id set default nextval('public.measurment_baths_seq');

-- Таблица с настройками
create table measurment_settings
(
	key character varying(100) primary key not null,
	value  character varying(255) ,
	description text
);


insert into measurment_settings(key, value, description)
values('min_temperature', '-10', 'Минимальное значение температуры'), 
('max_temperature', '50', 'Максимальное значение температуры'),
('min_pressure','500','Минимальное значение давления'),
('max_pressure','900','Максимальное значение давления'),
('min_wind_direction','0','Минимальное значение направления ветра'),
('max_wind_direction','59','Максимальное значение направления ветра'),
('calc_table_temperature','15.9','Табличное значение температуры'),
('calc_table_pressure','750','Табличное значение наземного давления'),
('min_height','0','Минимальная высота'),
('max_height','400','Максимальная высота');


raise notice 'Создание общих справочников и наполнение выполнено успешно'; 

/*
 3. Подготовка расчетных структур
 ==========================================
 */

create table calc_temperature_correction
(
   temperature numeric(8,2) not null primary key,
   correction numeric(8,2) not null
);

insert into public.calc_temperature_correction(temperature, correction)
Values(0, 0.5),(5, 0.5),(10, 1), (20,1), (25, 2), (30, 3.5), (40, 4.5);

drop type  if exists interpolation_type;
create type interpolation_type as
(
	x0 numeric(8,2),
	x1 numeric(8,2),
	y0 numeric(8,2),
	y1 numeric(8,2)
);

-- Тип для входных параметров
drop type if exists input_params cascade;
create type input_params as
(
	height numeric(8,2),
	temperature numeric(8,2),
	pressure numeric(8,2),
	wind_direction numeric(8,2),
	wind_speed numeric(8,2),
	bullet_demolition_range numeric(8,2)
);

-- Тип с результатами проверки
drop type if exists check_result cascade;
create type check_result as
(
	is_check boolean,
	error_message text,
	params input_params
);

-- Результат расчета коррекций для температуры по высоте
drop type if exists temperature_correction cascade;
create type temperature_correction as
(
	calc_height_id integer,
	height integer,
	deviation integer
);



-- Таблица заголовков к поправочных таблицам
create sequence calc_header_correction_seq;
create table calc_header_correction
(
	id integer not null primary key default nextval('public.calc_header_correction_seq'),
	measurment_type_id integer not null,
	header varchar(100) not null,
	description text not null,
	values integer[] not null
);

-- Добавим уникальный индекс для отсечки ошибок
create unique index ix_calc_header_correction_header_type on calc_header_correction(measurment_type_id, header);

-- Добавим заголовки
insert into calc_header_correction(measurment_type_id, header, description, values) 
values (1, 'table2', 'Заголовок для Таблицы № 2 (ДМК)', array[0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 20, 30, 40, 50]),
       (2, 'table2','Заголовок для Таблицы № 2 (ВР)', array[0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 20, 30, 40, 50]),
	   (2, 'table3', 'Заголовок для Таблицы № 3 (ВР)', array[40,50,60,70,80,90,100,110,120,130,140,150]);


-- Таблица 2 список высот в разрезе типа оборудования
create sequence calc_height_correction_seq;
create table calc_height_correction
(
	id integer primary key not null default nextval('public.calc_height_correction_seq'),
	height integer not null,
	measurment_type_id integer not null
);

insert into calc_height_correction(height, measurment_type_id)
values(200,1),(400,1),(800,1),(1200,1),(1600,1),(2000,1),(2400,1),(3000,1),(4000,1),
	  (200,2),(400,2),(800,2),(1200,2),(1600,2),(2000,2),(2400,2),(3000,2),(4000,2);


-- Таблица 2 набор корректировок
create sequence calc_temperature_height_correction_seq;
create table calc_temperature_height_correction
(
	id integer primary key not null default nextval('public.calc_temperature_height_correction_seq'),
	calc_height_id integer not null,
	calc_temperature_header_id integer not null,
	positive_values numeric[],
	negative_values numeric[]
);

insert into calc_temperature_height_correction(calc_height_id, calc_temperature_header_id, positive_values, negative_values)
values
(1,1,array[1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 20, 30, 30, 30], array[ -1, -2, -3, -4, -5, -6, -7, -8, -8, -9, -20, -29, -39, -49]), --200
(2,1,array[1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 20, 30, 30, 30], array[-1, -2, -3, -4, -5, -6, -6, -7, -8, -9, -19, -29, -38, -48]), --400
(3,1,array[1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 20, 30, 30, 30], array[-1, -2, -3, -4, -5, -6, -6, -7, -7, -8, -18, -28, -37, -46]), --800
(4,1,array[1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 20, 30, 30, 30], array[-1, -2, -3, -4, -4, -5, -5, -6, -7, -8, -17, -26, -35, -44]), --1200
(5,1,array[ 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 20, 30, 30, 30], array[-1, -2, -3, -3, -4, -4, -5, -6, -7, -7, -17, -25, -34, -42]), --1600
(6,1,array[1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 20, 30, 30, 30], array[-1, -2, -3, -3, -4, -4, -5, -6, -6, -7, -16, -24, -32, -40]), --2000
(7,1,array[1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 20, 30, 30, 30], array[-1, -2, -2, -3, -4, -4, -5, -5, -6, -7, -15, -23, -31, -38]), --2400
(8,1,array[1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 20, 30, 30, 30], array[-1, -2, -2, -3, -4, -4, -4, -5, -5, -6, -15, -22, -30, -37]), --3000
(9,1,array[1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 20, 30, 30, 30], array[ -1, -2, -2, -3, -4, -4, -4, -4, -5, -6, -14, -20, -27, -34]); --4000
	
	  
raise notice 'Расчетные структуры сформированы';

/*
 4. Создание связей
 ==========================================
 */

begin 

	-- Связь между заголовком корректирующих таблиц и типом оборудования (один ко многим)
	alter table public.calc_header_correction
	add constraint  measurment_type_id_fk
	foreign key (measurment_type_id)
	references public.measurment_types(id);

	-- Связь между списком высот и типом оборудования (один ко многим)
	alter table public.calc_height_correction
	add constraint measurment_type_id_fk
	foreign key (measurment_type_id)
	references public.measurment_types(id);

	-- Связи между таблицей с корректировками и заголовками (один ко многим)
	alter table public.calc_temperature_height_correction
	add constraint calc_temperature_header_id_fk
	foreign key (calc_temperature_header_id)
	references public.calc_temperature_height_correction(id);

	alter table public.calc_temperature_height_correction
	add constraint calc_height_id_fk
	foreign key (calc_height_id)
	references public.calc_height_correction(id);

	-- Связь между пачками измерений с пользователями (один ко многим)
	alter table public.measurment_baths
	add constraint emploee_id_fk 
	foreign key (emploee_id)
	references public.employees (id);

	-- Связь между пачками измерений и измерениями (один к одному)
	alter table public.measurment_baths
	add constraint measurment_input_param_id_fk 
	foreign key(measurment_input_param_id)
	references public.measurment_input_params(id);

	-- Связь между измерениями и типом оборудования (один ко многим)
	alter table public.measurment_input_params
	add constraint measurment_type_id_fk
	foreign key(measurment_type_id)
	references public.measurment_types (id);

	-- Связь между пользователем и званием (один ко многим)
	alter table public.employees
	add constraint military_rank_id_fk
	foreign key(military_rank_id)
	references public.military_ranks (id);

end;

raise notice 'Связи сформированы';
raise notice 'Формируем индексы';

create index if not exists ix_measurment_baths_emploee_id on public.measurment_baths(emploee_id);
create index if not exists ix_measurment_baths_measurment_input_param_id on public.measurment_baths(measurment_input_param_id);

raise notice 'Индексы сформирован';

/*
 4. Создает расчетные и вспомогательные функции
 ==========================================
 */

-- Функция для расчета отклонения приземной виртуальной температуры
drop function if exists   public.fn_calc_header_temperature;
create function public.fn_calc_header_temperature(
	par_temperature numeric(8,2))
    returns numeric(8,2)
    language 'plpgsql'
as $BODY$
declare
	default_temperature numeric(8,2) default 15.9;
	default_temperature_key character varying default 'calc_table_temperature' ;
	virtual_temperature numeric(8,2) default 0;
	deltaTv numeric(8,2) default 0;
	var_result numeric(8,2) default 0;
begin	

	raise notice 'Расчет отклонения приземной виртуальной температуры по температуре %', par_temperature;

	-- Определим табличное значение температуры
	Select coalesce(value::numeric(8,2), default_temperature) 
	from public.measurment_settings 
	into virtual_temperature
	where 
		key = default_temperature_key;

    -- Вирутальная поправка
	deltaTv := par_temperature + 
		public.fn_calc_temperature_interpolation(par_temperature => par_temperature);
		
	-- Отклонение приземной виртуальной температуры
	var_result := deltaTv - virtual_temperature;
	
	return var_result;
end;
$BODY$;


-- Функция для формирования даты в специальном формате
drop function if exists public.fn_calc_header_period;
create function public.fn_calc_header_period(
	par_period timestamp with time zone)
    RETURNS text
    LANGUAGE 'sql'
    COST 100
    VOLATILE PARALLEL UNSAFE

RETURN ((((CASE WHEN (EXTRACT(day FROM par_period) < (10)::numeric) THEN '0'::text ELSE ''::text END || (EXTRACT(day FROM par_period))::text) || CASE WHEN (EXTRACT(hour FROM par_period) < (10)::numeric) THEN '0'::text ELSE ''::text END) || (EXTRACT(hour FROM par_period))::text) || "left"(CASE WHEN (EXTRACT(minute FROM par_period) < (10)::numeric) THEN '0'::text ELSE (EXTRACT(minute FROM par_period))::text END, 1));


-- Функция для расчета отклонения наземного давления
drop function if exists public.fn_calc_header_pressure;
create function public.fn_calc_header_pressure
(
	par_pressure numeric(8,2))
	returns numeric(8,2)
	language 'plpgsql'
as $body$
declare
	default_pressure numeric(8,2) default 750;
	table_pressure numeric(8,2) default null;
	default_pressure_key character varying default 'calc_table_pressure' ;
begin

	raise notice 'Расчет отклонения наземного давления для %', par_pressure;
	
	-- Определяем граничное табличное значение
	if not exists (select 1 from public.measurment_settings where key = default_pressure_key ) then
	Begin
		table_pressure :=  default_pressure;
	end;
	else
	begin
		select value::numeric(18,2) 
		into table_pressure
		from  public.measurment_settings where key = default_pressure_key;
	end;
	end if;

	
	-- Результат
	return par_pressure - coalesce(table_pressure,table_pressure) ;

end;
$body$;


-- Функция для проверки входных параметров
drop function if exists public.fn_check_input_params(numeric(8,2), numeric(8,2),   numeric(8,2), numeric(8,2), numeric(8,2), numeric(8,2));
create function public.fn_check_input_params(
	par_height numeric,
	par_temperature numeric,
	par_pressure numeric,
	par_wind_direction numeric,
	par_wind_speed numeric,
	par_bullet_demolition_range numeric)
    returns check_result
    language 'plpgsql'
as $body$
declare
	var_result public.check_result;
begin
	var_result.is_check = False;
	
	-- Температура
	if not exists (
		select 1 from (
				select 
						coalesce(min_temperature , '0')::numeric(8,2) as min_temperature, 
						coalesce(max_temperature, '0')::numeric(8,2) as max_temperature
				from 
				(select 1 ) as t
					cross join
					( select value as  min_temperature from public.measurment_settings where key = 'min_temperature' ) as t1
					cross join 
					( select value as  max_temperature from public.measurment_settings where key = 'max_temperature' ) as t2
				) as t	
			where
				par_temperature between min_temperature and max_temperature
			) then

			var_result.error_message := format('Температура % не укладывает в диаппазон!', par_temperature);
	end if;

	var_result.params.temperature = par_temperature;

	
	-- Давление
	if not exists (
		select 1 from (
			select 
					coalesce(min_pressure , '0')::numeric(8,2) as min_pressure, 
					coalesce(max_pressure, '0')::numeric(8,2) as max_pressure
			from 
			(select 1 ) as t
				cross join
				( select value as  min_pressure from public.measurment_settings where key = 'min_pressure' ) as t1
				cross join 
				( select value as  max_pressure from public.measurment_settings where key = 'max_pressure' ) as t2
			) as t	
			where
				par_pressure between min_pressure and max_pressure
				) then

			var_result.error_message := format('Давление %s не укладывает в диаппазон!', par_pressure);
	end if;

	var_result.params.pressure = par_pressure;			

		-- Высота
		if not exists (
			select 1 from (
				select 
						coalesce(min_height , '0')::numeric(8,2) as min_height, 
						coalesce(max_height, '0')::numeric(8,2) as  max_height
				from 
				(select 1 ) as t
					cross join
					( select value as  min_height from public.measurment_settings where key = 'min_height' ) as t1
					cross join 
					( select value as  max_height from public.measurment_settings where key = 'max_height' ) as t2
				) as t	
				where
				par_height between min_height and max_height
				) then

				var_result.error_message := format('Высота  %s не укладывает в диаппазон!', par_height);
		end if;

		var_result.params.height = par_height;
		
		-- Напрвление ветра
		if not exists (
			select 1 from (	
				select 
						coalesce(min_wind_direction , '0')::numeric(8,2) as min_wind_direction, 
						coalesce(max_wind_direction, '0')::numeric(8,2) as max_wind_direction
				from 
				(select 1 ) as t
					cross join
					( select value as  min_wind_direction from public.measurment_settings where key = 'min_wind_direction' ) as t1
					cross join 
					( select value as  max_wind_direction from public.measurment_settings where key = 'max_wind_direction' ) as t2
			)
				where
					par_wind_direction between min_wind_direction and max_wind_direction
			) then

			var_result.error_message := format('Направление ветра %s не укладывает в диаппазон!', par_wind_direction);
	end if;
			
	var_result.params.wind_direction = par_wind_direction;	
	var_result.params.wind_speed = par_wind_speed;

	if coalesce(var_result.error_message,'') = ''  then
		var_result.is_check = True;
	end if;	

	return var_result;
	
end;
$body$;

-- Функция для проверки параметров
drop function if exists public.fn_check_input_params(input_params);
create function public.fn_check_input_params(
	par_param input_params
)
returns public.input_params
language 'plpgsql'
as $body$
declare
	var_result check_result;
begin

	var_result := fn_check_input_params(
		par_param.height, par_param.temperature, par_param.pressure, par_param.wind_direction,
		par_param.wind_speed, par_param.bullet_demolition_range
	);

	if var_result.is_check = False then
		raise exception 'Ошибка %', var_result.error_message;
	end if;
	
	return var_result.params;
end ;
$body$;
		
-- Функция для расчета интерполяции
drop function if exists public.fn_calc_temperature_interpolation;
create function public.fn_calc_temperature_interpolation(
		par_temperature numeric(8,2))
		returns numeric
		language 'plpgsql'
as $body$
	-- Расчет интерполяции 
	declare 
			var_interpolation interpolation_type;
	        var_result numeric(8,2) default 0;
	        var_min_temparure numeric(8,2) default 0;
	        var_max_temperature numeric(8,2) default 0;
	        var_denominator numeric(8,2) default 0;
	begin

  				raise notice 'Расчет интерполяции для температуры %', par_temperature;

                -- Проверим, возможно температура совпадает со значением в справочнике
                if exists (select 1 from public.calc_temperature_correction where temperature = par_temperature ) then
                begin
                        select correction 
                        into  var_result 
                        from  public.calc_temperature_correction
                        where 
                                temperature = par_temperature;
                end;
                else    
                begin
                        -- Получим диапазон в котором работают поправки
                        select min(temperature), max(temperature) 
                        into var_min_temparure, var_max_temperature
                        from public.calc_temperature_correction;

                        if par_temperature < var_min_temparure or   
                           par_temperature > var_max_temperature then

                                raise exception 'Некорректно передан параметр! Невозможно рассчитать поправку. Значение должно укладываться в диаппазон: %, %',
                                        var_min_temparure, var_max_temperature;
                        end if;   

                        -- Получим граничные параметры

                        select x0, y0, x1, y1 
						 into var_interpolation.x0, var_interpolation.y0, var_interpolation.x1, var_interpolation.y1
                        from
                        (
                                select t1.temperature as x0, t1.correction as y0
                                from public.calc_temperature_correction as t1
                                where t1.temperature <= par_temperature
                                order by t1.temperature desc
                                limit 1
                        ) as leftPart
                        cross join
                        (
                                select t1.temperature as x1, t1.correction as y1
                                from public.calc_temperature_correction as t1
                                where t1.temperature >= par_temperature
                                order by t1.temperature 
                                limit 1
                        ) as rightPart;

                        raise notice 'Граничные значения %', var_interpolation;

                        -- Расчет поправки
                        var_denominator := var_interpolation.x1 - var_interpolation.x0;
                        if var_denominator = 0.0 then

                                raise exception 'Деление на нуль. Возможно, некорректные данные в таблице с поправками!';

                        end if;

						var_result := (par_temperature - var_interpolation.x0) * (var_interpolation.y1 - var_interpolation.y0) / var_denominator + var_interpolation.y0;

                end;
                end if;

				return var_result;

end;
$body$;

-- Функция для генерации случайной даты
drop function if exists fn_get_random_timestamp;
create function fn_get_random_timestamp(
	par_min_value timestamp,
	par_max_value timestamp)
returns timestamp
language 'plpgsql'
as $body$
begin
	 return random() * (par_max_value - par_min_value) + par_min_value;
end;
$body$;

-- Функция для генерации случайного целого числа из диаппазона
drop function if exists fn_get_randon_integer;
create function fn_get_randon_integer(
	par_min_value integer,
	par_max_value integer
	)
returns integer
language 'plpgsql'
as $body$
begin
	return floor((par_max_value + 1 - par_min_value)*random())::integer + par_min_value;
end;
$body$;

-- Функция для гнерации случайного текста
drop function if exists fn_get_random_text;
create function fn_get_random_text(
   par_length int,
   par_list_of_chars text DEFAULT 'АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯабвгдеёжзийклмнопрстуфхцчшщъыьэюяABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz_0123456789'
)
returns text
language 'plpgsql'
as $body$
declare
    var_len_of_list integer default length(par_list_of_chars);
    var_position integer;
    var_result text = '';
	var_random_number integer;
	var_max_value integer;
	var_min_value integer;
begin

	var_min_value := 10;
	var_max_value := 50;
	
    for var_position in 1 .. par_length loop
        -- добавляем к строке случайный символ
	    var_random_number := fn_get_randon_integer(var_min_value, var_max_value );
        var_result := var_result || substr(par_list_of_chars,  var_random_number ,1);
    end loop;
	
    return var_result;
	
end;
$body$;


-- Функция для расчета метео приближенный
drop function if exists fn_calc_header_meteo_avg;
create function fn_calc_header_meteo_avg(
	par_params input_params
)
returns text
language 'plpgsql'
as $body$
declare
	var_result text;
	var_params input_params;
begin

	-- Проверяю аргументы
	var_params := public.fn_check_input_params(par_params);
	
	select
		-- Дата
		public.fn_calc_header_period(now()) ||
		--Высота расположения метеопоста над уровнем моря.
	    lpad( 340::text, 4, '0' ) ||
		-- Отклонение наземного давления атмосферы
		lpad(
				case when coalesce(var_params.pressure,0) < 0 then
					'5' 
				else ''
				end ||
				lpad ( abs(( coalesce(var_params.pressure, 0) )::int)::text,2,'0')
			, 3, '0') as "БББ",
		-- Отклонение приземной виртуальной температуры	
		lpad( 
				case when coalesce( var_params.temperature, 0) < 0 then
					'5'
				else
					''
				end ||
				( coalesce(var_params.temperature,0)::int)::text
			, 2,'0')
		into 	var_result;
	return 	var_result;

end;
$body$;

-- Процедура для расчета поправок по температуре в разрезе высот
create procedure public.sp_calc_temperature_deviation(
	in par_temperature_correction numeric(8,2),
	in par_measurement_type_id integer,
	inout par_corrections temperature_correction[]
	)
language 'plpgsql'
as $BODY$
declare
	var_row record;
	var_index integer;
	var_header_correction integer[];
	var_right_index integer;
	var_left_index integer;
	var_header_index integer;
	var_deviation integer;
	var_table integer[];
	var_correction temperature_correction;
	var_table_row text;
begin

-- Проверяем наличие данные в таблице
if not exists ( 
	
		select 1
			from public.calc_height_correction as t1
			inner join public.calc_temperature_height_correction as t2 
				on t2.calc_height_id = t1.id
			where 
					measurment_type_id = par_measurement_type_id

			) then
			
		raise exception 'Для расчета поправок к температуре не хватает данных!';
	
	end if;


	raise notice '| Высота   | Поправка  |';
	raise notice '|----------|-----------|';

	for var_row in 
			-- Запрос на выборку высот
			select t2.*, t1.height 
			from public.calc_height_correction as t1
			inner join public.calc_temperature_height_correction as t2 
				on t2.calc_height_id = t1.id
			where measurment_type_id = par_measurement_type_id
		loop
			-- Получаем индекс корректировки
			var_index := par_temperature_correction::integer;
			-- Получаем заголовок 
			var_header_correction := (select values from public.calc_header_correction
				where id = var_row.calc_temperature_header_id and header = 'table2');

			-- Проверяем данные
			if array_length(var_header_correction, 1) = 0 then
				raise exception 'Невозможно произвести расчет по высоте % Некорректные исходные данные или настройки',  var_row.height;
			end if;

			if array_length(var_header_correction, 1) < var_index then
				raise exception 'Невозможно произвести расчет по высоте % Некорректные исходные данные или настройки',  var_row.height;
			end if;

			-- Получаем левый и правый индекс
			var_right_index := abs(var_index % 10);
			var_header_index := abs(var_index) - var_right_index;

			-- Определяем корретировки
			if par_temperature_correction >= 0 then
				var_table := var_row.positive_values;
			else
				var_table := var_row.negative_values;
			end if;

			if 	var_header_index = 0 then
				var_header_index := 1;
			end if;

			var_left_index := var_header_correction[ var_header_index];
			if var_left_index = 0 then
				var_left_index := 1;
			end if;	

			-- Поправка на высоту	
			var_deviation:= var_table[ var_left_index  ] + var_table[ var_right_index     ];

			select '|' || lpad(var_row.height::text,10, ' ') || '|' || lpad(var_deviation::text,11,' ') || '|'
			into
				var_table_row;
				
			raise notice '%', var_table_row;

			var_correction.calc_height_id := var_row.calc_height_id;
			var_correction.height := var_row.height;
			var_correction.deviation := var_deviation;
			par_corrections := array_append(par_corrections, var_correction);
	end loop;

	raise notice '|----------|-----------|';

end;
$BODY$;



raise notice 'Структура сформирована успешно';
end $$;

-- //////////////////////////////////////////////////////////////////////////////
-- Проверка расчета
do $$
declare
	var_pressure_value numeric(8,2) default 0;
	var_temperature_value numeric(8,2) default 0;
	
	var_period text;
	var_pressure text;
	var_height text;
	var_temperature text;
begin

	var_pressure_value :=  round(public.fn_calc_header_pressure(743));
	var_temperature_value := round( public.fn_calc_header_temperature(3));
	
	select
		-- Дата
		public.fn_calc_header_period(now()) as "ДДЧЧМ",
		--Высота расположения метеопоста над уровнем моря.
	    lpad( 340::text, 4, '0' ) as "ВВВВ",
		-- Отклонение наземного давления атмосферы
		lpad(
				case when var_pressure_value < 0 then
					'5' 
				else ''
				end ||
				lpad ( abs((var_pressure_value)::int)::text,2,'0')
			, 3, '0') as "БББ",
		-- Отклонение приземной виртуальной температуры	
		lpad( 
				case when var_temperature_value < 0 then
					'5'
				else
					''
				end ||
				(abs(var_temperature_value)::int)::text
			, 2,'0') as "TT"
		into
			var_period, var_height, var_pressure, var_temperature;

		raise notice '==============================';
		raise notice 'Пример расчета метео приближенный';
		raise notice 'ДДЧЧМ %, ВВВВ %,  БББ % , TT %', 	var_period, var_height, var_pressure, var_temperature;
		
end $$;


-- Проверка входных параметров
do $$
declare 
	var_result public.check_result;
begin

	raise notice '=====================================';
	raise notice 'Проверка работы функции [fn_check_input_params]';
	raise notice 'Положительный сценарий';
	
	-- Корректный вариант
	var_result := public.fn_check_input_params(par_height => 400, par_temperature => 23, par_pressure => 740, par_wind_direction => 5, par_wind_speed => 5, par_bullet_demolition_range => 5 );
			
	if var_result.is_check != True then
		raise notice '-> Проверка не пройдена!';
	else
		raise notice '-> Проверка пройдена';
	end if;

	raise notice 'Сообщени: %', var_result.error_message;

	-- Некорректный вариант
	var_result := public.fn_check_input_params(par_height => -400, par_temperature => 23, par_pressure => 740, par_wind_direction => 5, par_wind_speed => 5, par_bullet_demolition_range => 5 );
	raise notice 'Отрицательный сценарий';

	if var_result.is_check != False then
		raise notice '-> Проверка не пройдена!';
	else
		raise notice '-> Проверка пройдена';
	end if;

	
	raise notice 'Сообщение: %', var_result.error_message;

	raise notice '=====================================';
	
end $$;

-- Проверка расчета поправок по высоте для температуры
do $$
declare
	var_corrections public.temperature_correction[];
begin
	raise notice 'Проверка расчета поправок к температуре по высоте [sp_calc_temperature_deviation]';

	call public.sp_calc_temperature_deviation( par_temperature_correction => 3::numeric,  par_measurement_type_id => 1::integer, 
			par_corrections => var_corrections::public.temperature_correction[]);
	raise notice 'Результат расчета для корректировки 3 и типа оборудования 1: % ',  var_corrections;
	raise notice '=====================================';
end $$;

-- Генерация тестовых данных
do $$
declare
	 var_position integer;
	 var_emploee_ids integer[];
	 var_emploee_quantity integer default 5;
	 var_min_rank integer;
	 var_max_rank integer;
	 var_emploee_id integer;
	 var_current_emploee_id integer;
	 var_index integer;
	 var_measure_type_id integer;
	 var_measure_input_data_id integer;
begin

	-- Определяем макс дипазон по должностям
	select min(id), max(id) 
	into var_min_rank,var_max_rank
	from public.military_ranks;

	
	-- Формируем список пользователей
	for var_position in 1 .. var_emploee_quantity loop
		insert into public.employees(name, birthday, military_rank_id )
		select 
			fn_get_random_text(25),								-- name
			fn_get_random_timestamp('1978-01-01','2000-01-01'), 				-- birthday
			fn_get_randon_integer(var_min_rank, var_max_rank)  -- military_rank_id
			;
		select id into var_emploee_id from public.employees order by id desc limit 1;	
		var_emploee_ids := var_emploee_ids || var_emploee_id;
	end loop;

	raise notice 'Сформированы тестовые пользователи  %', var_emploee_ids;

	-- Формируем для каждого по 100 измерений
	foreach var_current_emploee_id in ARRAY var_emploee_ids LOOP
		for var_index in 1 .. 100 loop
			var_measure_type_id := fn_get_randon_integer(1,2);
			
			insert into public.measurment_input_params(measurment_type_id, height, temperature, pressure, wind_direction, wind_speed)
			select
				var_measure_type_id,
				fn_get_randon_integer(0,600)::numeric(8,2), -- height
				fn_get_randon_integer(0, 50)::numeric(8,2), -- temperature
				fn_get_randon_integer(500, 850)::numeric(8,2), -- pressure
				fn_get_randon_integer(0,59)::numeric(8,2), -- ind_direction
				fn_get_randon_integer(0,59)::numeric(8,2)	-- wind_speed
				;

			select id into var_measure_input_data_id from 	measurment_input_params order by id desc limit 1;
				
			insert into public.measurment_baths( emploee_id, measurment_input_param_id, started)
			select
				var_current_emploee_id,
				var_measure_input_data_id,
				fn_get_random_timestamp('2025-02-01 00:00', '2025-02-05 00:00')
			;	
		end loop;
	
	end loop;

	raise notice 'Набор тестовых данных сформирован успешно';
	
end $$;


create  view vw_report_fails_statistics
as
-- Отчет:  Статистика ошибок при проведении измерений
-- ФИО, должность, количество измерений, количество ошибок
with emploee_cte as
(
		-- ФИО  | Должность
		select 
			t1.id , t1.name as user_name, t2.description as position
			from public.employees as t1
			inner join public.military_ranks as t2 on t1.military_rank_id = t2.id
),
measurements_cte as
(
		--  Кол-во измерений
		select count(*) as quantity,  emploee_id 
		from public.measurment_input_params as t1
		inner join public.measurment_baths as t2 on t2.measurment_input_param_id = t1.id
		group by emploee_id
), 
fails_cte as
(
		-- Количество ошибочных данных
		select 
			count(*) as quantity_fails,
			emploee_id 
		from public.measurment_input_params as t1
		inner join public.measurment_baths as t2 on t2.measurment_input_param_id = t1.id
		where
			(public.fn_check_input_params(height, temperature, pressure, wind_direction, wind_speed, bullet_demolition_range)::public.check_result).is_check = False
		group by emploee_id
)
-- Основной запрос
select 
	user_name, position, coalesce(quantity,0) as quantity, coalesce(quantity_fails,0) as quantity_fails
from emploee_cte as t1
left join measurements_cte as t2 on t1.id = t2.emploee_id
left join fails_cte as t3 on t1.id = t3.emploee_id
order by coalesce(quantity_fails,0) desc;


create view vw_report_fails_height_statistics
as
-- Отчет: "Самая эффективная высота измерения"
-- | ФИО пользователя | Звание | Мин. высота метеопоста | Макс. высота метепоста | Всего измерений | Из них ошибочны |
with emploee_cte as
(
		-- ФИО  | Должность
		select 
			t1.id , t1.name as user_name, t2.description as position
			from public.employees as t1
			inner join public.military_ranks as t2 on t1.military_rank_id = t2.id
),
measurements_cte as
(
		--  Кол-во измерений
		select count(*) as quantity,  emploee_id 
		from public.measurment_input_params as t1
		inner join public.measurment_baths as t2 on t2.measurment_input_param_id = t1.id
		group by emploee_id
), 
fails_cte as
(
		-- Количество ошибочных данных
		select 
			count(*) as quantity_fails,
			emploee_id 
		from public.measurment_input_params as t1
		inner join public.measurment_baths as t2 on t2.measurment_input_param_id = t1.id
		where
			(public.fn_check_input_params(height, temperature, pressure, wind_direction, wind_speed, bullet_demolition_range)::public.check_result).is_check = False
		group by emploee_id
),
measurments_height_cte as
(
		--  Статистика по высотам
		select min(height) as min_height, max(height) as max_height,  emploee_id 
		from public.measurment_input_params as t1
		inner join public.measurment_baths as t2 on t2.measurment_input_param_id = t1.id
		group by emploee_id
)
-- Основной запрос
select 
	user_name, position, coalesce(quantity,0) as quantity, coalesce(quantity_fails,0) as quantity_fails, 
		coalesce(min_height, 0) as min_height, coalesce(max_height, 0) as max_height 
from emploee_cte as t1
left join measurements_cte as t2 on t1.id = t2.emploee_id
left join fails_cte as t3 on t1.id = t3.emploee_id
left join measurments_height_cte as t4 on t1.id = t4.emploee_id
order by coalesce(quantity_fails,0) asc, coalesce(min_height, 0) asc ;

-- Проверка отчета
select * from vw_report_fails_height_statistics






