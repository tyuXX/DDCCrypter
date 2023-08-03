// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Parameters.ParametersWithIV
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Crypto.Parameters
{
    public class ParametersWithIV : ICipherParameters
    {
        private readonly ICipherParameters parameters;
        private readonly byte[] iv;

        public ParametersWithIV( ICipherParameters parameters, byte[] iv )
          : this( parameters, iv, 0, iv.Length )
        {
        }

        public ParametersWithIV( ICipherParameters parameters, byte[] iv, int ivOff, int ivLen )
        {
            if (iv == null)
                throw new ArgumentNullException( nameof( iv ) );
            this.parameters = parameters;
            this.iv = new byte[ivLen];
            Array.Copy( iv, ivOff, this.iv, 0, ivLen );
        }

        public byte[] GetIV() => (byte[])this.iv.Clone();

        public ICipherParameters Parameters => this.parameters;
    }
}
