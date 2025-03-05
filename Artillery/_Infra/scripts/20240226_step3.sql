explain
select * from public.measurment_input_params as t1
inner join public.measurment_baths as t2 on t1.id = t2.measurment_input_param_id
inner join public.employees as t3 on t3.id = t2.emploee_id
where
	t2.emploee_id = 10

	--               ->  Seq Scan on measurment_baths t2  (cost=0.00..472.51 rows=500 width=20)

	create index ix_measurment_baths_emploee_id on measurment_baths(emploee_id)

	--               ->  Index Scan using ix_measurment_baths_emploee_id on measurment_baths t2  (cost=0.29..20.04 rows=500 width=20)