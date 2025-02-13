do $$
declare
var_emploees text[];
var_emploee text;
begin
    -- Базовый цикл
	var_emploees := array['Георгий', 'Егор', 'Никита' ];
	raise notice 'var_emploees %', var_emploees;

	foreach var_emploee in array var_emploees loop
		raise notice 'var_emploee %', var_emploee;
	end loop;

	

end $$;