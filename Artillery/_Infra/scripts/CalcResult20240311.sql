do $$
declare
	par_input_params public.input_params_type;
	par_results public.calc_result_type[];
	rec record;
begin

	par_input_params.height := 430;
	par_input_params.temperature := 15;
	par_input_params.pressure := 780;
	par_input_params.wind_direction := 2;
	par_input_params.wind_speed := 10;
	par_input_params.bullet_demolition_range := 0;

	
	call public.sp_calc_corrections(par_input_params => par_input_params, par_measurement_type_id => 2,
		par_results => par_results);

	raise notice '| measurement_type_id  | height | deltapressure | deltatemperature | deviationtemperature | deviationwind | deviationwinddirection |';
	raise notice '|----------------------|--------|---------------|------------------|----------------------|---------------|------------------------|';

	foreach rec in ARRAY par_results LOOP
		raise notice '| % | % | % | % | % | % | % |' ,
			lpad(rec.measurement_type_id::text, 20, ' '), 
			lpad(rec.height::text, 6, ' '), 
			lpad(rec.deltapressure::text, 13, ' '), 
			lpad(rec.deltatemperature::text, 16, ' '), 
			lpad(rec.deviationtemperature::text, 20, ' '),
			lpad(rec.deviationwind::text, 13, ' '),
			lpad(rec.deviationwinddirection::text, 22, ' ');
	end loop;
	
end $$