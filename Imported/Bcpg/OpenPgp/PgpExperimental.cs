﻿// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.OpenPgp.PgpExperimental
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Bcpg.OpenPgp
{
    public class PgpExperimental : PgpObject
    {
        private readonly ExperimentalPacket p;

        public PgpExperimental( BcpgInputStream bcpgIn ) => this.p = (ExperimentalPacket)bcpgIn.ReadPacket();
    }
}
