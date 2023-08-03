// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Utilities.IO.Streams
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System.IO;

namespace Org.BouncyCastle.Utilities.IO
{
    public sealed class Streams
    {
        private const int BufferSize = 512;

        private Streams()
        {
        }

        public static void Drain( Stream inStr )
        {
            byte[] buffer = new byte[512];
            do
                ;
            while (inStr.Read( buffer, 0, buffer.Length ) > 0);
        }

        public static byte[] ReadAll( Stream inStr )
        {
            MemoryStream outStr = new();
            PipeAll( inStr, outStr );
            return outStr.ToArray();
        }

        public static byte[] ReadAllLimited( Stream inStr, int limit )
        {
            MemoryStream outStr = new();
            PipeAllLimited( inStr, limit, outStr );
            return outStr.ToArray();
        }

        public static int ReadFully( Stream inStr, byte[] buf ) => ReadFully( inStr, buf, 0, buf.Length );

        public static int ReadFully( Stream inStr, byte[] buf, int off, int len )
        {
            int num1;
            int num2;
            for (num1 = 0; num1 < len; num1 += num2)
            {
                num2 = inStr.Read( buf, off + num1, len - num1 );
                if (num2 < 1)
                    break;
            }
            return num1;
        }

        public static void PipeAll( Stream inStr, Stream outStr )
        {
            byte[] buffer = new byte[512];
            int count;
            while ((count = inStr.Read( buffer, 0, buffer.Length )) > 0)
                outStr.Write( buffer, 0, count );
        }

        public static long PipeAllLimited( Stream inStr, long limit, Stream outStr )
        {
            byte[] buffer = new byte[512];
            long num = 0;
            int count;
            while ((count = inStr.Read( buffer, 0, buffer.Length )) > 0)
            {
                if (limit - num < count)
                    throw new StreamOverflowException( "Data Overflow" );
                num += count;
                outStr.Write( buffer, 0, count );
            }
            return num;
        }
    }
}
