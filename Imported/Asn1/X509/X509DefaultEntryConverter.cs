// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X509.X509DefaultEntryConverter
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System;
using System.IO;

namespace Org.BouncyCastle.Asn1.X509
{
    public class X509DefaultEntryConverter : X509NameEntryConverter
    {
        public override Asn1Object GetConvertedValue( DerObjectIdentifier oid, string value )
        {
            if (value.Length != 0)
            {
                if (value[0] == '#')
                {
                    try
                    {
                        return this.ConvertHexEncoded( value, 1 );
                    }
                    catch (IOException ex)
                    {
                        throw new Exception( "can't recode value for oid " + oid.Id );
                    }
                }
            }
            if (value.Length != 0 && value[0] == '\\')
                value = value.Substring( 1 );
            if (oid.Equals( X509Name.EmailAddress ) || oid.Equals( X509Name.DC ))
                return new DerIA5String( value );
            if (oid.Equals( X509Name.DateOfBirth ))
                return new DerGeneralizedTime( value );
            return oid.Equals( X509Name.C ) || oid.Equals( X509Name.SerialNumber ) || oid.Equals( X509Name.DnQualifier ) || oid.Equals( X509Name.TelephoneNumber ) ? new DerPrintableString( value ) : (Asn1Object)new DerUtf8String( value );
        }
    }
}
