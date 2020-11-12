# SchoolsServerTest
Решение на задание:

Описание предметной области

Ваша система содержит данные о детях (ФИО, ДР, Адрес), которые учатся в школах. Школы (Название, адрес) содержат классы (Номер, литера). Каждый ребенок может учиться только в одном классе в конкретный момент времени. Необходимо хранить историю перемещений учащегося.

Описание методов REST API

Необходимо реализовать REST API на языке C#.

 

Для начала необходимо спроектировать модели данных, которыми будет оперировать ваше приложение.

В вашей API должно быть 3 контроллера: для работы со школами, для работы с классами и для работы с учениками.

Каждый контроллер должен иметь:

·         метод для добавления объекта конкретного типа

·         метод для получения объекта конкретного типа по его уникальному идентификатору

·         метод для получения списка всех объектов конкретного типа по идентификатору другого связанного объекта (список классов по ключу школы, списка учеников по ключу школы или класса на заданную дату, список всех школ – без параметров)

А также контроллер для работы с зачислениями/отчислениями:

·         Метод для зачисления ученика в класс

·         Метод для отчисления ученика из его текущего класса

Все методы должны корректно обрабатывать запросы c ошибочными входными данными. Обмен данными должен происходить в формате JSON (можете и XML, но в таком случае желательно приложить XSD схему).

Данные можно хранить в памяти приложения. Но будет хорошо, если вы добавите какое-либо хранилище (от файла/файлов до подключения какой-либо БД, предпочтительнее MS SQL Server или PostgreSQL).

 

Ограничения по используемым фреймворкам и библиотекам нет.