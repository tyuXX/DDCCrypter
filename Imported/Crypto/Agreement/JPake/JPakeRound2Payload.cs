// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Agreement.JPake.JPakeRound2Payload
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math;
using System;

namespace Org.BouncyCastle.Crypto.Agreement.JPake
{
    public class JPakeRound2Payload
    {
        private readonly string participantId;
        private readonly BigInteger a;
        private readonly BigInteger[] knowledgeProofForX2s;

        public JPakeRound2Payload(
          string participantId,
          BigInteger a,
          BigInteger[] knowledgeProofForX2s )
        {
            JPakeUtilities.ValidateNotNull( participantId, nameof( participantId ) );
            JPakeUtilities.ValidateNotNull( a, nameof( a ) );
            JPakeUtilities.ValidateNotNull( knowledgeProofForX2s, nameof( knowledgeProofForX2s ) );
            this.participantId = participantId;
            this.a = a;
            this.knowledgeProofForX2s = new BigInteger[knowledgeProofForX2s.Length];
            knowledgeProofForX2s.CopyTo( this.knowledgeProofForX2s, 0 );
        }

        public virtual string ParticipantId => this.participantId;

        public virtual BigInteger A => this.a;

        public virtual BigInteger[] KnowledgeProofForX2s
        {
            get
            {
                BigInteger[] destinationArray = new BigInteger[this.knowledgeProofForX2s.Length];
                Array.Copy( knowledgeProofForX2s, destinationArray, this.knowledgeProofForX2s.Length );
                return destinationArray;
            }
        }
    }
}
