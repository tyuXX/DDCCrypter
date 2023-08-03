// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X9.X9ECParametersHolder
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Asn1.X9
{
    public abstract class X9ECParametersHolder
    {
        private X9ECParameters parameters;

        public X9ECParameters Parameters
        {
            get
            {
                lock (this)
                {
                    if (this.parameters == null)
                        this.parameters = this.CreateParameters();
                    return this.parameters;
                }
            }
        }

        protected abstract X9ECParameters CreateParameters();
    }
}
