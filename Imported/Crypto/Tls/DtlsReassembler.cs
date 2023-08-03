// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.DtlsReassembler
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System.Collections;

namespace Org.BouncyCastle.Crypto.Tls
{
    internal class DtlsReassembler
    {
        private readonly byte mMsgType;
        private readonly byte[] mBody;
        private readonly IList mMissing = Platform.CreateArrayList();

        internal DtlsReassembler( byte msg_type, int length )
        {
            this.mMsgType = msg_type;
            this.mBody = new byte[length];
            this.mMissing.Add( new DtlsReassembler.Range( 0, length ) );
        }
        internal DtlsReassembler() { }

        internal byte MsgType => this.mMsgType;

        internal byte[] GetBodyIfComplete() => this.mMissing.Count != 0 ? null : this.mBody;

        internal void ContributeFragment(
          byte msg_type,
          int length,
          byte[] buf,
          int off,
          int fragment_offset,
          int fragment_length )
        {
            int val2 = fragment_offset + fragment_length;
            if (mMsgType != msg_type || this.mBody.Length != length || val2 > length)
                return;
            if (fragment_length == 0)
            {
                if (fragment_offset != 0 || this.mMissing.Count <= 0 || ((DtlsReassembler.Range)this.mMissing[0]).End != 0)
                    return;
                this.mMissing.RemoveAt( 0 );
            }
            else
            {
                for (int index = 0; index < this.mMissing.Count; ++index)
                {
                    DtlsReassembler.Range range = (DtlsReassembler.Range)this.mMissing[index];
                    if (range.Start >= val2)
                        break;
                    if (range.End > fragment_offset)
                    {
                        int destinationIndex = System.Math.Max( range.Start, fragment_offset );
                        int start = System.Math.Min( range.End, val2 );
                        int length1 = start - destinationIndex;
                        Array.Copy( buf, off + destinationIndex - fragment_offset, mBody, destinationIndex, length1 );
                        if (destinationIndex == range.Start)
                        {
                            if (start == range.End)
                                this.mMissing.RemoveAt( index-- );
                            else
                                range.Start = start;
                        }
                        else
                        {
                            if (start != range.End)
                                this.mMissing.Insert( ++index, new DtlsReassembler.Range( start, range.End ) );
                            range.End = destinationIndex;
                        }
                    }
                }
            }
        }

        internal void Reset()
        {
            this.mMissing.Clear();
            this.mMissing.Add( new DtlsReassembler.Range( 0, this.mBody.Length ) );
        }

        private class Range
        {
            private int mStart;
            private int mEnd;

            internal Range( int start, int end )
            {
                this.mStart = start;
                this.mEnd = end;
            }

            public int Start
            {
                get => this.mStart;
                set => this.mStart = value;
            }

            public int End
            {
                get => this.mEnd;
                set => this.mEnd = value;
            }
        }
    }
}
