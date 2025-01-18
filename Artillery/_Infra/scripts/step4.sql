DO $$
BEGIN

	truncate table public.calc_temperatures_correction;
	insert into public.calc_temperatures_correction(temperature, correction)
	Values(0, 0.5),(5, 0.5),(10, 1), (20,1), (25, 2), (30, 3.5), (40, 4.5);

	-- select * from  public.calc_temperatures_correction;
END$$;