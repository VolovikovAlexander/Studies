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


