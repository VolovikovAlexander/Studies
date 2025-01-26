do $$
declare
	input_data public.input_data_batch;
	"result" numeric(8,2) default 0;
begin
	input_data.device_type_id := 1;
	input_data.height := 500;
	input_data.temperature := 23;
	input_data.pressure := 780;
	input_data.wind_speed := 5;

	select public.fn_calc_temperature(input_data => input_data)
	into "result";

	raise notice 'result %', "result";
	
end $$
