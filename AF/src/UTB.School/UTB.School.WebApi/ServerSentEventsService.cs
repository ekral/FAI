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
        private readonly ConcurrentDictionary<Guid, Channel<SseItem<StudentDto[]>>> subscribers = [];

        public async Task WriteAsync(StudentDto[] students)
        {
            // Pošli zprávu všem připojeným klientům
            foreach (Channel<SseItem<StudentDto[]>> channel in subscribers.Values)
            {
                SseItem<StudentDto[]> sseItem = new(students);

                await channel.Writer.WriteAsync(sseItem);
            }
        }

        // Zavolá se, když se klient připojí na GET /stream
        public IAsyncEnumerable<SseItem<StudentDto[]>> InitAndGetStream(StudentDto[] initData, CancellationToken ct)
        {
            // Vytvoř kanál pro tohoto konkrétního klienta
            var clientId = Guid.NewGuid();

            var clientChannel = Channel.CreateBounded<SseItem<StudentDto[]>>(new BoundedChannelOptions(20) { FullMode = BoundedChannelFullMode.DropOldest});

            ct.Register(() => subscribers.TryRemove(clientId, out _));
            
            subscribers.TryAdd(clientId, clientChannel);

            clientChannel.Writer.TryWrite(new SseItem<StudentDto[]>(initData));

            return clientChannel.Reader.ReadAllAsync(ct);
        }
    }
}
