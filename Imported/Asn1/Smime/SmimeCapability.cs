// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Smime.SmimeCapability
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.Pkcs;
using System;

namespace Org.BouncyCastle.Asn1.Smime
{
    public class SmimeCapability : Asn1Encodable
    {
        public static readonly DerObjectIdentifier PreferSignedData = PkcsObjectIdentifiers.PreferSignedData;
        public static readonly DerObjectIdentifier CannotDecryptAny = PkcsObjectIdentifiers.CannotDecryptAny;
        public static readonly DerObjectIdentifier SmimeCapabilitiesVersions = PkcsObjectIdentifiers.SmimeCapabilitiesVersions;
        public static readonly DerObjectIdentifier DesCbc = new DerObjectIdentifier( "1.3.14.3.2.7" );
        public static readonly DerObjectIdentifier DesEde3Cbc = PkcsObjectIdentifiers.DesEde3Cbc;
        public static readonly DerObjectIdentifier RC2Cbc = PkcsObjectIdentifiers.RC2Cbc;
        private DerObjectIdentifier capabilityID;
        private Asn1Object parameters;

        public SmimeCapability( Asn1Sequence seq )
        {
            this.capabilityID = (DerObjectIdentifier)seq[0].ToAsn1Object();
            if (seq.Count <= 1)
                return;
            this.parameters = seq[1].ToAsn1Object();
        }

        public SmimeCapability( DerObjectIdentifier capabilityID, Asn1Encodable parameters )
        {
            this.capabilityID = capabilityID != null ? capabilityID : throw new ArgumentNullException( nameof( capabilityID ) );
            if (parameters == null)
                return;
            this.parameters = parameters.ToAsn1Object();
        }

        public static SmimeCapability GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case SmimeCapability _:
                    return (SmimeCapability)obj;
                case Asn1Sequence _:
                    return new SmimeCapability( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Invalid SmimeCapability" );
            }
        }

        public DerObjectIdentifier CapabilityID => this.capabilityID;

        public Asn1Object Parameters => this.parameters;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new Asn1EncodableVector( new Asn1Encodable[1]
            {
         capabilityID
            } );
            if (this.parameters != null)
                v.Add( parameters );
            return new DerSequence( v );
        }
    }
}
