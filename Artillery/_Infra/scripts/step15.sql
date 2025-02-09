-- FUNCTION: public.fn_calc_header(input_data_meteo11)

-- DROP FUNCTION IF EXISTS public.fn_calc_header(input_data_meteo11);

CREATE OR REPLACE FUNCTION public.fn_calc_header(
	input_data input_data_meteo11)
    RETURNS text
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE PARALLEL UNSAFE
AS $BODY$
declare 
	head_temperature numeric(8,2) default 0;
	head_pressure numeric(8,2) default 0;
	head_period text default '';
	head_height text default ''; 
	
begin

    -- БББТТ
	head_pressure := round(public.fn_calc_pressure(input_data));
	head_temperature	:= round(public.fn_calc_temperature(input_data));
	-- ДДЧЧМ
	head_period := public.fn_calc_period(now());
	-- ВВВВ
	head_height := lpad( (input_data.height::int)::text, 4, '0' );
	
	return 
		head_period  || ',' ||
		head_height || ',' ||
		lpad(
				(head_pressure::int)::text || 
				case when  head_temperature < 0 then '5' else '' end || 
				(head_temperature::int)::text,
			 5,
			 '0');

end
$BODY$;

ALTER FUNCTION public.fn_calc_header(input_data_meteo11)
    OWNER TO admin;
