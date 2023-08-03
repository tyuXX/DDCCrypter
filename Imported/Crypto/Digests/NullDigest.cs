// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Digests.NullDigest
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System;
using System.IO;

namespace Org.BouncyCastle.Crypto.Digests
{
    public class NullDigest : IDigest
    {
        private readonly MemoryStream bOut = new MemoryStream();

        public string AlgorithmName => "NULL";

        public int GetByteLength() => 0;

        public int GetDigestSize() => (int)this.bOut.Length;

        public void Update( byte b ) => this.bOut.WriteByte( b );

        public void BlockUpdate( byte[] inBytes, int inOff, int len ) => this.bOut.Write( inBytes, inOff, len );

        public int DoFinal( byte[] outBytes, int outOff )
        {
            byte[] array = this.bOut.ToArray();
            array.CopyTo( outBytes, outOff );
            this.Reset();
            return array.Length;
        }

        public void Reset() => this.bOut.SetLength( 0L );
    }
}
