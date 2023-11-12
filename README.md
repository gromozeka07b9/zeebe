# zeebe
Здесь буду выкладывать материалы которые использую при изучении.

StateChanger - проект простого воркера на dotnet. Его задача пока что просто логгировать вызовы.

StateChanger.bpmn - схема, в которой есть задача, вызывающая сервис.

docker-compose.yaml - скрипт запускает кластер из 3 инстансов Zeebe.

gateway.proto - прото grpc, для построения клиента/админки.

Управление Zeebe через команду https://github.com/camunda-community-hub/zbctl-via-npm

Для запуска примера нужно:

1. "docker-compose up". Убедиться что кластер стартовал.
2. "zbctl deploy StateChanger.bpmn --insecure". Команда должна вернуть json, из нее нужно взять bpmnProcessId (например Process_1oi88v7).
3. Запустить сервис StateChanger, убедиться что он подключился к кластеру - не должно быть ошибок в консоли.
3. "zbctl create instance Process_1oi88v7 --insecure".
4. В логе сервиса должно быть видно, что произошел его вызов.