// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.DerBitString
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math;
using Org.BouncyCastle.Utilities;
using System;
using System.Text;

namespace Org.BouncyCastle.Asn1
{
    public class DerBitString : DerStringBase
    {
        private static readonly char[] table = new char[16]
        {
      '0',
      '1',
      '2',
      '3',
      '4',
      '5',
      '6',
      '7',
      '8',
      '9',
      'A',
      'B',
      'C',
      'D',
      'E',
      'F'
        };
        protected readonly byte[] mData;
        protected readonly int mPadBits;

        public static DerBitString GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case DerBitString _:
                    return (DerBitString)obj;
                default:
                    throw new ArgumentException( "illegal object in GetInstance: " + Platform.GetTypeName( obj ) );
            }
        }

        public static DerBitString GetInstance( Asn1TaggedObject obj, bool isExplicit )
        {
            Asn1Object asn1Object = obj.GetObject();
            return isExplicit || asn1Object is DerBitString ? GetInstance( asn1Object ) : FromAsn1Octets( ((Asn1OctetString)asn1Object).GetOctets() );
        }

        public DerBitString( byte[] data, int padBits )
        {
            if (data == null)
                throw new ArgumentNullException( nameof( data ) );
            if (padBits < 0 || padBits > 7)
                throw new ArgumentException( "must be in the range 0 to 7", nameof( padBits ) );
            if (data.Length == 0 && padBits != 0)
                throw new ArgumentException( "if 'data' is empty, 'padBits' must be 0" );
            this.mData = Arrays.Clone( data );
            this.mPadBits = padBits;
        }

        public DerBitString( byte[] data )
          : this( data, 0 )
        {
        }

        public DerBitString( int namedBits )
        {
            if (namedBits == 0)
            {
                this.mData = new byte[0];
                this.mPadBits = 0;
            }
            else
            {
                int length = (BigInteger.BitLen( namedBits ) + 7) / 8;
                byte[] numArray = new byte[length];
                int index1 = length - 1;
                for (int index2 = 0; index2 < index1; ++index2)
                {
                    numArray[index2] = (byte)namedBits;
                    namedBits >>= 8;
                }
                numArray[index1] = (byte)namedBits;
                int num = 0;
                while ((namedBits & (1 << num)) == 0)
                    ++num;
                this.mData = numArray;
                this.mPadBits = num;
            }
        }

        public DerBitString( Asn1Encodable obj )
          : this( obj.GetDerEncoded() )
        {
        }

        public virtual byte[] GetOctets()
        {
            if (this.mPadBits != 0)
                throw new InvalidOperationException( "attempt to get non-octet aligned data from BIT STRING" );
            return Arrays.Clone( this.mData );
        }

        public virtual byte[] GetBytes()
        {
            byte[] bytes = Arrays.Clone( this.mData );
            if (this.mPadBits > 0)
            {
                byte[] numArray;
                IntPtr index;
                (numArray = bytes)[(int)(index = (IntPtr)(bytes.Length - 1))] = (byte)(numArray[(int)index] & (uint)(byte)(byte.MaxValue << this.mPadBits));
            }
            return bytes;
        }

        public virtual int PadBits => this.mPadBits;

        public virtual int IntValue
        {
            get
            {
                int intValue = 0;
                int num1 = System.Math.Min( 4, this.mData.Length );
                for (int index = 0; index < num1; ++index)
                    intValue |= this.mData[index] << (8 * index);
                if (this.mPadBits > 0 && num1 == this.mData.Length)
                {
                    int num2 = (1 << this.mPadBits) - 1;
                    intValue &= ~(num2 << (8 * (num1 - 1)));
                }
                return intValue;
            }
        }

        internal override void Encode( DerOutputStream derOut )
        {
            if (this.mPadBits > 0)
            {
                int num1 = this.mData[this.mData.Length - 1];
                int num2 = (1 << this.mPadBits) - 1;
                int num3 = num1 & num2;
                if (num3 != 0)
                {
                    byte[] bytes = Arrays.Prepend( this.mData, (byte)this.mPadBits );
                    bytes[bytes.Length - 1] = (byte)(num1 ^ num3);
                    derOut.WriteEncoded( 3, bytes );
                    return;
                }
            }
            derOut.WriteEncoded( 3, (byte)this.mPadBits, this.mData );
        }

        protected override int Asn1GetHashCode() => this.mPadBits.GetHashCode() ^ Arrays.GetHashCode( this.mData );

        protected override bool Asn1Equals( Asn1Object asn1Object ) => asn1Object is DerBitString derBitString && this.mPadBits == derBitString.mPadBits && Arrays.AreEqual( this.mData, derBitString.mData );

        public override string GetString()
        {
            StringBuilder stringBuilder = new StringBuilder( "#" );
            byte[] derEncoded = this.GetDerEncoded();
            for (int index = 0; index != derEncoded.Length; ++index)
            {
                uint num = derEncoded[index];
                stringBuilder.Append( table[(int)(IntPtr)((num >> 4) & 15U)] );
                stringBuilder.Append( table[derEncoded[index] & 15] );
            }
            return stringBuilder.ToString();
        }

        internal static DerBitString FromAsn1Octets( byte[] octets )
        {
            int padBits = octets.Length >= 1 ? octets[0] : throw new ArgumentException( "truncated BIT STRING detected", nameof( octets ) );
            byte[] data = Arrays.CopyOfRange( octets, 1, octets.Length );
            return padBits > 0 && padBits < 8 && data.Length > 0 && (data[data.Length - 1] & ((1 << padBits) - 1)) != 0 ? new BerBitString( data, padBits ) : new DerBitString( data, padBits );
        }
    }
}
