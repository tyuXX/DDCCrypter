// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.IsisMtt.IsisMttObjectIdentifiers
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Asn1.IsisMtt
{
    public abstract class IsisMttObjectIdentifiers
    {
        public static readonly DerObjectIdentifier IdIsisMtt = new( "1.3.36.8" );
        public static readonly DerObjectIdentifier IdIsisMttCP = new( IdIsisMtt.ToString() + ".1" );
        public static readonly DerObjectIdentifier IdIsisMttCPAccredited = new( IdIsisMttCP.ToString() + ".1" );
        public static readonly DerObjectIdentifier IdIsisMttAT = new( IdIsisMtt.ToString() + ".3" );
        public static readonly DerObjectIdentifier IdIsisMttATDateOfCertGen = new( IdIsisMttAT.ToString() + ".1" );
        public static readonly DerObjectIdentifier IdIsisMttATProcuration = new( IdIsisMttAT.ToString() + ".2" );
        public static readonly DerObjectIdentifier IdIsisMttATAdmission = new( IdIsisMttAT.ToString() + ".3" );
        public static readonly DerObjectIdentifier IdIsisMttATMonetaryLimit = new( IdIsisMttAT.ToString() + ".4" );
        public static readonly DerObjectIdentifier IdIsisMttATDeclarationOfMajority = new( IdIsisMttAT.ToString() + ".5" );
        public static readonly DerObjectIdentifier IdIsisMttATIccsn = new( IdIsisMttAT.ToString() + ".6" );
        public static readonly DerObjectIdentifier IdIsisMttATPKReference = new( IdIsisMttAT.ToString() + ".7" );
        public static readonly DerObjectIdentifier IdIsisMttATRestriction = new( IdIsisMttAT.ToString() + ".8" );
        public static readonly DerObjectIdentifier IdIsisMttATRetrieveIfAllowed = new( IdIsisMttAT.ToString() + ".9" );
        public static readonly DerObjectIdentifier IdIsisMttATRequestedCertificate = new( IdIsisMttAT.ToString() + ".10" );
        public static readonly DerObjectIdentifier IdIsisMttATNamingAuthorities = new( IdIsisMttAT.ToString() + ".11" );
        public static readonly DerObjectIdentifier IdIsisMttATCertInDirSince = new( IdIsisMttAT.ToString() + ".12" );
        public static readonly DerObjectIdentifier IdIsisMttATCertHash = new( IdIsisMttAT.ToString() + ".13" );
        public static readonly DerObjectIdentifier IdIsisMttATNameAtBirth = new( IdIsisMttAT.ToString() + ".14" );
        public static readonly DerObjectIdentifier IdIsisMttATAdditionalInformation = new( IdIsisMttAT.ToString() + ".15" );
        public static readonly DerObjectIdentifier IdIsisMttATLiabilityLimitationFlag = new( "0.2.262.1.10.12.0" );
    }
}
