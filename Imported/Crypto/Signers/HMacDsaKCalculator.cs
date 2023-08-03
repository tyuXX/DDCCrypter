// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Signers.HMacDsaKCalculator
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Macs;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Signers
{
    public class HMacDsaKCalculator : IDsaKCalculator
    {
        private readonly HMac hMac;
        private readonly byte[] K;
        private readonly byte[] V;
        private BigInteger n;

        public HMacDsaKCalculator( IDigest digest )
        {
            this.hMac = new HMac( digest );
            this.V = new byte[this.hMac.GetMacSize()];
            this.K = new byte[this.hMac.GetMacSize()];
        }

        public virtual bool IsDeterministic => true;

        public virtual void Init( BigInteger n, SecureRandom random ) => throw new InvalidOperationException( "Operation not supported" );

        public void Init( BigInteger n, BigInteger d, byte[] message )
        {
            this.n = n;
            Arrays.Fill( this.V, 1 );
            Arrays.Fill( this.K, 0 );
            byte[] numArray1 = new byte[(n.BitLength + 7) / 8];
            byte[] sourceArray1 = BigIntegers.AsUnsignedByteArray( d );
            Array.Copy( sourceArray1, 0, numArray1, numArray1.Length - sourceArray1.Length, sourceArray1.Length );
            byte[] numArray2 = new byte[(n.BitLength + 7) / 8];
            BigInteger n1 = this.BitsToInt( message );
            if (n1.CompareTo( n ) >= 0)
                n1 = n1.Subtract( n );
            byte[] sourceArray2 = BigIntegers.AsUnsignedByteArray( n1 );
            Array.Copy( sourceArray2, 0, numArray2, numArray2.Length - sourceArray2.Length, sourceArray2.Length );
            this.hMac.Init( new KeyParameter( this.K ) );
            this.hMac.BlockUpdate( this.V, 0, this.V.Length );
            this.hMac.Update( 0 );
            this.hMac.BlockUpdate( numArray1, 0, numArray1.Length );
            this.hMac.BlockUpdate( numArray2, 0, numArray2.Length );
            this.hMac.DoFinal( this.K, 0 );
            this.hMac.Init( new KeyParameter( this.K ) );
            this.hMac.BlockUpdate( this.V, 0, this.V.Length );
            this.hMac.DoFinal( this.V, 0 );
            this.hMac.BlockUpdate( this.V, 0, this.V.Length );
            this.hMac.Update( 1 );
            this.hMac.BlockUpdate( numArray1, 0, numArray1.Length );
            this.hMac.BlockUpdate( numArray2, 0, numArray2.Length );
            this.hMac.DoFinal( this.K, 0 );
            this.hMac.Init( new KeyParameter( this.K ) );
            this.hMac.BlockUpdate( this.V, 0, this.V.Length );
            this.hMac.DoFinal( this.V, 0 );
        }

        public virtual BigInteger NextK()
        {
            byte[] numArray = new byte[(this.n.BitLength + 7) / 8];
            BigInteger bigInteger;
            while (true)
            {
                int length;
                for (int destinationIndex = 0; destinationIndex < numArray.Length; destinationIndex += length)
                {
                    this.hMac.BlockUpdate( this.V, 0, this.V.Length );
                    this.hMac.DoFinal( this.V, 0 );
                    length = System.Math.Min( numArray.Length - destinationIndex, this.V.Length );
                    Array.Copy( V, 0, numArray, destinationIndex, length );
                }
                bigInteger = this.BitsToInt( numArray );
                if (bigInteger.SignValue <= 0 || bigInteger.CompareTo( this.n ) >= 0)
                {
                    this.hMac.BlockUpdate( this.V, 0, this.V.Length );
                    this.hMac.Update( 0 );
                    this.hMac.DoFinal( this.K, 0 );
                    this.hMac.Init( new KeyParameter( this.K ) );
                    this.hMac.BlockUpdate( this.V, 0, this.V.Length );
                    this.hMac.DoFinal( this.V, 0 );
                }
                else
                    break;
            }
            return bigInteger;
        }

        private BigInteger BitsToInt( byte[] t )
        {
            BigInteger bigInteger = new( 1, t );
            if (t.Length * 8 > this.n.BitLength)
                bigInteger = bigInteger.ShiftRight( (t.Length * 8) - this.n.BitLength );
            return bigInteger;
        }
    }
}
