create type calc_result_response as
(
	-- Заголовок
	header text,
	-- Результат 
	calc_result calc_result_type[]
);

