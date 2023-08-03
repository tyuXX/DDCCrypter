﻿// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Cms.RecipientKeyIdentifier
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.Cms
{
    public class RecipientKeyIdentifier : Asn1Encodable
    {
        private Asn1OctetString subjectKeyIdentifier;
        private DerGeneralizedTime date;
        private OtherKeyAttribute other;

        public RecipientKeyIdentifier(
          Asn1OctetString subjectKeyIdentifier,
          DerGeneralizedTime date,
          OtherKeyAttribute other )
        {
            this.subjectKeyIdentifier = subjectKeyIdentifier;
            this.date = date;
            this.other = other;
        }

        public RecipientKeyIdentifier( byte[] subjectKeyIdentifier )
          : this( subjectKeyIdentifier, null, null )
        {
        }

        public RecipientKeyIdentifier(
          byte[] subjectKeyIdentifier,
          DerGeneralizedTime date,
          OtherKeyAttribute other )
        {
            this.subjectKeyIdentifier = new DerOctetString( subjectKeyIdentifier );
            this.date = date;
            this.other = other;
        }

        public RecipientKeyIdentifier( Asn1Sequence seq )
        {
            this.subjectKeyIdentifier = Asn1OctetString.GetInstance( seq[0] );
            switch (seq.Count)
            {
                case 1:
                    break;
                case 2:
                    if (seq[1] is DerGeneralizedTime)
                    {
                        this.date = (DerGeneralizedTime)seq[1];
                        break;
                    }
                    this.other = OtherKeyAttribute.GetInstance( seq[2] );
                    break;
                case 3:
                    this.date = (DerGeneralizedTime)seq[1];
                    this.other = OtherKeyAttribute.GetInstance( seq[2] );
                    break;
                default:
                    throw new ArgumentException( "Invalid RecipientKeyIdentifier" );
            }
        }

        public static RecipientKeyIdentifier GetInstance( Asn1TaggedObject ato, bool explicitly ) => GetInstance( Asn1Sequence.GetInstance( ato, explicitly ) );

        public static RecipientKeyIdentifier GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case RecipientKeyIdentifier _:
                    return (RecipientKeyIdentifier)obj;
                case Asn1Sequence _:
                    return new RecipientKeyIdentifier( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Invalid RecipientKeyIdentifier: " + Platform.GetTypeName( obj ) );
            }
        }

        public Asn1OctetString SubjectKeyIdentifier => this.subjectKeyIdentifier;

        public DerGeneralizedTime Date => this.date;

        public OtherKeyAttribute OtherKeyAttribute => this.other;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new( new Asn1Encodable[1]
            {
         subjectKeyIdentifier
            } );
            v.AddOptional( date, other );
            return new DerSequence( v );
        }
    }
}
