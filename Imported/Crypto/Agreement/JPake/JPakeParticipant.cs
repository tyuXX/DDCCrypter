// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Agreement.JPake.JPakeParticipant
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;

namespace Org.BouncyCastle.Crypto.Agreement.JPake
{
    public class JPakeParticipant
    {
        public static readonly int STATE_INITIALIZED = 0;
        public static readonly int STATE_ROUND_1_CREATED = 10;
        public static readonly int STATE_ROUND_1_VALIDATED = 20;
        public static readonly int STATE_ROUND_2_CREATED = 30;
        public static readonly int STATE_ROUND_2_VALIDATED = 40;
        public static readonly int STATE_KEY_CALCULATED = 50;
        public static readonly int STATE_ROUND_3_CREATED = 60;
        public static readonly int STATE_ROUND_3_VALIDATED = 70;
        private string participantId;
        private char[] password;
        private IDigest digest;
        private readonly SecureRandom random;
        private readonly BigInteger p;
        private readonly BigInteger q;
        private readonly BigInteger g;
        private string partnerParticipantId;
        private BigInteger x1;
        private BigInteger x2;
        private BigInteger gx1;
        private BigInteger gx2;
        private BigInteger gx3;
        private BigInteger gx4;
        private BigInteger b;
        private int state;

        public JPakeParticipant( string participantId, char[] password )
          : this( participantId, password, JPakePrimeOrderGroups.NIST_3072 )
        {
        }

        public JPakeParticipant( string participantId, char[] password, JPakePrimeOrderGroup group )
          : this( participantId, password, group, new Sha256Digest(), new SecureRandom() )
        {
        }

        public JPakeParticipant(
          string participantId,
          char[] password,
          JPakePrimeOrderGroup group,
          IDigest digest,
          SecureRandom random )
        {
            JPakeUtilities.ValidateNotNull( participantId, nameof( participantId ) );
            JPakeUtilities.ValidateNotNull( password, nameof( password ) );
            JPakeUtilities.ValidateNotNull( group, nameof( p ) );
            JPakeUtilities.ValidateNotNull( digest, nameof( digest ) );
            JPakeUtilities.ValidateNotNull( random, nameof( random ) );
            if (password.Length == 0)
                throw new ArgumentException( "Password must not be empty." );
            this.participantId = participantId;
            this.password = new char[password.Length];
            Array.Copy( password, this.password, password.Length );
            this.p = group.P;
            this.q = group.Q;
            this.g = group.G;
            this.digest = digest;
            this.random = random;
            this.state = STATE_INITIALIZED;
        }

        public virtual int State => this.state;

        public virtual JPakeRound1Payload CreateRound1PayloadToSend()
        {
            if (this.state >= STATE_ROUND_1_CREATED)
                throw new InvalidOperationException( "Round 1 payload already created for " + this.participantId );
            this.x1 = JPakeUtilities.GenerateX1( this.q, this.random );
            this.x2 = JPakeUtilities.GenerateX2( this.q, this.random );
            this.gx1 = JPakeUtilities.CalculateGx( this.p, this.g, this.x1 );
            this.gx2 = JPakeUtilities.CalculateGx( this.p, this.g, this.x2 );
            BigInteger[] zeroKnowledgeProof1 = JPakeUtilities.CalculateZeroKnowledgeProof( this.p, this.q, this.g, this.gx1, this.x1, this.participantId, this.digest, this.random );
            BigInteger[] zeroKnowledgeProof2 = JPakeUtilities.CalculateZeroKnowledgeProof( this.p, this.q, this.g, this.gx2, this.x2, this.participantId, this.digest, this.random );
            this.state = STATE_ROUND_1_CREATED;
            return new JPakeRound1Payload( this.participantId, this.gx1, this.gx2, zeroKnowledgeProof1, zeroKnowledgeProof2 );
        }

        public virtual void ValidateRound1PayloadReceived( JPakeRound1Payload round1PayloadReceived )
        {
            if (this.state >= STATE_ROUND_1_VALIDATED)
                throw new InvalidOperationException( "Validation already attempted for round 1 payload for " + this.participantId );
            this.partnerParticipantId = round1PayloadReceived.ParticipantId;
            this.gx3 = round1PayloadReceived.Gx1;
            this.gx4 = round1PayloadReceived.Gx2;
            BigInteger[] knowledgeProofForX1 = round1PayloadReceived.KnowledgeProofForX1;
            BigInteger[] knowledgeProofForX2 = round1PayloadReceived.KnowledgeProofForX2;
            JPakeUtilities.ValidateParticipantIdsDiffer( this.participantId, round1PayloadReceived.ParticipantId );
            JPakeUtilities.ValidateGx4( this.gx4 );
            JPakeUtilities.ValidateZeroKnowledgeProof( this.p, this.q, this.g, this.gx3, knowledgeProofForX1, round1PayloadReceived.ParticipantId, this.digest );
            JPakeUtilities.ValidateZeroKnowledgeProof( this.p, this.q, this.g, this.gx4, knowledgeProofForX2, round1PayloadReceived.ParticipantId, this.digest );
            this.state = STATE_ROUND_1_VALIDATED;
        }

        public virtual JPakeRound2Payload CreateRound2PayloadToSend()
        {
            if (this.state >= STATE_ROUND_2_CREATED)
                throw new InvalidOperationException( "Round 2 payload already created for " + this.participantId );
            if (this.state < STATE_ROUND_1_VALIDATED)
                throw new InvalidOperationException( "Round 1 payload must be validated prior to creating round 2 payload for " + this.participantId );
            BigInteger ga = JPakeUtilities.CalculateGA( this.p, this.gx1, this.gx3, this.gx4 );
            BigInteger x2s = JPakeUtilities.CalculateX2s( this.q, this.x2, JPakeUtilities.CalculateS( this.password ) );
            BigInteger a = JPakeUtilities.CalculateA( this.p, this.q, ga, x2s );
            BigInteger[] zeroKnowledgeProof = JPakeUtilities.CalculateZeroKnowledgeProof( this.p, this.q, ga, a, x2s, this.participantId, this.digest, this.random );
            this.state = STATE_ROUND_2_CREATED;
            return new JPakeRound2Payload( this.participantId, a, zeroKnowledgeProof );
        }

        public virtual void ValidateRound2PayloadReceived( JPakeRound2Payload round2PayloadReceived )
        {
            if (this.state >= STATE_ROUND_2_VALIDATED)
                throw new InvalidOperationException( "Validation already attempted for round 2 payload for " + this.participantId );
            if (this.state < STATE_ROUND_1_VALIDATED)
                throw new InvalidOperationException( "Round 1 payload must be validated prior to validation round 2 payload for " + this.participantId );
            BigInteger ga = JPakeUtilities.CalculateGA( this.p, this.gx3, this.gx1, this.gx2 );
            this.b = round2PayloadReceived.A;
            BigInteger[] knowledgeProofForX2s = round2PayloadReceived.KnowledgeProofForX2s;
            JPakeUtilities.ValidateParticipantIdsDiffer( this.participantId, round2PayloadReceived.ParticipantId );
            JPakeUtilities.ValidateParticipantIdsEqual( this.partnerParticipantId, round2PayloadReceived.ParticipantId );
            JPakeUtilities.ValidateGa( ga );
            JPakeUtilities.ValidateZeroKnowledgeProof( this.p, this.q, ga, this.b, knowledgeProofForX2s, round2PayloadReceived.ParticipantId, this.digest );
            this.state = STATE_ROUND_2_VALIDATED;
        }

        public virtual BigInteger CalculateKeyingMaterial()
        {
            if (this.state >= STATE_KEY_CALCULATED)
                throw new InvalidOperationException( "Key already calculated for " + this.participantId );
            if (this.state < STATE_ROUND_2_VALIDATED)
                throw new InvalidOperationException( "Round 2 payload must be validated prior to creating key for " + this.participantId );
            BigInteger s = JPakeUtilities.CalculateS( this.password );
            Array.Clear( password, 0, this.password.Length );
            this.password = null;
            BigInteger keyingMaterial = JPakeUtilities.CalculateKeyingMaterial( this.p, this.q, this.gx4, this.x2, s, this.b );
            this.x1 = null;
            this.x2 = null;
            this.b = null;
            this.state = STATE_KEY_CALCULATED;
            return keyingMaterial;
        }

        public virtual JPakeRound3Payload CreateRound3PayloadToSend( BigInteger keyingMaterial )
        {
            if (this.state >= STATE_ROUND_3_CREATED)
                throw new InvalidOperationException( "Round 3 payload already created for " + this.participantId );
            if (this.state < STATE_KEY_CALCULATED)
                throw new InvalidOperationException( "Keying material must be calculated prior to creating round 3 payload for " + this.participantId );
            BigInteger macTag = JPakeUtilities.CalculateMacTag( this.participantId, this.partnerParticipantId, this.gx1, this.gx2, this.gx3, this.gx4, keyingMaterial, this.digest );
            this.state = STATE_ROUND_3_CREATED;
            return new JPakeRound3Payload( this.participantId, macTag );
        }

        public virtual void ValidateRound3PayloadReceived(
          JPakeRound3Payload round3PayloadReceived,
          BigInteger keyingMaterial )
        {
            if (this.state >= STATE_ROUND_3_VALIDATED)
                throw new InvalidOperationException( "Validation already attempted for round 3 payload for " + this.participantId );
            if (this.state < STATE_KEY_CALCULATED)
                throw new InvalidOperationException( "Keying material must be calculated prior to validating round 3 payload for " + this.participantId );
            JPakeUtilities.ValidateParticipantIdsDiffer( this.participantId, round3PayloadReceived.ParticipantId );
            JPakeUtilities.ValidateParticipantIdsEqual( this.partnerParticipantId, round3PayloadReceived.ParticipantId );
            JPakeUtilities.ValidateMacTag( this.participantId, this.partnerParticipantId, this.gx1, this.gx2, this.gx3, this.gx4, keyingMaterial, this.digest, round3PayloadReceived.MacTag );
            this.gx1 = null;
            this.gx2 = null;
            this.gx3 = null;
            this.gx4 = null;
            this.state = STATE_ROUND_3_VALIDATED;
        }
    }
}
