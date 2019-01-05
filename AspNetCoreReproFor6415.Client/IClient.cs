using System.Threading;
using System.Threading.Tasks;
using Refit;

namespace AspNetCoreReproFor6415.Client
{
    public interface IClient
    {
        [Post("/{a}/x/{b}/y/{c}/z")]
        Task PostUnbufferedAsync(
            string a,
            string b,
            string c,
            MyModel content,
            CancellationToken cancellationToken = default);

        [Post("/{a}/x/{b}/y/{c}/z")]
        Task PostBufferedAsync(
            string a,
            string b,
            string c,
            [Body(buffered: true)] MyModel content,
            CancellationToken cancellationToken = default);
    }
}
