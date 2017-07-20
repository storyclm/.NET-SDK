# StoryCLM .NET SDK

StoryCLM .NET SDK позволяет легко интегрировать систему [StoryCLM](http://breffi.ru/ru/storyclm) c Вашим приложением на .NET.
Данная библиотека сделана на базе [REST API](https://github.com/storyclm/documentation/blob/master/RESTAPI.md) сервиса [StoryCLM](http://breffi.ru/ru/storyclm) и максимально упращает работу с API.

Ниже будет рассказано как устаносить библиотеку, настроить проект для работы с ней и показана работа SDK на конкретных примерах.

## Frameworks:
* .NET Standard 1.1

## Установка

Установить SDK можно двумя способами:
* NuGet Package Manager.
* Загрузить SDK непосредственно из этого репозитория.

### NuGet

[NuGet Package Manager](https://www.nuget.org/) должен быть предварительно установлен и настроен в Visual Studio 2015 или более поздей версии.

**Установка через NuGet Package Manager с использованием консоли**

Выполните следующую команду в консоли NuGet Package Manager для установки SDK и всех его зависимостей:
```
PM> Install-Package StoryCLM
```
**Установка через NuGet Package Manager с использованием расширения Visual Studio**

Чтобы установить SDK с помощью расширения Visual Studio NuGet, выполните следующие действия:
* В обозревателе решений щелкните правой кнопкой мыши на проект и выберите "Управление NuGet пакетами".
* В поиске введите "StoryCLM" и нажмите Enter.
* После завершения поиска, выберете из списка StoryCLM .NET SDK и нажмите кнопку "Установить".

### Git

Перейдите в корневой каталог [репозитория](https://github.com/storyclm/.NET-SDK) и нажмите на кнопку "Clone or вownload".

![rest Image 5](./images/1.png)

## Настройка и получение учетных данных

**Подключение пространств имен**

В файл, в котором Вы хотите использовать SDK, необходимо добавить следующий код:
```cs
using StoryCLM.SDK;
using StoryCLM.SDK.Models;
```
**Получени учетных данных**

Для того что бы получить доступ к API своего клиента нужно на панели администрирования создать и настроить приложение. 

*Более подробную информацию о учетных данных, типах клиента, о аутентификации и авторизации в системе нужно ознакомиться с документацией по [REST API](https://github.com/storyclm/documentation/blob/master/RESTAPI.md#Активация).*

**Использование класса SCLM**

SCLM - это класс-контекст, который содержит методы для работы с API StoryCLM. Для работы нужно создать экземпляр класса SCLM и пройти аутентификацию.

```cs
SCLM sclm = new SCLM();
```

**Aутентификация**

Клиент может взаимодействовать с API StoryCLM в двух режимах:

* **От имени сервиса (Service)** - используется для интеграции StoryCLM с другой системой. Для аутентификации достаточно только ClientId и Secret. Клиент получает доступ только в рамках клиента StoryCLM в котором ему были выданы учетные данные.
* **От имени пользователя StoryCLM (Application)** - используется в мобильных или настольных приложениях. Помимо ClientId и Secret для успешной аутентифкации требуются еще Username и Password пользователя. Клиент работает от имени пользователя StoryCLM и, за исключением некоторых разделов API (Таблицы), получает доступ ко всем ресурсам которые доступны пользователю в клиента StoryCLM в которых у него есть права.


Аутентифкация от имени сервиса:
```cs
SCLM sclm = new SCLM();
StoryToken token = await sclm.AuthAsync(clientId, secret);
```
Аутентификация от имени пользователя:
```cs
SCLM sclm = new SCLM();
StoryToken token = await sclm.AuthAsync(clientId, secret, username, password);
```
В случаи успешной аутентификации будет возвращен объект StoryToken. Объект StoryToken имеет следующие поля:

* **AccessToken** - маркер доступа. 
* **RefreshToken** - маркер обновления. Выдается если клиент работет от имени пользователя.
* **Expires** - дата истеченя срока маркера доступа.

В случае необходимости StoryToken можно получить из контекста, обратясь к полю "Token".
```cs
StoryToken token = sclm.Token;
```

**Практики обновления маркеров доступа**

После успешной аутентифкации маркер доступа автоматически сохраняется на все время жизни контекста и автоматически прикрепляется к каждому запросу к серверу. Стоит учитывать, что маркер доступа "живет" один час. После истечения срока действия текущего маркера доступа нужно получуть новый маркер. Этот процесс разнится в зависимости от типа аутентификации и способа интеграции с StoryCLM.

**Service** - клиент работает от имени сервиса. Если время жизни контекста больше времени жизни маркера доступа, то при каждом следущем обращении сервер будет возвращать код 401. Это означает что клиент не авторизован. Что бы этого не произошло нужно контролировать время жизни маркера доступа и после истечения срока его действия выполнить аутентификацию и получить новый маркер. 

Если сессии короткие, то можно сохранять StoryToken между сессиимяи и присваивать его контексту.
```cs
SCLM sclm = new SCLM();
StoryToken token = GetToken();
sclm.Token = token;
```
При таком подходе следует контролировать поле Expires объекта StoryToken что бы выявить момент когда следует обновить маркер.

Лучшей практикой для приложений которые используют подход "Service", будет создавать сесии не больше часа и производить обмен данными с StoryCLM, при этом каждый раз получая новый маркер доступа. Не следует сохранять и хранить маркер доступа в незащищенном хранилище и в открытом виде.

**Application** - клиент работает от имени пользователя StoryCLM. После аутентификации помимо маркера доступа сервер возвращает маркер обновляения (RefreshToken). Время жизни этого маркера год. Он используется для получения нового маркера доступа без логина и пароля. Это сделано для того что бы каждый раз когда маркер доступа становится просроченым не заставлять пользователя вводить логин и пароль. Контекст автоматически извлекает маркер обновления, контролирует время жизни маркера доступа и самостоятельно получает новые маркеры доступа и обновления. Таким образом контекст может существовать и выполнять успешные запросы к серверу неограниченное колличество времени.

Так же как и с "Service" подходом можно извлекать объект StoryToken, сохранять его и нужный момент использовать. На лучшей практикой будет сохранять только маркер обновления. В таком случае не нужно хранить логин и пароль пользователя, так же маркер обновления можно хранить в отркытом виде. Каждый раз при запуске приложения нужно извлекать из хранилища маркер обновления и выполнить один раз получение нового маркера доступа:
```cs
SCLM sclm = new SCLM();
string refreshToken = GetRefreshToken();
await sclm.AuthAsync(clientId, secret, refreshToken);
```
По завершению работы приложения, нужно сохранть маркер обновления.


## Таблицы


[Таблицы](https://github.com/storyclm/documentation/blob/master/TABLES.md) - это реляционное хранилище данных.

*Более подробная информация содержится в разделе ["Таблицы"](https://github.com/storyclm/documentation/blob/master/TABLES.md) документации.*

SDK при работе с таблицами оперирует объектами. Объект должен соответсвовать схеме таблицы. Один объект - одна запись.
Создадим таблицу Profiles на панели администрирования и класс Profile в проекте с полями которые будут соответсвовать схеме таблицы Profiles:
```cs
    /// <summary>
    /// Профиль пользователя
    /// </summary>
    public class Profile
    {
        /// <summary>
        /// Идендификатор записи
        /// Зависит от провайдера таблиц
        /// </summary>
        public string _id { get; set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Возраст
        /// </summary>
        public long Age { get; set; }

        /// <summary>
        /// Пол
        /// </summary>
        public bool Gender { get; set; }

        /// <summary>
        /// Рейтинг
        /// </summary>
        public double Rating { get; set; }

        /// <summary>
        /// Дата регистрации
        /// </summary>
        public DateTime Created { get; set; }

    }
```

#### Method: Task<IEnumerable\<StoryTable\<T>>> GetTablesAsync\<T>(int clientId)

**Описание:**

Получает cписок таблиц клиента.

**Параметры:**
* T - параметризованный тип (generic), описывающий сущность в таблице.
* clientId - Идентификатор клиента в базе данных.

**Возвращаемое значение:**

Список таблиц

**Пример:**
```cs
SCLM sclm = new SCLM();
await sclm.AuthAsync(clientId, secret);
IEnumerable<StoryTable<Profile>> tables = await sclm.GetTablesAsync<Profile>(clientId);
```
#### Method: Task<StoryTable\<T>> GetTableAsync\<T>(int tableId)

**Описание:**

Получает таблицу по идентификатору.

**Параметры:**
* T - параметризованный тип (generic), описывающий сущность в таблице.
* tableId - Идентификатор таблицы.

**Возвращаемое значение:**

Объект "Таблица"

**Пример:**
```cs
SCLM sclm = new SCLM();
await sclm.AuthAsync(clientId, secret);
StoryTable<Profile> table = await sclm.GetTableAsync<Profile>(tableId);
```

Методы GetTablesAsync и GetTablesAsync возвращают типизированные объекты StoryTable\<Profile>, в данном случае с типом Profile. Это означает что все операции над данными в этой таблице будут производится с участием этого типа. Для того что бы производить операции с данными таблицы у класса StoryTable есть определенный набор методов. 


#### Method: Task\<T> InsertAsync(T o)

**Описание:**

Добавляет новый объект в таблицу.
Объект должен соответствовать схеме таблицы.

**Параметры:**
* o - Новый объект.

**Возвращаемое значение:**

Новый объект в таблице.

**Пример:**
```cs
SCLM sclm = new SCLM();
await sclm.AuthAsync(clientId, secret);
StoryTable<Profile> table = await sclm.GetTableAsync<Profile>(tableId);
Profile profile = await table.InsertAsync(new Profile());
```

#### Method: Task<IEnumerable\<T>> InsertAsync(IEnumerable<T> o)

**Описание:**

Добавляет коллекцию новых объектов в таблицу.
Каждая объект должен соответствовать схеме таблицы.

**Параметры:**
* o - коллекция новых объектов

**Возвращаемое значение:**

Коллекция новых объектов.

**Пример:**
```cs
SCLM sclm = new SCLM();
await sclm.AuthAsync(clientId, secret);
StoryTable<Profile> table = await sclm.GetTableAsync<Profile>(tableId);
List<Profile> profiles = new List<Profile>(await table.InsertAsync(Profile.CreateProfiles()));
```

#### Method: Task\<T> UpdateAsync(T o)

**Описание:**

Обновляет объект в таблице.
Идентификатор записи остается неизменным.

**Параметры:**
* o - обновляемый объект.

**Возвращаемое значение:**

Обновленный объект.

**Пример:**
```cs
SCLM sclm = new SCLM();
await sclm.AuthAsync(clientId, secret);
StoryTable<Profile> table = await sclm.GetTableAsync<Profile>(tableId);
Profile updatedProfile = await table.UpdateAsync(Profile.UpdateProfile(profile));
```

#### Method: Task<IEnumerable\<T>> UpdateAsync(IEnumerable\<T> o)

**Описание:**

Обновляет коллекцию объектов в таблице.
Идентификатор записи остается неизменным.

**Параметры:**
* o - коллекция обновляемых объектов.

**Возвращаемое значение:**

Коллекция обновленных объектов.

**Пример:**
```cs
SCLM sclm = new SCLM();
await sclm.AuthAsync(clientId, secret);
StoryTable<Profile> table = await sclm.GetTableAsync<Profile>(tableId);
List<Profile> updatedProfiles = new List<Profile>(await table.UpdateAsync(Profile.UpdateProfiles(profiles)));
```

#### Method: Task\<T> DeleteAsync(string id)

**Описание:**

Удаляет объект таблицы по идентификатору.

**Параметры:**
* id - Идентификатор записи.

**Возвращаемое значение:**

Удаленный объект.

**Пример:**
```cs
SCLM sclm = new SCLM();
await sclm.AuthAsync(clientId, secret);
StoryTable<Profile> table = await sclm.GetTableAsync<Profile>(tableId);
Profile deleteResult = await table.DeleteAsync(profile._id);
```

#### Method: Task<IEnumerable\<T>> DeleteAsync(IEnumerable\<string> ids)

**Описание:**

Удаляет коллекцию объектов таблицы по поллекции идентификаторов.

**Параметры:**
* ids - коллекция идентификаторов.

**Возвращаемое значение:**

Коллекция удаленных объектов.

**Пример:**
```cs
SCLM sclm = new SCLM();
await sclm.AuthAsync(clientId, secret);
StoryTable<Profile> table = await sclm.GetTableAsync<Profile>(tableId);
IEnumerable<Profile> deleteResults = await table.DeleteAsync(profiles.Select(t=> t._id));
```

#### Method: Task\<long> CountAsync()

**Описание:**

Получает колличество записей в таблице.

**Возвращаемое значение:**

Колличество записей.

**Пример:**
```cs
SCLM sclm = new SCLM();
await sclm.AuthAsync(clientId, secret);
StoryTable<Profile> table = await sclm.GetTableAsync<Profile>(tableId);
long count = await table.CountAsync(tableId);
```

#### Method: Task\<long> CountAsync(string query)

**Описание:**

Получает колличество записей в таблице по запросу.
Запрос должен быть в формате [TablesQuery](https://github.com/storyclm/documentation/blob/master/TABLES_QUERY.md).

**Параметры:**
* query - запрос в формате [TablesQuery](https://github.com/storyclm/documentation/blob/master/TABLES_QUERY.md)

**Возвращаемое значение:**

Колличество записей.

**Пример:**
```cs
SCLM sclm = new SCLM();
await sclm.AuthAsync(clientId, secret);
StoryTable<Profile> table = await sclm.GetTableAsync<Profile>(tableId);
long count = await table.CountAsync("[age][gt][30]");
```

#### Method: Task\<long> LogCountAsync()

**Описание:**

Получает колличество записей лога таблицы.

**Возвращаемое значение:**

Колличество записей.

**Пример:**
```cs
SCLM sclm = new SCLM();
await sclm.AuthAsync(clientId, secret);
StoryTable<Profile> table = await sclm.GetTableAsync<Profile>(tableId);
long count = await table.LogCountAsync();
```

#### Method: Task\<long> LogCountAsync(DateTime date)

**Описание:**

Получает колличество записей лога после указанной даты.

**Параметры:**
* date - Дата, после которой будет произведена выборка.

**Возвращаемое значение:**

Колличество записей.

**Пример:**
```cs
SCLM sclm = new SCLM();
await sclm.AuthAsync(clientId, secret);
StoryTable<Profile> table = await sclm.GetTableAsync<Profile>(tableId);
long count = await table.LogCountAsync((DateTime.Now.AddDays(-25)));
```

#### Method: Task<IEnumerable\<ApiLog>> LogAsync(int skip, int take)

**Описание:**

Получает записи лога.

**Параметры:**
* skip - Отступ в запросе. Сколько первых элементов нужно пропустить. По умолчанию - 0.
* take - Максимальное количество записей, которые будут получены. По умолчанию - 100, максимально 1000.

**Возвращаемое значение:**

Коллекция записей лога.

**Пример:**
```cs
SCLM sclm = new SCLM();
await sclm.AuthAsync(clientId, secret);
StoryTable<Profile> table = await sclm.GetTableAsync<Profile>(tableId);
IEnumerable<ApiLog> = await table.LogAsync(0, 1000);
```

#### Method: Task<IEnumerable\<ApiLog>> LogAsync(DateTime date, int skip, int take)

**Описание:**

Получает записи лога, после указаной даты.

**Параметры:**
* date - Дата, после которой будет произведена выборка.
* skip - Отступ в запросе. Сколько первых элементов нужно пропустить. По умолчанию - 0.
* take - Максимальное количество записей, которые будут получены. По умолчанию - 100, максимально 1000.

**Возвращаемое значение:**

Коллекция записей лога.

**Пример:**
```cs
SCLM sclm = new SCLM();
await sclm.AuthAsync(clientId, secret);
StoryTable<Profile> table = await sclm.GetTableAsync<Profile>(tableId);
IEnumerable<ApiLog> = await table.LogAsync(DateTime.Now.AddDays(-25), 0, 1000);
```

#### Method: Task\<T> FindAsync(string id)

**Описание:**

Получает запись таблицы по идентификатору.

**Параметры:**
* id - Идентификатор записи.

**Возвращаемое значение:**

Объект в таблице.

**Пример:**
```cs
SCLM sclm = new SCLM();
await sclm.AuthAsync(clientId, secret);
StoryTable<Profile> table = await sclm.GetTableAsync<Profile>(tableId);
Profile profile = await table.FindAsync(id);
```

#### Method: Task<IEnumerable\<T>> FindAsync(IEnumerable\<string> ids)

**Описание:**

Получает коллекцию записей по списку идентификаторов.

**Параметры:**
* ids - коллекция идентификаторов.

**Возвращаемое значение:**

Коллекция объектов.

**Пример:**
```cs
SCLM sclm = new SCLM();
await sclm.AuthAsync(clientId, secret);
StoryTable<Profile> table = await sclm.GetTableAsync<Profile>(tableId);
IEnumerable<Profile> profiles = await table.FindAsync(ids);
```

#### Method: Task<IEnumerable\<T>> FindAsync(int skip, int take)

**Описание:**

Получает постранично все данные таблицы.

**Параметры:**
* skip - Отступ в запросе. Сколько первых элементов нужно пропустить. По умолчанию - 0.
* take - Максимальное количество записей, которые будут получены. По умолчанию - 100, максимально 1000.

**Возвращаемое значение:**

Коллекция объектов.

**Пример:**
```cs
SCLM sclm = new SCLM();
await sclm.AuthAsync(clientId, secret);
StoryTable<Profile> table = await sclm.GetTableAsync<Profile>(tableId);
IEnumerable<Profile> profiles = await table.FindAsync(0, 1000);
```

#### Method: Task<IEnumerable\<T>> FindAsync(string query, string sortfield, int sort, int skip, int take)

**Описание:**

Получает постранично данные по запросу.
Формат запроса - [TablesQuery](https://github.com/storyclm/documentation/blob/master/TABLES_QUERY.md).

[TablesQuery](https://github.com/storyclm/documentation/blob/master/TABLES_QUERY.md) - это язык запросов, разработанного специально для StoryCLM. 
Запрос в данном формате легко транслируется в любы другие языки запросов.

Параметры из которых создается запрос могут быть двух типов: Comparison и Logical.

**Параметры:**
* query - Запрос в формате [TablesQuery](https://github.com/storyclm/documentation/blob/master/TABLES_QUERY.md).
* sortfield - Поле, по которому нужно произвести сортировку.
* sort - Тип сортировки.
* tableId - Идентификатор таблицы в базе данных.
* skip - Отступ в запросе. Сколько первых элементов нужно пропустить. По умолчанию - 0.
* take - Максимальное количество записей, которые будут получены. По умолчанию - 100, максимально 1000.

**Возвращаемое значение:**

Коллекция объектов.

**Пример:**
```cs
SCLM sclm = new SCLM();
await sclm.AuthAsync(clientId, secret);
StoryTable<Profile> table = await sclm.GetTableAsync();

//возраст меньше или равен 30
IEnumerable<Profile> profiles  = await table.FindAsync("[age][lte][30]", "age", 1, 0, 100);

//поле "name" начинается с символа "T"
profiles = await table.FindAsync("[name][sw][\"T\"]", "age", 1, 0, 100);

//поле "name" содержит строку "ad"
profiles = await table.FindAsync("[Name][cn][\"ad\"]", "age", 1, 0, 100);

//поиск имен из списка
profiles = await table.FindAsync("[Name][in][\"Stanislav\",\"Tamerlan\"]", "age", 1, 0, 100);

//Выбрать женщин, имя ("name") которых начинается со строки "V" 
profiles = await table.FindAsync("[gender][eq][false][and][Name][sw][\"V\"]", "age", 1, 0, 100);

//Выбрать мужчин младше 30 и женщин старше 30
profiles = await table.FindAsync("[gender][eq][true][and][age][lt][30][or][gender][eq][false][and][age][gt][30]", "age", 1, 0, 100);

//поле "name" начинается с символов "T" или "S" при этом возраст должен быть равен 22
profiles = await table.FindAsync("([name][sw][\"S\"][or][name][sw][\"T\"])[and][age][eq][22]", "age", 1, 0, 100);

//Выбрать всех с возрастом НЕ в интервале [25,30] и с именами на "S" и "Т"
profiles = await table.FindAsync("([age][lt][22][or][age][gt][30])[and]([name][sw][\"S\"][or][name][sw][\"T\"])", "age", 1, 0, 100);
```






