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
        private readonly ConcurrentDictionary<Guid, Channel<SseItem<StudentDto>>> subscribers = [];

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
            
            subscribers.TryAdd(clientId, clientChannel);

            return clientChannel.Reader.ReadAllAsync(ct);
        }
    }
}
