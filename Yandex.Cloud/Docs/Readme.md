#### Краткая программа курса
# Построение витрин данных на основе Yandex.ClickHouse

Стек: `python`, `Yandex.ClickHouse`, `swagger\OpenApi`, `JavaScript\bootstrap`

### Легенда
>	Вы включены в команду разработки дополнительного программного обеспечения для проведения мониторинга всех этапов строительства
>на промышленной площадке. В настоящее время, у Вас в организации есть ERP система полного цикла которая позволяет вести учет всех данных
>связанных со строительством любых объектов. Однако, для проведения каждого этапа аудита требуется выезд на объект группы специалистов
>для оценки прогресса. Далее, составляется акт на бумажном носителе, Данный акт и набор документов рассылается всем субподрядчикам. 
>	Каждый субподрядчик, проверяет документы и ведет диалог с контроллирующим органом. Для ускорения данного процесса и повышения
>`прозрачности` деятельности принято решение расширить текущую ERP систему путем создание внешнего модуля (Rest Api) и отдельного WEB приложения (личный кабинет субподрядчика).
>	Так же, ввиду того, что требуется не только хранить всю историю работы но и проводить анализ данных на предмет качества работы
>каждого субподрядчика, принято решение, включить во внешний модуль часть аналитики на основе облачной платформы [Yandex.ClickHouse](https://yandex.ru/dev/clickhouse/)



### Подготовка
1. Установить ClickHouse - https://clickhouse.com/docs/en/install
2. Установить Visual Studio Code - https://code.visualstudio.com/download
	- Установить плагины: Python, PyTest (`ms-python.python`, `littlefoxteam.vscode-python-test-adapter` ,`pamaron.pytest-runner`)
3. Установить pip утилиту `sudo apt install python3-pip`
	- Установить расширение для Python `pip install clickhouse-connect`

### Основные объекты

Основными сущностями в проекте будут считаться: `объект капитального строительства`, `застройщик`, `субподрядчик`, `исполнитель`
Приложение должно оперировать следующими документами: `акт`, `реектр актов`
В рамках каждого документа назначается оценка состояния: `сумма штрафа`, `статус работ`

Определим статусы объекта:

| Статус        | Описание                |
|---------------|-------------------------|
| Подготовка    | Все документы для начала строительства / этапа собраны и проверены. Разрешения выданы |
| Старт         | На объекте начаты строительные работы. Представители субподрядчика прибыли на объект |
| Проверка пройдена | Очередная проверка пройдена без замечаний со стороны контроллирующих органов |
| Замечания | Очередная проверка пройдена с замечаниями. Необходимо устранить все замечания |
| Завершено | Все замечания устранены. Все проверки пройдены. Оплату можно проводить |

### Архитектура

Выбираем вариант - [многослойная архитектура](https://ru.wikipedia.org/wiki/%D0%9C%D0%BD%D0%BE%D0%B3%D0%BE%D1%83%D1%80%D0%BE%D0%B2%D0%BD%D0%B5%D0%B2%D0%B0%D1%8F_%D0%B0%D1%80%D1%85%D0%B8%D1%82%D0%B5%D0%BA%D1%82%D1%83%D1%80%D0%B0)
В проекте будем разделять модели на: `основные`, `для передачи и обработки данных`
Создадим пустой проект и добавим каталоги: `Src/Models`, `Tests`

Создадим основные модели.

| Модель          | Наименование           |
|-----------------|------------------------|
| [progress_status](../Src/Models/Statuses.py)  | Описание статусов  |
| [building](../Src/Models/Building.py) | Объект капитального строительства |
| [contractor](../Src/Models/Contractor.py) | Застройщик / Подрядчик / Субподрядчик |
| [executor](../Src/Models/Executor.py)  | Исполнитель   |
| [act](../Src/Models/Act.py)  | Документ **Акт проверки** |
| | |
| [period](../Src/Models/Period.py) | Обвертка для работы с датой-временем |
| [guid](../Src/Models/Guid.py) | Обвертка для работы с уникальным кодом |
| | |

Так же, добавим [модульные тесты](../Tests/test_models.py).

### Проектирование основных сервисов

> Подготовка
Устанавливаем модуль [connexion](https://connexion.readthedocs.io/en/latest/) для разработки WEB сервисов `REST API`
```
pip install connexion[swagger-ui]
```

Добавим вызовы [RestApi](../Main.py) с свяжем их со специальных классом - [репозиторий](../Src/Services/Repo.py).

| Url (endpoint)   | Описание                    |
|------------------|-----------------------------|
| /api/acts/       | **GET**, получить список всех актов |
| /api/acts/uid    | **GET**, получить карточку конкретного акта по его коду  |
| /api/executors/   | **GET**, получить список всех исполнителей |
| /api/executors/uid | **GET**, получить карточку конкретного исполнителя по его коду |
| /api/contractors/ | **GET**, получить список всех застройщиков |
| /api/contractors/uid | **GET**, получить карточку конкретного застройщика по его коду |
| | | 


Так же, добавим [модульные тесты](../Tests/test_reposity.py) и тесты на [конвертацию данных](../Tests/test_to_json.py) в формат json
Для проверки работы RestApi  в класс репозиторий добавим демо данные. Удобней это сделать при помощи фабричного метода. 
Пример:

```python
def create(is_demo = False):
        """
        Фабричный метод
        """
        main = repo()
        if is_demo == True:
            main.load_demo()
        else:    
            main.load()

        return main
```

Для работы с API создадим специальный [yaml файл](../Swagger.yaml) в котором создадим описание всех точек вызова. Для проверки, запускаем: http://127.0.0.1:8080/api/ui

#### Задания
1. Доработать [yaml файл](../Swagger.yaml). Включить в него описания вызовов для остальных сервисов. Пример: https://swagger.io/docs/specification/basic-structure/
2. Найти ошибку в коде. При сериализации объекта [executor](../Src/Models/Executor.py) значения поля `contractor` выглядит следующим образом:
```json
 "contra\u0441tor": {
                "description": "",
                "guid": "3de8f7f4-7e4b-4d91-8bfd-a3cfd00bfbf4",
                "name": "test2"
 }
 ```
 3. Написать простой **Js** код, который будет отображать данные от `RestApi`. Желательно, использовать систему генерации **Js** кода, например: https://github.com/RicoSuter/NSwag
 4. Доработать метод `load_demo` класса [repo](../Src/Services/Repo.py). Добавить в него генерацию всех сущностей.
 5. Доработать модель [акта](../Src/Models/Act.py). Добавить свойство - объект капитального строительства (ОКС), [building](../Src/Models/Building.py)
 

### Проектирование базы данных
#### ClickHouse

Подключение:
```
clickhouse-client --host rc1a-7ut3ob6t69958voj.mdb.yandexcloud.net                   --secure                   --user user                   --database db                   --port 9440                   --ask-password
```

При удачном подключении:
```
rc1a-7ut3ob6t69958voj.mdb.yandexcloud.net :)
```

Таблицы:
| Наименование                 | Описание                | SQL запрос                     |
|------------------------------|-------------------------|--------------------------------|
| `buildings`                  | Таблица всех объектов капитального строительства (ОКС) | `create table buildings(id String, name String, description String, primary key[id]) engine = MergeTree;` | 
| `statuses`                   | Таблица статусов | `create table statuses(id UUID, code Int, name String, description String, primary key[id]) engine = MergeTree;` |
| `executors`                  | Таблица исполнителей | `create table executors(id UUID, name String, description String, contractor_id UUID not null, primary key[id]) engine = MergeTree;` |
| `contractors`                | Таблица застройщиков | `create table contractors(id UUID, parent_id UUID, name String,description String, primary key[id]) engine = MergeTree;` |
| `acts`                       | Таблица с основной информацией по актам | `create table acts(id UUID, building_id UUID not Null, executor_id UUID,  period DateTime not null,  primary key[id]) engine = MergeTree;` |
| `acts_contractor_links`      | Таблица связи акта с застройщиками | `create table acts_contractors_links(id UUID, period DateTime not null, contractor_id UUID not null,  primary key[id]) engine = MergeTree;` |
| `acts_status_links`          | Таблица связи акта со статусом | `create table acts_status_links(id UUID, period DateTime not null, status_code Int not null, comments String, amount  Float32 default(0), primary key[id]) engine = MergeTree` |
| | | |


Проверка:
```sql
show tables;

┌─name───────────────────┐
│ acts                   │
│ acts_contractors_links │
│ acts_status_links      │
│ buildings              │
│ contractors            │
│ executors              │
│ statuses               │
└────────────────────────┘
```

Добавим статусы:
```sql
insert into statuses(id, name, code, description) select generateUUIDv4(),'preparation',1, 'Все документы для начала строительства / этапа собраны и проверены. Разрешения выданы';
insert into statuses(id, name, code, description) select generateUUIDv4(),'start',2, 'На объекте начаты строительные работы. Представители субподрядчика прибыли на объект';
insert into statuses(id, name, code, description) select generateUUIDv4(),'passing',3, 'Очередная проверка пройдена без замечаний со стороны контроллирующих органов';
insert into statuses(id, name, code, description) select generateUUIDv4(),'failure',4, 'Очередная проверка пройдена с замечаниями. Необходимо устранить все замечания';
insert into statuses(id, name, code, description) select generateUUIDv4(),'finish',5, 'Все замечания устранены. Все проверки пройдены. Оплату можно проводить';
```

##### Задания
1. Доработать таблицу `statuses`. Исключить возможность дублирования записей по полям: **name**,**code**. Сделать в виде SQL скрипта в котором сразу же включить проверку.
2. Изенить таблицу `acts_status_links`. Сделать связь со статусом не по **UUID** , а по коду **Int**. Сделать в виде SQL скрипта.

#### Работа с ClickHouse

1. Установим [пакет](https://readthedocs.org/projects/clickhouse-driver/downloads/pdf/latest/) 
```
pip install clickhouse-driver
```

2. Создадим [отдельный класс](../Src/Services/Proxy.py) для работы с базой данных `proxy`. 
3. Так же, добавим для проверки [модульный тест](../Tests/test_proxy.py)
4. Для всех моделей добавим метод `__str__` и в каждом методе определим SQL команду для вставки данных. 

Пример:

```python
    def __str__(self):
        """
        Сформировать SQL запрос на вставку данных
        """
        sql = "insert into executors(id, name, description, contractor_id) values('%s', '%s', '%s', '%s')" % (self.id.toJSON(), self.name, self.description,  self.contraсtor.id.toJSON())
        return sql
```        

#### Генерация тестовых данных

1. Создадим [отдельный класс](../Generator.py) c автозапуском. Последовательно в нем реализуем генерацию всех моделей с засечкой времени выполнения.
2. Для фиксации времени выполнения добавим новый метод в модель `period`

Пример:
```
Генерация записей ОКС
Старт: 2023-05-03 11:11:59
Сформировано успешно 100 записей ОКС за 13.980264 сек.
```

##### Задания
1. По сформированной генератором таблице `contractors`, найти заказчика с самой `глубокой` связью. Поле `parent_id` является ключом `id` связанной записи. Самой глубокой связью считается такая связь между `id -> parent_id` которая повторяется наибольшее количество раз. **id -> parent_id -> id -> parent_id** и т.д.

```
SELECT *
FROM contractors

Query id: bb870377-796a-46f2-bb30-1848a6ef6e72

┌─id───────────────────────────────────┬─parent_id────────────────────────────┬─name────────────┬─description─┐
│ f7743f60-5a88-4ae7-8289-e27c77e3e930 │ 63a171ff-109f-464b-b5c4-c6f53652b11f │ contractor № 16 │             │
│ bc2f0750-26f0-4285-863a-8b56e6315b9e │ aa2f788c-386c-4fc9-97ae-de795e63c13a │ contractor № 19 │             │
│ 8cffd131-8f7c-4250-8769-00114458e6ae │ 63a171ff-109f-464b-b5c4-c6f53652b11f │ contractor № 11 │             │
│ 801d1580-86b5-406a-8a95-78680d29ee38 │ 23801ef3-5c8c-43f8-b5d2-571bd840efca │ contractor № 21 │             │
│ 1661d2ec-da3c-4599-95fc-42ee6f4b37fe │ f7743f60-5a88-4ae7-8289-e27c77e3e930 │ contractor № 23 │             │
│ aa2f788c-386c-4fc9-97ae-de795e63c13a │ ea5db567-834a-4e4f-a7a8-224f81b97d57 │ contractor № 18 │             │
│ 5bce3d60-bf56-4349-9c31-ba44cfbaeefa │ 00000000-0000-0000-0000-000000000000 │ contractor № 1  │             │
```


















 






	


