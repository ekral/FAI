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
6. **Koncepty** - `IAsyncEnumerable`, `async foreach`, `CancellationToken`, cancellation patterns

---

## 1. Co je SSE a kdy ji používat

### a) Server Sent Events - princip

```
Klient                     Server
  |                          |
  |------- GET /stream ------>|
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

namespace UTB.School.WebApi
{
    public class ServerSentEventsService
    {
        // Dictionary: Guid (klient) → Channel (jeho zprávy)
        private readonly ConcurrentDictionary<Guid, Channel<StudentDto>> subscribers = [];

        // Zavolá se, když se vytvoří nový student (například z CreateStudent endpointu)
        public async Task WriteAsync(StudentDto student)
        {
            // Pošli zprávu všem připojeným klientům
            foreach (var channel in subscribers.Values)
            {
                await channel.Writer.WriteAsync(student);
            }
        }

        // Zavolá se, když se klient připojí na GET /stream
        public async IAsyncEnumerable<StudentDto> InitAndGetStream([EnumeratorCancellation] CancellationToken ct)
        {
            // Vytvoř kanál pro TOhoto konkrétního klienta
            var clientId = Guid.NewGuid();
            var clientChannel = Channel.CreateUnbounded<StudentDto>();

            // Zaregistruj klienta do slovníku
            subscribers.TryAdd(clientId, clientChannel);

            try
            {
                // Pak poslouchat zprávy pro TOHOTO klienta
                await foreach (var item in clientChannel.Reader.ReadAllAsync(ct))
                {
                    yield return item;
                }
            }
            finally
            {
                // Odstraň klienta když se odpojí
                subscribers.TryRemove(clientId, out _);
            }
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
var clientChannel = Channel.CreateUnbounded<StudentDto>();
```

`Channel` je thread-safe fronta zpráv:
- `Writer` píše do kanálu
- `Reader` čte z kanálu
- Není potřeba `lock()`, Channel to má zabudované

**3. ConcurrentDictionary - thread-safe slovník**

```csharp
private readonly ConcurrentDictionary<Guid, Channel<SseItem<int>>> subscribers = [];
```

Bez `ConcurrentDictionary` bychom potřebovali `lock()`. S ním je práce bezpečná:
- více threadů může pracovat s slovníkem zároveň
- `subscribers.Values` vrátí snapshot kanálů

**4. [EnumeratorCancellation] - zrušení iterace**

```csharp
public async IAsyncEnumerable<SseItem<int>> InitAndGetStream([EnumeratorCancellation] CancellationToken ct)
```

Atribut `[EnumeratorCancellation]` zajistí, že `CancellationToken` se automaticky předá do `ReadAllAsync(ct)`. Když klient zavře prohlížeč, token se zruší a cyklus skončí.

**5. finally blok - cleanup**

```csharp
finally
{
    subscribers.TryRemove(clientId, out _);
}
```

Zaručuje, že když se klient odpojí (nebo dojde k výjimce), je jeho kanál ze slovníku odstraněn. Bez toho by **memory leak** - kanály by zůstaly v paměti.

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
            await foreach (var sseEvent in parser.EnumerateAsync(ct))
            {
                if (!string.IsNullOrEmpty(sseEvent.Data))
                {
                    // sseEvent.Data je JSON string: {"id":1,"name":"Jan","isActive":true}
                    var student = System.Text.Json.JsonSerializer.Deserialize<StudentDto>(sseEvent.Data);
                    if (student is not null)
                    {
                        yield return student;
                    }
                }
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
await foreach (var sseEvent in parser.EnumerateAsync(ct))
{
    if (!string.IsNullOrEmpty(sseEvent.Data))
    {
        // sseEvent.Data je JSON string: {"id":1,"name":"Jan","isActive":true}
        var student = System.Text.Json.JsonSerializer.Deserialize<StudentDto>(sseEvent.Data);
        if (student is not null)
        {
            yield return student;
        }
    }
}
```

SSE formát vypadá takto:

```
data: {"id":1,"name":"Jan","isActive":true}
data: {"id":2,"name":"Eva","isActive":true}
```

`SseParser` to parsuje a vrací `SseItem<string>` objekty. Obsah v `.Data` je JSON string, kterou deserializujeme na `StudentDto`.

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

## 7. Rekapitulace: Jak to celé funguje

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
11. WriteAsync() poslal StudentDto VŠEM připojeným klientům
    ↓
12. Klient si StudentDto vezme z kanálu
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
public async IAsyncEnumerable<T> MyStream([EnumeratorCancellation] CancellationToken ct)
{
    await foreach (var item in source.ReadAllAsync(ct))
    {
        yield return item;
    }
}
```

Token slouží pro zrušení operace. Když zavoláme `cts.Cancel()`, `ReadAllAsync(ct)` skončí s `OperationCanceledException`.

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

## 9. HTML a JavaScript - vanilla SSE konzumace (bez frameworků)

SSE je univerzální technologie, která funguje **bez jakýchkoliv frameworků**. Zde je ukázka, jak konzumovat SSE stream v čistém HTML a JavaScriptu.

### a) HTML stránka

```html
<!DOCTYPE html>
<html lang="cs">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Nově přidaní studenti - SSE</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            max-width: 800px;
            margin: 50px auto;
            padding: 20px;
        }
        
        table {
            width: 100%;
            border-collapse: collapse;
            margin-top: 20px;
        }
        
        th, td {
            border: 1px solid #ddd;
            padding: 12px;
            text-align: left;
        }
        
        th {
            background-color: #4CAF50;
            color: white;
        }
        
        tr:hover {
            background-color: #f5f5f5;
        }
        
        .status {
            padding: 10px;
            margin: 10px 0;
            border-radius: 4px;
        }
        
        .status.waiting {
            background-color: #e3f2fd;
            color: #1976d2;
        }
        
        .status.connected {
            background-color: #e8f5e9;
            color: #388e3c;
        }
        
        .status.error {
            background-color: #ffebee;
            color: #c62828;
        }
    </style>
</head>
<body>
    <h1>Nově přidaní studenti - Real-time SSE</h1>
    
    <div id="status" class="status waiting">
        ⏳ Čekám na připojení...
    </div>
    
    <button onclick="startStream()">Spustit stream</button>
    <button onclick="stopStream()" style="margin-left: 10px;">Zastavit stream</button>
    
    <table id="studentTable">
        <thead>
            <tr>
                <th>Id</th>
                <th>Jméno</th>
                <th>Aktivní</th>
                <th>Čas přijetí</th>
            </tr>
        </thead>
        <tbody id="studentBody">
            <tr>
                <td colspan="4" style="text-align: center;">Žádní studenti zatím...</td>
            </tr>
        </tbody>
    </table>
    
    <script src="sse-client.js"></script>
</body>
</html>
```

### b) JavaScript - SSE čtečka (sse-client.js)

```javascript
// Globální proměnné
let eventSource = null;
const API_URL = 'https://localhost:5001/stream'; // Změň port podle tvé konfigurace

// Spustit stream
function startStream() {
    if (eventSource !== null) {
        console.log('Stream již běží');
        return;
    }
    
    // Vyčisti tabulku
    clearTable();
    updateStatus('waiting', '⏳ Připojuji se...');
    
    try {
        // Vytvoř EventSource pro SSE
        eventSource = new EventSource(API_URL);
        
        // Naslouchej na 'message' event (výchozí)
        eventSource.addEventListener('message', (event) => {
            try {
                // event.data je JSON string
                const student = JSON.parse(event.data);
                console.log('Nový student přijat:', student);
                
                addStudentToTable(student);
                updateStatus('connected', '✅ Připojeno - čekám na zprávy...');
            } catch (error) {
                console.error('Chyba při parsování JSON:', error);
                updateStatus('error', '❌ Chyba: ' + error.message);
            }
        });
        
        // Když se stream otevře
        eventSource.addEventListener('open', () => {
            console.log('EventSource otevřen');
            updateStatus('connected', '✅ Připojeno - čekám na zprávy...');
        });
        
        // Když dojde k chybě
        eventSource.addEventListener('error', (event) => {
            console.error('SSE error:', event);
            
            if (eventSource.readyState === EventSource.CLOSED) {
                updateStatus('error', '❌ Spojení zavřeno');
                eventSource = null;
            } else if (eventSource.readyState === EventSource.CONNECTING) {
                updateStatus('waiting', '⏳ Pokus o znovupřipojení...');
            }
        });
        
    } catch (error) {
        console.error('Chyba při vytváření EventSource:', error);
        updateStatus('error', '❌ Chyba: ' + error.message);
        eventSource = null;
    }
}

// Zastavit stream
function stopStream() {
    if (eventSource !== null) {
        eventSource.close();
        eventSource = null;
        updateStatus('waiting', '⏹️ Stream zastaven');
        console.log('Stream zastaven');
    }
}

// Přidej studenta do tabulky
function addStudentToTable(student) {
    const tbody = document.getElementById('studentBody');
    
    // Odstraň "Žádní studenti" řádek, když přijde první student
    if (tbody.querySelector('tr td[colspan="4"]')) {
        tbody.innerHTML = '';
    }
    
    // Vytvoř nový řádek
    const row = document.createElement('tr');
    const now = new Date().toLocaleTimeString('cs-CZ');
    
    row.innerHTML = `
        <td>${student.id}</td>
        <td>${escapeHtml(student.name)}</td>
        <td>${student.isActive ? '✅ Ano' : '❌ Ne'}</td>
        <td>${now}</td>
    `;
    
    // Přidej na začátek tabulky (nejnovější nahoře)
    tbody.insertBefore(row, tbody.firstChild);
}

// Vyčisti tabulku
function clearTable() {
    const tbody = document.getElementById('studentBody');
    tbody.innerHTML = '<tr><td colspan="4" style="text-align: center;">Žádní studenti zatím...</td></tr>';
}

// Aktualizuj status zprávu
function updateStatus(className, message) {
    const statusDiv = document.getElementById('status');
    statusDiv.className = `status ${className}`;
    statusDiv.textContent = message;
}

// Bezpečné zobrazení HTML (prevence XSS)
function escapeHtml(text) {
    const div = document.createElement('div');
    div.textContent = text;
    return div.innerHTML;
}

// Spusť stream automaticky při načtení stránky
window.addEventListener('load', () => {
    console.log('Stránka načtena, spouštím stream...');
    startStream();
});

// Zastavit stream když se stránka zavře
window.addEventListener('beforeunload', () => {
    if (eventSource !== null) {
        stopStream();
    }
});
```

### c) Vysvětlení

**1. EventSource API**

```javascript
const eventSource = new EventSource(API_URL);
```

`EventSource` je nativní browser API pro SSE. Je to součást HTML5 standardu a funguje ve všech moderních browserech.

**2. Message handler**

```javascript
eventSource.addEventListener('message', (event) => {
    const student = JSON.parse(event.data);
    addStudentToTable(student);
});
```

Každou přijatou zprávu parsujeme z JSON a přidáme do tabulky.

**3. Error handling**

```javascript
eventSource.addEventListener('error', (event) => {
    if (eventSource.readyState === EventSource.CLOSED) {
        // Spojení bylo zavřeno
    }
});
```

EventSource má vestavěné automatické znovupřipojování. Pokud server vrátí 5xx chybu, browser si automaticky zkusí znovupřipojit.

**4. Bezpečnost - XSS prevention**

```javascript
function escapeHtml(text) {
    const div = document.createElement('div');
    div.textContent = text;
    return div.innerHTML;
}
```

Vždy escapuj data z API, protože klient nám poslal `name` a mohla by obsahovat malicious JavaScript.

### d) Porovnání: Vanilla JS vs. .NET/Blazor

| Aspekt | Vanilla JS | .NET/Blazor |
|---|---|---|
| **Složitost** | Jednodušší, 50 řádků JS | Složitější, `IAsyncEnumerable`, `StateHasChanged` |
| **Performance** | Přímá DOM manipulace | Přes SignalR |
| **Type-safety** | Bez type-checkingu | C# type-checking |
| **Server-side rendering** | Ne | Ano (SSR) |
| **Real-time UI refresh** | Manuální DOM update | Automatická |
| **Vhodnost** | Jednoduché stránky | Komplexní interaktivní UI |

### e) Co když chceš CSS animaci při přijetí nového studenta?

```javascript
function addStudentToTable(student) {
    const tbody = document.getElementById('studentBody');
    
    // ... existující kód ...
    
    const row = document.createElement('tr');
    row.style.animation = 'slideIn 0.5s ease-in';
    
    // ... zbytek kódu ...
}
```

A CSS:

```css
@keyframes slideIn {
    from {
        opacity: 0;
        transform: translateX(-20px);
    }
    to {
        opacity: 1;
        transform: translateX(0);
    }
}
```

---

## 10. Jak aplikaci spustit a testovat

```powershell
dotnet run --project .\UTB.School.AppHost
```

### Testování Blazor verze:

1. Otevřete `https://localhost:5173/updates` (nebo správný port)
2. V jiné záložce vytvořte studenta přes formulář
3. V `/updates` se student automaticky objeví

### Testování vanilla JS verze:

1. Změňte port v `sse-client.js` na správný port vaší API
2. Otevřete `index.html` v prohlížeči
3. Klikněte "Spustit stream"
4. V jiné záložce vytvořte studenta přes API (např. curl nebo Postman)
5. V vanilla JS stránce se student zobrazí

---

## 11. Co jsme si procvičili

- Princip SSE (Server Sent Events) pro jednosměrnou real-time komunikaci
- Broadcast pattern s `ConcurrentDictionary` a `Channel` (.NET)
- `IAsyncEnumerable<T>` a `async foreach` (.NET)
- `[EnumeratorCancellation]` pro správné zrušení streamů
- `IAsyncDisposable` pro cleanup v Blazoru
- `StateHasChanged()` pro refresh UI (Blazor)
- `HttpCompletionOption.ResponseHeadersRead` pro streaming
- **Nativní EventSource API v JavaScriptu**
- **DOM manipulace v čistém JavaScriptu**
- **XSS prevention v frontend kódu**

---

## ❓ Kontrolní otázky

1. Jaký je rozdíl mezi SSE a WebSockets?
2. Proč používáme `ConcurrentDictionary` místo normálního `Dictionary` s `lock()`?
3. Co dělá `[EnumeratorCancellation]` a proč je potřebný?
4. Proč je `finally` blok v `InitAndGetStream()` důležitý?
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


