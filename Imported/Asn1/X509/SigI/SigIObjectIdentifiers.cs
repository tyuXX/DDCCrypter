// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X509.SigI.SigIObjectIdentifiers
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Asn1.X509.SigI
{
    public sealed class SigIObjectIdentifiers
    {
        public static readonly DerObjectIdentifier IdSigI = new( "1.3.36.8" );
        public static readonly DerObjectIdentifier IdSigIKP = new( IdSigI.ToString() + ".2" );
        public static readonly DerObjectIdentifier IdSigICP = new( IdSigI.ToString() + ".1" );
        public static readonly DerObjectIdentifier IdSigION = new( IdSigI.ToString() + ".4" );
        public static readonly DerObjectIdentifier IdSigIKPDirectoryService = new( IdSigIKP.ToString() + ".1" );
        public static readonly DerObjectIdentifier IdSigIONPersonalData = new( IdSigION.ToString() + ".1" );
        public static readonly DerObjectIdentifier IdSigICPSigConform = new( IdSigICP.ToString() + ".1" );

        private SigIObjectIdentifiers()
        {
        }
    }
}
