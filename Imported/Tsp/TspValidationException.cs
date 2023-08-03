// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Tsp.TspValidationException
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Tsp
{
    [Serializable]
    public class TspValidationException : TspException
    {
        private int failureCode;

        public TspValidationException( string message )
          : base( message )
        {
            this.failureCode = -1;
        }

        public TspValidationException( string message, int failureCode )
          : base( message )
        {
            this.failureCode = failureCode;
        }

        public int FailureCode => this.failureCode;
    }
}
