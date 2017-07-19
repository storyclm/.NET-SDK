# StoryCLM .NET SDK

StoryCLM .NET SDK ��������� ����� ������������� ������� [StoryCLM](http://breffi.ru/ru/storyclm) c ����� ����������� �� .NET.
������ ���������� ������� �� ���� [REST API](https://github.com/storyclm/documentation/blob/master/RESTAPI.md) ������� [StoryCLM](http://breffi.ru/ru/storyclm) � ����������� �������� ������ � API.

���� ����� ���������� ��� ���������� ����������, ��������� ������ ��� ������ � ��� � �������� ������ SDK �� ���������� ��������.

## Frameworks:
* .NET Standard 1.1

## ���������

���������� SDK ����� ����� ���������:
* NuGet Package Manager.
* ��������� SDK ��������������� �� ����� �����������.

### NuGet

[NuGet Package Manager](https://www.nuget.org/) ������ ���� �������������� ���������� � �������� � Visual Studio 2015 ��� ����� ������ ������.

**��������� ����� NuGet Package Manager � �������������� �������**

��������� ��������� ������� � ������� NuGet Package Manager ��� ��������� SDK � ���� ��� ������������:
```
PM> Install-Package StoryCLM
```
**��������� ����� NuGet Package Manager � �������������� ���������� Visual Studio**

����� ���������� SDK � ������� ���������� Visual Studio NuGet, ��������� ��������� ��������:
* � ������������ ������� �������� ������ ������� ���� �� ������ � �������� "���������� NuGet ��������".
* � ������ ������� "StoryCLM" � ������� Enter.
* ����� ���������� ������, �������� �� ������ StoryCLM .NET SDK � ������� ������ "����������".

### Git

��������� � �������� ������� [�����������](https://github.com/storyclm/.NET-SDK) � ������� �� ������ "Clone or �ownload".

![rest Image 5](./images/1.png)

## ��������� � ��������� ������� ������

**����������� ����������� ����**

� ����, � ������� �� ������ ������������ SDK, ���������� �������� ��������� ���:
```cs
using StoryCLM.SDK;
using StoryCLM.SDK.Models;
```
**�������� ������� ������**

��� ���� ��� �� �������� ������ � API ������ ������� ����� �� ������ ����������������� ������� � ��������� ����������. 

*����� ��������� ���������� � ������� ������, ����� �������, � �������������� � ����������� � ������� ����� ������������ � ������������� �� [REST API](https://github.com/storyclm/documentation/blob/master/RESTAPI.md#���������).*

**������������� ������ SCLM**

SCLM - ��� �����-��������, ������� �������� ������ ��� ������ � API StoryCLM. ��� ������ ����� ������� ��������� ������ SCLM � ������ ��������������.

```cs
SCLM sclm = new SCLM();
```

**A�������������**

������ ����� ����������������� � API StoryCLM � ���� �������:

* **�� ����� ������� (Service)** - ������������ ��� ���������� StoryCLM � ������ ��������. ��� �������������� ���������� ������ ClientId � Secret. ������ �������� ������ ������ � ������ ������� StoryCLM � ������� ��� ���� ������ ������� ������.
* **�� ����� ������������ StoryCLM (Application)** - ������������ � ��������� ��� ���������� �����������. ������ ClientId � Secret ��� �������� ������������� ��������� ��� Username � Password ������������. ������ �������� �� ����� ������������ StoryCLM �, �� ����������� ��������� �������� API (�������), �������� ������ �� ���� �������� ������� �������� ������������ � ������� StoryCLM � ������� � ���� ���� �����.


������������� �� ����� �������:
```cs
SCLM sclm = new SCLM();
StoryToken token = await sclm.AuthAsync(clientId, secret);
```
�������������� �� ����� ������������:
```cs
SCLM sclm = new SCLM();
StoryToken token = await sclm.AuthAsync(clientId, secret, username, password);
```
� ������ �������� �������������� ����� ��������� ������ StoryToken. ������ StoryToken ����� ��������� ����:

* **AccessToken** - ������ �������. 
* **RefreshToken** - ������ ����������. �������� ���� ������ ������� �� ����� ������������.
* **Expires** - ���� �������� ����� ������� �������.

� ������ ������������� StoryToken ����� �������� �� ���������, �������� � ���� "Token".
```cs
StoryToken token = sclm.Token;
```

**�������� ���������� �������� �������**

����� �������� ������������� ������ ������� ������������� ����������� �� ��� ����� ����� ��������� � ������������� ������������� � ������� ������� � �������. ����� ���������, ��� ������ ������� "�����" ���� ���. ����� ��������� ����� �������� �������� ������� ������� ����� �������� ����� ������. ���� ������� �������� � ����������� �� ���� �������������� � ������� ���������� � StoryCLM.

**Service** - ������ �������� �� ����� �������. ���� ����� ����� ��������� ������ ������� ����� ������� �������, �� ��� ������ �������� ��������� ������ ����� ���������� ��� 401. ��� �������� ��� ������ �� �����������. ��� �� ����� �� ��������� ����� �������������� ����� ����� ������� ������� � ����� ��������� ����� ��� �������� ��������� �������������� � �������� ����� ������. 

���� ������ ��������, �� ����� ��������� StoryToken ����� ��������� � ����������� ��� ���������.
```cs
SCLM sclm = new SCLM();
StoryToken token = GetToken();
sclm.Token = token;
```
��� ����� ������� ������� �������������� ���� Expires ������� StoryToken ��� �� ������� ������ ����� ������� �������� ������.

������ ��������� ��� ���������� ������� ���������� ������ "Service", ����� ��������� ����� �� ������ ���� � ����������� ����� ������� � StoryCLM, ��� ���� ������ ��� ������� ����� ������ �������. �� ������� ��������� � ������� ������ ������� � ������������ ��������� � � �������� ����.

**Application** - ������ �������� �� ����� ������������ StoryCLM. ����� �������������� ������ ������� ������� ������ ���������� ������ ����������� (RefreshToken). ����� ����� ����� ������� ���. �� ������������ ��� ��������� ������ ������� ������� ��� ������ � ������. ��� ������� ��� ���� ��� �� ������ ��� ����� ������ ������� ���������� ����������� �� ���������� ������������ ������� ����� � ������. �������� ������������� ��������� ������ ����������, ������������ ����� ����� ������� ������� � �������������� �������� ����� ������� ������� � ����������. ����� ������� �������� ����� ������������ � ��������� �������� ������� � ������� �������������� ����������� �������.

��� �� ��� � � "Service" �������� ����� ��������� ������ StoryToken, ��������� ��� � ������ ������ ������������. �� ������ ��������� ����� ��������� ������ ������ ����������. � ����� ������ �� ����� ������� ����� � ������ ������������, ��� �� ������ ���������� ����� ������� � �������� ����. ������ ��� ��� ������� ���������� ����� ��������� �� ��������� ������ ���������� � ��������� ���� ��� ��������� ������ ������� �������:
```cs
SCLM sclm = new SCLM();
string refreshToken = GetRefreshToken();
await sclm.AuthAsync(clientId, secret, refreshToken);
```
�� ���������� ������ ����������, ����� �������� ������ ����������.


## �������


[�������](https://github.com/storyclm/documentation/blob/master/TABLES.md) - ��� ����������� ��������� ������.

*����� ��������� ���������� ���������� � ������� ["�������"](https://github.com/storyclm/documentation/blob/master/TABLES.md) ������������.*

#### Method: Task<IEnumerable\<ApiTable>> GetTablesAsync(int clientId)

**��������:**

�������� c����� ������ �������.

**���������:**
* T - ����������������� ��� (generic), ����������� �������� � �������.
* clientId - ������������� ������� � ���� ������.

**������������ ��������:**

������ ������

**������:**
```cs
SCLM sclm = SCLM.Instance;
IEnumerable<ApiTable> tables = await sclm.GetTablesAsync(228);
```

SDK ��� ������ � ��������� ��������� ���������. ������ ������ �������������� ����� �������. ���� ������ - ���� ������.
�������� ������� Profiles �� ������ ����������������� � ����� Profile � �������, ���� �������� ����� �������������� ����� ������� Profiles:
```cs
    /// <summary>
    /// ������� ������������
    /// </summary>
    public class Profile
    {
        /// <summary>
        /// ������������� ������
        /// ������� �� ���������� ������
        /// </summary>
        public string _id { get; set; }

        /// <summary>
        /// ��� ������������
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// �������
        /// </summary>
        public long Age { get; set; }

        /// <summary>
        /// ���
        /// </summary>
        public bool Gender { get; set; }

        /// <summary>
        /// �������
        /// </summary>
        public double Rating { get; set; }

        /// <summary>
        /// ���� �����������
        /// </summary>
        public DateTime Created { get; set; }

    }
```
#### Method: Task\<T> InsertAsync\<T>(int tableId, T o)

**��������:**

��������� ����� ������ � �������.
������ ������ ��������������� ����� �������.

**���������:**
* T - ����������������� ��� (generic), ����������� �������� � �������.
* tableId - ������������� ������� � ���� ������.
* o - ����� ������.

**������������ ��������:**

����� ������ � �������.

**������:**
```cs
SCLM sclm = SCLM.Instance;
Profile profile = await sclm.InsertAsync<Profile>(tableId, new Profile());
```

#### Method: Task<IEnumerable\<T>> InsertAsync\<T>(int tableId, IEnumerable<T> o)

**��������:**

��������� ��������� ����� �������� � �������.
������ ������ ������ ��������������� ����� �������.

**���������:**
* T - ����������������� ��� (generic), ����������� �������� � �������.
* tableId - ������������� ������� � ���� ������.
* o - ��������� ����� ��������

**������������ ��������:**

��������� ����� ��������.

**������:**
```cs
SCLM sclm = SCLM.Instance;
List<Profile> profiles = new List<Profile>(await sclm.InsertAsync<Profile>(tableId, Profile.CreateProfiles()));
```

#### Method: Task\<T> UpdateAsync\<T>(int tableId, T o)

**��������:**

��������� ������ � �������.
������������� ������ �������� ����������.

**���������:**
* T - ����������������� ��� (generic), ����������� �������� � �������.
* tableId - ������������� ������� � ���� ������.
* o - ����������� ������.

**������������ ��������:**

����������� ������.

**������:**
```cs
SCLM sclm = SCLM.Instance;
Profile updatedProfile = await sclm.UpdateAsync<Profile>(tableId, Profile.UpdateProfile(profile));
```

#### Method: Task<IEnumerable\<T>> UpdateAsync\<T>(int tableId, IEnumerable\<T> o)

**��������:**

��������� ��������� �������� � �������.
������������� ������ �������� ����������.

**���������:**
* T - ����������������� ��� (generic), ����������� �������� � �������.
* tableId - ������������� ������� � ���� ������.
* o - ��������� ����������� ��������.

**������������ ��������:**

��������� ����������� ��������.

**������:**
```cs
SCLM sclm = SCLM.Instance;
List<Profile> updatedProfiles = new List<Profile>(await sclm.UpdateAsync<Profile>(tableId, Profile.UpdateProfiles(profiles)));
```

#### Method: Task\<T> DeleteAsync\<T>(int tableId, string id)

**��������:**

������� ������ ������� �� ��������������.

**���������:**
* T - ����������������� ��� (generic), ����������� �������� � �������.
* tableId - ������������� ������� � ���� ������.
* id - ������������� ������.

**������������ ��������:**

��������� ������.

**������:**
```cs
SCLM sclm = SCLM.Instance;
Profile deleteResult = await sclm.DeleteAsync<Profile>(tableId, profile._id);
```

#### Method: Task<IEnumerable\<T>> DeleteAsync\<T>(int tableId, IEnumerable\<string> ids)

**��������:**

������� ��������� �������� ������� �� ��������� ���������������.

**���������:**
* T - ����������������� ��� (generic), ����������� �������� � �������.
* tableId - ������������� ������� � ���� ������.
* ids - ��������� ���������������.

**������������ ��������:**

��������� ��������� ��������.

**������:**
```cs
SCLM sclm = SCLM.Instance;
IEnumerable<Profile> deleteResults = await sclm.DeleteAsync<Profile>(tableId, profiles.Select(t=> t._id));
```

#### Method: Task\<long> CountAsync(int tableId)

**��������:**

�������� ����������� ������� � �������.

**���������:**
* tableId - ������������� ������� � ���� ������.

**������������ ��������:**

����������� �������.

**������:**
```cs
SCLM sclm = SCLM.Instance;
long count = await sclm.CountAsync(tableId);
```

#### Method: Task\<long> CountAsync(int tableId, string query)

**��������:**

�������� ����������� ������� � ������� �� �������.
������ ������ ���� � ������� [TablesQuery](https://github.com/storyclm/documentation/blob/master/TABLES_QUERY.md).

**���������:**
* tableId - ������������� ������� � ���� ������.
* query - ������ � ������� [TablesQuery](https://github.com/storyclm/documentation/blob/master/TABLES_QUERY.md)

**������������ ��������:**

����������� �������.

**������:**
```cs
SCLM sclm = SCLM.Instance;
long count = await sclm.CountAsync(tableId, "[age][gt][30]");
```

#### Method: Task\<long> LogCountAsync(int tableId)

**��������:**

�������� ����������� ������� ���� �������.

**���������:**
* tableId - ������������� ������� � ���� ������.

**������������ ��������:**

����������� �������.

**������:**
```cs
SCLM sclm = SCLM.Instance;
long count = await sclm.LogCountAsync(tableId);
```

#### Method: Task\<long> LogCountAsync(int tableId, DateTime date)

**��������:**

�������� ����������� ������� ���� ����� ��������� ����.

**���������:**
* tableId - ������������� ������� � ���� ������.
* date - ����, ����� ������� ����� ����������� �������.

**������������ ��������:**

����������� �������.

**������:**
```cs
SCLM sclm = SCLM.Instance;
long count = await sclm.LogCountAsync(tableId, (DateTime.Now.AddDays(-25)));
```

#### Method: Task<IEnumerable\<ApiLog>> LogAsync(int tableId, int skip, int take)

**��������:**

�������� ������ ����.

**���������:**
* tableId - ������������� ������� � ���� ������.
* skip - ������ � �������. ������� ������ ��������� ����� ����������. �� ��������� - 0.
* take - ������������ ���������� �������, ������� ����� ��������. �� ��������� - 100, ����������� 1000.

**������������ ��������:**

��������� ������� ����.

**������:**
```cs
SCLM sclm = SCLM.Instance;
IEnumerable<ApiLog> = await sclm.LogAsync(tableId, 0, 1000);
```

#### Method: Task<IEnumerable\<ApiLog>> LogAsync(int tableId, DateTime date, int skip, int take)

**��������:**

�������� ������ ����, ����� �������� ����.

**���������:**
* tableId - ������������� ������� � ���� ������.
* date - ����, ����� ������� ����� ����������� �������.
* skip - ������ � �������. ������� ������ ��������� ����� ����������. �� ��������� - 0.
* take - ������������ ���������� �������, ������� ����� ��������. �� ��������� - 100, ����������� 1000.

**������������ ��������:**

��������� ������� ����.

**������:**
```cs
SCLM sclm = SCLM.Instance;
IEnumerable<ApiLog> = await sclm.LogAsync(tableId, DateTime.Now.AddDays(-25), 0, 1000);
```

#### Method: Task\<T> FindAsync\<T>(int tableId, string id)

**��������:**

�������� ������ ������� �� ��������������.

**���������:**
* T - ����������������� ��� (generic), ����������� �������� � �������.
* tableId - ������������� ������� � ���� ������.

**������������ ��������:**

������ � �������.

**������:**
```cs
SCLM sclm = SCLM.Instance;
Profile profile = await sclm.FindAsync<Profile>(tableId, id);
```

#### Method: Task<IEnumerable\<T>> FindAsync\<T>(int tableId, IEnumerable\<string> ids)

**��������:**

�������� ��������� ������� �� ������ ���������������.

**���������:**
* T - ����������������� ��� (generic), ����������� �������� � �������.
* tableId - ������������� ������� � ���� ������.
* ids - ��������� ���������������.

**������������ ��������:**

��������� ��������.

**������:**
```cs
SCLM sclm = SCLM.Instance;
IEnumerable<Profile> profiles = await sclm.FindAsync<Profile>(tableId, ids);
```

#### Method: Task<IEnumerable\<T>> FindAsync\<T>(int tableId, int skip, int take)

**��������:**

�������� ����������� ��� ������ �������.

**���������:**
* T - ����������������� ��� (generic), ����������� �������� � �������.
* tableId - ������������� ������� � ���� ������.
* skip - ������ � �������. ������� ������ ��������� ����� ����������. �� ��������� - 0.
* take - ������������ ���������� �������, ������� ����� ��������. �� ��������� - 100, ����������� 1000.

**������������ ��������:**

��������� ��������.

**������:**
```cs
SCLM sclm = SCLM.Instance;
IEnumerable<Profile> profiles = sclm.FindAsync<Profile>(tableId, 0, 1000).Result;
```

#### Method: Task<IEnumerable\<T>> FindAsync\<T>(int tableId, string query, string sortfield, int sort, int skip, int take)

**��������:**

�������� ����������� ������ �� �������.
������ ������� - [TablesQuery](https://github.com/storyclm/documentation/blob/master/TABLES_QUERY.md).

[TablesQuery](https://github.com/storyclm/documentation/blob/master/TABLES_QUERY.md) - ��� ���� ��������, �������������� ���������� ��� StoryCLM. 
������ � ������ ������� ����� ������������� � ���� ������ ����� ��������.

��������� �� ������� ��������� ������ ����� ���� ���� �����: Comparison � Logical.

**���������:**
* T - ����������������� ��� (generic), ����������� �������� � �������.
* query - ������ � ������� [TablesQuery](https://github.com/storyclm/documentation/blob/master/TABLES_QUERY.md).
* sortfield - ����, �� �������� ����� ���������� ����������.
* sort - ��� ����������.
* tableId - ������������� ������� � ���� ������.
* skip - ������ � �������. ������� ������ ��������� ����� ����������. �� ��������� - 0.
* take - ������������ ���������� �������, ������� ����� ��������. �� ��������� - 100, ����������� 1000.

**������������ ��������:**

��������� ��������.

**������:**
```cs
SCLM sclm = SCLM.Instance;

//������� ������ ��� ����� 30
IEnumerable<Profile> profiles  = sclm.FindAsync<Profile>(tableId, "[age][lte][30]", "age", 1, 0, 100).Result;

//���� "name" ���������� � ������� "T"
profiles = sclm.FindAsync<Profile>(tableId, "[name][sw][\"T\"]", "age", 1, 0, 100).Result;

//���� "name" �������� ������ "ad"
profiles = sclm.FindAsync<Profile>(tableId, "[Name][cn][\"ad\"]", "age", 1, 0, 100).Result;

//����� ���� �� ������
profiles = sclm.FindAsync<Profile>(tableId, "[Name][in][\"Stanislav\",\"Tamerlan\"]", "age", 1, 0, 100).Result;

//������� ������, ��� ("name") ������� ���������� �� ������ "V" 
profiles = sclm.FindAsync<Profile>(tableId, "[gender][eq][false][and][Name][sw][\"V\"]", "age", 1, 0, 100).Result;

//������� ������ ������ 30 � ������ ������ 30
profiles = sclm.FindAsync<Profile>(tableId, "[gender][eq][true][and][age][lt][30][or][gender][eq][false][and][age][gt][30]", "age", 1, 0, 100).Result;

//���� "name" ���������� � �������� "T" ��� "S" ��� ���� ������� ������ ���� ����� 22
profiles = sclm.FindAsync<Profile>(tableId, "([name][sw][\"S\"][or][name][sw][\"T\"])[and][age][eq][22]", "age", 1, 0, 100).Result;

//������� ���� � ��������� �� � ��������� [25,30] � � ������� �� "S" � "�"
profiles = sclm.FindAsync<Profile>(tableId, "([age][lt][22][or][age][gt][30])[and]([name][sw][\"S\"][or][name][sw][\"T\"])", "age", 1, 0, 100).Result;
```






