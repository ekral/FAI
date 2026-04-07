using System.Collections.Concurrent;
using System.Net.ServerSentEvents;
using System.Runtime.CompilerServices;
using System.Threading.Channels;
using UTB.School.Contracts;

namespace UTB.School.WebApi
{
    public class ServerSentEventsService
    {
        private readonly ConcurrentDictionary<Guid, Channel<StudentDto>> subscribers = [];

        // Zavolá se, když se vytvoří nový student
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
