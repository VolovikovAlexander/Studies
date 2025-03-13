create or replace function fn_tr_temp_input_params()
returns trigger as
$$
declare 
	var_check_result public.check_result_type;
	var_input_params public.input_params_type;
	var_response public.calc_result_response_type;
	var_calc_result public.calc_result_type[];
begin

	-- Проверяем параметры
	var_check_result := fn_check_input_params(  
				NEW.height,
				NEW.temperature,
				NEW.pressure,
				NEW.wind_direction,
				NEW.wind_speed,
				NEW.bullet_demolition_range
			);

	if var_check_result.is_check = False then
		raise notice 'error %',  var_check_result.error_message;
		NEW.error_message := var_check_result.error_message;
		return NEW;
	
	end if;

	var_input_params := var_check_result.params;

	-- Формируем заговок
	var_response.header := public.fn_calc_header_meteo_avg(var_input_params);

	-- Формируем расчет
	call public.sp_calc_corrections(par_input_params => var_input_params, 
		par_measurement_type_id => NEW.measurment_type_id, 
		par_results => var_calc_result);
		
	var_response.calc_result := var_calc_result;
	
	-- Запоминаем результат
	NEW.calc_result = row_to_json(var_response);
	return NEW;
end;
$$
LANGUAGE plpgsql;
