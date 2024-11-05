// HashingHandler by Simon Field

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace HashingHandler.Formats.Stream;

public class StreamHashProvider : IHashingProvider<System.IO.Stream>, IHashingProviderAsync<System.IO.Stream>
{
    private readonly int _bufferSize;

    public StreamHashProvider(int bufferSize = 8192)
    {
        if (bufferSize <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(bufferSize), "Buffer size must be greater than zero.");
        }

        _bufferSize = bufferSize;
    }

    public byte[] ConvertToBytes(System.IO.Stream data)
    {
        if (data == null) throw new ArgumentNullException(nameof(data));

        return ReadStream(data, false);
    }

    public Task<byte[]> ConvertToBytes(System.IO.Stream data, CancellationToken cancellationToken = default)
    {
        if (data == null) throw new ArgumentNullException(nameof(data));

        return Task.Run(() => ReadStream(data, true), cancellationToken);
    }

    private byte[] ReadStream(System.IO.Stream data, bool isAsync)
    {
        using var memoryStream = new MemoryStream();
        byte[] buffer = new byte[_bufferSize];
        int bytesRead;

        if (isAsync)
        {
            return ReadStreamAsync(data, memoryStream, buffer).GetAwaiter().GetResult();
        }

        while ((bytesRead = data.Read(buffer, 0, buffer.Length)) > 0)
        {
            memoryStream.Write(buffer, 0, bytesRead);
        }

        return memoryStream.ToArray();
    }

    private async Task<byte[]> ReadStreamAsync(System.IO.Stream data, MemoryStream memoryStream, byte[] buffer)
    {
        int bytesRead;

        while ((bytesRead = await data.ReadAsync(buffer, 0, buffer.Length)) > 0)
        {
            await memoryStream.WriteAsync(buffer, 0, bytesRead);
        }

        return memoryStream.ToArray();
    }
}
