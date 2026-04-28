# 06 Server Sent Events (SSE) - Real-time komunikace

**autor: Erik Král ekral@utb.cz**

S asistencí: GitHub Copilot (GPT-5.3-Codex)

## 🎯 Definice

- **Server Sent Events (SSE)** je technologie pro **jednosměrnou real-time komunikaci** ze serveru na klienty.
- Server otvírá dlouhodobé HTTP spojení a posílá **stream zpráv** v čase.
- Klient poslouchá na tomto streamu a přijímá aktualizace bez nutnosti polling-u.
- V .NET se pro SSE používá `System.Net.ServerSentEvents` a async iterace.

Použité technologie:

- `System.Net.ServerSentEvents` (.NET 10)
- `async IAsyncEnumerable<T>`
- `System.Threading.Channels`
- `ConcurrentDictionary`
- `Blazor Interactive Server`
- `HttpClient` s `ResponseHeadersRead`

---

## Co je cílem tohoto materiálu

V tomto materiálu navážeme na SSR klienta a Web API. Cíl je porozumět:

1. **Co je SSE a proč se používá** - jednosměrná real-time komunikace
2. **Alternativy k SSE** - polling, WebSockets, SignalR
3. **Serverová strana** - `ServerSentEventsService` se `ConcurrentDictionary` a broadcast logikou
4. **Klientská strana** - `SchoolService` konzumující SSE stream
5. **Komponenta** - `LiveUpdates.razor` zobrazující přijímané zprávy
6. **Koncepty** - `IAsyncEnumerable`, `async foreach`, `CancellationToken`, cleanup přes `ct.Register`

---

## 0. Úvod: `yield`, `await foreach` a `Channel`

Než půjdeme na SSE, je dobré si ujasnit základní stavební bloky streamování dat v C#.

### a) `yield` - generování posloupnosti postupně

Pomocí `yield return` ve metodě, která vrací  `IEnumerable<T>`, můžeme generovat posloupnost prvků postupně a používat při tom různé algoritmy. 

```csharp
static IEnumerable<int> Cisla()
{
    Thread.Sleep(1000);
    yield return 1;
    Thread.Sleep(1000);
    yield return 2;
    Thread.Sleep(1000);
    yield return 3;
}
```

> Poznámka: `yield return break` ukončí generování před ukončením metody.

Takovou posloupnost potom používáme třeba přes klasické `foreach`.

```csharp
foreach (var cislo in Cisla())
{
    Console.WriteLine(cislo);
}
```

---

### b) `yield` v asynchronní metodě

Když mezi položkami potřebujeme asynchronní operace (např. I/O, síť, čekání na data), použijeme `async IAsyncEnumerable<T>`:

```csharp
static async IAsyncEnumerable<int> CislaAsync()
{
    await Task.Delay(1000);
    yield return 1;
    await Task.Delay(1000);
    yield return 2;
    await Task.Delay(1000);
    yield return 3;
}
```

Konzumace probíhá přes `await foreach`:

```csharp
await foreach (var cislo in CislaAsync())
{
    Console.WriteLine(cislo);
}
```

Výhoda: vlákno se mezi prvky neblokuje, protože čekání probíhá asynchronně (`await`). Díky tomu aplikace mezitím zvládne dělat další práci.

---

### c) `IEnumerable<T>` vs `IAsyncEnumerable<T>`

- `IEnumerable<T>` je synchronní: `foreach` interně zavolá `GetEnumerator()` a pak opakovaně `MoveNext()` pro načtení dalšího prvku (hodnota je v `Current`).
- `IAsyncEnumerable<T>` je asynchronní: `await foreach` interně zavolá `GetAsyncEnumerator()` a pak opakovaně `await MoveNextAsync()` pro načtení dalšího prvku (hodnota je v `Current`).
- `IEnumerable<T>` je vhodné pro data, která máme rychle v paměti.
- `IAsyncEnumerable<T>` je vhodné pro streamy a data přicházející v čase (síť, databáze, SSE).

---

### d) `Channel<T>` pro předávání dat mezi vlákny

`Channel<T>` umožňuje bezpečně přidávat (`Writer`) a odebírat (`Reader`) prvky i z různých vláken.

Klíčové je, že `Reader` umí vrátit právě `IAsyncEnumerable<T>`:

```csharp
await foreach (var item in channel.Reader.ReadAllAsync(ct))
{
    yield return item;
}
```

Krátká ukázka z `Program.cs`: zápis běží v jiném vlákně (`Task.Run`), čtení běží v `Main`:

```csharp
Channel<int> channel = Channel.CreateBounded<int>(
    new BoundedChannelOptions(10) { FullMode = BoundedChannelFullMode.DropOldest });

_ = Task.Run(() => RunWriter(channel.Writer)); // producer v jiném vlákně

await foreach (int znak in channel.Reader.ReadAllAsync()) // consumer v Main
{
    Console.WriteLine($"Zadal jsi znak: {znak}");
}

static void RunWriter(ChannelWriter<int> chanelWriter)
{
    ConsoleKeyInfo keyInfo;

    do
    {
        keyInfo = Console.ReadKey(true);
        chanelWriter.TryWrite(keyInfo.KeyChar);
    } while (keyInfo.Key != ConsoleKey.Escape);

    chanelWriter.Complete();
}
```

To je přesně pattern, který používáme i v SSE: producer zapisuje do kanálu, consumer čte přes `await foreach` a posílá data dál klientovi.

---

## 1. Co je SSE a kdy ji používat

### a) Server Sent Events - princip

```
Klient                     Server
  |                          |
  |------- GET /stream ----->|
  |                          |
  |<----- Zpráva 1 ----------|
  |<----- Zpráva 2 ----------|
  |<----- Zpráva 3 ----------|
  |                          |
  (Spojení zůstává otevřené, klient čeká na nové zprávy)
```

SSE je **jednosměrná**: jen server může iniciovat data. Pokud klient potřebuje poslat data, musí udělat samostatný HTTP request (nejčastěji `POST` nebo `PUT`).

### b) Výhody SSE

- ✅ **Jednoduchá** - počítá se s ní v HTTP standardu
- ✅ **Automatické reconnection** - browser sám pokusí znovupřipojit
- ✅ **Nativní** - bez třetích stran (oproti WebSocketům)
- ✅ **Efektivní** - nižší overhead než polling

### c) Nevýhody SSE

- ❌ **Jednosměrná** - nelze poslat data serveru skrz SSE
- ❌ **Limity** - max 6 paralelních spojení per domain (HTTP/1.1), lépe v HTTP/2
- ❌ **Proxy problémy** - některé proxy SSE nepodporují

---

## 2. Alternativy k SSE

| Technologie | Jednosměrná | Obousměrná | Latence | Nastavení |
|---|---|---|---|---|
| **Polling** | - | - | Vysoká (N sekund) | Nejjednodušší |
| **Long Polling** | Pseudo | Pseudo | Nižší | Středně těžké |
| **SSE** | ✅ Server→Klient | ❌ | Nízká | Střední |
| **WebSockets** | ❌ | ✅ Obousměrné | Velmi nízká | Těžké |
| **SignalR** | ❌ | ✅ Obousměrné | Velmi nízká | Nejjednoduší (abstrakce) |

### Kdy co použít:

- **Polling** - jednoduché aktualizace, nepotřebuješ real-time (refresh každých 30 sekund)
- **SSE** - jednosměrné notifikace ze serveru (nové objednávky, chat zprávy od ostatních)
- **WebSockets** - chat, gaming, obousměrná komunikace v reálném čase
- **SignalR** - pokud chceš abstrakci (automaticky vybírá SSE/WebSockets/LongPolling)

V našem příkladu používáme **SSE**, protože chceme ukázat, jak server notifikuje více klientů o nových událostech (vytvoření nového studenta).

---

## 3. Serverová strana: ServerSentEventsService

### a) Princip broadcast patternů

Máme jeden server a více klientů. Když server vygeneruje zprávu, měl by ji poslat **všem připojeným klientům**. To se jmenuje **broadcast**.

```
Server
  ├─ Klient A (kanálA)
  ├─ Klient B (kanálB)
  └─ Klient C (kanálC)

WriteAsync(student) → pošli všem kanálům
```

### b) Implementace s ConcurrentDictionary

```csharp
using System.Collections.Concurrent;
using System.Net.ServerSentEvents;
using System.Runtime.CompilerServices;
using System.Threading.Channels;
using UTB.School.Contracts;
using UTB.School.Db;

namespace UTB.School.WebApi
{
    public class ServerSentEventsService
    {
        // Dictionary: Guid (klient) → Channel (jeho zprávy)
        private readonly ConcurrentDictionary<Guid, Channel<SseItem<StudentDto>>> subscribers = [];

        // Zavolá se, když se vytvoří nový student (například z CreateStudent endpointu)
        public async Task WriteAsync(StudentDto student)
        {
            // Pošli zprávu všem připojeným klientům
            foreach (Channel<SseItem<StudentDto>> channel in subscribers.Values)
            {
                SseItem<StudentDto> sseItem = new(student, "student");

                await channel.Writer.WriteAsync(sseItem);
            }
        }

        // Zavolá se, když se klient připojí na GET /stream
        public IAsyncEnumerable<SseItem<StudentDto>> InitAndGetStream(CancellationToken ct)
        {
            // Vytvoř kanál pro tohoto konkrétního klienta
            var clientId = Guid.NewGuid();

            var clientChannel = Channel.CreateBounded<SseItem<StudentDto>>(new BoundedChannelOptions(20) { FullMode = BoundedChannelFullMode.DropOldest});

            SseItem<StudentDto> sseItem = new(new StudentDto(-1, "", false), "init");

            clientChannel.Writer.TryWrite(sseItem);

            ct.Register(() => subscribers.TryRemove(clientId, out _));

            // Zaregistruj klienta do slovníku
            subscribers.TryAdd(clientId, clientChannel);

            return clientChannel.Reader.ReadAllAsync(ct);
        }
    }
}
```

### c) Vysvětlení klíčových konceptů

**1. Guid per client (identifikátor)**

```csharp
var clientId = Guid.NewGuid(); // Každý klient dostane unikátní ID
subscribers.TryAdd(clientId, clientChannel); // Přidej do slovníku
```

Proč Guid a ne index? Protože `ConcurrentDictionary` potřebuje klíč pro jednoduché a bezpečné odebírání:

```csharp
subscribers.TryRemove(clientId, out _); // Smaž přesně TOHOTO klienta
```

**2. Channel<T> - bezpečný kanál pro async**

```csharp
var clientChannel = Channel.CreateBounded<SseItem<StudentDto>>(
    new BoundedChannelOptions(20) { FullMode = BoundedChannelFullMode.DropOldest });
```

`Channel` je thread-safe fronta zpráv:
- `Writer` píše do kanálu
- `Reader` čte z kanálu
- Není potřeba `lock()`, Channel to má zabudované
- Bounded varianta chrání před nekonečným růstem paměti

**3. ConcurrentDictionary - thread-safe slovník**

```csharp
private readonly ConcurrentDictionary<Guid, Channel<SseItem<StudentDto>>> subscribers = [];
```

Bez `ConcurrentDictionary` bychom potřebovali `lock()`. S ním je práce bezpečná:
- více threadů může pracovat s slovníkem zároveň
- `subscribers.Values` vrátí snapshot kanálů

**4. Init event + filtrování na klientovi**

```csharp
SseItem<StudentDto> sseItem = new(new StudentDto(-1, "", false), "init");
clientChannel.Writer.TryWrite(sseItem);
```

Po připojení klient dostane inicializační SSE event typu `init`. Klientská strana ho ignoruje a zpracovává jen skutečné události `student`.

**5. Cleanup přes CancellationToken**

```csharp
ct.Register(() => subscribers.TryRemove(clientId, out _));
```

Když se klient odpojí, token se zruší a callback odebere klienta ze slovníku. Následné čtení probíhá jednoduše přes `return clientChannel.Reader.ReadAllAsync(ct);`.

---

## 4. Serverová strana: Endpoint a registrace

V `Program.cs` máme:

```csharp
// Registruj service
builder.Services.AddSingleton<ServerSentEventsService>();

// Endpoint pro SSE stream
app.MapGet("/stream", GetUpdates);

// Když se vytvoří student, poslej SSE zprávu všem
static async Task<Created<StudentDto>> CreateStudent(StudentRequestDto request, SchoolContext context, ServerSentEventsService eventService)
{
    var student = new Student { Name = request.Name, IsActive = request.IsActive };
    context.Add(student);
    await context.SaveChangesAsync();

    // Pošli SSE zprávu s novým studentem všem klientům
    var studentDto = new StudentDto(student.Id, student.Name, student.IsActive);
    await eventService.WriteAsync(studentDto);

    return TypedResults.Created($"/students/{student.Id}", studentDto);
}

// Endpoint pro SSE stream
static async Task<ServerSentEventsResult<StudentDto>> GetUpdates(ServerSentEventsService updates, CancellationToken cancellationToken)
{
    return TypedResults.ServerSentEvents(updates.InitAndGetStream(cancellationToken));
}
```

Klíčové body:

- `AddSingleton<ServerSentEventsService>()` - **jeden instanci na dobu běhu** aplikace
- Všichni klienti pracují se **stejnou instancí**, viz `subscribers` slovník
- Při `CreateStudent` voláme `eventService.WriteAsync()` - posíláme zprávu všem

---

## 5. Klientská strana: SchoolService

Klient se připojuje na `/stream` a čte SSE zprávy. Přidali jsme metodu do `SchoolService`:

```csharp
using System.Net;
using System.Net.ServerSentEvents;
using System.Runtime.CompilerServices;
using System.Text.Json;
using UTB.School.Contracts;

namespace UTB.School.Web
{
    public class SchoolService(HttpClient httpClient)
    {
        // ...existující CRUD metody GetStudentsAsync, CreateStudentAsync, atd...

        // Nová metoda pro SSE stream
        public async IAsyncEnumerable<StudentDto> StreamSseMessagesAsync([EnumeratorCancellation] CancellationToken ct = default)
        {
            // Otevři HTTP stream s volbou ResponseHeadersRead
            // (klíčové pro streaming - nebudeme čekat na celé response body)
            using var response = await httpClient.GetAsync("/stream", HttpCompletionOption.ResponseHeadersRead, ct);
            response.EnsureSuccessStatusCode();

            // Získej stream obsahu
            using var stream = await response.Content.ReadAsStreamAsync(ct);
            
            // Parsuj SSE zprávy (vrací string)
            SseParser<string> parser = SseParser.Create(stream);

            // Iteruj přes SSE items
            await foreach (SseItem<string> sseEvent in parser.EnumerateAsync(ct))
            {
                if(sseEvent.EventType == "init")
                {
                    continue;
                }

                StudentDto? student = JsonSerializer.Deserialize<StudentDto>(sseEvent.Data, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

                if (student is null)
                {
                    continue;
                }

                yield return student;
            }
        }
    }
}
```

### Vysvětlení klíčových konceptů

**1. HttpCompletionOption.ResponseHeadersRead**

```csharp
using var response = await httpClient.GetAsync("/stream", HttpCompletionOption.ResponseHeadersRead, ct);
```

Normálně `GetAsync` čeká až se **vše** stáhne. S `ResponseHeadersRead` vrátí hned, jakmile dostane hlavičky. Pak můžeme číst z `stream` postupně.

**2. SseParser<T> - parsování SSE formátu**

```csharp
SseParser<string> parser = SseParser.Create(stream);
await foreach (SseItem<string> sseEvent in parser.EnumerateAsync(ct))
{
    if (sseEvent.EventType == "init")
    {
        continue;
    }

    StudentDto? student = JsonSerializer.Deserialize<StudentDto>(
        sseEvent.Data,
        new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

    if (student is null)
    {
        continue;
    }

    yield return student;
}
```

SSE formát vypadá takto:

```
event: init
data: {"id":-1,"name":"","isActive":false}

event: student
data: {"id":1,"name":"Jan","isActive":true}
```

`SseParser` to parsuje na `SseItem<string>`. Klient ignoruje `init` event a z `student` eventu deserializuje `.Data` na `StudentDto`.

---

## 6. Klientská strana: LiveUpdates.razor komponenta

```razor
@page "/updates"
@rendermode @(new InteractiveServerRenderMode(prerender: false))
@inject SchoolService SchoolService
@implements IAsyncDisposable
@using UTB.School.Contracts

<h3>Nově přidaní studenti</h3>

@if (students.Count == 0)
{
    <p><em>Čekám na nové studenty...</em></p>
}
else
{
    <table class="table">
        <thead>
            <tr>
                <th>Id</th>
                <th>Jméno</th>
                <th>Aktivní</th>
            </tr>
        </thead>
        <tbody>
            @foreach(var student in students)
            {
                <tr>
                    <td>@student.Id</td>
                    <td>@student.Name</td>
                    <td>@(student.IsActive ? "Ano" : "Ne")</td>
                </tr>
            }
        </tbody>
    </table>
}

@code {
    private List<StudentDto> students = [];
    private CancellationTokenSource? cts;

    protected override void OnInitialized()
    {
        cts = new CancellationTokenSource();
        // Spusť stream bez await - běží na pozadí
        _ = ReadMessagesAsync(cts.Token);
    }

    private async Task ReadMessagesAsync(CancellationToken ct)
    {
        try
        {
            // Voláme SSE stream ze service
            await foreach (var student in SchoolService.StreamSseMessagesAsync(ct))
            {
                students.Add(student);
                // Říkáme Blazoru, aby znovu vykreslil stránku
                await InvokeAsync(StateHasChanged);
            }
        }
        catch (OperationCanceledException)
        {
            // Normální - klient se odpojil
        }
        catch (Exception ex)
        {
            students.Add(new StudentDto(0, $"Chyba: {ex.Message}", false));
            await InvokeAsync(StateHasChanged);
        }
    }

    // Cleanup - zrušíme stream když se komponenta zavře
    async ValueTask IAsyncDisposable.DisposeAsync()
    {
        cts?.Cancel();
        cts?.Dispose();
    }
}
```

### Vysvětlení

**1. OnInitialized() a spuštění na pozadí**

```csharp
protected override void OnInitialized()
{
    cts = new CancellationTokenSource();
    _ = ReadMessagesAsync(cts.Token); // _ = znamená "ignoruj výsledek"
}
```

Nemůžeme volat `await` v `OnInitialized()` (není `async`). Místo toho "zapálíme" coroutine na pozadí a necháme ji běžet.

**2. async foreach - přijetí zpráv**

```csharp
await foreach (var student in SchoolService.StreamSseMessagesAsync(ct))
{
    students.Add(student);
    await InvokeAsync(StateHasChanged);
}
```

`async foreach` čeká na zprávy ze serveru. Jakmile se přijme StudentDto, přidáme ho do seznamu a řekneme Blazoru, aby znovu vykreslil UI.

**3. StateHasChanged() - refresh UI**

```csharp
await InvokeAsync(StateHasChanged);
```

V Blazoru Interactive Server se změny propagují přes SignalR. Bez `InvokeAsync` by se změna neprojevila v prohlížeči. `InvokeAsync` zajistí, že se render spustí v správném kontextu.

**4. IAsyncDisposable - cleanup**

```csharp
@implements IAsyncDisposable

async ValueTask IAsyncDisposable.DisposeAsync()
{
    cts?.Cancel();
    cts?.Dispose();
}
```

Když se komponenta zavře (uživatel přejde na jinou stránku), Blazor automaticky volá `DisposeAsync()`. My tam zrušíme token, což způsobí, že `async foreach` skončí.

Bez tohoto by se SSE stream nechal běžet na pozadí a byla by **memory leak**.

---

## 7. Klientská strana: Index.html a EventSource API

Pro připojení na SSE stream bez framework jsme vytvořili jednoduchou HTML stránku s **nativním EventSource API**.

### a) Vytvoření SSE spojení

```javascript
const evtSource = new EventSource("https://localhost:7195/sse");
```

`EventSource` je nativní WebAPI prohlížeče, která se automaticky stará o:
- Připojení na daný endpoint
- Čtení zpráv ze serveru
- Automatické reconnection při ztrátě spojení

### b) Event handlery

**1. onopen - Úspěšné připojení**

```javascript
evtSource.onopen = () => {
    statusElement.textContent = "SSE connected";
    booksElement.innerHTML = "";
};
```

Vyvolá se, jakmile je spojení navázáno. Ideální místo pro inicializaci UI.

**2. onerror - Chyba nebo odpojení**

```javascript
evtSource.onerror = () => {
    statusElement.textContent = "SSE error";
    booksElement.innerHTML = "";
};
```

Vyvolá se při chybě komunikace nebo když se klient odpojí. Browser se pak snaží automaticky znovupřipojit.

**3. onmessage - Přijetí zprávy**

```javascript
evtSource.onmessage = (event) => {
    statusElement.textContent = "SSE message";

    // Parsuj JSON z event.data
    const books = JSON.parse(event.data);

    // Vykresluj tabulku
    let html = "<table><tr><th>Id</th><th>Title</th><th>IsArchived</th></tr>";
    for (const book of books) {
        html += `<tr><td>${book.id}</td><td>${book.title}</td><td>${book.isArchived}</td></tr>`;
    }
    html += "</table>";

    booksElement.innerHTML = html;
};
```

Každá zpráva ze serveru má vlastnost `event.data`, která obsahuje JSON řetězec. Parsujeme ho a vykreslujeme tabulku.

### c) SSE stream a CORS

Server vrací `ServerSentEventsResult<BookDto[]>` s iniciálními daty a poté kontinuálně posílá aktualizace:

```javascript
// Zpráva 1 - iniciální data
{"id":1,"title":"C# Guide","isArchived":false}, {...}

// Zpráva 2 - po vytvoření nové knihy
{"id":2,"title":"ASP.NET Core","isArchived":false}, {...}, ...
```

**CORS konfiguraci serveru** jsme nastavili v `Program.cs`:

```csharp
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("https://localhost:7263")  // Klient
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

app.UseCors();
```

**Proč je CORS důležitý:**
- EventSource API normálně podléhá **same-origin policy** (prohlížeč blokuje cross-domain requesty)
- S CORS říkáme serveru: "Povolte requests z `https://localhost:7263`"
- `AllowCredentials()` povoluje cookies a autentifikaci přes SSE
- `AllowAnyHeader()` a `AllowAnyMethod()` dávají maximální flexibilitu

### d) Úplný HTML kód

```html
<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <title>SseDemo Javascript</title>
</head>
<body>
    <p id="status"></p>
    <div id="books"></div>

    <script>
        const evtSource = new EventSource("https://localhost:7195/sse");
        const statusElement = document.getElementById("status");
        const booksElement = document.getElementById("books");

        evtSource.onopen = () => {
            statusElement.textContent = "SSE connected";
            booksElement.innerHTML = "";
        };

        evtSource.onerror = () => {
            statusElement.textContent = "SSE error";
            booksElement.innerHTML = "";
        };

        evtSource.onmessage = (event) => {
            statusElement.textContent = "SSE message";
            const books = JSON.parse(event.data);

            let html = "<table><tr><th>Id</th><th>Title</th><th>IsArchived</th></tr>";
            for (const book of books) {
                html += `<tr><td>${book.id}</td><td>${book.title}</td><td>${book.isArchived}</td></tr>`;
            }
            html += "</table>";

            booksElement.innerHTML = html;
        };
    </script>
</body>
</html>
```

---

## 8. Rekapitulace: Jak to celé funguje

```
1. Uživatel otevře /updates
   ↓
2. LiveUpdates.razor se inicializuje
   ↓
3. OnInitialized() spustí ReadMessagesAsync() na pozadí
   ↓
4. ReadMessagesAsync() zavolá SchoolService.StreamSseMessagesAsync()
   ↓
5. SchoolService se připojí na GET /stream
   ↓
6. Server vytvoří ServerSentEventsService.InitAndGetStream()
   ↓
7. Server čeká na nové zprávy
   ↓
8. Klient čeká v async foreach
   ↓
9. Na Formě CreateStudent.razor se vytvoří nový student
   ↓
10. CreateStudent endpoint zavolá eventService.WriteAsync(studentDto)
    ↓
11. WriteAsync() pošle SSE event typu "student" všem připojeným klientům
    ↓
12. Klient ignoruje "init" event, zpracuje jen "student" event
    ↓
13. SchoolService parsuje JSON string na StudentDto
    ↓
14. StudentDto se přidá do students[] seznamu
    ↓
15. StateHasChanged() refreshne UI
    ↓
16. Uživatel vidí nového studenta v tabulce
```

---

## 8. Koncepty k zapamatování

### IAsyncEnumerable<T>

```csharp
public async IAsyncEnumerable<string> StreamSseMessagesAsync(...)
{
    await foreach (var item in source)
    {
        yield return item;
    }
}
```

Je to jako `IEnumerable<T>`, ale asynchronní. Umožňuje vrátit prvky jeden za druhým, a mezi nimi můžeme `await`.

### async foreach

```csharp
await foreach (var message in stream)
{
    // Čekáme na další zprávu
}
```

Normální `foreach` je synchronní. `async foreach` se používá s `IAsyncEnumerable` a dovoluje `await` uvnitř.

### CancellationToken

```csharp
ct.Register(() => subscribers.TryRemove(clientId, out _));
return clientChannel.Reader.ReadAllAsync(ct);
```

Token slouží pro zrušení operace i cleanup. Když zavoláme `cts.Cancel()`, `ReadAllAsync(ct)` skončí a callback z `ct.Register(...)` odhlásí klienta.

### Thread-safety bez lock

```csharp
private readonly ConcurrentDictionary<Guid, Channel<...>> subscribers = [];

// Bezpečné zároveň:
subscribers.TryAdd(id, channel);
subscribers.TryRemove(id, out _);
foreach (var ch in subscribers.Values) { }
```

`ConcurrentDictionary` a `Channel` jsou thread-safe, takže nepotřebujeme manuální `lock()`.

---

### Testování Blazor verze:

1. Otevřete `https://localhost:5173/updates` (nebo správný port)
2. V jiné záložce vytvořte studenta přes formulář
3. V `/updates` se student automaticky objeví
---

## 9. Co jsme si procvičili

- Princip SSE (Server Sent Events) pro jednosměrnou real-time komunikaci
- Broadcast pattern s `ConcurrentDictionary` a `Channel` (.NET)
- `IAsyncEnumerable<T>` a `async foreach` (.NET)
- `init`/`student` SSE event typy a jejich zpracování na klientovi
- `IAsyncDisposable` pro cleanup v Blazoru
- `StateHasChanged()` pro refresh UI (Blazor)
- `HttpCompletionOption.ResponseHeadersRead` pro streaming

---

## ❓ Kontrolní otázky

1. Jaký je rozdíl mezi SSE a WebSockets?
2. Proč používáme `ConcurrentDictionary` místo normálního `Dictionary` s `lock()`?
3. Jaký je účel `init` eventu a proč ho klient ignoruje?
4. Proč je v `InitAndGetStream()` použito `ct.Register(() => subscribers.TryRemove(...))`?
5. Jak se liší `StateHasChanged()` v Interactive Server renderu od Static SSR?
6. Proč musíme volat `HttpCompletionOption.ResponseHeadersRead` místo výchozího?
7. Co by se stalo, pokud bychom zapoměli implementovat `IAsyncDisposable`?
8. Jak by se změnila architektura, pokud bychom chtěli, aby zprávy viděli jen určité klienti?
9. Jaký je rozdíl mezi EventSource a WebSocket API v JavaScriptu?
10. Proč je important escapování HTML v vanilla JS kódu? Jak by mohla nastat XSS chyba?

---

## 🔗 Dodatečné zdroje

- [System.Net.ServerSentEvents - Microsoft Docs](https://learn.microsoft.com/en-us/dotnet/api/system.net.serversent events)
- [IAsyncEnumerable - Microsoft Docs](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.iasyncenumerable-1)
- [Server-sent events (SSE) - MDN Web Docs](https://developer.mozilla.org/en-US/docs/Web/API/Server-sent_events)
- [EventSource API - MDN Web Docs](https://developer.mozilla.org/en-US/docs/Web/API/EventSource)
- [Channels in .NET - Microsoft Docs](https://learn.microsoft.com/en-us/dotnet/api/system.threading.channels)


