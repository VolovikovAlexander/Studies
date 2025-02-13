-- select get_random_period()

CREATE OR REPLACE FUNCTION public.get_random_period()
    RETURNS timestamp
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE PARALLEL UNSAFE
AS $BODY$
declare 
	var_result timestamp;
begin
	select TIMESTAMP '2023-01-01' +
       ((TIMESTAMP '2026-12-31' - TIMESTAMP '2023-01-01')) * random()
	 into   var_result;

	 return var_result;
end
$BODY$;

ALTER FUNCTION public.get_random_period()
    OWNER TO admin;
