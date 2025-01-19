-- FUNCTION: public.fn_calc_temperatures_correction(numeric, numeric, numeric, numeric, numeric)

-- DROP FUNCTION IF EXISTS public.fn_calc_temperatures_correction(numeric, numeric, numeric, numeric, numeric);

CREATE OR REPLACE FUNCTION public.fn_calc_temperatures_correction(
	par_x numeric,
	par_batch public.interpolation_batch)
    RETURNS numeric
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE PARALLEL UNSAFE
AS $BODY$
declare
	denominator numeric(8,2) default 0;
	correction numeric(8,2) default 0;
begin
	denominator := par_batch.x1 - par_batch.x0;
	correction := (par_x - par_batch.x0) * (par_batch.y1 - par_batch.y0) / denominator + par_batch.y0;
	
	return correction;

end
$BODY$;


