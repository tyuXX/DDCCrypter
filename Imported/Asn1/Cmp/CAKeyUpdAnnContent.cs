// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Cmp.CAKeyUpdAnnContent
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Asn1.Cmp
{
    public class CAKeyUpdAnnContent : Asn1Encodable
    {
        private readonly CmpCertificate oldWithNew;
        private readonly CmpCertificate newWithOld;
        private readonly CmpCertificate newWithNew;

        private CAKeyUpdAnnContent( Asn1Sequence seq )
        {
            this.oldWithNew = CmpCertificate.GetInstance( seq[0] );
            this.newWithOld = CmpCertificate.GetInstance( seq[1] );
            this.newWithNew = CmpCertificate.GetInstance( seq[2] );
        }

        public static CAKeyUpdAnnContent GetInstance( object obj )
        {
            switch (obj)
            {
                case CAKeyUpdAnnContent _:
                    return (CAKeyUpdAnnContent)obj;
                case Asn1Sequence _:
                    return new CAKeyUpdAnnContent( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Invalid object: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public virtual CmpCertificate OldWithNew => this.oldWithNew;

        public virtual CmpCertificate NewWithOld => this.newWithOld;

        public virtual CmpCertificate NewWithNew => this.newWithNew;

        public override Asn1Object ToAsn1Object() => new DerSequence( new Asn1Encodable[3]
        {
       oldWithNew,
       newWithOld,
       newWithNew
        } );
    }
}
