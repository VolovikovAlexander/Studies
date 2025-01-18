Здравствуйте, Илья.
Хочу, немного, прокоментировать указанный Вами выше код, который по Вашему мнению нужно исключить.
Данный код выполняет следующее действия:
- Отправляем запрос в `Camunda.api`
> GET
> /Operations/{processInstanceKey}/State
> Получить текущее состояние процесса в системе Camunda

- Система `Camunda.api` формирует агрегацию, одной из частью агрегации идет запрос в `Camunda Platform 7`. Вызывается Rest метод [getRenderedForm](https://docs.camunda.org/rest/camunda-bpm-platform/7.21-SNAPSHOT/#tag/Task/operation/getRenderedForm). 

>  "form": "{\n  \"components\": [\n    {\n      \"label\": \"Текст заявки\",\n      \"type\": \"textarea\",\n      \"layout\": {\n        \"row\": \"Row_1k4gi8g\",\n        \"columns\": null\n      },\n      \"id\": \"Field_0o4byoh\",\n      \"key\": \"FactoryRequestText\",\n      \"disabled\": true\n    },\n    {\n      \"label\": \"Комментарий\",\n      \"type\": \"textarea\",\n      \"layout\": {\n        \"row\": \"Row_0y969hf\",\n        \"columns\": null\n      },\n      \"id\": \"Field_10l998n\",\n      \"key\": \"AcceptComment\"\n    }\n  ],\n  \"type\": \"default\",\n  \"id\": \"FactoryRequest_AcceptForm\",\n  \"exporter\": {\n    \"name\": \"Camunda Modeler\",\n    \"version\": \"5.28.0\"\n  },\n  \"executionPlatform\": \"Camunda Platform\",\n  \"executionPlatformVersion\": \"7.21.0\",\n  \"schemaVersion\": 16\n}"

- Далее, идет анализ контента и построение формы в системе `uzdt.front`. 

> 	var root = document.createElement('div');
	root.innerHTML = state.html;
	const inputs: HTMLInputElement[] = [];

Таким образом, данный код исключить нельзя, т.к. является часть системы на основе BPMN постоителя схем бизнес процессов. Если убрать данный функционал, то тем самым будет **исключен** основной функционал, а именно: `построение HTML форм ввода данных непосредственно из системы Camunda`

1. Альтернативным вариантов может быть замена на формы, созданные непосредственно в системе `uzdt.front` однако при таком подходе исключается возможность создание форм / схем бизнес процессов непосредственно пользователями что не соответствует текущему Техническому заданию и контракту.
2. Следующим альтернативным вариантом может быть передача данных по переменным и для построения формы на основе данных, например [getFormVariables](https://docs.camunda.org/rest/camunda-bpm-platform/7.21-SNAPSHOT/#tag/Task/operation/getFormVariables) однако при таким варианте пользователь не имеет возможности назначать в BPMN схемах ограничения на поля ввода, что не соответствует условия технического задания и контракта да и не безопасно.

Спасибо. Прошу ознакомится и дать обратную связь.




