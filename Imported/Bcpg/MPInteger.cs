// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.MPInteger
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math;
using System;

namespace Org.BouncyCastle.Bcpg
{
    public class MPInteger : BcpgObject
    {
        private readonly BigInteger val;

        public MPInteger( BcpgInputStream bcpgIn )
        {
            byte[] numArray = bcpgIn != null ? new byte[(((bcpgIn.ReadByte() << 8) | bcpgIn.ReadByte()) + 7) / 8] : throw new ArgumentNullException( nameof( bcpgIn ) );
            bcpgIn.ReadFully( numArray );
            this.val = new BigInteger( 1, numArray );
        }

        public MPInteger( BigInteger val )
        {
            if (val == null)
                throw new ArgumentNullException( nameof( val ) );
            this.val = val.SignValue >= 0 ? val : throw new ArgumentException( "Values must be positive", nameof( val ) );
        }

        public BigInteger Value => this.val;

        public override void Encode( BcpgOutputStream bcpgOut )
        {
            bcpgOut.WriteShort( (short)this.val.BitLength );
            bcpgOut.Write( this.val.ToByteArrayUnsigned() );
        }

        internal static void Encode( BcpgOutputStream bcpgOut, BigInteger val )
        {
            bcpgOut.WriteShort( (short)val.BitLength );
            bcpgOut.Write( val.ToByteArrayUnsigned() );
        }
    }
}
