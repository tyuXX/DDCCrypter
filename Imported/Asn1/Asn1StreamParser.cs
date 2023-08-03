// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Asn1StreamParser
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System.IO;

namespace Org.BouncyCastle.Asn1
{
    public class Asn1StreamParser
    {
        private readonly Stream _in;
        private readonly int _limit;
        private readonly byte[][] tmpBuffers;

        public Asn1StreamParser( Stream inStream )
          : this( inStream, Asn1InputStream.FindLimit( inStream ) )
        {
        }

        public Asn1StreamParser( Stream inStream, int limit )
        {
            this._in = inStream.CanRead ? inStream : throw new ArgumentException( "Expected stream to be readable", nameof( inStream ) );
            this._limit = limit;
            this.tmpBuffers = new byte[16][];
        }

        public Asn1StreamParser( byte[] encoding )
          : this( new MemoryStream( encoding, false ), encoding.Length )
        {
        }

        internal IAsn1Convertible ReadIndef( int tagValue )
        {
            switch (tagValue)
            {
                case 4:
                    return new BerOctetStringParser( this );
                case 8:
                    return new DerExternalParser( this );
                case 16:
                    return new BerSequenceParser( this );
                case 17:
                    return new BerSetParser( this );
                default:
                    throw new Asn1Exception( "unknown BER object encountered: 0x" + tagValue.ToString( "X" ) );
            }
        }

        internal IAsn1Convertible ReadImplicit( bool constructed, int tag )
        {
            if (this._in is IndefiniteLengthInputStream)
            {
                if (!constructed)
                    throw new IOException( "indefinite length primitive encoding encountered" );
                return this.ReadIndef( tag );
            }
            if (constructed)
            {
                switch (tag)
                {
                    case 4:
                        return new BerOctetStringParser( this );
                    case 16:
                        return new DerSequenceParser( this );
                    case 17:
                        return new DerSetParser( this );
                }
            }
            else
            {
                switch (tag)
                {
                    case 4:
                        return new DerOctetStringParser( (DefiniteLengthInputStream)this._in );
                    case 16:
                        throw new Asn1Exception( "sets must use constructed encoding (see X.690 8.11.1/8.12.1)" );
                    case 17:
                        throw new Asn1Exception( "sequences must use constructed encoding (see X.690 8.9.1/8.10.1)" );
                }
            }
            throw new Asn1Exception( "implicit tagging not implemented" );
        }

        internal Asn1Object ReadTaggedObject( bool constructed, int tag )
        {
            if (!constructed)
            {
                DefiniteLengthInputStream lengthInputStream = (DefiniteLengthInputStream)this._in;
                return new DerTaggedObject( false, tag, new DerOctetString( lengthInputStream.ToArray() ) );
            }
            Asn1EncodableVector v = this.ReadVector();
            return this._in is IndefiniteLengthInputStream ? (v.Count != 1 ? new BerTaggedObject( false, tag, BerSequence.FromVector( v ) ) : (Asn1Object)new BerTaggedObject( true, tag, v[0] )) : (v.Count != 1 ? new DerTaggedObject( false, tag, DerSequence.FromVector( v ) ) : (Asn1Object)new DerTaggedObject( true, tag, v[0] ));
        }

        public virtual IAsn1Convertible ReadObject()
        {
            int tag = this._in.ReadByte();
            if (tag == -1)
                return null;
            this.Set00Check( false );
            int num = Asn1InputStream.ReadTagNumber( this._in, tag );
            bool flag = (tag & 32) != 0;
            int length = Asn1InputStream.ReadLength( this._in, this._limit );
            if (length < 0)
            {
                if (!flag)
                    throw new IOException( "indefinite length primitive encoding encountered" );
                Asn1StreamParser parser = new( new IndefiniteLengthInputStream( this._in, this._limit ), this._limit );
                if ((tag & 64) != 0)
                    return new BerApplicationSpecificParser( num, parser );
                return (tag & 128) != 0 ? new BerTaggedObjectParser( true, num, parser ) : parser.ReadIndef( num );
            }
            DefiniteLengthInputStream lengthInputStream = new( this._in, length );
            if ((tag & 64) != 0)
                return new DerApplicationSpecific( flag, num, lengthInputStream.ToArray() );
            if ((tag & 128) != 0)
                return new BerTaggedObjectParser( flag, num, new Asn1StreamParser( lengthInputStream ) );
            if (flag)
            {
                switch (num)
                {
                    case 4:
                        return new BerOctetStringParser( new Asn1StreamParser( lengthInputStream ) );
                    case 8:
                        return new DerExternalParser( new Asn1StreamParser( lengthInputStream ) );
                    case 16:
                        return new DerSequenceParser( new Asn1StreamParser( lengthInputStream ) );
                    case 17:
                        return new DerSetParser( new Asn1StreamParser( lengthInputStream ) );
                    default:
                        throw new IOException( "unknown tag " + num + " encountered" );
                }
            }
            else
            {
                if (num == 4)
                    return new DerOctetStringParser( lengthInputStream );
                try
                {
                    return Asn1InputStream.CreatePrimitiveDerObject( num, lengthInputStream, this.tmpBuffers );
                }
                catch (ArgumentException ex)
                {
                    throw new Asn1Exception( "corrupted stream detected", ex );
                }
            }
        }

        private void Set00Check( bool enabled )
        {
            if (!(this._in is IndefiniteLengthInputStream))
                return;
            ((IndefiniteLengthInputStream)this._in).SetEofOn00( enabled );
        }

        internal Asn1EncodableVector ReadVector()
        {
            Asn1EncodableVector asn1EncodableVector = new( new Asn1Encodable[0] );
            IAsn1Convertible asn1Convertible;
            while ((asn1Convertible = this.ReadObject()) != null)
                asn1EncodableVector.Add( asn1Convertible.ToAsn1Object() );
            return asn1EncodableVector;
        }
    }
}
