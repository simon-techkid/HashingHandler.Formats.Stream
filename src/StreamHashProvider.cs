// HashingHandler by Simon Field

using System.IO;

namespace HashingHandler.Formats.Stream;

public class StreamHashProvider : IHashingProvider<System.IO.Stream>
{
    public byte[] ConvertToBytes(System.IO.Stream data)
    {
        byte[] buffer = new byte[1024];
        using MemoryStream memoryStream = new();
        int read;
        while ((read = data.Read(buffer, 0, buffer.Length)) > 0)
        {
            memoryStream.Write(buffer, 0, read);
        }
        return memoryStream.ToArray();
    }
}
