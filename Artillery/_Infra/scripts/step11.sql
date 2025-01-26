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

ALTER FUNCTION public.fn_calc_temperature(input_data_batch)
    OWNER TO admin;
