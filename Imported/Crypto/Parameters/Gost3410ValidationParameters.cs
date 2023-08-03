// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Parameters.Gost3410ValidationParameters
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Crypto.Parameters
{
    public class Gost3410ValidationParameters
    {
        private int x0;
        private int c;
        private long x0L;
        private long cL;

        public Gost3410ValidationParameters( int x0, int c )
        {
            this.x0 = x0;
            this.c = c;
        }

        public Gost3410ValidationParameters( long x0L, long cL )
        {
            this.x0L = x0L;
            this.cL = cL;
        }

        public int C => this.c;

        public int X0 => this.x0;

        public long CL => this.cL;

        public long X0L => this.x0L;

        public override bool Equals( object obj ) => obj is Gost3410ValidationParameters validationParameters && validationParameters.c == this.c && validationParameters.x0 == this.x0 && validationParameters.cL == this.cL && validationParameters.x0L == this.x0L;

        public override int GetHashCode() => this.c.GetHashCode() ^ this.x0.GetHashCode() ^ this.cL.GetHashCode() ^ this.x0L.GetHashCode();
    }
}
