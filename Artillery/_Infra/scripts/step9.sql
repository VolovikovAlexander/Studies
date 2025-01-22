-- Связь между аблицами default_calc_heights и default_calc_temperature_corrections
ALTER TABLE IF EXISTS public.default_calc_temperature_corrections
    ADD CONSTRAINT fk_default_calc_temperature_corrections_default_calc_height_id FOREIGN KEY (default_calc_height_id)
    REFERENCES public.default_calc_heights (id) MATCH SIMPLE
    ON UPDATE NO ACTION
    ON DELETE NO ACTION;

-- Добавим Primary key - составной
ALTER TABLE IF EXISTS public.default_calc_temperature_corrections
	ADD CONSTRAINT default_calc_temperature_corrections_pkey  PRIMARY KEY (default_calc_height_id, is_positive);

