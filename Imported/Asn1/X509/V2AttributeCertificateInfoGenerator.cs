﻿// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X509.V2AttributeCertificateInfoGenerator
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System;

namespace Org.BouncyCastle.Asn1.X509
{
    public class V2AttributeCertificateInfoGenerator
    {
        internal DerInteger version;
        internal Holder holder;
        internal AttCertIssuer issuer;
        internal AlgorithmIdentifier signature;
        internal DerInteger serialNumber;
        internal Asn1EncodableVector attributes;
        internal DerBitString issuerUniqueID;
        internal X509Extensions extensions;
        internal DerGeneralizedTime startDate;
        internal DerGeneralizedTime endDate;

        public V2AttributeCertificateInfoGenerator()
        {
            this.version = new DerInteger( 1 );
            this.attributes = new Asn1EncodableVector( new Asn1Encodable[0] );
        }

        public void SetHolder( Holder holder ) => this.holder = holder;

        public void AddAttribute( string oid, Asn1Encodable value ) => this.attributes.Add( new AttributeX509( new DerObjectIdentifier( oid ), new DerSet( value ) ) );

        public void AddAttribute( AttributeX509 attribute ) => this.attributes.Add( attribute );

        public void SetSerialNumber( DerInteger serialNumber ) => this.serialNumber = serialNumber;

        public void SetSignature( AlgorithmIdentifier signature ) => this.signature = signature;

        public void SetIssuer( AttCertIssuer issuer ) => this.issuer = issuer;

        public void SetStartDate( DerGeneralizedTime startDate ) => this.startDate = startDate;

        public void SetEndDate( DerGeneralizedTime endDate ) => this.endDate = endDate;

        public void SetIssuerUniqueID( DerBitString issuerUniqueID ) => this.issuerUniqueID = issuerUniqueID;

        public void SetExtensions( X509Extensions extensions ) => this.extensions = extensions;

        public AttributeCertificateInfo GenerateAttributeCertificateInfo()
        {
            if (this.serialNumber == null || this.signature == null || this.issuer == null || this.startDate == null || this.endDate == null || this.holder == null || this.attributes == null)
                throw new InvalidOperationException( "not all mandatory fields set in V2 AttributeCertificateInfo generator" );
            Asn1EncodableVector v = new Asn1EncodableVector( new Asn1Encodable[5]
            {
         version,
         holder,
         issuer,
         signature,
         serialNumber
            } )
            {
                new AttCertValidityPeriod( this.startDate, this.endDate ),
                new DerSequence( this.attributes )
            };
            if (this.issuerUniqueID != null)
                v.Add( issuerUniqueID );
            if (this.extensions != null)
                v.Add( extensions );
            return AttributeCertificateInfo.GetInstance( new DerSequence( v ) );
        }
    }
}
