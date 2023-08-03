// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.X509.IX509Extension
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Utilities.Collections;

namespace Org.BouncyCastle.X509
{
    public interface IX509Extension
    {
        ISet GetCriticalExtensionOids();

        ISet GetNonCriticalExtensionOids();

        [Obsolete( "Use version taking a DerObjectIdentifier instead" )]
        Asn1OctetString GetExtensionValue( string oid );

        Asn1OctetString GetExtensionValue( DerObjectIdentifier oid );
    }
}
