do $$
declare
   var_min_temperature numeric(8,2);
   var_max_temperature numeric(8,2);
   var_step numeric(8,2) default 0.01;
   var_current_temperature numeric(8,2) ;
   var_calc_param public.input_data_meteo11;
   var_correction numeric(8,2);
   var_quantity integer default 1;
	   var_start_period timetsamp

begin

	begin
		var_start_period := now()
	end
	
	-- Получить период
	select min(temperature), max(temperature) 
	into var_min_temperature, var_max_temperature
	from public.calc_temperatures_correction;

	-- Рассчитать интерполяцию
	for var_current_temperature IN var_min_temperature..var_max_temperature loop
	begin
		var_calc_param.temperature := var_current_temperature;
		var_correction := public.fn_calc_temperature(var_calc_param);
		RAISE NOTICE 'temperature %, correction %', var_current_temperature, var_correction;
		var_current_temperature := var_current_temperature + var_step;

		var_quantity := var_quantity + 1;
	end;
	end loop;


	Raise notice 'quantity %', var_quantity;
end$$