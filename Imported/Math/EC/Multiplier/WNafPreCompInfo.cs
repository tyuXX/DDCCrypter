﻿// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Math.EC.Multiplier.WNafPreCompInfo
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Math.EC.Multiplier
{
    public class WNafPreCompInfo : PreCompInfo
    {
        protected ECPoint[] m_preComp = null;
        protected ECPoint[] m_preCompNeg = null;
        protected ECPoint m_twice = null;

        public virtual ECPoint[] PreComp
        {
            get => this.m_preComp;
            set => this.m_preComp = value;
        }

        public virtual ECPoint[] PreCompNeg
        {
            get => this.m_preCompNeg;
            set => this.m_preCompNeg = value;
        }

        public virtual ECPoint Twice
        {
            get => this.m_twice;
            set => this.m_twice = value;
        }
    }
}
