// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X9.ECNamedCurveTable
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.Anssi;
using Org.BouncyCastle.Asn1.Nist;
using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Asn1.TeleTrust;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Collections;
using System.Collections;

namespace Org.BouncyCastle.Asn1.X9
{
    public class ECNamedCurveTable
    {
        public static X9ECParameters GetByName( string name ) => (((X962NamedCurves.GetByName( name ) ?? SecNamedCurves.GetByName( name )) ?? NistNamedCurves.GetByName( name )) ?? TeleTrusTNamedCurves.GetByName( name )) ?? AnssiNamedCurves.GetByName( name );

        public static string GetName( DerObjectIdentifier oid ) => (((X962NamedCurves.GetName( oid ) ?? SecNamedCurves.GetName( oid )) ?? NistNamedCurves.GetName( oid )) ?? TeleTrusTNamedCurves.GetName( oid )) ?? AnssiNamedCurves.GetName( oid );

        public static DerObjectIdentifier GetOid( string name ) => (((X962NamedCurves.GetOid( name ) ?? SecNamedCurves.GetOid( name )) ?? NistNamedCurves.GetOid( name )) ?? TeleTrusTNamedCurves.GetOid( name )) ?? AnssiNamedCurves.GetOid( name );

        public static X9ECParameters GetByOid( DerObjectIdentifier oid ) => ((X962NamedCurves.GetByOid( oid ) ?? SecNamedCurves.GetByOid( oid )) ?? TeleTrusTNamedCurves.GetByOid( oid )) ?? AnssiNamedCurves.GetByOid( oid );

        public static IEnumerable Names
        {
            get
            {
                IList arrayList = Platform.CreateArrayList();
                CollectionUtilities.AddRange( arrayList, X962NamedCurves.Names );
                CollectionUtilities.AddRange( arrayList, SecNamedCurves.Names );
                CollectionUtilities.AddRange( arrayList, NistNamedCurves.Names );
                CollectionUtilities.AddRange( arrayList, TeleTrusTNamedCurves.Names );
                CollectionUtilities.AddRange( arrayList, AnssiNamedCurves.Names );
                return arrayList;
            }
        }
    }
}
