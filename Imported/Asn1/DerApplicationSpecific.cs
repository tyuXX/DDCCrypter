// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.DerApplicationSpecific
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System.IO;

namespace Org.BouncyCastle.Asn1
{
    public class DerApplicationSpecific : Asn1Object
    {
        private readonly bool isConstructed;
        private readonly int tag;
        private readonly byte[] octets;

        internal DerApplicationSpecific( bool isConstructed, int tag, byte[] octets )
        {
            this.isConstructed = isConstructed;
            this.tag = tag;
            this.octets = octets;
        }

        public DerApplicationSpecific( int tag, byte[] octets )
          : this( false, tag, octets )
        {
        }

        public DerApplicationSpecific( int tag, Asn1Encodable obj )
          : this( true, tag, obj )
        {
        }

        public DerApplicationSpecific( bool isExplicit, int tag, Asn1Encodable obj )
        {
            Asn1Object asn1Object = obj.ToAsn1Object();
            byte[] derEncoded = asn1Object.GetDerEncoded();
            this.isConstructed = Asn1TaggedObject.IsConstructed( isExplicit, asn1Object );
            this.tag = tag;
            if (isExplicit)
            {
                this.octets = derEncoded;
            }
            else
            {
                int lengthOfHeader = this.GetLengthOfHeader( derEncoded );
                byte[] destinationArray = new byte[derEncoded.Length - lengthOfHeader];
                Array.Copy( derEncoded, lengthOfHeader, destinationArray, 0, destinationArray.Length );
                this.octets = destinationArray;
            }
        }

        public DerApplicationSpecific( int tagNo, Asn1EncodableVector vec )
        {
            this.tag = tagNo;
            this.isConstructed = true;
            MemoryStream memoryStream = new();
            for (int index = 0; index != vec.Count; ++index)
            {
                try
                {
                    byte[] derEncoded = vec[index].GetDerEncoded();
                    memoryStream.Write( derEncoded, 0, derEncoded.Length );
                }
                catch (IOException ex)
                {
                    throw new InvalidOperationException( "malformed object", ex );
                }
            }
            this.octets = memoryStream.ToArray();
        }

        private int GetLengthOfHeader( byte[] data )
        {
            int num1 = data[1];
            if (num1 == 128 || num1 <= sbyte.MaxValue)
                return 2;
            int num2 = num1 & sbyte.MaxValue;
            if (num2 > 4)
                throw new InvalidOperationException( "DER length more than 4 bytes: " + num2 );
            return num2 + 2;
        }

        public bool IsConstructed() => this.isConstructed;

        public byte[] GetContents() => this.octets;

        public int ApplicationTag => this.tag;

        public Asn1Object GetObject() => FromByteArray( this.GetContents() );

        public Asn1Object GetObject( int derTagNo )
        {
            if (derTagNo >= 31)
                throw new IOException( "unsupported tag number" );
            byte[] encoded = this.GetEncoded();
            byte[] data = this.ReplaceTagNumber( derTagNo, encoded );
            if ((encoded[0] & 32) != 0)
            {
                byte[] numArray;
                (numArray = data)[0] = (byte)(numArray[0] | 32U);
            }
            return FromByteArray( data );
        }

        internal override void Encode( DerOutputStream derOut )
        {
            int flags = 64;
            if (this.isConstructed)
                flags |= 32;
            derOut.WriteEncoded( flags, this.tag, this.octets );
        }

        protected override bool Asn1Equals( Asn1Object asn1Object ) => asn1Object is DerApplicationSpecific applicationSpecific && this.isConstructed == applicationSpecific.isConstructed && this.tag == applicationSpecific.tag && Arrays.AreEqual( this.octets, applicationSpecific.octets );

        protected override int Asn1GetHashCode() => this.isConstructed.GetHashCode() ^ this.tag.GetHashCode() ^ Arrays.GetHashCode( this.octets );

        private byte[] ReplaceTagNumber( int newTag, byte[] input )
        {
            int num1 = input[0] & 31;
            int sourceIndex = 1;
            if (num1 == 31)
            {
                int num2 = 0;
                int num3 = input[sourceIndex++] & byte.MaxValue;
                if ((num3 & sbyte.MaxValue) == 0)
                    throw new InvalidOperationException( "corrupted stream - invalid high tag number found" );
                for (; num3 >= 0 && (num3 & 128) != 0; num3 = input[sourceIndex++] & byte.MaxValue)
                    num2 = (num2 | (num3 & sbyte.MaxValue)) << 7;
                int num4 = num2 | (num3 & sbyte.MaxValue);
            }
            byte[] destinationArray = new byte[input.Length - sourceIndex + 1];
            Array.Copy( input, sourceIndex, destinationArray, 1, destinationArray.Length - 1 );
            destinationArray[0] = (byte)newTag;
            return destinationArray;
        }
    }
}
