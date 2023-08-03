// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.CryptoPro.ECGost3410NamedCurves
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Collections;
using System.Collections;

namespace Org.BouncyCastle.Asn1.CryptoPro
{
    public sealed class ECGost3410NamedCurves
    {
        internal static readonly IDictionary objIds = Platform.CreateHashtable();
        internal static readonly IDictionary parameters = Platform.CreateHashtable();
        internal static readonly IDictionary names = Platform.CreateHashtable();

        private ECGost3410NamedCurves()
        {
        }

        static ECGost3410NamedCurves()
        {
            BigInteger q1 = new BigInteger( "115792089237316195423570985008687907853269984665640564039457584007913129639319" );
            BigInteger bigInteger1 = new BigInteger( "115792089237316195423570985008687907853073762908499243225378155805079068850323" );
            FpCurve curve1 = new FpCurve( q1, new BigInteger( "115792089237316195423570985008687907853269984665640564039457584007913129639316" ), new BigInteger( "166" ), bigInteger1, BigInteger.One );
            ECDomainParameters domainParameters1 = new ECDomainParameters( curve1, curve1.CreatePoint( new BigInteger( "1" ), new BigInteger( "64033881142927202683649881450433473985931760268884941288852745803908878638612" ) ), bigInteger1 );
            parameters[CryptoProObjectIdentifiers.GostR3410x2001CryptoProA] = domainParameters1;
            BigInteger q2 = new BigInteger( "115792089237316195423570985008687907853269984665640564039457584007913129639319" );
            BigInteger bigInteger2 = new BigInteger( "115792089237316195423570985008687907853073762908499243225378155805079068850323" );
            FpCurve curve2 = new FpCurve( q2, new BigInteger( "115792089237316195423570985008687907853269984665640564039457584007913129639316" ), new BigInteger( "166" ), bigInteger2, BigInteger.One );
            ECDomainParameters domainParameters2 = new ECDomainParameters( curve2, curve2.CreatePoint( new BigInteger( "1" ), new BigInteger( "64033881142927202683649881450433473985931760268884941288852745803908878638612" ) ), bigInteger2 );
            parameters[CryptoProObjectIdentifiers.GostR3410x2001CryptoProXchA] = domainParameters2;
            BigInteger q3 = new BigInteger( "57896044618658097711785492504343953926634992332820282019728792003956564823193" );
            BigInteger bigInteger3 = new BigInteger( "57896044618658097711785492504343953927102133160255826820068844496087732066703" );
            FpCurve curve3 = new FpCurve( q3, new BigInteger( "57896044618658097711785492504343953926634992332820282019728792003956564823190" ), new BigInteger( "28091019353058090096996979000309560759124368558014865957655842872397301267595" ), bigInteger3, BigInteger.One );
            ECDomainParameters domainParameters3 = new ECDomainParameters( curve3, curve3.CreatePoint( new BigInteger( "1" ), new BigInteger( "28792665814854611296992347458380284135028636778229113005756334730996303888124" ) ), bigInteger3 );
            parameters[CryptoProObjectIdentifiers.GostR3410x2001CryptoProB] = domainParameters3;
            BigInteger q4 = new BigInteger( "70390085352083305199547718019018437841079516630045180471284346843705633502619" );
            BigInteger bigInteger4 = new BigInteger( "70390085352083305199547718019018437840920882647164081035322601458352298396601" );
            FpCurve curve4 = new FpCurve( q4, new BigInteger( "70390085352083305199547718019018437841079516630045180471284346843705633502616" ), new BigInteger( "32858" ), bigInteger4, BigInteger.One );
            ECDomainParameters domainParameters4 = new ECDomainParameters( curve4, curve4.CreatePoint( new BigInteger( "0" ), new BigInteger( "29818893917731240733471273240314769927240550812383695689146495261604565990247" ) ), bigInteger4 );
            parameters[CryptoProObjectIdentifiers.GostR3410x2001CryptoProXchB] = domainParameters4;
            BigInteger q5 = new BigInteger( "70390085352083305199547718019018437841079516630045180471284346843705633502619" );
            BigInteger bigInteger5 = new BigInteger( "70390085352083305199547718019018437840920882647164081035322601458352298396601" );
            FpCurve curve5 = new FpCurve( q5, new BigInteger( "70390085352083305199547718019018437841079516630045180471284346843705633502616" ), new BigInteger( "32858" ), bigInteger5, BigInteger.One );
            ECDomainParameters domainParameters5 = new ECDomainParameters( curve5, curve5.CreatePoint( new BigInteger( "0" ), new BigInteger( "29818893917731240733471273240314769927240550812383695689146495261604565990247" ) ), bigInteger5 );
            parameters[CryptoProObjectIdentifiers.GostR3410x2001CryptoProC] = domainParameters5;
            objIds["GostR3410-2001-CryptoPro-A"] = CryptoProObjectIdentifiers.GostR3410x2001CryptoProA;
            objIds["GostR3410-2001-CryptoPro-B"] = CryptoProObjectIdentifiers.GostR3410x2001CryptoProB;
            objIds["GostR3410-2001-CryptoPro-C"] = CryptoProObjectIdentifiers.GostR3410x2001CryptoProC;
            objIds["GostR3410-2001-CryptoPro-XchA"] = CryptoProObjectIdentifiers.GostR3410x2001CryptoProXchA;
            objIds["GostR3410-2001-CryptoPro-XchB"] = CryptoProObjectIdentifiers.GostR3410x2001CryptoProXchB;
            names[CryptoProObjectIdentifiers.GostR3410x2001CryptoProA] = "GostR3410-2001-CryptoPro-A";
            names[CryptoProObjectIdentifiers.GostR3410x2001CryptoProB] = "GostR3410-2001-CryptoPro-B";
            names[CryptoProObjectIdentifiers.GostR3410x2001CryptoProC] = "GostR3410-2001-CryptoPro-C";
            names[CryptoProObjectIdentifiers.GostR3410x2001CryptoProXchA] = "GostR3410-2001-CryptoPro-XchA";
            names[CryptoProObjectIdentifiers.GostR3410x2001CryptoProXchB] = "GostR3410-2001-CryptoPro-XchB";
        }

        public static ECDomainParameters GetByOid( DerObjectIdentifier oid ) => (ECDomainParameters)parameters[oid];

        public static IEnumerable Names => new EnumerableProxy( names.Values );

        public static ECDomainParameters GetByName( string name )
        {
            DerObjectIdentifier objId = (DerObjectIdentifier)objIds[name];
            return objId != null ? (ECDomainParameters)parameters[objId] : null;
        }

        public static string GetName( DerObjectIdentifier oid ) => (string)names[oid];

        public static DerObjectIdentifier GetOid( string name ) => (DerObjectIdentifier)objIds[name];
    }
}
