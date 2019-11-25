**Web API for simple Library Information System**

--------------------------------------------------------------------------------

This Web API I`ve created while my .*NET Backend Development Internship* in the
Mercury Development Company.

The Web API is aimed to serve a simple Library Information System.
The project implements **layered architecture** and consists of the following
*layers*
: 

* Data Access Layer (DAL);
* Business Logic Layer (BLL);
* Authentication/Authorization Layer (Auth);
* Common Layer;
* Web Application.

**Data Access Layer (DAL)**

DAL consists of the data-related entities (such as User, Book, Author) and the 
descriprion of Database Context.

**Business Logic Layer (BLL)**

BLL contains the business logic of the program. The main part of it is Services.
They transmit data from DAL to the further layer which, in that project, is 
Web Application.
BLL also contains Validators, various Helpers and Logger classes.

**Authentication/Authorization Layer (Auth)**

This level implements Authentication and Authorization in the project.

**Common Layer** 

Common Layer contais stuff which is common for several layers of the program.
E.g. there are exceptions which present in BLL and DAL layers.
As far as it also has Data Thransfer Objects (DTO) we can say that it is used to
establish communication between the different levels of the layers hierarchy.


**Web Application**

Is the "highest" level of the layers hierarchy. The part of that level is the 
Api Controllers. Api Controllers are "external" interface of the program. 
Generally speaking, they allows to communicate with the program via 
HTTP-protocol. 

--------------------------------------------------------------------------------
**! Next are some descriptions of the program in Russian !**

**Authorization**

   Системой могут пользоваться только авторизованные пользователи. 

Чтобы авторизоваться в системе, необходимо обратиться к эндпоинту 
`‘api/users/authenticate'`. 

Для входа в систему пользователь вводит свой Email и пароль. 
Авторизованный пользователь имеет определенную категорию прав. 
Реализовано две роли: *User* и *Admin*.

Все CRUD-операции с *пользователями* системы (за исключением получения 
информации пользователя о самом себе) может выполнять только Admin.
	При первом запуске системы и инициализации базы данных в БД автоматически 
	добавляются 3 пользователя:
	
	
1. Администратор системы (Name: admin, Email: admin, Password: 111, Role: Admin);
2. Пользователь 1 (Name: Елена Сергеевна Менделеева, Email: e.mendeleeva@library.ru, Password: 123, Role: User);
3. Пользователь 2 (Name: Дмитрий Иванович Книголюбов, Email: d.knigolyubov@library.ru, Password: 123, Role: User).


Механизм авторизации реализован с помощью *JWT (JSON Web Token)*. 

При успешной авторизации возвращается объект *UserDto* с полем *Token*, 
содержащим сгенерированный для данного пользователя токен.  
Этот токен используется для выполнения запросов к системе, 
требующих авторизации.

**Filtering and paging**

Для рациональной нагрузки на систему реализованы фильтрация и паджинация данных,
возвращаемых при запросах на получение книг, членов и пользователей библиотеки.

Для книг реализована фильтрация по названию и имени автора, 
для членов и пользователей библиотеки – по имени.

При обращении к эндпоинтам `‘api/books’`, `‘api/members’` и `‘api/users’` 
тело ответа имеет поля *nextPage* и *previousPage*, содержащие URL-запроса 
на получение следующей и предыдущей страниц соответственно.


**Logging**

В рамках задания необходимо было реализовать логирование 
определенных событий в файл, а именно:
* Добавление книги;
* Добавление читателя;
* Выдача книги;
* Возврат книги.

Для реализации логирования в файл был применен фреймворк *SeriLog*. 

Созданные логи хранятся в проекте веб-приложения папке 
Logs (`LibraryIs/LibraryIS.WebApplication/Logs`).

Логи представляют собой файлы формата .txt. 
Для каждой даты создается отдельный файл лога. 

В логах указано точное время наступления события, а также краткая информация
о книге и члене, принявших участие в событии.
Кроме того, в лог заносится информация об авторизованном пользователе,
совершившим действие.


**Testing asynchronous working**

Для проверки одновременной работы нескольких библиотекарей был реализован
простейший консольный клиент-эмулятор библиотекаря. 
Тестирование проводилось следующим образом.

 Было создано два похожих консольных приложения *LibraryIS.TestingConsoleApp* 
 и *LibraryIS.TestingConsoleApp2*. Первое приложение, после авторизации от имени
 администратора, осуществляло беспрерывную выдачу и прием одной и тоже книги
 одному и тому пользователю, то есть постоянно отправляло запросы на сервер.
 Второе приложение осуществляло аналогичные действия, 
 только выполняло авторизацию от имени другого пользователя.

После одновременного запуска этих двух приложений, работа сервиса оставалась 
стабильной и он удачно справлялся со всеми посылаемыми к нему запросами.  
Кроме того, в файле лога все совершаемые операции были записаны и
хаотичны смешаны друг с другом. На основе вышеперечисленных фактов и
было сделано заключение о возможности асинхронной работы разработанной системы.
