-- Список табличных высот
insert into public.default_calc_heights(height) values(200), (400), (800), (1200), (1600), (2000), (2400), (3000), (4000);
-- select * from default_calc_heights

-- Положительные значения для высоты 200
insert into public.default_calc_temperature_corrections(default_calc_height_id, data, is_positive)
values(1, array[0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 20, 30, 30, 30], True);
-- select * from default_calc_temperature_corrections
