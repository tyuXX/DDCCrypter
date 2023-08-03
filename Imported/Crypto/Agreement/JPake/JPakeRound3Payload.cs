// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Agreement.JPake.JPakeRound3Payload
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math;

namespace Org.BouncyCastle.Crypto.Agreement.JPake
{
    public class JPakeRound3Payload
    {
        private readonly string participantId;
        private readonly BigInteger macTag;

        public JPakeRound3Payload( string participantId, BigInteger magTag )
        {
            this.participantId = participantId;
            this.macTag = magTag;
        }

        public virtual string ParticipantId => this.participantId;

        public virtual BigInteger MacTag => this.macTag;
    }
}
