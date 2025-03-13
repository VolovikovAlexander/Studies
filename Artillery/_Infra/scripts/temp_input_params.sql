create sequence temp_input_params_seq;
create table temp_input_params
(
	id integer not null primary key default nextval('public.temp_input_params_seq'),
	emploee_name varchar(100),
	measurment_type_id integer NOT NULL,
    height numeric(8,2) DEFAULT 0,
    temperature numeric(8,2) DEFAULT 0,
    pressure numeric(8,2) DEFAULT 0,
    wind_direction numeric(8,2) DEFAULT 0,
    wind_speed numeric(8,2) DEFAULT 0,
    bullet_demolition_range numeric(8,2) DEFAULT 0,
	measurment_input_params_id integer,
	error_message text,
	calc_result jsonb
);

