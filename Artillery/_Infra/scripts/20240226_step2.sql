create sequence application_log_history_seq;
create table application_log_history
(
	id integer not null primary key default nextval('public.application_log_history_seq'),
	started timestamp not null,
	message text,
	data jsonb,
	login varchar(100) default current_user
);


 --row_to_json(data)
--SELECT * FROM jsonb_populate_record(NULL::temperature_correction, '{"calc_height_id": 1, "height": 50, "deviation": 10}'::jsonb);

