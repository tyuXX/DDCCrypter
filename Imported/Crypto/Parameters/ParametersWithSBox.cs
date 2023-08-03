// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Parameters.ParametersWithSBox
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Crypto.Parameters
{
    public class ParametersWithSBox : ICipherParameters
    {
        private ICipherParameters parameters;
        private byte[] sBox;

        public ParametersWithSBox( ICipherParameters parameters, byte[] sBox )
        {
            this.parameters = parameters;
            this.sBox = sBox;
        }

        public byte[] GetSBox() => this.sBox;

        public ICipherParameters Parameters => this.parameters;
    }
}
