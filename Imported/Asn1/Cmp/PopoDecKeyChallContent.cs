// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Cmp.PopoDecKeyChallContent
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Asn1.Cmp
{
    public class PopoDecKeyChallContent : Asn1Encodable
    {
        private readonly Asn1Sequence content;

        private PopoDecKeyChallContent( Asn1Sequence seq ) => this.content = seq;

        public static PopoDecKeyChallContent GetInstance( object obj )
        {
            switch (obj)
            {
                case PopoDecKeyChallContent _:
                    return (PopoDecKeyChallContent)obj;
                case Asn1Sequence _:
                    return new PopoDecKeyChallContent( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "Invalid object: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public virtual Challenge[] ToChallengeArray()
        {
            Challenge[] challengeArray = new Challenge[this.content.Count];
            for (int index = 0; index != challengeArray.Length; ++index)
                challengeArray[index] = Challenge.GetInstance( this.content[index] );
            return challengeArray;
        }

        public override Asn1Object ToAsn1Object() => content;
    }
}
