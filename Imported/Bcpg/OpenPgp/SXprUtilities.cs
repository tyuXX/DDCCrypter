// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.OpenPgp.SXprUtilities
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities.IO;
using System.IO;

namespace Org.BouncyCastle.Bcpg.OpenPgp
{
    public sealed class SXprUtilities
    {
        private SXprUtilities()
        {
        }

        private static int ReadLength( Stream input, int ch )
        {
            int num = ch - 48;
            while ((ch = input.ReadByte()) >= 0 && ch != 58)
                num = (num * 10) + ch - 48;
            return num;
        }

        internal static string ReadString( Stream input, int ch )
        {
            char[] chArray = new char[ReadLength( input, ch )];
            for (int index = 0; index != chArray.Length; ++index)
                chArray[index] = (char)input.ReadByte();
            return new string( chArray );
        }

        internal static byte[] ReadBytes( Stream input, int ch )
        {
            byte[] buf = new byte[ReadLength( input, ch )];
            Streams.ReadFully( input, buf );
            return buf;
        }

        internal static S2k ParseS2k( Stream input )
        {
            SkipOpenParenthesis( input );
            ReadString( input, input.ReadByte() );
            byte[] iv = ReadBytes( input, input.ReadByte() );
            long iterationCount64 = long.Parse( ReadString( input, input.ReadByte() ) );
            SkipCloseParenthesis( input );
            return new SXprUtilities.MyS2k( HashAlgorithmTag.Sha1, iv, iterationCount64 );
        }

        internal static void SkipOpenParenthesis( Stream input )
        {
            if (input.ReadByte() != 40)
                throw new IOException( "unknown character encountered" );
        }

        internal static void SkipCloseParenthesis( Stream input )
        {
            if (input.ReadByte() != 41)
                throw new IOException( "unknown character encountered" );
        }

        private class MyS2k : S2k
        {
            private readonly long mIterationCount64;

            internal MyS2k( HashAlgorithmTag algorithm, byte[] iv, long iterationCount64 )
              : base( algorithm, iv, (int)iterationCount64 )
            {
                this.mIterationCount64 = iterationCount64;
            }

            public override long IterationCount => this.mIterationCount64;
        }
    }
}
