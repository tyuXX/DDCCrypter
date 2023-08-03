// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Agreement.Kdf.DHKekGenerator
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Crypto.Utilities;
using System;

namespace Org.BouncyCastle.Crypto.Agreement.Kdf
{
    public class DHKekGenerator : IDerivationFunction
    {
        private readonly IDigest digest;
        private DerObjectIdentifier algorithm;
        private int keySize;
        private byte[] z;
        private byte[] partyAInfo;

        public DHKekGenerator( IDigest digest ) => this.digest = digest;

        public virtual void Init( IDerivationParameters param )
        {
            DHKdfParameters dhKdfParameters = (DHKdfParameters)param;
            this.algorithm = dhKdfParameters.Algorithm;
            this.keySize = dhKdfParameters.KeySize;
            this.z = dhKdfParameters.GetZ();
            this.partyAInfo = dhKdfParameters.GetExtraInfo();
        }

        public virtual IDigest Digest => this.digest;

        public virtual int GenerateBytes( byte[] outBytes, int outOff, int len )
        {
            if (outBytes.Length - len < outOff)
                throw new DataLengthException( "output buffer too small" );
            long bytes = len;
            int digestSize = this.digest.GetDigestSize();
            if (bytes > 8589934591L)
                throw new ArgumentException( "Output length too large" );
            int num = (int)((bytes + digestSize - 1L) / digestSize);
            byte[] numArray = new byte[this.digest.GetDigestSize()];
            uint n = 1;
            for (int index = 0; index < num; ++index)
            {
                this.digest.BlockUpdate( this.z, 0, this.z.Length );
                Asn1EncodableVector v = new Asn1EncodableVector( new Asn1Encodable[1]
                {
           new DerSequence(new Asn1Encodable[2]
          {
             algorithm,
             new DerOctetString(Pack.UInt32_To_BE(n))
          })
                } );
                if (this.partyAInfo != null)
                    v.Add( new DerTaggedObject( true, 0, new DerOctetString( this.partyAInfo ) ) );
                v.Add( new DerTaggedObject( true, 2, new DerOctetString( Pack.UInt32_To_BE( (uint)this.keySize ) ) ) );
                byte[] derEncoded = new DerSequence( v ).GetDerEncoded();
                this.digest.BlockUpdate( derEncoded, 0, derEncoded.Length );
                this.digest.DoFinal( numArray, 0 );
                if (len > digestSize)
                {
                    Array.Copy( numArray, 0, outBytes, outOff, digestSize );
                    outOff += digestSize;
                    len -= digestSize;
                }
                else
                    Array.Copy( numArray, 0, outBytes, outOff, len );
                ++n;
            }
            this.digest.Reset();
            return (int)bytes;
        }
    }
}
