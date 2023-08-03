// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.X509.AttributeCertificateIssuer
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.X509.Store;
using System;

namespace Org.BouncyCastle.X509
{
    public class AttributeCertificateIssuer : IX509Selector, ICloneable
    {
        internal readonly Asn1Encodable form;

        public AttributeCertificateIssuer( AttCertIssuer issuer ) => this.form = issuer.Issuer;

        public AttributeCertificateIssuer( X509Name principal ) => this.form = new V2Form( new GeneralNames( new GeneralName( principal ) ) );

        private object[] GetNames()
        {
            GeneralName[] names1 = (!(this.form is V2Form) ? (GeneralNames)this.form : ((V2Form)this.form).IssuerName).GetNames();
            int length = 0;
            for (int index = 0; index != names1.Length; ++index)
            {
                if (names1[index].TagNo == 4)
                    ++length;
            }
            object[] names2 = new object[length];
            int num = 0;
            for (int index = 0; index != names1.Length; ++index)
            {
                if (names1[index].TagNo == 4)
                    names2[num++] = X509Name.GetInstance( names1[index].Name );
            }
            return names2;
        }

        public X509Name[] GetPrincipals()
        {
            object[] names = this.GetNames();
            int length = 0;
            for (int index = 0; index != names.Length; ++index)
            {
                if (names[index] is X509Name)
                    ++length;
            }
            X509Name[] principals = new X509Name[length];
            int num = 0;
            for (int index = 0; index != names.Length; ++index)
            {
                if (names[index] is X509Name)
                    principals[num++] = (X509Name)names[index];
            }
            return principals;
        }

        private bool MatchesDN( X509Name subject, GeneralNames targets )
        {
            GeneralName[] names = targets.GetNames();
            for (int index = 0; index != names.Length; ++index)
            {
                GeneralName generalName = names[index];
                if (generalName.TagNo == 4)
                {
                    try
                    {
                        if (X509Name.GetInstance( generalName.Name ).Equivalent( subject ))
                            return true;
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            return false;
        }

        public object Clone() => new AttributeCertificateIssuer( AttCertIssuer.GetInstance( form ) );

        public bool Match( X509Certificate x509Cert )
        {
            if (!(this.form is V2Form))
                return this.MatchesDN( x509Cert.SubjectDN, (GeneralNames)this.form );
            V2Form form = (V2Form)this.form;
            if (form.BaseCertificateID == null)
                return this.MatchesDN( x509Cert.SubjectDN, form.IssuerName );
            return form.BaseCertificateID.Serial.Value.Equals( x509Cert.SerialNumber ) && this.MatchesDN( x509Cert.IssuerDN, form.BaseCertificateID.Issuer );
        }

        public override bool Equals( object obj )
        {
            if (obj == this)
                return true;
            return obj is AttributeCertificateIssuer && this.form.Equals( ((AttributeCertificateIssuer)obj).form );
        }

        public override int GetHashCode() => this.form.GetHashCode();

        public bool Match( object obj ) => obj is X509Certificate && this.Match( (X509Certificate)obj );
    }
}
