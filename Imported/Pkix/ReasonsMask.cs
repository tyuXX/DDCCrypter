// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Pkix.ReasonsMask
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X509;

namespace Org.BouncyCastle.Pkix
{
    internal class ReasonsMask
    {
        private int _reasons;
        internal static readonly ReasonsMask AllReasons = new ReasonsMask( 33023 );

        internal ReasonsMask( int reasons ) => this._reasons = reasons;

        internal ReasonsMask()
          : this( 0 )
        {
        }

        internal void AddReasons( ReasonsMask mask ) => this._reasons |= mask.Reasons.IntValue;

        internal bool IsAllReasons => this._reasons == AllReasons._reasons;

        internal ReasonsMask Intersect( ReasonsMask mask )
        {
            ReasonsMask reasonsMask = new ReasonsMask();
            reasonsMask.AddReasons( new ReasonsMask( this._reasons & mask.Reasons.IntValue ) );
            return reasonsMask;
        }

        internal bool HasNewReasons( ReasonsMask mask ) => (this._reasons | (mask.Reasons.IntValue ^ this._reasons)) != 0;

        public ReasonFlags Reasons => new ReasonFlags( this._reasons );
    }
}
