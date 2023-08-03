// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.X509.Extension.X509ExtensionUtilities
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Security.Certificates;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections;
using System.IO;

namespace Org.BouncyCastle.X509.Extension
{
    public class X509ExtensionUtilities
    {
        public static Asn1Object FromExtensionValue( Asn1OctetString extensionValue ) => Asn1Object.FromByteArray( extensionValue.GetOctets() );

        public static ICollection GetIssuerAlternativeNames( X509Certificate cert ) => GetAlternativeName( cert.GetExtensionValue( X509Extensions.IssuerAlternativeName ) );

        public static ICollection GetSubjectAlternativeNames( X509Certificate cert ) => GetAlternativeName( cert.GetExtensionValue( X509Extensions.SubjectAlternativeName ) );

        private static ICollection GetAlternativeName( Asn1OctetString extVal )
        {
            IList arrayList1 = Platform.CreateArrayList();
            if (extVal != null)
            {
                try
                {
                    foreach (GeneralName generalName in Asn1Sequence.GetInstance( FromExtensionValue( extVal ) ))
                    {
                        IList arrayList2 = Platform.CreateArrayList();
                        arrayList2.Add( generalName.TagNo );
                        switch (generalName.TagNo)
                        {
                            case 0:
                            case 3:
                            case 5:
                                arrayList2.Add( generalName.Name.ToAsn1Object() );
                                break;
                            case 1:
                            case 2:
                            case 6:
                                arrayList2.Add( ((IAsn1String)generalName.Name).GetString() );
                                break;
                            case 4:
                                arrayList2.Add( X509Name.GetInstance( generalName.Name ).ToString() );
                                break;
                            case 7:
                                arrayList2.Add( Asn1OctetString.GetInstance( generalName.Name ).GetOctets() );
                                break;
                            case 8:
                                arrayList2.Add( DerObjectIdentifier.GetInstance( generalName.Name ).Id );
                                break;
                            default:
                                throw new IOException( "Bad tag number: " + generalName.TagNo );
                        }
                        arrayList1.Add( arrayList2 );
                    }
                }
                catch (Exception ex)
                {
                    throw new CertificateParsingException( ex.Message );
                }
            }
            return arrayList1;
        }
    }
}
