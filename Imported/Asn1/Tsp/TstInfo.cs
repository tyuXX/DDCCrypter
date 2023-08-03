// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Tsp.TstInfo
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Utilities;
using System.Collections;
using System.IO;

namespace Org.BouncyCastle.Asn1.Tsp
{
    public class TstInfo : Asn1Encodable
    {
        private readonly DerInteger version;
        private readonly DerObjectIdentifier tsaPolicyId;
        private readonly MessageImprint messageImprint;
        private readonly DerInteger serialNumber;
        private readonly DerGeneralizedTime genTime;
        private readonly Accuracy accuracy;
        private readonly DerBoolean ordering;
        private readonly DerInteger nonce;
        private readonly GeneralName tsa;
        private readonly X509Extensions extensions;

        public static TstInfo GetInstance( object o )
        {
            switch (o)
            {
                case null:
                case TstInfo _:
                    return (TstInfo)o;
                case Asn1Sequence _:
                    return new TstInfo( (Asn1Sequence)o );
                case Asn1OctetString _:
                    try
                    {
                        return GetInstance( Asn1Object.FromByteArray( ((Asn1OctetString)o).GetOctets() ) );
                    }
                    catch (IOException ex)
                    {
                        throw new ArgumentException( "Bad object format in 'TstInfo' factory." );
                    }
                default:
                    throw new ArgumentException( "Unknown object in 'TstInfo' factory: " + Platform.GetTypeName( o ) );
            }
        }

        private TstInfo( Asn1Sequence seq )
        {
            IEnumerator enumerator = seq.GetEnumerator();
            enumerator.MoveNext();
            this.version = DerInteger.GetInstance( enumerator.Current );
            enumerator.MoveNext();
            this.tsaPolicyId = DerObjectIdentifier.GetInstance( enumerator.Current );
            enumerator.MoveNext();
            this.messageImprint = MessageImprint.GetInstance( enumerator.Current );
            enumerator.MoveNext();
            this.serialNumber = DerInteger.GetInstance( enumerator.Current );
            enumerator.MoveNext();
            this.genTime = DerGeneralizedTime.GetInstance( enumerator.Current );
            this.ordering = DerBoolean.False;
            while (enumerator.MoveNext())
            {
                Asn1Object current = (Asn1Object)enumerator.Current;
                if (current is Asn1TaggedObject)
                {
                    DerTaggedObject tagObj = (DerTaggedObject)current;
                    switch (tagObj.TagNo)
                    {
                        case 0:
                            this.tsa = GeneralName.GetInstance( tagObj, true );
                            break;
                        case 1:
                            this.extensions = X509Extensions.GetInstance( tagObj, false );
                            break;
                        default:
                            throw new ArgumentException( "Unknown tag value " + tagObj.TagNo );
                    }
                }
                if (current is DerSequence)
                    this.accuracy = Accuracy.GetInstance( current );
                if (current is DerBoolean)
                    this.ordering = DerBoolean.GetInstance( current );
                if (current is DerInteger)
                    this.nonce = DerInteger.GetInstance( current );
            }
        }

        public TstInfo(
          DerObjectIdentifier tsaPolicyId,
          MessageImprint messageImprint,
          DerInteger serialNumber,
          DerGeneralizedTime genTime,
          Accuracy accuracy,
          DerBoolean ordering,
          DerInteger nonce,
          GeneralName tsa,
          X509Extensions extensions )
        {
            this.version = new DerInteger( 1 );
            this.tsaPolicyId = tsaPolicyId;
            this.messageImprint = messageImprint;
            this.serialNumber = serialNumber;
            this.genTime = genTime;
            this.accuracy = accuracy;
            this.ordering = ordering;
            this.nonce = nonce;
            this.tsa = tsa;
            this.extensions = extensions;
        }

        public DerInteger Version => this.version;

        public MessageImprint MessageImprint => this.messageImprint;

        public DerObjectIdentifier Policy => this.tsaPolicyId;

        public DerInteger SerialNumber => this.serialNumber;

        public Accuracy Accuracy => this.accuracy;

        public DerGeneralizedTime GenTime => this.genTime;

        public DerBoolean Ordering => this.ordering;

        public DerInteger Nonce => this.nonce;

        public GeneralName Tsa => this.tsa;

        public X509Extensions Extensions => this.extensions;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new( new Asn1Encodable[5]
            {
         version,
         tsaPolicyId,
         messageImprint,
         serialNumber,
         genTime
            } );
            if (this.accuracy != null)
                v.Add( accuracy );
            if (this.ordering != null && this.ordering.IsTrue)
                v.Add( ordering );
            if (this.nonce != null)
                v.Add( nonce );
            if (this.tsa != null)
                v.Add( new DerTaggedObject( true, 0, tsa ) );
            if (this.extensions != null)
                v.Add( new DerTaggedObject( false, 1, extensions ) );
            return new DerSequence( v );
        }
    }
}
