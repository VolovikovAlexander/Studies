CREATE OR REPLACE FUNCTION public.fn_calc_temperatures_correction(
    -- Значение для которого нужно посчитать интерполяцию
	par_x numeric(8,2),
	par_x0 numeric(8,2),
	par_x1 numeric(8,2),
	par_y0 numeric(8,2),
	par_y1 numeric(8,2))
    RETURNS numeric
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE PARALLEL UNSAFE
AS $BODY$
declare
	denominator numeric(8,2) default 0;
	correction numeric(8,2) default 0;
begin
	denominator := par_x1 - par_x0;
	correction := (par_x - par_x0) * (par_y1 - par_y0) / denominator + par_y0;
	
	return correction;

end
$BODY$;

