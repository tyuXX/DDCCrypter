// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Check
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Crypto
{
    internal class Check
    {
        internal static void DataLength( bool condition, string msg )
        {
            if (condition)
                throw new DataLengthException( msg );
        }

        internal static void DataLength( byte[] buf, int off, int len, string msg )
        {
            if (off + len > buf.Length)
                throw new DataLengthException( msg );
        }

        internal static void OutputLength( byte[] buf, int off, int len, string msg )
        {
            if (off + len > buf.Length)
                throw new OutputLengthException( msg );
        }
    }
}
