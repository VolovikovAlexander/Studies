-- PROCEDURE: public.sp_write_log()

-- DROP PROCEDURE IF EXISTS public.sp_write_log();

CREATE OR REPLACE PROCEDURE public.sp_write_log(
		in message text,
		in data ANYELEMENT
	)
LANGUAGE 'plpgsql'
AS $BODY$
begin

	if coalesce(message, '') != '' then
		insert into public.application_log_history(started, message, data)
		values (now(), message, row_to_json(data));
	end if;
	
end;
$BODY$;

