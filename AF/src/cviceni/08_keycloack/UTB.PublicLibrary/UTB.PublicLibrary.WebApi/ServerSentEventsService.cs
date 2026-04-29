using System.Collections.Concurrent;
using System.Net.ServerSentEvents;
using System.Threading.Channels;
using UTB.PublicLibrary.Contracts;

namespace UTB.PublicLibrary.WebApi
{
    public class ServerSentEventsService
    {
        private readonly ConcurrentDictionary<Guid, Channel<SseItem<BookDto[]>>> subscribers = [];

        public async Task WriteAsync(BookDto[] books)
        {
            // Pošli zprávu všem připojeným klientům
            foreach (Channel<SseItem<BookDto[]>> channel in subscribers.Values)
            {
                SseItem<BookDto[]> sseItem = new(books);

                await channel.Writer.WriteAsync(sseItem);
            }
        }

        // Zavolá se, když se klient připojí na GET /stream
        public IAsyncEnumerable<SseItem<BookDto[]>> InitAndGetStream(BookDto[] initData, CancellationToken ct)
        {
            // Vytvoř kanál pro tohoto konkrétního klienta
            var clientId = Guid.NewGuid();

            var clientChannel = Channel.CreateBounded<SseItem<BookDto[]>>(new BoundedChannelOptions(20) { FullMode = BoundedChannelFullMode.DropOldest });

            ct.Register(() => subscribers.TryRemove(clientId, out _));

            subscribers.TryAdd(clientId, clientChannel);

            clientChannel.Writer.TryWrite(new SseItem<BookDto[]>(initData));

            return clientChannel.Reader.ReadAllAsync(ct);
        }
    }
}
