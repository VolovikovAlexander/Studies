do $$
declare 
	input_data input_data_meteo11;
	
begin
	-- Пример для проверки расчета метосредний
	input_data.pressure = 765;
	input_data.temperature = 25;
	input_data.height = 100;
	input_data.device_type_id = 2;
	input_data.wind_speed = 6;
	input_data.wind_direct = 15;


	RAISE NOTICE 'fn_calc_header %',  fn_calc_header(input_data);

end $$
