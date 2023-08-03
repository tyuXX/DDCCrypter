// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.LimitedInputStream
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities.IO;
using System.IO;

namespace Org.BouncyCastle.Asn1
{
    internal abstract class LimitedInputStream : BaseInputStream
    {
        protected readonly Stream _in;
        private int _limit;

        internal LimitedInputStream( Stream inStream, int limit )
        {
            this._in = inStream;
            this._limit = limit;
        }

        internal virtual int GetRemaining() => this._limit;

        protected virtual void SetParentEofDetect( bool on )
        {
            if (!(this._in is IndefiniteLengthInputStream))
                return;
            ((IndefiniteLengthInputStream)this._in).SetEofOn00( on );
        }
    }
}
