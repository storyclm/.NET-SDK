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

SDK ��� ������ � ��������� ��������� ���������. ������ ������ �������������� ����� �������. ���� ������ - ���� ������.
�������� ������� Profiles �� ������ ����������������� � ����� Profile � ������� � ������ ������� ����� �������������� ����� ������� Profiles:
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

#### Method: Task<IEnumerable\<StoryTable\<T>>> GetTablesAsync\<T>(int clientId)

**��������:**

�������� c����� ������ �������.

**���������:**
* T - ����������������� ��� (generic), ����������� �������� � �������.
* clientId - ������������� ������� � ���� ������.

**������������ ��������:**

������ ������

**������:**
```cs
SCLM sclm = new SCLM();
await sclm.AuthAsync(clientId, secret);
IEnumerable<StoryTable<Profile>> tables = await sclm.GetTablesAsync<Profile>(clientId);
```
#### Method: Task<StoryTable\<T>> GetTableAsync\<T>(int tableId)

**��������:**

�������� ������� �� ��������������.

**���������:**
* T - ����������������� ��� (generic), ����������� �������� � �������.
* tableId - ������������� �������.

**������������ ��������:**

������ "�������"

**������:**
```cs
SCLM sclm = new SCLM();
await sclm.AuthAsync(clientId, secret);
StoryTable<Profile> table = await sclm.GetTableAsync<Profile>(tableId);
```

������ GetTablesAsync � GetTablesAsync ���������� �������������� ������� StoryTable\<Profile>, � ������ ������ � ����� Profile. ��� �������� ��� ��� �������� ��� ������� � ���� ������� ����� ������������ � �������� ����� ����. ��� ���� ��� �� ����������� �������� � ������� ������� � ������ StoryTable ���� ������������ ����� �������. 


#### Method: Task\<T> InsertAsync(T o)

**��������:**

��������� ����� ������ � �������.
������ ������ ��������������� ����� �������.

**���������:**
* o - ����� ������.

**������������ ��������:**

����� ������ � �������.

**������:**
```cs
SCLM sclm = new SCLM();
await sclm.AuthAsync(clientId, secret);
StoryTable<Profile> table = await sclm.GetTableAsync<Profile>(tableId);
Profile profile = await table.InsertAsync(new Profile());
```

#### Method: Task<IEnumerable\<T>> InsertAsync(IEnumerable<T> o)

**��������:**

��������� ��������� ����� �������� � �������.
������ ������ ������ ��������������� ����� �������.

**���������:**
* o - ��������� ����� ��������

**������������ ��������:**

��������� ����� ��������.

**������:**
```cs
SCLM sclm = new SCLM();
await sclm.AuthAsync(clientId, secret);
StoryTable<Profile> table = await sclm.GetTableAsync<Profile>(tableId);
List<Profile> profiles = new List<Profile>(await table.InsertAsync(Profile.CreateProfiles()));
```

#### Method: Task\<T> UpdateAsync(T o)

**��������:**

��������� ������ � �������.
������������� ������ �������� ����������.

**���������:**
* o - ����������� ������.

**������������ ��������:**

����������� ������.

**������:**
```cs
SCLM sclm = new SCLM();
await sclm.AuthAsync(clientId, secret);
StoryTable<Profile> table = await sclm.GetTableAsync<Profile>(tableId);
Profile updatedProfile = await table.UpdateAsync(Profile.UpdateProfile(profile));
```

#### Method: Task<IEnumerable\<T>> UpdateAsync(IEnumerable\<T> o)

**��������:**

��������� ��������� �������� � �������.
������������� ������ �������� ����������.

**���������:**
* o - ��������� ����������� ��������.

**������������ ��������:**

��������� ����������� ��������.

**������:**
```cs
SCLM sclm = new SCLM();
await sclm.AuthAsync(clientId, secret);
StoryTable<Profile> table = await sclm.GetTableAsync<Profile>(tableId);
List<Profile> updatedProfiles = new List<Profile>(await table.UpdateAsync(Profile.UpdateProfiles(profiles)));
```

#### Method: Task\<T> DeleteAsync(string id)

**��������:**

������� ������ ������� �� ��������������.

**���������:**
* id - ������������� ������.

**������������ ��������:**

��������� ������.

**������:**
```cs
SCLM sclm = new SCLM();
await sclm.AuthAsync(clientId, secret);
StoryTable<Profile> table = await sclm.GetTableAsync<Profile>(tableId);
Profile deleteResult = await table.DeleteAsync(profile._id);
```

#### Method: Task<IEnumerable\<T>> DeleteAsync(IEnumerable\<string> ids)

**��������:**

������� ��������� �������� ������� �� ��������� ���������������.

**���������:**
* ids - ��������� ���������������.

**������������ ��������:**

��������� ��������� ��������.

**������:**
```cs
SCLM sclm = new SCLM();
await sclm.AuthAsync(clientId, secret);
StoryTable<Profile> table = await sclm.GetTableAsync<Profile>(tableId);
IEnumerable<Profile> deleteResults = await table.DeleteAsync(profiles.Select(t=> t._id));
```

#### Method: Task\<long> CountAsync()

**��������:**

�������� ����������� ������� � �������.

**������������ ��������:**

����������� �������.

**������:**
```cs
SCLM sclm = new SCLM();
await sclm.AuthAsync(clientId, secret);
StoryTable<Profile> table = await sclm.GetTableAsync<Profile>(tableId);
long count = await table.CountAsync(tableId);
```

#### Method: Task\<long> CountAsync(string query)

**��������:**

�������� ����������� ������� � ������� �� �������.
������ ������ ���� � ������� [TablesQuery](https://github.com/storyclm/documentation/blob/master/TABLES_QUERY.md).

**���������:**
* query - ������ � ������� [TablesQuery](https://github.com/storyclm/documentation/blob/master/TABLES_QUERY.md)

**������������ ��������:**

����������� �������.

**������:**
```cs
SCLM sclm = new SCLM();
await sclm.AuthAsync(clientId, secret);
StoryTable<Profile> table = await sclm.GetTableAsync<Profile>(tableId);
long count = await table.CountAsync("[age][gt][30]");
```

#### Method: Task\<long> LogCountAsync()

**��������:**

�������� ����������� ������� ���� �������.

**������������ ��������:**

����������� �������.

**������:**
```cs
SCLM sclm = new SCLM();
await sclm.AuthAsync(clientId, secret);
StoryTable<Profile> table = await sclm.GetTableAsync<Profile>(tableId);
long count = await table.LogCountAsync();
```

#### Method: Task\<long> LogCountAsync(DateTime date)

**��������:**

�������� ����������� ������� ���� ����� ��������� ����.

**���������:**
* date - ����, ����� ������� ����� ����������� �������.

**������������ ��������:**

����������� �������.

**������:**
```cs
SCLM sclm = new SCLM();
await sclm.AuthAsync(clientId, secret);
StoryTable<Profile> table = await sclm.GetTableAsync<Profile>(tableId);
long count = await table.LogCountAsync((DateTime.Now.AddDays(-25)));
```

#### Method: Task<IEnumerable\<ApiLog>> LogAsync(int skip, int take)

**��������:**

�������� ������ ����.

**���������:**
* skip - ������ � �������. ������� ������ ��������� ����� ����������. �� ��������� - 0.
* take - ������������ ���������� �������, ������� ����� ��������. �� ��������� - 100, ����������� 1000.

**������������ ��������:**

��������� ������� ����.

**������:**
```cs
SCLM sclm = new SCLM();
await sclm.AuthAsync(clientId, secret);
StoryTable<Profile> table = await sclm.GetTableAsync<Profile>(tableId);
IEnumerable<ApiLog> = await table.LogAsync(0, 1000);
```

#### Method: Task<IEnumerable\<ApiLog>> LogAsync(DateTime date, int skip, int take)

**��������:**

�������� ������ ����, ����� �������� ����.

**���������:**
* date - ����, ����� ������� ����� ����������� �������.
* skip - ������ � �������. ������� ������ ��������� ����� ����������. �� ��������� - 0.
* take - ������������ ���������� �������, ������� ����� ��������. �� ��������� - 100, ����������� 1000.

**������������ ��������:**

��������� ������� ����.

**������:**
```cs
SCLM sclm = new SCLM();
await sclm.AuthAsync(clientId, secret);
StoryTable<Profile> table = await sclm.GetTableAsync<Profile>(tableId);
IEnumerable<ApiLog> = await table.LogAsync(DateTime.Now.AddDays(-25), 0, 1000);
```

#### Method: Task\<T> FindAsync(string id)

**��������:**

�������� ������ ������� �� ��������������.

**���������:**
* id - ������������� ������.

**������������ ��������:**

������ � �������.

**������:**
```cs
SCLM sclm = new SCLM();
await sclm.AuthAsync(clientId, secret);
StoryTable<Profile> table = await sclm.GetTableAsync<Profile>(tableId);
Profile profile = await table.FindAsync(id);
```

#### Method: Task<IEnumerable\<T>> FindAsync(IEnumerable\<string> ids)

**��������:**

�������� ��������� ������� �� ������ ���������������.

**���������:**
* ids - ��������� ���������������.

**������������ ��������:**

��������� ��������.

**������:**
```cs
SCLM sclm = new SCLM();
await sclm.AuthAsync(clientId, secret);
StoryTable<Profile> table = await sclm.GetTableAsync<Profile>(tableId);
IEnumerable<Profile> profiles = await table.FindAsync(ids);
```

#### Method: Task<IEnumerable\<T>> FindAsync(int skip, int take)

**��������:**

�������� ����������� ��� ������ �������.

**���������:**
* skip - ������ � �������. ������� ������ ��������� ����� ����������. �� ��������� - 0.
* take - ������������ ���������� �������, ������� ����� ��������. �� ��������� - 100, ����������� 1000.

**������������ ��������:**

��������� ��������.

**������:**
```cs
SCLM sclm = new SCLM();
await sclm.AuthAsync(clientId, secret);
StoryTable<Profile> table = await sclm.GetTableAsync<Profile>(tableId);
IEnumerable<Profile> profiles = await table.FindAsync(0, 1000);
```

#### Method: Task<IEnumerable\<T>> FindAsync(string query, string sortfield, int sort, int skip, int take)

**��������:**

�������� ����������� ������ �� �������.
������ ������� - [TablesQuery](https://github.com/storyclm/documentation/blob/master/TABLES_QUERY.md).

[TablesQuery](https://github.com/storyclm/documentation/blob/master/TABLES_QUERY.md) - ��� ���� ��������, �������������� ���������� ��� StoryCLM. 
������ � ������ ������� ����� ������������� � ���� ������ ����� ��������.

��������� �� ������� ��������� ������ ����� ���� ���� �����: Comparison � Logical.

**���������:**
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
SCLM sclm = new SCLM();
await sclm.AuthAsync(clientId, secret);
StoryTable<Profile> table = await sclm.GetTableAsync();

//������� ������ ��� ����� 30
IEnumerable<Profile> profiles  = await table.FindAsync("[age][lte][30]", "age", 1, 0, 100);

//���� "name" ���������� � ������� "T"
profiles = await table.FindAsync("[name][sw][\"T\"]", "age", 1, 0, 100);

//���� "name" �������� ������ "ad"
profiles = await table.FindAsync("[Name][cn][\"ad\"]", "age", 1, 0, 100);

//����� ���� �� ������
profiles = await table.FindAsync("[Name][in][\"Stanislav\",\"Tamerlan\"]", "age", 1, 0, 100);

//������� ������, ��� ("name") ������� ���������� �� ������ "V" 
profiles = await table.FindAsync("[gender][eq][false][and][Name][sw][\"V\"]", "age", 1, 0, 100);

//������� ������ ������ 30 � ������ ������ 30
profiles = await table.FindAsync("[gender][eq][true][and][age][lt][30][or][gender][eq][false][and][age][gt][30]", "age", 1, 0, 100);

//���� "name" ���������� � �������� "T" ��� "S" ��� ���� ������� ������ ���� ����� 22
profiles = await table.FindAsync("([name][sw][\"S\"][or][name][sw][\"T\"])[and][age][eq][22]", "age", 1, 0, 100);

//������� ���� � ��������� �� � ��������� [25,30] � � ������� �� "S" � "�"
profiles = await table.FindAsync("([age][lt][22][or][age][gt][30])[and]([name][sw][\"S\"][or][name][sw][\"T\"])", "age", 1, 0, 100);
```

#### Method: Task\<T> MaxAsync\<T>(string field, string query = null)

**��������:**

������������ ������������ ��������.

**���������:**
* T - ��� ����.
* field - �������� ����.
* query - ������ � ������� [TablesQuery](https://github.com/storyclm/documentation/blob/master/TABLES_QUERY.md).

**������������ ��������:**

������������ ��������.

**������:**
```cs
SCLM sclm = new SCLM();
await sclm.AuthAsync(clientId, secret);
StoryTable<Profile> table = await sclm.GetTableAsync<Profile>(tableId);

bool boolResult = await storyTable.MaxAsync<bool>("Gender");
int intResult = await storyTable.MaxAsync<int>("Age");
string stringResult = await storyTable.MaxAsync<string>("Name");
DateTime datetimeResult = await storyTable.MaxAsync<DateTime>("Created");
double doubleResult = await storyTable.MaxAsync<double>("Rating");

bool boolQueryResult = await storyTable.MaxAsync<bool>("Gender", "[Gender][eq][false]");
int intQueryResult = await storyTable.MaxAsync<int>("Age", "[Gender][eq][false]");
string stringQueryResult = await storyTable.MaxAsync<string>("Name", "[Gender][eq][false]");
DateTime datetimeQueryResult = await storyTable.MaxAsync<DateTime>("Created", "[Gender][eq][false]");
double doubleQueryResult = await storyTable.MaxAsync<double>("Rating", "[Gender][eq][false]");
```

#### Method: Task\<T> MinAsync\<T>(string field, string query = null)

**��������:**

������������ ����������� ��������.

**���������:**
* T - ��� ����.
* field - �������� ����.
* query - ������ � ������� [TablesQuery](https://github.com/storyclm/documentation/blob/master/TABLES_QUERY.md).

**������������ ��������:**

������������ ��������.

**������:**
```cs
SCLM sclm = new SCLM();
await sclm.AuthAsync(clientId, secret);
StoryTable<Profile> table = await sclm.GetTableAsync<Profile>(tableId);

bool boolResult = await storyTable.MinAsync<bool>("Gender");
int intResult = await storyTable.MinAsync<int>("Age");
string stringResult = await storyTable.MinAsync<string>("Name");
DateTime datetimeResult = await storyTable.MinAsync<DateTime>("Created");
double doubleResult = await storyTable.MinAsync<double>("Rating");

bool boolQueryResult = await storyTable.MinAsync<bool>("Gender", "[Gender][eq][false]");
int intQueryResult = await storyTable.MinAsync<int>("Age", "[Gender][eq][false]");
string stringQueryResult = await storyTable.MinAsync<string>("Name", "[Gender][eq][false]");
DateTime datetimeQueryResult = await storyTable.MinAsync<DateTime>("Created", "[Gender][eq][false]");
double doubleQueryResult = await storyTable.MinAsync<double>("Rating", "[Gender][eq][false]");
```

#### Method: Task\<T> SumAsync\<T>(string field, string query = null)

**��������:**

���������� �����.

**���������:**
* T - ��� ����.
* field - �������� ����.
* query - ������ � ������� [TablesQuery](https://github.com/storyclm/documentation/blob/master/TABLES_QUERY.md).

**������������ ��������:**

�����.

**������:**
```cs
SCLM sclm = new SCLM();
await sclm.AuthAsync(clientId, secret);
StoryTable<Profile> table = await sclm.GetTableAsync<Profile>(tableId);

bool boolSumResult = await storyTable.SumAsync<bool>("Gender");
long intSumResult = await storyTable.SumAsync<long>("Age");
double doubleSumResult = await storyTable.SumAsync<double>("Rating");

bool boolQuerySumResult = await storyTable.SumAsync<bool>("Gender", "[Gender][eq][true]");
int intQuerySumResult = await storyTable.SumAsync<int>("Age", "[Gender][eq][true]");
double doubleQuerySumResult = await storyTable.SumAsync<double>("Rating", "[Gender][eq][false]");
```

#### Method: Task\<T> AvgAsync\<T>(string field, string query = null)

**��������:**

���������� ������� ��������.

**���������:**
* T - ��� ����.
* field - �������� ����.
* query - ������ � ������� [TablesQuery](https://github.com/storyclm/documentation/blob/master/TABLES_QUERY.md).

**������������ ��������:**

������� ��������.

**������:**
```cs
SCLM sclm = new SCLM();
await sclm.AuthAsync(clientId, secret);
StoryTable<Profile> table = await sclm.GetTableAsync<Profile>(tableId);

double intAvgResult = await storyTable.AvgAsync("Age");
double doubleAvgResult = await storyTable.AvgAsync("Rating");

double intQueryAvgResult = await storyTable.AvgAsync("Age", "[Gender][eq][true]");
double doubleQueryAvgResult = await storyTable.AvgAsync("Rating", "[Gender][eq][false]");
```

#### Method: Task\<T> FirstAsync(string query = null, string sortfield = null, int? sort = null)

**��������:**

���������� ������ ������� ��� null.

**���������:**
* query - ������ � ������� [TablesQuery](https://github.com/storyclm/documentation/blob/master/TABLES_QUERY.md).
* sortfield - ����, �� �������� ����� ���������� ����������.
* sort - ��� ����������.

**������������ ��������:**

������.

**������:**
```cs
SCLM sclm = new SCLM();
await sclm.AuthAsync(clientId, secret);
StoryTable<Profile> table = await sclm.GetTableAsync<Profile>(tableId);

Profile profile = await storyTable.FirstAsync("[Age][eq][33]", "age", 1);
Profile profile1 = await storyTable.FirstAsync(sortfield: "age", sort: 1);
Profile profile2 = await storyTable.FirstAsync(sortfield: "age");
Profile profile3 = await storyTable.FirstAsync();
```

#### Method: Task\<T> LastAsync(string query = null, string sortfield = null, int? sort = null)

**��������:**

���������� ��������� ������ ��� null.

**���������:**
* query - ������ � ������� [TablesQuery](https://github.com/storyclm/documentation/blob/master/TABLES_QUERY.md).
* sortfield - ����, �� �������� ����� ���������� ����������.
* sort - ��� ����������.

**������������ ��������:**

������.

**������:**
```cs
SCLM sclm = new SCLM();
await sclm.AuthAsync(clientId, secret);
StoryTable<Profile> table = await sclm.GetTableAsync<Profile>(tableId);

Profile profile4 = await storyTable.LastAsync("[Age][eq][22]", "age", 1);
Profile profile5 = await storyTable.LastAsync(sortfield: "age", sort: 1);
Profile profile6 = await storyTable.LastAsync(sortfield: "age");
Profile profile7 = await storyTable.LastAsync();
```

