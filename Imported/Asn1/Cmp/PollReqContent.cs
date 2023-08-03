// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Cmp.PollReqContent
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Asn1.Cmp
{
    public class PollReqContent : Asn1Encodable
    {
        private readonly Asn1Sequence content;

        private PollReqContent( Asn1Sequence seq ) => this.content = seq;

        public static PollReqContent GetInstance( object obj )
        {
            switch (obj)
            {
                case PollReqContent _:
                    return (PollReqContent)obj;
                case Asn1Sequence _:
                    return new PollReqContent( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Invalid object: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public virtual DerInteger[][] GetCertReqIDs()
        {
            DerInteger[][] certReqIds = new DerInteger[this.content.Count][];
            for (int index = 0; index != certReqIds.Length; ++index)
                certReqIds[index] = SequenceToDerIntegerArray( (Asn1Sequence)this.content[index] );
            return certReqIds;
        }

        private static DerInteger[] SequenceToDerIntegerArray( Asn1Sequence seq )
        {
            DerInteger[] derIntegerArray = new DerInteger[seq.Count];
            for (int index = 0; index != derIntegerArray.Length; ++index)
                derIntegerArray[index] = DerInteger.GetInstance( seq[index] );
            return derIntegerArray;
        }

        public override Asn1Object ToAsn1Object() => content;
    }
}
