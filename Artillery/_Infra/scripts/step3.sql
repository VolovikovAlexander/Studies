DO $$
BEGIN

 -- Свяжем две таблицы measurment_params и measurement_batch
 alter table public.measurment_params 
 add constraint fk_measurment_param_measurement_batch_id foreign key (measurment_batch_id)
 references public.measurement_batch(id);

 -- Свяжем две таблицы measurment_params и measurment_types
 alter table public.measurment_params 
 add constraint fk_measurment_param_measurment_types foreign key (measurment_type_id)
 references public.measurment_types(id);
 
END$$;