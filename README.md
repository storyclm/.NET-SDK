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

SCLM - это сингтон класс, который содержит все методы для работы с API.

Первым делом, необходимо аутентифицироваться и полчуть токен доступа.
```cs
SCLM sclm = SCLM.Instance;
Tokent token = await sclm.AuthAsync(clientId, secret);
```
Токен автоматически сохраняется. Стоит учитывать, что токен "живет" один час. 
После чего нужно получуть новый токен, вызвав метод AuthAsync.

## Примеры

### Таблицы

[Таблицы](https://github.com/storyclm/documentation/blob/master/TABLES.md) - это релиационное хранилище данных.

*Более подробная информация содержится в разделе ["Таблицы"](https://github.com/storyclm/documentation/blob/master/TABLES.md) документации.*

**Получить все таблицы клиента**
```cs
SCLM sclm = SCLM.Instance;
IEnumerable<ApiTable> tables = await sclm.GetTablesAsync(clientId);
```

Для работы с таблицей создадим объект Profile. Этот объект будет соотвествовать записи в таблице "Profile".
```cs

```






