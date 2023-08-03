// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.ECPublicBcpgKey
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using System.IO;

namespace Org.BouncyCastle.Bcpg
{
    public abstract class ECPublicBcpgKey : BcpgObject, IBcpgKey
    {
        internal DerObjectIdentifier oid;
        internal BigInteger point;

        protected ECPublicBcpgKey( BcpgInputStream bcpgIn )
        {
            this.oid = DerObjectIdentifier.GetInstance( Asn1Object.FromByteArray( ReadBytesOfEncodedLength( bcpgIn ) ) );
            this.point = new MPInteger( bcpgIn ).Value;
        }

        protected ECPublicBcpgKey( DerObjectIdentifier oid, ECPoint point )
        {
            this.point = new BigInteger( 1, point.GetEncoded() );
            this.oid = oid;
        }

        protected ECPublicBcpgKey( DerObjectIdentifier oid, BigInteger encodedPoint )
        {
            this.point = encodedPoint;
            this.oid = oid;
        }

        public string Format => "PGP";

        public override byte[] GetEncoded()
        {
            try
            {
                return base.GetEncoded();
            }
            catch (IOException ex)
            {
                return null;
            }
        }

        public override void Encode( BcpgOutputStream bcpgOut )
        {
            byte[] encoded = this.oid.GetEncoded();
            bcpgOut.Write( encoded, 1, encoded.Length - 1 );
            MPInteger mpInteger = new MPInteger( this.point );
            bcpgOut.WriteObject( mpInteger );
        }

        public virtual BigInteger EncodedPoint => this.point;

        public virtual DerObjectIdentifier CurveOid => this.oid;

        protected static byte[] ReadBytesOfEncodedLength( BcpgInputStream bcpgIn )
        {
            int num = bcpgIn.ReadByte();
            switch (num)
            {
                case 0:
                case byte.MaxValue:
                    throw new IOException( "future extensions not yet implemented." );
                default:
                    byte[] buffer = new byte[num + 2];
                    bcpgIn.ReadFully( buffer, 2, buffer.Length - 2 );
                    buffer[0] = 6;
                    buffer[1] = (byte)num;
                    return buffer;
            }
        }
    }
}
