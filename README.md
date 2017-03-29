# StoryCLM .NET SDK

StoryCLM .NET SDK позволяет легко интегрировать систему [StoryCLM](http://breffi.ru/ru/storyclm) c Вашим приложением на .NET.
Данная библиотека сделана на базе [REST API](https://github.com/storyclm/documentation/blob/master/RESTAPI.md) сервиса [StoryCLM](http://breffi.ru/ru/storyclm) и максимально упращает работу с API.

Ниже будет рассказано как устаносить библиотеку, настроить проект для работы с ней и показана работа SDK на конкретных примерах.

## Frameworks:
* .NET Framework 4.5

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

## Настройка и получние ключей

**Подключение пространств имен**

В файл, в котором Вы хотите использовать SDK, необходимо добавить следующий код:
```cs
using StoryCLM.SDK;
using StoryCLM.SDK.Models;
```
**Активация API и получение ключей доступа**

Для того что бы получить доступ к API своего клиента нужно его активировать на панели администрирования и получить ключ доступа. 

*Что бы узнать как активировать API, получить ключи доступа и узнать подробную информацию о аутентификации и авторизации в системе нужно ознакомиться с документацией по [REST API](https://github.com/storyclm/documentation/blob/master/RESTAPI.md#Активация).*

**Использование класса SCLM и аутентификация**

SCLM - это singleton, который содержит все методы для работы с API.

Первым делом, необходимо аутентифицироваться и полчуть токен доступа.
```cs
SCLM sclm = SCLM.Instance;
Tokent token = await sclm.AuthAsync(clientId, secret);
```
Токен автоматически сохраняется. Стоит учитывать, что токен "живет" один час. 
После чего нужно получуть новый токен, вызвав метод AuthAsync.

## Таблицы


[Таблицы](https://github.com/storyclm/documentation/blob/master/TABLES.md) - это реляционное хранилище данных.

*Более подробная информация содержится в разделе ["Таблицы"](https://github.com/storyclm/documentation/blob/master/TABLES.md) документации.*

#### Method: Task<IEnumerable\<ApiTable>> GetTablesAsync(int clientId)

**Описание:**

Получает cписок таблиц клиента.

**Параметры:**
* T - параметризованный тип (generic), описывающий сущность в таблице.
* clientId - Идентификатор клиента в базе данных.

**Возвращаемое значение:**

Список таблиц

**Пример:**
```cs
SCLM sclm = SCLM.Instance;
IEnumerable<ApiTable> tables = await sclm.GetTablesAsync(228);
```

SDK при работе с таблицами оперирует объектами. Объект должен соответсвовать схеме таблицы. Один объект - одна запись.
Создадим таблицу Profiles на панели администрирования и кдасс Profile в проекте, поля которого будут соответсвовать схеме таблицы Profiles:
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
#### Method: Task\<T> InsertAsync\<T>(int tableId, T o)

**Описание:**

Добавляет новый объект в таблицу.
Объект должен соответствовать схеме таблицы.

**Параметры:**
* T - параметризованный тип (generic), описывающий сущность в таблице.
* tableId - Идентификатор таблицы в базе данных.
* o - Новый объект.

**Возвращаемое значение:**

Новый объект в таблице.

**Пример:**
```cs
SCLM sclm = SCLM.Instance;
Profile profile = await sclm.InsertAsync<Profile>(tableId, new Profile());
```

#### Method: Task<IEnumerable\<T>> InsertAsync\<T>(int tableId, IEnumerable<T> o)

**Описание:**

Добавляет коллекцию новых объектов в таблицу.
Каждая объект должен соответствовать схеме таблицы.

**Параметры:**
* T - параметризованный тип (generic), описывающий сущность в таблице.
* tableId - Идентификатор таблицы в базе данных.
* o - коллекция новых объектов

**Возвращаемое значение:**

Коллекция новых объектов.

**Пример:**
```cs
SCLM sclm = SCLM.Instance;
List<Profile> profiles = new List<Profile>(await sclm.InsertAsync<Profile>(tableId, Profile.CreateProfiles()));
```

#### Method: Task\<T> UpdateAsync\<T>(int tableId, T o)

**Описание:**

Обновляет объект в таблице.
Идентификатор записи остается неизменным.

**Параметры:**
* T - параметризованный тип (generic), описывающий сущность в таблице.
* tableId - Идентификатор таблицы в базе данных.
* o - обновляемый объект.

**Возвращаемое значение:**

Обновленный объект.

**Пример:**
```cs
SCLM sclm = SCLM.Instance;
Profile updatedProfile = await sclm.UpdateAsync<Profile>(tableId, Profile.UpdateProfile(profile));
```

#### Method: Task<IEnumerable\<T>> UpdateAsync\<T>(int tableId, IEnumerable\<T> o)

**Описание:**

Обновляет коллекцию объектов в таблице.
Идентификатор записи остается неизменным.

**Параметры:**
* T - параметризованный тип (generic), описывающий сущность в таблице.
* tableId - Идентификатор таблицы в базе данных.
* o - коллекция обновляемых объектов.

**Возвращаемое значение:**

Коллекция обновленных объектов.

**Пример:**
```cs
SCLM sclm = SCLM.Instance;
List<Profile> updatedProfiles = new List<Profile>(await sclm.UpdateAsync<Profile>(tableId, Profile.UpdateProfiles(profiles)));
```

#### Method: Task\<T> DeleteAsync\<T>(int tableId, string id)

**Описание:**

Удаляет объект таблицы по идентификатору.

**Параметры:**
* T - параметризованный тип (generic), описывающий сущность в таблице.
* tableId - Идентификатор таблицы в базе данных.
* id - Идентификатор записи.

**Возвращаемое значение:**

Удаленный объект.

**Пример:**
```cs
SCLM sclm = SCLM.Instance;
Profile deleteResult = await sclm.DeleteAsync<Profile>(tableId, profile._id);
```

#### Method: Task<IEnumerable\<T>> DeleteAsync\<T>(int tableId, IEnumerable\<string> ids)

**Описание:**

Удаляет коллекцию объектов таблицы по поллекции идентификаторов.

**Параметры:**
* T - параметризованный тип (generic), описывающий сущность в таблице.
* tableId - Идентификатор таблицы в базе данных.
* ids - коллекция идентификаторов.

**Возвращаемое значение:**

Коллекция удаленных объектов.

**Пример:**
```cs
SCLM sclm = SCLM.Instance;
IEnumerable<Profile> deleteResults = await sclm.DeleteAsync<Profile>(tableId, profiles.Select(t=> t._id));
```

#### Method: Task\<long> CountAsync(int tableId)

**Описание:**

Получает колличество записей в таблице.

**Параметры:**
* tableId - Идентификатор таблицы в базе данных.

**Возвращаемое значение:**

Колличество записей.

**Пример:**
```cs
SCLM sclm = SCLM.Instance;
long count = await sclm.CountAsync(tableId);
```

#### Method: Task\<long> CountAsync(int tableId, string query)

**Описание:**

Получает колличество записей в таблице по запросу.
Запрос должен быть в формате [TablesQuery](https://github.com/storyclm/documentation/blob/master/TABLES_QUERY.md).

**Параметры:**
* tableId - Идентификатор таблицы в базе данных.
* query - запрос в формате [TablesQuery](https://github.com/storyclm/documentation/blob/master/TABLES_QUERY.md)

**Возвращаемое значение:**

Колличество записей.

**Пример:**
```cs
SCLM sclm = SCLM.Instance;
long count = await sclm.CountAsync(tableId, "[age][gt][30]");
```

#### Method: Task\<long> LogCountAsync(int tableId)

**Описание:**

Получает колличество записей лога таблицы.

**Параметры:**
* tableId - Идентификатор таблицы в базе данных.

**Возвращаемое значение:**

Колличество записей.

**Пример:**
```cs
SCLM sclm = SCLM.Instance;
long count = await sclm.LogCountAsync(tableId);
```

#### Method: Task\<long> LogCountAsync(int tableId, DateTime date)

**Описание:**

Получает колличество записей лога после указанной даты.

**Параметры:**
* tableId - Идентификатор таблицы в базе данных.
* date - Дата, после которой будет произведена выборка.

**Возвращаемое значение:**

Колличество записей.

**Пример:**
```cs
SCLM sclm = SCLM.Instance;
long count = await sclm.LogCountAsync(tableId, (DateTime.Now.AddDays(-25)));
```

#### Method: Task<IEnumerable\<ApiLog>> LogAsync(int tableId, int skip, int take)

**Описание:**

Получает записи лога.

**Параметры:**
* tableId - Идентификатор таблицы в базе данных.
* skip - Отступ в запросе. Сколько первых элементов нужно пропустить. По умолчанию - 0.
* take - Максимальное количество записей, которые будут получены. По умолчанию - 100, максимально 1000.

**Возвращаемое значение:**

Коллекция записей лога.

**Пример:**
```cs
SCLM sclm = SCLM.Instance;
IEnumerable<ApiLog> = await sclm.LogAsync(tableId, 0, 1000);
```

#### Method: Task<IEnumerable\<ApiLog>> LogAsync(int tableId, DateTime date, int skip, int take)

**Описание:**

Получает записи лога, после указаной даты.

**Параметры:**
* tableId - Идентификатор таблицы в базе данных.
* date - Дата, после которой будет произведена выборка.
* skip - Отступ в запросе. Сколько первых элементов нужно пропустить. По умолчанию - 0.
* take - Максимальное количество записей, которые будут получены. По умолчанию - 100, максимально 1000.

**Возвращаемое значение:**

Коллекция записей лога.

**Пример:**
```cs
SCLM sclm = SCLM.Instance;
IEnumerable<ApiLog> = await sclm.LogAsync(tableId, DateTime.Now.AddDays(-25), 0, 1000);
```

#### Method: Task\<T> FindAsync\<T>(int tableId, string id)

**Описание:**

Получает запись таблицы по идентификатору.

**Параметры:**
* T - параметризованный тип (generic), описывающий сущность в таблице.
* tableId - Идентификатор таблицы в базе данных.

**Возвращаемое значение:**

Объект в таблице.

**Пример:**
```cs
SCLM sclm = SCLM.Instance;
Profile profile = await sclm.FindAsync<Profile>(tableId, id);
```

#### Method: Task<IEnumerable\<T>> FindAsync\<T>(int tableId, IEnumerable\<string> ids)

**Описание:**

Получает коллекцию записей по списку идентификаторов.

**Параметры:**
* T - параметризованный тип (generic), описывающий сущность в таблице.
* tableId - Идентификатор таблицы в базе данных.
* ids - коллекция идентификаторов.

**Возвращаемое значение:**

Коллекция объектов.

**Пример:**
```cs
SCLM sclm = SCLM.Instance;
IEnumerable<Profile> profiles = await sclm.FindAsync<Profile>(tableId, ids);
```

#### Method: Task<IEnumerable\<T>> FindAsync\<T>(int tableId, int skip, int take)

**Описание:**

Получает постранично все данные таблицы.

**Параметры:**
* T - параметризованный тип (generic), описывающий сущность в таблице.
* tableId - Идентификатор таблицы в базе данных.
* skip - Отступ в запросе. Сколько первых элементов нужно пропустить. По умолчанию - 0.
* take - Максимальное количество записей, которые будут получены. По умолчанию - 100, максимально 1000.

**Возвращаемое значение:**

Коллекция объектов.

**Пример:**
```cs
SCLM sclm = SCLM.Instance;
IEnumerable<Profile> profiles = sclm.FindAsync<Profile>(tableId, 0, 1000).Result;
```

#### Method: Task<IEnumerable\<T>> FindAsync\<T>(int tableId, string query, string sortfield, int sort, int skip, int take)

**Описание:**

Получает постранично данные по запросу.
Формат запроса - [TablesQuery](https://github.com/storyclm/documentation/blob/master/TABLES_QUERY.md).

[TablesQuery](https://github.com/storyclm/documentation/blob/master/TABLES_QUERY.md) - это язык запросов, разработанного специально для StoryCLM. 
Запрос в данном формате легко транслируется в любы другие языки запросов.

Параметры из которых создается запрос могут быть двух типов: Comparison и Logical.

**Параметры:**
* T - параметризованный тип (generic), описывающий сущность в таблице.
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
SCLM sclm = SCLM.Instance;

//возраст меньше или равен 30
IEnumerable<Profile> profiles  = sclm.FindAsync<Profile>(tableId, "[age][lte][30]", "age", 1, 0, 100).Result;

//поле "name" начинается с символа "T"
profiles = sclm.FindAsync<Profile>(tableId, "[name][sw][\"T\"]", "age", 1, 0, 100).Result;

//поле "name" содержит строку "ad"
profiles = sclm.FindAsync<Profile>(tableId, "[Name][cn][\"ad\"]", "age", 1, 0, 100).Result;

//поиск имен из списка
profiles = sclm.FindAsync<Profile>(tableId, "[Name][in][\"Stanislav\",\"Tamerlan\"]", "age", 1, 0, 100).Result;

//Выбрать женщин, имя ("name") которых начинается со строки "V" 
profiles = sclm.FindAsync<Profile>(tableId, "[gender][eq][false][and][Name][sw][\"V\"]", "age", 1, 0, 100).Result;

//Выбрать мужчин младше 30 и женщин старше 30
profiles = sclm.FindAsync<Profile>(tableId, "[gender][eq][true][and][age][lt][30][or][gender][eq][false][and][age][gt][30]", "age", 1, 0, 100).Result;

//поле "name" начинается с символов "T" или "S" при этом возраст должен быть равен 22
profiles = sclm.FindAsync<Profile>(tableId, "([name][sw][\"S\"][or][name][sw][\"T\"])[and][age][eq][22]", "age", 1, 0, 100).Result;

//Выбрать всех с возрастом НЕ в интервале [25,30] и с именами на "S" и "Т"
profiles = sclm.FindAsync<Profile>(tableId, "([age][lt][22][or][age][gt][30])[and]([name][sw][\"S\"][or][name][sw][\"T\"])", "age", 1, 0, 100).Result;
```






