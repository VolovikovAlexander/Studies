/*
 5. Пример скрипты для расчета интерполяции
 ==========================================
 */

 
 do $$
 declare 
 	var_interpolation interpolation_type;
	var_temperature integer default 22;
	var_result numeric(8,2) default 0;
	var_min_temparure numeric(8,2) default 0;
	var_max_temperature numeric(8,2) default 0;
	var_denominator numeric(8,2) default 0;
 begin
		raise notice 'Расчет интерполяции для температуры %', var_temperature;

		-- Проверим, возможно температура совпадает со значением в справочнике
		if exists (select 1 from public.calc_temperatures_correction where temperature = var_temperature ) then
		begin
			select correction 
			into  var_result 
			from  public.calc_temperatures_correction
			where 
				temperature = var_temperature;
		end;
		else	
		begin
			-- Получим диапазон в котором работают поправки
			select min(temperature), max(temperature) 
			into var_min_temparure, var_max_temperature
			from public.calc_temperatures_correction;

			if var_temperature < var_min_temparure or   
			   var_temperature > var_max_temperature then

				raise exception 'Некорректно передан параметр! Невозможно рассчитать поправку. Значение должно укладываться в диаппазон: %, %',
					var_min_temparure, var_max_temperature;
			end if;   

			-- Получим граничные параметры

			select x0, y0, x1, y1 
			into var_interpolation.x0, var_interpolation.y0, var_interpolation.x1, var_interpolation.y1
			from
			(
				select t1.temperature as x0, t1.correction as y0
				from public.calc_temperatures_correction as t1
				where t1.temperature <= var_temperature
				order by t1.temperature desc
				limit 1
			) as leftPart
			cross join
			(
				select t1.temperature as x1, t1.correction as y1
				from public.calc_temperatures_correction as t1
				where t1.temperature >= var_temperature
				order by t1.temperature 
				limit 1
			) as rightPart;
			
			raise notice 'Граничные значения %', var_interpolation;

			-- Расчет поправки
			var_denominator := var_interpolation.x1 - var_interpolation.x0;
			if var_denominator = 0.0 then

				raise exception 'Деление на нуль. Возможно, некорректные данные в таблице с поправками!';
			
			end if;
			
                       var_result := (var_temperature - var_interpolation.x0) * (var_interpolation.y1 - var_interpolation.y0) / var_denominator + var_interpolation.y0;
		
		end;
		end if;

	        raise notice 'Результат: %', var_result;

 end $$;
