// HashingHandler by Simon Field

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace HashingHandler.Formats.Stream;

public class StreamHashProvider : IHashingProviderAsync<System.IO.Stream>
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

        return ReadStream(data);
    }

    public async Task<byte[]> ConvertToBytesAsync(System.IO.Stream data, CancellationToken cancellationToken = default)
    {
        if (data == null) throw new ArgumentNullException(nameof(data));

        return await ReadStreamAsync(data, cancellationToken);
    }

    private byte[] ReadStream(System.IO.Stream data)
    {
        using MemoryStream memoryStream = new();
        byte[] buffer = new byte[_bufferSize];
        int bytesRead;

        while ((bytesRead = data.Read(buffer, 0, buffer.Length)) > 0)
        {
            memoryStream.Write(buffer, 0, bytesRead);
        }

        return memoryStream.ToArray();
    }

    private async Task<byte[]> ReadStreamAsync(System.IO.Stream data, CancellationToken cancellationToken)
    {
        using MemoryStream memoryStream = new();
        byte[] buffer = new byte[_bufferSize];
        int bytesRead;

        while ((bytesRead = await data.ReadAsync(buffer, 0, buffer.Length, cancellationToken)) > 0)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await memoryStream.WriteAsync(buffer, 0, bytesRead, cancellationToken);
        }

        return memoryStream.ToArray();
    }
}
