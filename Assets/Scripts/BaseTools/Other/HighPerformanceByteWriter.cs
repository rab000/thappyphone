

/// <summary>
/// 高效ByteWriter
/// </summary>
public class HighPerformanceByteWriter
{
    byte[] buffer;
    int i;
    public HighPerformanceByteWriter(int length)
    {
        Length = length;
        buffer = new byte[length];//这里可以加byte[] 池
        i = 0;
    }

    public int Length { get; private set; }

    public void Write(byte v)
    {
        buffer[i++] = v;
    }

    public void Write(bool v)
    {
        if (v) buffer[i++] = 0x01;
        else buffer[i++] = 0x00;
    }

    public void Write(ushort v)
    {
        buffer[i++] = (byte)v;
        buffer[i++] = (byte)(v >> 8);
    }

    public void Write(int v)
    {
        buffer[i++] = (byte)v;
        buffer[i++] = (byte)(v >> 8);
        buffer[i++] = (byte)(v >> 16);
        buffer[i++] = (byte)(v >> 24);
    }

    public void Write(uint v)
    {
        buffer[i++] = (byte)v;
        buffer[i++] = (byte)(v >> 8);
        buffer[i++] = (byte)(v >> 16);
        buffer[i++] = (byte)(v >> 24);
    }

    public void Write(long v)
    {
        buffer[i++] = (byte)v;
        buffer[i++] = (byte)(v >> 8);
        buffer[i++] = (byte)(v >> 16);
        buffer[i++] = (byte)(v >> 24);
        buffer[i++] = (byte)(v >> 32);
        buffer[i++] = (byte)(v >> 40);
        buffer[i++] = (byte)(v >> 48);
        buffer[i++] = (byte)(v >> 56);
    }

    public void Write(ulong v)
    {
        buffer[i++] = (byte)v;
        buffer[i++] = (byte)(v >> 8);
        buffer[i++] = (byte)(v >> 16);
        buffer[i++] = (byte)(v >> 24);
        buffer[i++] = (byte)(v >> 32);
        buffer[i++] = (byte)(v >> 40);
        buffer[i++] = (byte)(v >> 48);
        buffer[i++] = (byte)(v >> 56);
    }

    //public void Write(double v)
    //{
    //    Write(FloatEncoder.Float64bits(v));
    //}

    public void Write(string v)
    {
        byte[] strBytes = System.Text.Encoding.Default.GetBytes(v);
        int len = strBytes.Length;
        Write(len);
        for (int j = 0; j < len; j++)
        {
            buffer[i++] = strBytes[j];
        }
    }

    public void Write(byte[] v)
    {
        for (int j = 0; j < v.Length; j++)
        {
            buffer[i++] = v[j];
        }
    }

    public byte[] EndWrite()
    {
        return buffer;
    }


}


