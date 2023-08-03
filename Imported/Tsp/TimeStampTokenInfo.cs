// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Tsp.TimeStampTokenInfo
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.Tsp;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Math;

namespace Org.BouncyCastle.Tsp
{
    public class TimeStampTokenInfo
    {
        private TstInfo tstInfo;
        private DateTime genTime;

        public TimeStampTokenInfo( TstInfo tstInfo )
        {
            this.tstInfo = tstInfo;
            try
            {
                this.genTime = tstInfo.GenTime.ToDateTime();
            }
            catch (Exception ex)
            {
                throw new TspException( "unable to parse genTime field: " + ex.Message );
            }
        }

        public bool IsOrdered => this.tstInfo.Ordering.IsTrue;

        public Accuracy Accuracy => this.tstInfo.Accuracy;

        public DateTime GenTime => this.genTime;

        public GenTimeAccuracy GenTimeAccuracy => this.Accuracy != null ? new GenTimeAccuracy( this.Accuracy ) : null;

        public string Policy => this.tstInfo.Policy.Id;

        public BigInteger SerialNumber => this.tstInfo.SerialNumber.Value;

        public GeneralName Tsa => this.tstInfo.Tsa;

        public BigInteger Nonce => this.tstInfo.Nonce != null ? this.tstInfo.Nonce.Value : null;

        public AlgorithmIdentifier HashAlgorithm => this.tstInfo.MessageImprint.HashAlgorithm;

        public string MessageImprintAlgOid => this.tstInfo.MessageImprint.HashAlgorithm.Algorithm.Id;

        public byte[] GetMessageImprintDigest() => this.tstInfo.MessageImprint.GetHashedMessage();

        public byte[] GetEncoded() => this.tstInfo.GetEncoded();

        public TstInfo TstInfo => this.tstInfo;
    }
}
