// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.X509.X509CrlEntry
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Utilities;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security.Certificates;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.X509.Extension;
using System;
using System.Collections;
using System.Text;

namespace Org.BouncyCastle.X509
{
    public class X509CrlEntry : X509ExtensionBase
    {
        private CrlEntry c;
        private bool isIndirect;
        private X509Name previousCertificateIssuer;
        private X509Name certificateIssuer;

        public X509CrlEntry( CrlEntry c )
        {
            this.c = c;
            this.certificateIssuer = this.loadCertificateIssuer();
        }

        public X509CrlEntry( CrlEntry c, bool isIndirect, X509Name previousCertificateIssuer )
        {
            this.c = c;
            this.isIndirect = isIndirect;
            this.previousCertificateIssuer = previousCertificateIssuer;
            this.certificateIssuer = this.loadCertificateIssuer();
        }

        private X509Name loadCertificateIssuer()
        {
            if (!this.isIndirect)
                return null;
            Asn1OctetString extensionValue = this.GetExtensionValue( X509Extensions.CertificateIssuer );
            if (extensionValue == null)
                return this.previousCertificateIssuer;
            try
            {
                GeneralName[] names = GeneralNames.GetInstance( X509ExtensionUtilities.FromExtensionValue( extensionValue ) ).GetNames();
                for (int index = 0; index < names.Length; ++index)
                {
                    if (names[index].TagNo == 4)
                        return X509Name.GetInstance( names[index].Name );
                }
            }
            catch (Exception ex)
            {
            }
            return null;
        }

        public X509Name GetCertificateIssuer() => this.certificateIssuer;

        protected override X509Extensions GetX509Extensions() => this.c.Extensions;

        public byte[] GetEncoded()
        {
            try
            {
                return this.c.GetDerEncoded();
            }
            catch (Exception ex)
            {
                throw new CrlException( ex.ToString() );
            }
        }

        public BigInteger SerialNumber => this.c.UserCertificate.Value;

        public DateTime RevocationDate => this.c.RevocationDate.ToDateTime();

        public bool HasExtensions => this.c.Extensions != null;

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            string newLine = Platform.NewLine;
            stringBuilder.Append( "        userCertificate: " ).Append( SerialNumber ).Append( newLine );
            stringBuilder.Append( "         revocationDate: " ).Append( RevocationDate ).Append( newLine );
            stringBuilder.Append( "      certificateIssuer: " ).Append( this.GetCertificateIssuer() ).Append( newLine );
            X509Extensions extensions = this.c.Extensions;
            if (extensions != null)
            {
                IEnumerator enumerator = extensions.ExtensionOids.GetEnumerator();
                if (enumerator.MoveNext())
                {
                    stringBuilder.Append( "   crlEntryExtensions:" ).Append( newLine );
                    do
                    {
                        DerObjectIdentifier current = (DerObjectIdentifier)enumerator.Current;
                        X509Extension extension = extensions.GetExtension( current );
                        if (extension.Value != null)
                        {
                            Asn1Object asn1Object = Asn1Object.FromByteArray( extension.Value.GetOctets() );
                            stringBuilder.Append( "                       critical(" ).Append( extension.IsCritical ).Append( ") " );
                            try
                            {
                                if (current.Equals( X509Extensions.ReasonCode ))
                                    stringBuilder.Append( new CrlReason( DerEnumerated.GetInstance( asn1Object ) ) );
                                else if (current.Equals( X509Extensions.CertificateIssuer ))
                                {
                                    stringBuilder.Append( "Certificate issuer: " ).Append( GeneralNames.GetInstance( (Asn1Sequence)asn1Object ) );
                                }
                                else
                                {
                                    stringBuilder.Append( current.Id );
                                    stringBuilder.Append( " value = " ).Append( Asn1Dump.DumpAsString( asn1Object ) );
                                }
                                stringBuilder.Append( newLine );
                            }
                            catch (Exception ex)
                            {
                                stringBuilder.Append( current.Id );
                                stringBuilder.Append( " value = " ).Append( "*****" ).Append( newLine );
                            }
                        }
                        else
                            stringBuilder.Append( newLine );
                    }
                    while (enumerator.MoveNext());
                }
            }
            return stringBuilder.ToString();
        }
    }
}
