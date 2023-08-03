// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.UseSrtpData
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Crypto.Tls
{
    public class UseSrtpData
    {
        protected readonly int[] mProtectionProfiles;
        protected readonly byte[] mMki;

        public UseSrtpData( int[] protectionProfiles, byte[] mki )
        {
            if (protectionProfiles == null || protectionProfiles.Length < 1 || protectionProfiles.Length >= 32768)
                throw new ArgumentException( "must have length from 1 to (2^15 - 1)", nameof( protectionProfiles ) );
            if (mki == null)
                mki = TlsUtilities.EmptyBytes;
            else if (mki.Length > byte.MaxValue)
                throw new ArgumentException( "cannot be longer than 255 bytes", nameof( mki ) );
            this.mProtectionProfiles = protectionProfiles;
            this.mMki = mki;
        }

        public virtual int[] ProtectionProfiles => this.mProtectionProfiles;

        public virtual byte[] Mki => this.mMki;
    }
}
