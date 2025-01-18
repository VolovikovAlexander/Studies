DO $$
BEGIN
	
	-- Подключаем инкремент в таблицы справочники
	alter table public.measurement_batch alter column id set default nextval('public.measurment_batch_seq');
	alter table public.measurment_params alter column id set default nextval('public.measurment_params_seq');

END$$;