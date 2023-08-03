// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Anssi.AnssiNamedCurves
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Collections;
using Org.BouncyCastle.Utilities.Encoders;
using System.Collections;

namespace Org.BouncyCastle.Asn1.Anssi
{
    public class AnssiNamedCurves
    {
        private static readonly IDictionary objIds = Platform.CreateHashtable();
        private static readonly IDictionary curves = Platform.CreateHashtable();
        private static readonly IDictionary names = Platform.CreateHashtable();

        private static ECCurve ConfigureCurve( ECCurve curve ) => curve;

        private static BigInteger FromHex( string hex ) => new BigInteger( 1, Hex.Decode( hex ) );

        private static void DefineCurve(
          string name,
          DerObjectIdentifier oid,
          X9ECParametersHolder holder )
        {
            objIds.Add( Platform.ToUpperInvariant( name ), oid );
            names.Add( oid, name );
            curves.Add( oid, holder );
        }

        static AnssiNamedCurves() => DefineCurve( "FRP256v1", AnssiObjectIdentifiers.FRP256v1, Frp256v1Holder.Instance );

        public static X9ECParameters GetByName( string name )
        {
            DerObjectIdentifier oid = GetOid( name );
            return oid != null ? GetByOid( oid ) : null;
        }

        public static X9ECParameters GetByOid( DerObjectIdentifier oid ) => ((X9ECParametersHolder)curves[oid])?.Parameters;

        public static DerObjectIdentifier GetOid( string name ) => (DerObjectIdentifier)objIds[Platform.ToUpperInvariant( name )];

        public static string GetName( DerObjectIdentifier oid ) => (string)names[oid];

        public static IEnumerable Names => new EnumerableProxy( names.Values );

        internal class Frp256v1Holder : X9ECParametersHolder
        {
            internal static readonly X9ECParametersHolder Instance = new AnssiNamedCurves.Frp256v1Holder();

            private Frp256v1Holder()
            {
            }

            protected override X9ECParameters CreateParameters()
            {
                BigInteger q = FromHex( "F1FD178C0B3AD58F10126DE8CE42435B3961ADBCABC8CA6DE8FCF353D86E9C03" );
                BigInteger a = FromHex( "F1FD178C0B3AD58F10126DE8CE42435B3961ADBCABC8CA6DE8FCF353D86E9C00" );
                BigInteger b = FromHex( "EE353FCA5428A9300D4ABA754A44C00FDFEC0C9AE4B1A1803075ED967B7BB73F" );
                byte[] seed = null;
                BigInteger bigInteger = FromHex( "F1FD178C0B3AD58F10126DE8CE42435B53DC67E140D2BF941FFDD459C6D655E1" );
                BigInteger one = BigInteger.One;
                ECCurve ecCurve = ConfigureCurve( new FpCurve( q, a, b, bigInteger, one ) );
                X9ECPoint g = new X9ECPoint( ecCurve, Hex.Decode( "04B6B3D4C356C139EB31183D4749D423958C27D2DCAF98B70164C97A2DD98F5CFF6142E0F7C8B204911F9271F0F3ECEF8C2701C307E8E4C9E183115A1554062CFB" ) );
                return new X9ECParameters( ecCurve, g, bigInteger, one, seed );
            }
        }
    }
}
