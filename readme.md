# Различные проекты для обучения
### Мастер классы, ИГУ

## GPSD
Краткий мастер класс по разработке приложения для обработки данных от сервиса  GPS/ГЛОНАСС.
Стек: `C#`, `PostgreSQL`

#### Подробности
В рамках мастер класса слушатели изучали варианты загрузки Json данных. Их интерпретацию. Изучали вопросы производительности и кастомизации загрузки данных.
Так же, слушатели курса спроектировали собственную базу данных и осуществили парсинг / загрузку данных полученный с GPS/ГЛОНАСС сервиса.
Вся разработка велась поэтапно: `Step1`,`Step2` и так далее. На каждом этапы ставились задачи и подводидись итоги.

#### Ссылки
http://sr.isu.ru/news/v-igu-proshel-hakaton-po-mobilnoy-razrabotke-labirint-realnosti
https://news.samsung.com/ru/reality-maze-hackathon


## Performance
Краткий мастер класс в котором поднимались вопросы производительности работы с базой данных.
Стек: `C#`, `MS SQL 2014`, `ADO.net`, `Dapper`, `EF`

#### Подробности
В рамках мастер класса слушатели разбирались с разными примерами баз данных. Вместе находили узкие места по производительности и пытались решить разными методами.
В качестве мерила использовали написанные `NUnit` тесты с набором операций. Засекали время выполнения при каждом этапе повышения производительности.

#### Ссылки
http://sr.isu.ru/news/master-klass-ot-spetsialista-mezhdunarodnoy-kompanii-bell-integrator


## Yandex.Cloud
Краткий мастер класс по работе с облачным сервисом `Yandex.Cloud` и базой данных `Yandex.ClickHouse`. 
Стек: `Python3`,`ClickHouse`,`connexion`

#### Подробности
В рамках мастер класса показывается основные приемы проектирования `Rest Api` сервисов с поддержкой `OpenApi`. Обсуждаются вопросы проектирования многослойной архитектуры. Изучаются вопросы проектирования базы данных.
Далее, на основе собственного генератора данных создается набор данных для Dashboad с различными показателями. 

## Artillery
Двухмесячный курс в рамках которого идет обучение `PostgreSQL`. Запуск и настройка. Проектирование структуры базы данных и разработка логики работы с использованием собственных типов, процедур и функций.
Основной особенностью курса является использование реального технического задания по подготовке метрологических данных для ствольной артиллерии.
Стек: `PostgreSQL`, `Docker`, [`PostRest`](https://docs.postgrest.org/en/v12/), `Html`,`JavaScript`

