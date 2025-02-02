-- FUNCTION: public.fn_calc_pressure(input_data_meteo11)

-- DROP FUNCTION IF EXISTS public.fn_calc_pressure(input_data_meteo11);

CREATE OR REPLACE FUNCTION public.fn_calc_pressure(
	input_data input_data_meteo11)
    RETURNS numeric
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE PARALLEL UNSAFE
AS $BODY$
declare
	default_pressure numeric(8,2) default 750;
	table_pressure numeric(8,2) default null;
begin

	if(input_data.pressure <= 0) then
	  	RAISE EXCEPTION 'Некорректно передан параметр. Давление <= 0!'; 
	end if;
	
	return  fn_calc_pressure(input_data.pressure);

end
$BODY$;

ALTER FUNCTION public.fn_calc_pressure(input_data_meteo11)
    OWNER TO admin;
