using System.Collections.Concurrent;
using System.Net.ServerSentEvents;
using System.Runtime.CompilerServices;
using System.Threading.Channels;

namespace UTB.School.WebApi
{
    public class ServerSentEventsService
    {
        private readonly ConcurrentDictionary<Guid, Channel<SseItem<int>>> subscribers = [];

        public async Task WriteAsync()
        {
            var item = new SseItem<int>(Random.Shared.Next(1, 100), "order-update")
            {
                ReconnectionInterval = TimeSpan.FromMinutes(1)
            };

            // Pošli zprávu všem připojeným klientům
            foreach (var channel in subscribers.Values)
            {
                await channel.Writer.WriteAsync(item);
            }
        }

        public async IAsyncEnumerable<SseItem<int>> InitAndGetStream([EnumeratorCancellation] CancellationToken ct)
        {
            // Vytvoř kanál pro tohoto klienta
            var clientId = Guid.NewGuid();
            var clientChannel = Channel.CreateUnbounded<SseItem<int>>();

            // Zaregistruj tohoto klienta
            subscribers.TryAdd(clientId, clientChannel);

            try
            {
                // Vždy poslat init zprávu jako první
                yield return new SseItem<int>(Random.Shared.Next(1, 100), "order-init")
                {
                    ReconnectionInterval = TimeSpan.FromMinutes(1)
                };

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
