// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Engines.NaccacheSternEngine
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Utilities;
using System.Collections;

namespace Org.BouncyCastle.Crypto.Engines
{
    public class NaccacheSternEngine : IAsymmetricBlockCipher
    {
        private bool forEncryption;
        private NaccacheSternKeyParameters key;
        private IList[] lookup = null;

        public string AlgorithmName => "NaccacheStern";

        public virtual void Init( bool forEncryption, ICipherParameters parameters )
        {
            this.forEncryption = forEncryption;
            if (parameters is ParametersWithRandom)
                parameters = ((ParametersWithRandom)parameters).Parameters;
            this.key = (NaccacheSternKeyParameters)parameters;
            if (this.forEncryption)
                return;
            NaccacheSternPrivateKeyParameters key = (NaccacheSternPrivateKeyParameters)this.key;
            IList smallPrimesList = key.SmallPrimesList;
            this.lookup = new IList[smallPrimesList.Count];
            for (int index1 = 0; index1 < smallPrimesList.Count; ++index1)
            {
                BigInteger val = (BigInteger)smallPrimesList[index1];
                int intValue = val.IntValue;
                this.lookup[index1] = Platform.CreateArrayList( intValue );
                this.lookup[index1].Add( BigInteger.One );
                BigInteger bigInteger = BigInteger.Zero;
                for (int index2 = 1; index2 < intValue; ++index2)
                {
                    bigInteger = bigInteger.Add( key.PhiN );
                    BigInteger e = bigInteger.Divide( val );
                    this.lookup[index1].Add( key.G.ModPow( e, key.Modulus ) );
                }
            }
        }

        [Obsolete( "Remove: no longer used" )]
        public virtual bool Debug
        {
            set
            {
            }
        }

        public virtual int GetInputBlockSize() => this.forEncryption ? ((this.key.LowerSigmaBound + 7) / 8) - 1 : (this.key.Modulus.BitLength / 8) + 1;

        public virtual int GetOutputBlockSize() => this.forEncryption ? (this.key.Modulus.BitLength / 8) + 1 : ((this.key.LowerSigmaBound + 7) / 8) - 1;

        public virtual byte[] ProcessBlock( byte[] inBytes, int inOff, int length )
        {
            if (this.key == null)
                throw new InvalidOperationException( "NaccacheStern engine not initialised" );
            if (length > this.GetInputBlockSize() + 1)
                throw new DataLengthException( "input too large for Naccache-Stern cipher.\n" );
            if (!this.forEncryption && length < this.GetInputBlockSize())
                throw new InvalidCipherTextException( "BlockLength does not match modulus for Naccache-Stern cipher.\n" );
            BigInteger plain = new( 1, inBytes, inOff, length );
            byte[] numArray;
            if (this.forEncryption)
            {
                numArray = this.Encrypt( plain );
            }
            else
            {
                IList arrayList = Platform.CreateArrayList();
                NaccacheSternPrivateKeyParameters key = (NaccacheSternPrivateKeyParameters)this.key;
                IList smallPrimesList = key.SmallPrimesList;
                for (int index = 0; index < smallPrimesList.Count; ++index)
                {
                    BigInteger bigInteger = plain.ModPow( key.PhiN.Divide( (BigInteger)smallPrimesList[index] ), key.Modulus );
                    IList list = this.lookup[index];
                    if (this.lookup[index].Count != ((BigInteger)smallPrimesList[index]).IntValue)
                        throw new InvalidCipherTextException( "Error in lookup Array for " + ((BigInteger)smallPrimesList[index]).IntValue + ": Size mismatch. Expected ArrayList with length " + ((BigInteger)smallPrimesList[index]).IntValue + " but found ArrayList of length " + lookup[index].Count );
                    int num = list.IndexOf( bigInteger );
                    if (num == -1)
                        throw new InvalidCipherTextException( "Lookup failed" );
                    arrayList.Add( BigInteger.ValueOf( num ) );
                }
                numArray = chineseRemainder( arrayList, smallPrimesList ).ToByteArray();
            }
            return numArray;
        }

        public virtual byte[] Encrypt( BigInteger plain )
        {
            byte[] destinationArray = new byte[(this.key.Modulus.BitLength / 8) + 1];
            byte[] byteArray = this.key.G.ModPow( plain, this.key.Modulus ).ToByteArray();
            Array.Copy( byteArray, 0, destinationArray, destinationArray.Length - byteArray.Length, byteArray.Length );
            return destinationArray;
        }

        public virtual byte[] AddCryptedBlocks( byte[] block1, byte[] block2 )
        {
            if (this.forEncryption)
            {
                if (block1.Length > this.GetOutputBlockSize() || block2.Length > this.GetOutputBlockSize())
                    throw new InvalidCipherTextException( "BlockLength too large for simple addition.\n" );
            }
            else if (block1.Length > this.GetInputBlockSize() || block2.Length > this.GetInputBlockSize())
                throw new InvalidCipherTextException( "BlockLength too large for simple addition.\n" );
            BigInteger bigInteger = new BigInteger( 1, block1 ).Multiply( new BigInteger( 1, block2 ) ).Mod( this.key.Modulus );
            byte[] destinationArray = new byte[(this.key.Modulus.BitLength / 8) + 1];
            byte[] byteArray = bigInteger.ToByteArray();
            Array.Copy( byteArray, 0, destinationArray, destinationArray.Length - byteArray.Length, byteArray.Length );
            return destinationArray;
        }

        public virtual byte[] ProcessData( byte[] data )
        {
            if (data.Length <= this.GetInputBlockSize())
                return this.ProcessBlock( data, 0, data.Length );
            int inputBlockSize = this.GetInputBlockSize();
            int outputBlockSize = this.GetOutputBlockSize();
            int inOff = 0;
            int length = 0;
            byte[] sourceArray = new byte[((data.Length / inputBlockSize) + 1) * outputBlockSize];
            while (inOff < data.Length)
            {
                byte[] numArray;
                if (inOff + inputBlockSize < data.Length)
                {
                    numArray = this.ProcessBlock( data, inOff, inputBlockSize );
                    inOff += inputBlockSize;
                }
                else
                {
                    numArray = this.ProcessBlock( data, inOff, data.Length - inOff );
                    inOff += data.Length - inOff;
                }
                if (numArray == null)
                    throw new InvalidCipherTextException( "cipher returned null" );
                numArray.CopyTo( sourceArray, length );
                length += numArray.Length;
            }
            byte[] destinationArray = new byte[length];
            Array.Copy( sourceArray, 0, destinationArray, 0, length );
            return destinationArray;
        }

        private static BigInteger chineseRemainder( IList congruences, IList primes )
        {
            BigInteger bigInteger1 = BigInteger.Zero;
            BigInteger m = BigInteger.One;
            for (int index = 0; index < primes.Count; ++index)
                m = m.Multiply( (BigInteger)primes[index] );
            for (int index = 0; index < primes.Count; ++index)
            {
                BigInteger prime = (BigInteger)primes[index];
                BigInteger bigInteger2 = m.Divide( prime );
                BigInteger val = bigInteger2.ModInverse( prime );
                BigInteger bigInteger3 = bigInteger2.Multiply( val ).Multiply( (BigInteger)congruences[index] );
                bigInteger1 = bigInteger1.Add( bigInteger3 );
            }
            return bigInteger1.Mod( m );
        }
    }
}
