// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Asn1InputStream
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities.IO;
using System;
using System.Collections;
using System.IO;

namespace Org.BouncyCastle.Asn1
{
    public class Asn1InputStream : FilterStream
    {
        private readonly int limit;
        private readonly byte[][] tmpBuffers;

        internal static int FindLimit( Stream input )
        {
            switch (input)
            {
                case LimitedInputStream _:
                    return ((LimitedInputStream)input).GetRemaining();
                case MemoryStream _:
                    MemoryStream memoryStream = (MemoryStream)input;
                    return (int)(memoryStream.Length - memoryStream.Position);
                default:
                    return int.MaxValue;
            }
        }

        public Asn1InputStream( Stream inputStream )
          : this( inputStream, FindLimit( inputStream ) )
        {
        }

        public Asn1InputStream( Stream inputStream, int limit )
          : base( inputStream )
        {
            this.limit = limit;
            this.tmpBuffers = new byte[16][];
        }

        public Asn1InputStream( byte[] input )
          : this( new MemoryStream( input, false ), input.Length )
        {
        }

        private Asn1Object BuildObject( int tag, int tagNo, int length )
        {
            bool flag = (tag & 32) != 0;
            DefiniteLengthInputStream lengthInputStream = new DefiniteLengthInputStream( this.s, length );
            if ((tag & 64) != 0)
                return new DerApplicationSpecific( flag, tagNo, lengthInputStream.ToArray() );
            if ((tag & 128) != 0)
                return new Asn1StreamParser( lengthInputStream ).ReadTaggedObject( flag, tagNo );
            if (!flag)
                return CreatePrimitiveDerObject( tagNo, lengthInputStream, this.tmpBuffers );
            switch (tagNo)
            {
                case 4:
                    return new BerOctetString( this.BuildDerEncodableVector( lengthInputStream ) );
                case 8:
                    return new DerExternal( this.BuildDerEncodableVector( lengthInputStream ) );
                case 16:
                    return this.CreateDerSequence( lengthInputStream );
                case 17:
                    return this.CreateDerSet( lengthInputStream );
                default:
                    throw new IOException( "unknown tag " + tagNo + " encountered" );
            }
        }

        internal Asn1EncodableVector BuildEncodableVector()
        {
            Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector( new Asn1Encodable[0] );
            Asn1Object asn1Object;
            while ((asn1Object = this.ReadObject()) != null)
                asn1EncodableVector.Add( asn1Object );
            return asn1EncodableVector;
        }

        internal virtual Asn1EncodableVector BuildDerEncodableVector( DefiniteLengthInputStream dIn ) => new Asn1InputStream( dIn ).BuildEncodableVector();

        internal virtual DerSequence CreateDerSequence( DefiniteLengthInputStream dIn ) => DerSequence.FromVector( this.BuildDerEncodableVector( dIn ) );

        internal virtual DerSet CreateDerSet( DefiniteLengthInputStream dIn ) => DerSet.FromVector( this.BuildDerEncodableVector( dIn ), false );

        public Asn1Object ReadObject()
        {
            int tag = this.ReadByte();
            if (tag <= 0)
            {
                if (tag == 0)
                    throw new IOException( "unexpected end-of-contents marker" );
                return null;
            }
            int num = ReadTagNumber( this.s, tag );
            bool flag = (tag & 32) != 0;
            int length = ReadLength( this.s, this.limit );
            if (length < 0)
            {
                if (!flag)
                    throw new IOException( "indefinite length primitive encoding encountered" );
                Asn1StreamParser parser = new Asn1StreamParser( new IndefiniteLengthInputStream( this.s, this.limit ), this.limit );
                if ((tag & 64) != 0)
                    return new BerApplicationSpecificParser( num, parser ).ToAsn1Object();
                if ((tag & 128) != 0)
                    return new BerTaggedObjectParser( true, num, parser ).ToAsn1Object();
                switch (num)
                {
                    case 4:
                        return new BerOctetStringParser( parser ).ToAsn1Object();
                    case 8:
                        return new DerExternalParser( parser ).ToAsn1Object();
                    case 16:
                        return new BerSequenceParser( parser ).ToAsn1Object();
                    case 17:
                        return new BerSetParser( parser ).ToAsn1Object();
                    default:
                        throw new IOException( "unknown BER object encountered" );
                }
            }
            else
            {
                try
                {
                    return this.BuildObject( tag, num, length );
                }
                catch (ArgumentException ex)
                {
                    throw new Asn1Exception( "corrupted stream detected", ex );
                }
            }
        }

        internal static int ReadTagNumber( Stream s, int tag )
        {
            int num1 = tag & 31;
            if (num1 == 31)
            {
                int num2 = 0;
                int num3 = s.ReadByte();
                if ((num3 & sbyte.MaxValue) == 0)
                    throw new IOException( "Corrupted stream - invalid high tag number found" );
                for (; num3 >= 0 && (num3 & 128) != 0; num3 = s.ReadByte())
                    num2 = (num2 | (num3 & sbyte.MaxValue)) << 7;
                if (num3 < 0)
                    throw new EndOfStreamException( "EOF found inside tag value." );
                num1 = num2 | (num3 & sbyte.MaxValue);
            }
            return num1;
        }

        internal static int ReadLength( Stream s, int limit )
        {
            int num1 = s.ReadByte();
            if (num1 < 0)
                throw new EndOfStreamException( "EOF found when length expected" );
            if (num1 == 128)
                return -1;
            if (num1 > sbyte.MaxValue)
            {
                int num2 = num1 & sbyte.MaxValue;
                if (num2 > 4)
                    throw new IOException( "DER length more than 4 bytes: " + num2 );
                num1 = 0;
                for (int index = 0; index < num2; ++index)
                {
                    int num3 = s.ReadByte();
                    if (num3 < 0)
                        throw new EndOfStreamException( "EOF found reading length" );
                    num1 = (num1 << 8) + num3;
                }
                if (num1 < 0)
                    throw new IOException( "Corrupted stream - negative length found" );
                if (num1 >= limit)
                    throw new IOException( "Corrupted stream - out of bounds length found" );
            }
            return num1;
        }

        internal static byte[] GetBuffer( DefiniteLengthInputStream defIn, byte[][] tmpBuffers )
        {
            int remaining = defIn.GetRemaining();
            if (remaining >= tmpBuffers.Length)
                return defIn.ToArray();
            byte[] buf = tmpBuffers[remaining] ?? (tmpBuffers[remaining] = new byte[remaining]);
            defIn.ReadAllIntoByteArray( buf );
            return buf;
        }

        internal static Asn1Object CreatePrimitiveDerObject(
          int tagNo,
          DefiniteLengthInputStream defIn,
          byte[][] tmpBuffers )
        {
            switch (tagNo)
            {
                case 1:
                    return DerBoolean.FromOctetString( GetBuffer( defIn, tmpBuffers ) );
                case 6:
                    return DerObjectIdentifier.FromOctetString( GetBuffer( defIn, tmpBuffers ) );
                case 10:
                    return DerEnumerated.FromOctetString( GetBuffer( defIn, tmpBuffers ) );
                default:
                    byte[] array = defIn.ToArray();
                    switch (tagNo)
                    {
                        case 2:
                            return new DerInteger( array );
                        case 3:
                            return DerBitString.FromAsn1Octets( array );
                        case 4:
                            return new DerOctetString( array );
                        case 5:
                            return DerNull.Instance;
                        case 12:
                            return new DerUtf8String( array );
                        case 18:
                            return new DerNumericString( array );
                        case 19:
                            return new DerPrintableString( array );
                        case 20:
                            return new DerT61String( array );
                        case 21:
                            return new DerVideotexString( array );
                        case 22:
                            return new DerIA5String( array );
                        case 23:
                            return new DerUtcTime( array );
                        case 24:
                            return new DerGeneralizedTime( array );
                        case 25:
                            return new DerGraphicString( array );
                        case 26:
                            return new DerVisibleString( array );
                        case 27:
                            return new DerGeneralString( array );
                        case 28:
                            return new DerUniversalString( array );
                        case 30:
                            return new DerBmpString( array );
                        default:
                            throw new IOException( "unknown tag " + tagNo + " encountered" );
                    }
            }
        }
    }
}
