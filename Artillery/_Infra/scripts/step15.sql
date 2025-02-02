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
begin

	head_pressure := round(public.fn_calc_pressure(input_data));
	head_temperature	:= round(public.fn_calc_temperature(input_data));

	return 
		lpad(
			(head_pressure::int)::text || case when  head_temperature < 0 then '5' else '' end || (head_temperature::int)::text,
			 5,
			 '0');

end
$BODY$;

ALTER FUNCTION public.fn_calc_header(input_data_meteo11)
    OWNER TO admin;
