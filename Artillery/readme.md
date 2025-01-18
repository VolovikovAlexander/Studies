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
####  Подготовка
1. [Запускаем Postgres в rjyntqytht](./_Infra)
2. Создаем локально базу данных `Meteo11`
3. Создаем первые справочники. Изучаем синтаксис запросов SQL

```
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
```
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
```
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

