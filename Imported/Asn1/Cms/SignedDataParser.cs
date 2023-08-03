// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Cms.SignedDataParser
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System.IO;

namespace Org.BouncyCastle.Asn1.Cms
{
    public class SignedDataParser
    {
        private Asn1SequenceParser _seq;
        private DerInteger _version;
        private object _nextObject;
        private bool _certsCalled;
        private bool _crlsCalled;

        public static SignedDataParser GetInstance( object o )
        {
            switch (o)
            {
                case Asn1Sequence _:
                    return new SignedDataParser( ((Asn1Sequence)o).Parser );
                case Asn1SequenceParser _:
                    return new SignedDataParser( (Asn1SequenceParser)o );
                default:
                    throw new IOException( "unknown object encountered: " + Platform.GetTypeName( o ) );
            }
        }

        public SignedDataParser( Asn1SequenceParser seq )
        {
            this._seq = seq;
            this._version = (DerInteger)seq.ReadObject();
        }

        public DerInteger Version => this._version;

        public Asn1SetParser GetDigestAlgorithms() => (Asn1SetParser)this._seq.ReadObject();

        public ContentInfoParser GetEncapContentInfo() => new( (Asn1SequenceParser)this._seq.ReadObject() );

        public Asn1SetParser GetCertificates()
        {
            this._certsCalled = true;
            this._nextObject = this._seq.ReadObject();
            if (!(this._nextObject is Asn1TaggedObjectParser) || ((Asn1TaggedObjectParser)this._nextObject).TagNo != 0)
                return null;
            Asn1SetParser objectParser = (Asn1SetParser)((Asn1TaggedObjectParser)this._nextObject).GetObjectParser( 17, false );
            this._nextObject = null;
            return objectParser;
        }

        public Asn1SetParser GetCrls()
        {
            if (!this._certsCalled)
                throw new IOException( "GetCerts() has not been called." );
            this._crlsCalled = true;
            if (this._nextObject == null)
                this._nextObject = this._seq.ReadObject();
            if (!(this._nextObject is Asn1TaggedObjectParser) || ((Asn1TaggedObjectParser)this._nextObject).TagNo != 1)
                return null;
            Asn1SetParser objectParser = (Asn1SetParser)((Asn1TaggedObjectParser)this._nextObject).GetObjectParser( 17, false );
            this._nextObject = null;
            return objectParser;
        }

        public Asn1SetParser GetSignerInfos()
        {
            if (!this._certsCalled || !this._crlsCalled)
                throw new IOException( "GetCerts() and/or GetCrls() has not been called." );
            if (this._nextObject == null)
                this._nextObject = this._seq.ReadObject();
            return (Asn1SetParser)this._nextObject;
        }
    }
}
