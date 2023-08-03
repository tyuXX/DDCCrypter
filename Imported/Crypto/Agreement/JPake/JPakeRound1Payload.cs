// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Agreement.JPake.JPakeRound1Payload
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math;

namespace Org.BouncyCastle.Crypto.Agreement.JPake
{
    public class JPakeRound1Payload
    {
        private readonly string participantId;
        private readonly BigInteger gx1;
        private readonly BigInteger gx2;
        private readonly BigInteger[] knowledgeProofForX1;
        private readonly BigInteger[] knowledgeProofForX2;

        public JPakeRound1Payload(
          string participantId,
          BigInteger gx1,
          BigInteger gx2,
          BigInteger[] knowledgeProofForX1,
          BigInteger[] knowledgeProofForX2 )
        {
            JPakeUtilities.ValidateNotNull( participantId, nameof( participantId ) );
            JPakeUtilities.ValidateNotNull( gx1, nameof( gx1 ) );
            JPakeUtilities.ValidateNotNull( gx2, nameof( gx2 ) );
            JPakeUtilities.ValidateNotNull( knowledgeProofForX1, nameof( knowledgeProofForX1 ) );
            JPakeUtilities.ValidateNotNull( knowledgeProofForX2, nameof( knowledgeProofForX2 ) );
            this.participantId = participantId;
            this.gx1 = gx1;
            this.gx2 = gx2;
            this.knowledgeProofForX1 = new BigInteger[knowledgeProofForX1.Length];
            Array.Copy( knowledgeProofForX1, this.knowledgeProofForX1, knowledgeProofForX1.Length );
            this.knowledgeProofForX2 = new BigInteger[knowledgeProofForX2.Length];
            Array.Copy( knowledgeProofForX2, this.knowledgeProofForX2, knowledgeProofForX2.Length );
        }

        public virtual string ParticipantId => this.participantId;

        public virtual BigInteger Gx1 => this.gx1;

        public virtual BigInteger Gx2 => this.gx2;

        public virtual BigInteger[] KnowledgeProofForX1
        {
            get
            {
                BigInteger[] destinationArray = new BigInteger[this.knowledgeProofForX1.Length];
                Array.Copy( knowledgeProofForX1, destinationArray, this.knowledgeProofForX1.Length );
                return destinationArray;
            }
        }

        public virtual BigInteger[] KnowledgeProofForX2
        {
            get
            {
                BigInteger[] destinationArray = new BigInteger[this.knowledgeProofForX2.Length];
                Array.Copy( knowledgeProofForX2, destinationArray, this.knowledgeProofForX2.Length );
                return destinationArray;
            }
        }
    }
}
