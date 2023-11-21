# zeebe
Здесь буду выкладывать материалы которые использую при изучении.

StateChanger - проект простого воркера на dotnet. Его задача пока что просто логгировать вызовы.

StateChanger.bpmn - схема, в которой есть задача, вызывающая сервис.

docker-compose.yaml - скрипт запускает кластер из 3 инстансов Zeebe.

gateway.proto - прото grpc, для построения клиента/админки.

Управление Zeebe через команду https://github.com/camunda-community-hub/zbctl-via-npm

Для запуска примера в минимальном виде нужно:

1. "docker-compose up". Убедиться что кластер стартовал.
2. "zbctl deploy StateChanger.bpmn --insecure". Команда должна вернуть json, из нее нужно взять bpmnProcessId (например Process_1oi88v7).
3. Запустить сервис StateChanger, убедиться что он подключился к кластеру - не должно быть ошибок в консоли.
3. "zbctl create instance Process_1oi88v7 --insecure --variables '{"new2":"test"}'". json содержит набор переменных, передаваемых запущенному инстансу.
4. В логе сервиса должно быть видно, что произошел его вызов.

Но более удобный вариант с консолью разработчика на базе репозитория https://github.com/camunda-community-hub/zeebe-simple-monitor
Для запуска примера просто стартуем другой docker compose:
1. docker-compose -f docker-compose-monitor.yaml --profile in-memory up
2. В браузере открываем ссылку http://localhost:8082/
В этом варианте можно обновлять схемы bpmn через UI + запускать новые инстансы и наблюдать состояние процессов.