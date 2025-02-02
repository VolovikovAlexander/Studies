-- FUNCTION: public.fn_calc_pressure(numeric)

-- DROP FUNCTION IF EXISTS public.fn_calc_pressure(numeric);

CREATE OR REPLACE FUNCTION public.fn_calc_pressure(
	pressure numeric)
    RETURNS numeric
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE PARALLEL UNSAFE
AS $BODY$
declare
	default_pressure numeric(8,2) default 750;
	table_pressure numeric(8,2) default null;
begin

	-- Определяем граничное табличное значение
	if not exists (select 1 from public.measurment_settings where key = 'pressure_750' ) then
	Begin
		table_pressure :=  default_pressure;
	end;
	else
	begin
		select value::numeric(18,2) 
		into table_pressure
		from  public.measurment_settings where key = 'pressure_750';
	end;
	end if;

	
	-- Результат
	return pressure - coalesce(table_pressure,table_pressure) ;

end
$BODY$;

ALTER FUNCTION public.fn_calc_pressure(numeric)
    OWNER TO admin;
