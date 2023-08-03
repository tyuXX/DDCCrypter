// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Pkix.PkixCertPathChecker
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities.Collections;
using Org.BouncyCastle.X509;

namespace Org.BouncyCastle.Pkix
{
    public abstract class PkixCertPathChecker
    {
        public abstract void Init( bool forward );

        public abstract bool IsForwardCheckingSupported();

        public abstract ISet GetSupportedExtensions();

        public abstract void Check( X509Certificate cert, ISet unresolvedCritExts );

        public virtual object Clone() => this.MemberwiseClone();
    }
}
