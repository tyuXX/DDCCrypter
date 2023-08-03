// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Parameters.ParametersWithSalt
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System;

namespace Org.BouncyCastle.Crypto.Parameters
{
    public class ParametersWithSalt : ICipherParameters
    {
        private byte[] salt;
        private ICipherParameters parameters;

        public ParametersWithSalt( ICipherParameters parameters, byte[] salt )
          : this( parameters, salt, 0, salt.Length )
        {
        }

        public ParametersWithSalt( ICipherParameters parameters, byte[] salt, int saltOff, int saltLen )
        {
            this.salt = new byte[saltLen];
            this.parameters = parameters;
            Array.Copy( salt, saltOff, this.salt, 0, saltLen );
        }

        public byte[] GetSalt() => this.salt;

        public ICipherParameters Parameters => this.parameters;
    }
}
