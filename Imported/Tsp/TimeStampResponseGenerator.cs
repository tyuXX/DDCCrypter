// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Tsp.TimeStampResponseGenerator
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Cmp;
using Org.BouncyCastle.Asn1.Cms;
using Org.BouncyCastle.Asn1.Tsp;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Utilities.Date;
using System;
using System.Collections;
using System.IO;

namespace Org.BouncyCastle.Tsp
{
    public class TimeStampResponseGenerator
    {
        private PkiStatus status;
        private Asn1EncodableVector statusStrings;
        private int failInfo;
        private TimeStampTokenGenerator tokenGenerator;
        private IList acceptedAlgorithms;
        private IList acceptedPolicies;
        private IList acceptedExtensions;

        public TimeStampResponseGenerator(
          TimeStampTokenGenerator tokenGenerator,
          IList acceptedAlgorithms )
          : this( tokenGenerator, acceptedAlgorithms, null, null )
        {
        }

        public TimeStampResponseGenerator(
          TimeStampTokenGenerator tokenGenerator,
          IList acceptedAlgorithms,
          IList acceptedPolicy )
          : this( tokenGenerator, acceptedAlgorithms, acceptedPolicy, null )
        {
        }

        public TimeStampResponseGenerator(
          TimeStampTokenGenerator tokenGenerator,
          IList acceptedAlgorithms,
          IList acceptedPolicies,
          IList acceptedExtensions )
        {
            this.tokenGenerator = tokenGenerator;
            this.acceptedAlgorithms = acceptedAlgorithms;
            this.acceptedPolicies = acceptedPolicies;
            this.acceptedExtensions = acceptedExtensions;
            this.statusStrings = new Asn1EncodableVector( new Asn1Encodable[0] );
        }

        private void AddStatusString( string statusString ) => this.statusStrings.Add( new DerUtf8String( statusString ) );

        private void SetFailInfoField( int field ) => this.failInfo |= field;

        private PkiStatusInfo GetPkiStatusInfo()
        {
            Asn1EncodableVector v = new Asn1EncodableVector( new Asn1Encodable[1]
            {
         new DerInteger((int) this.status)
            } );
            if (this.statusStrings.Count > 0)
                v.Add( new PkiFreeText( new DerSequence( this.statusStrings ) ) );
            if (this.failInfo != 0)
                v.Add( new TimeStampResponseGenerator.FailInfo( this.failInfo ) );
            return new PkiStatusInfo( new DerSequence( v ) );
        }

        public TimeStampResponse Generate(
          TimeStampRequest request,
          BigInteger serialNumber,
          DateTime genTime )
        {
            return this.Generate( request, serialNumber, new DateTimeObject( genTime ) );
        }

        public TimeStampResponse Generate(
          TimeStampRequest request,
          BigInteger serialNumber,
          DateTimeObject genTime )
        {
            TimeStampResp resp;
            try
            {
                if (genTime == null)
                    throw new TspValidationException( "The time source is not available.", 512 );
                request.Validate( this.acceptedAlgorithms, this.acceptedPolicies, this.acceptedExtensions );
                this.status = PkiStatus.Granted;
                this.AddStatusString( "Operation Okay" );
                PkiStatusInfo pkiStatusInfo = this.GetPkiStatusInfo();
                ContentInfo instance;
                try
                {
                    instance = ContentInfo.GetInstance( Asn1Object.FromByteArray( this.tokenGenerator.Generate( request, serialNumber, genTime.Value ).ToCmsSignedData().GetEncoded() ) );
                }
                catch (IOException ex)
                {
                    throw new TspException( "Timestamp token received cannot be converted to ContentInfo", ex );
                }
                resp = new TimeStampResp( pkiStatusInfo, instance );
            }
            catch (TspValidationException ex)
            {
                this.status = PkiStatus.Rejection;
                this.SetFailInfoField( ex.FailureCode );
                this.AddStatusString( ex.Message );
                resp = new TimeStampResp( this.GetPkiStatusInfo(), null );
            }
            try
            {
                return new TimeStampResponse( resp );
            }
            catch (IOException ex)
            {
                throw new TspException( "created badly formatted response!", ex );
            }
        }

        public TimeStampResponse GenerateFailResponse(
          PkiStatus status,
          int failInfoField,
          string statusString )
        {
            this.status = status;
            this.SetFailInfoField( failInfoField );
            if (statusString != null)
                this.AddStatusString( statusString );
            TimeStampResp resp = new TimeStampResp( this.GetPkiStatusInfo(), null );
            try
            {
                return new TimeStampResponse( resp );
            }
            catch (IOException ex)
            {
                throw new TspException( "created badly formatted response!", ex );
            }
        }

        private class FailInfo : DerBitString
        {
            internal FailInfo( int failInfoValue )
              : base( failInfoValue )
            {
            }
        }
    }
}
