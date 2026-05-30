using System.IO.Compression;

namespace OpenGLExercises.Core;

/// <summary>
/// Tiny dependency-free PNG writer (8-bit RGBA). Used by the capture mode so an
/// exercise can be rendered headlessly to an image for verification, without
/// pulling in System.Drawing or ImageSharp.
/// </summary>
public static class Png
{
    /// <summary>Write <paramref name="rgba"/> (row-major, top-to-bottom) as a PNG.</summary>
    public static void Save(string path, int width, int height, byte[] rgba)
    {
        using var fs = File.Create(path);
        Span<byte> signature = stackalloc byte[] { 137, 80, 78, 71, 13, 10, 26, 10 };
        fs.Write(signature);

        // IHDR
        var ihdr = new byte[13];
        WriteBE(ihdr, 0, (uint)width);
        WriteBE(ihdr, 4, (uint)height);
        ihdr[8] = 8;   // bit depth
        ihdr[9] = 6;   // colour type: RGBA
        ihdr[10] = 0;  // compression
        ihdr[11] = 0;  // filter
        ihdr[12] = 0;  // interlace
        WriteChunk(fs, "IHDR", ihdr);

        // IDAT: raw scanlines, each prefixed with filter byte 0, wrapped in a zlib stream.
        var raw = new byte[height * (width * 4 + 1)];
        int p = 0;
        for (int y = 0; y < height; y++)
        {
            raw[p++] = 0; // filter: none
            Array.Copy(rgba, y * width * 4, raw, p, width * 4);
            p += width * 4;
        }
        WriteChunk(fs, "IDAT", ZlibCompress(raw));

        WriteChunk(fs, "IEND", Array.Empty<byte>());
    }

    private static byte[] ZlibCompress(byte[] data)
    {
        using var ms = new MemoryStream();
        ms.WriteByte(0x78); // zlib header: CMF
        ms.WriteByte(0x9C); // FLG (default compression)
        using (var deflate = new DeflateStream(ms, CompressionLevel.Optimal, leaveOpen: true))
            deflate.Write(data, 0, data.Length);
        WriteBE(ms, Adler32(data));
        return ms.ToArray();
    }

    private static void WriteChunk(Stream s, string type, byte[] data)
    {
        Span<byte> len = stackalloc byte[4];
        WriteBE(len, 0, (uint)data.Length);
        s.Write(len);

        var typeBytes = new byte[4];
        for (int i = 0; i < 4; i++) typeBytes[i] = (byte)type[i];
        s.Write(typeBytes);
        s.Write(data);

        uint crc = Crc32(typeBytes, data);
        Span<byte> crcBytes = stackalloc byte[4];
        WriteBE(crcBytes, 0, crc);
        s.Write(crcBytes);
    }

    private static void WriteBE(Span<byte> dst, int offset, uint value)
    {
        dst[offset] = (byte)(value >> 24);
        dst[offset + 1] = (byte)(value >> 16);
        dst[offset + 2] = (byte)(value >> 8);
        dst[offset + 3] = (byte)value;
    }

    private static void WriteBE(Stream s, uint value)
    {
        s.WriteByte((byte)(value >> 24));
        s.WriteByte((byte)(value >> 16));
        s.WriteByte((byte)(value >> 8));
        s.WriteByte((byte)value);
    }

    private static uint Adler32(byte[] data)
    {
        const uint mod = 65521;
        uint a = 1, b = 0;
        foreach (var d in data)
        {
            a = (a + d) % mod;
            b = (b + a) % mod;
        }
        return (b << 16) | a;
    }

    private static readonly uint[] CrcTable = BuildCrcTable();

    private static uint[] BuildCrcTable()
    {
        var table = new uint[256];
        for (uint n = 0; n < 256; n++)
        {
            uint c = n;
            for (int k = 0; k < 8; k++)
                c = (c & 1) != 0 ? 0xEDB88320 ^ (c >> 1) : c >> 1;
            table[n] = c;
        }
        return table;
    }

    private static uint Crc32(byte[] type, byte[] data)
    {
        uint c = 0xFFFFFFFF;
        foreach (var b in type) c = CrcTable[(c ^ b) & 0xFF] ^ (c >> 8);
        foreach (var b in data) c = CrcTable[(c ^ b) & 0xFF] ^ (c >> 8);
        return c ^ 0xFFFFFFFF;
    }
}
