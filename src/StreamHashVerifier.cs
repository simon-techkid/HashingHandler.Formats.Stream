// HashingHandler by Simon Field

namespace HashingHandler.Formats.Stream;

public class StreamHashVerifier : HashVerifierBase<System.IO.Stream>
{
    /// <summary>
    /// Construct a <see cref="StreamHashVerifier"/> object with the default buffer size.
    /// </summary>
    public StreamHashVerifier()
    {
        HashProvider = new StreamHashProvider();
    }

    /// <summary>
    /// Construct a <see cref="StreamHashVerifier"/> object with the given buffer size, <paramref name="bufferSize"/>.
    /// </summary>
    /// <param name="bufferSize"></param>
    public StreamHashVerifier(int bufferSize)
    {
        HashProvider = new StreamHashProvider(bufferSize);
    }

    protected override IHashingProvider<System.IO.Stream> HashProvider { get; }
}
