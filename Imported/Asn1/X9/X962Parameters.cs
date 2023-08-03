// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X9.X962Parameters
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System;

namespace Org.BouncyCastle.Asn1.X9
{
    public class X962Parameters : Asn1Encodable, IAsn1Choice
    {
        private readonly Asn1Object _params;

        public static X962Parameters GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case X962Parameters _:
                    return (X962Parameters)obj;
                case Asn1Object _:
                    return new X962Parameters( (Asn1Object)obj );
                case byte[] _:
                    try
                    {
                        return new X962Parameters( Asn1Object.FromByteArray( (byte[])obj ) );
                    }
                    catch (Exception ex)
                    {
                        throw new ArgumentException( "unable to parse encoded data: " + ex.Message, ex );
                    }
                default:
                    throw new ArgumentException( "unknown object in getInstance()" );
            }
        }

        public X962Parameters( X9ECParameters ecParameters ) => this._params = ecParameters.ToAsn1Object();

        public X962Parameters( DerObjectIdentifier namedCurve ) => this._params = namedCurve;

        public X962Parameters( Asn1Object obj ) => this._params = obj;

        public bool IsNamedCurve => this._params is DerObjectIdentifier;

        public bool IsImplicitlyCA => this._params is Asn1Null;

        public Asn1Object Parameters => this._params;

        public override Asn1Object ToAsn1Object() => this._params;
    }
}
