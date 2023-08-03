// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.X509.X509Certificate
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Misc;
using Org.BouncyCastle.Asn1.Utilities;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Operators;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Security.Certificates;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Encoders;
using Org.BouncyCastle.X509.Extension;
using System;
using System.Collections;
using System.Text;

namespace Org.BouncyCastle.X509
{
    public class X509Certificate : X509ExtensionBase
    {
        private readonly X509CertificateStructure c;
        private readonly BasicConstraints basicConstraints;
        private readonly bool[] keyUsage;
        private bool hashValueSet;
        private int hashValue;

        protected X509Certificate()
        {
        }

        public X509Certificate( X509CertificateStructure c )
        {
            this.c = c;
            try
            {
                Asn1OctetString extensionValue = this.GetExtensionValue( new DerObjectIdentifier( "2.5.29.19" ) );
                if (extensionValue != null)
                    this.basicConstraints = BasicConstraints.GetInstance( X509ExtensionUtilities.FromExtensionValue( extensionValue ) );
            }
            catch (Exception ex)
            {
                throw new CertificateParsingException( "cannot construct BasicConstraints: " + ex );
            }
            try
            {
                Asn1OctetString extensionValue = this.GetExtensionValue( new DerObjectIdentifier( "2.5.29.15" ) );
                if (extensionValue != null)
                {
                    DerBitString instance = DerBitString.GetInstance( X509ExtensionUtilities.FromExtensionValue( extensionValue ) );
                    byte[] bytes = instance.GetBytes();
                    int num = (bytes.Length * 8) - instance.PadBits;
                    this.keyUsage = new bool[num < 9 ? 9 : num];
                    for (int index = 0; index != num; ++index)
                        this.keyUsage[index] = (bytes[index / 8] & (128 >> (index % 8))) != 0;
                }
                else
                    this.keyUsage = null;
            }
            catch (Exception ex)
            {
                throw new CertificateParsingException( "cannot construct KeyUsage: " + ex );
            }
        }

        public virtual X509CertificateStructure CertificateStructure => this.c;

        public virtual bool IsValidNow => this.IsValid( DateTime.UtcNow );

        public virtual bool IsValid( DateTime time ) => time.CompareTo( (object)this.NotBefore ) >= 0 && time.CompareTo( (object)this.NotAfter ) <= 0;

        public virtual void CheckValidity() => this.CheckValidity( DateTime.UtcNow );

        public virtual void CheckValidity( DateTime time )
        {
            if (time.CompareTo( (object)this.NotAfter ) > 0)
                throw new CertificateExpiredException( "certificate expired on " + this.c.EndDate.GetTime() );
            if (time.CompareTo( (object)this.NotBefore ) < 0)
                throw new CertificateNotYetValidException( "certificate not valid until " + this.c.StartDate.GetTime() );
        }

        public virtual int Version => this.c.Version;

        public virtual BigInteger SerialNumber => this.c.SerialNumber.Value;

        public virtual X509Name IssuerDN => this.c.Issuer;

        public virtual X509Name SubjectDN => this.c.Subject;

        public virtual DateTime NotBefore => this.c.StartDate.ToDateTime();

        public virtual DateTime NotAfter => this.c.EndDate.ToDateTime();

        public virtual byte[] GetTbsCertificate() => this.c.TbsCertificate.GetDerEncoded();

        public virtual byte[] GetSignature() => this.c.GetSignatureOctets();

        public virtual string SigAlgName => SignerUtilities.GetEncodingName( this.c.SignatureAlgorithm.Algorithm );

        public virtual string SigAlgOid => this.c.SignatureAlgorithm.Algorithm.Id;

        public virtual byte[] GetSigAlgParams() => this.c.SignatureAlgorithm.Parameters != null ? this.c.SignatureAlgorithm.Parameters.GetDerEncoded() : null;

        public virtual DerBitString IssuerUniqueID => this.c.TbsCertificate.IssuerUniqueID;

        public virtual DerBitString SubjectUniqueID => this.c.TbsCertificate.SubjectUniqueID;

        public virtual bool[] GetKeyUsage() => this.keyUsage != null ? (bool[])this.keyUsage.Clone() : null;

        public virtual IList GetExtendedKeyUsage()
        {
            Asn1OctetString extensionValue = this.GetExtensionValue( new DerObjectIdentifier( "2.5.29.37" ) );
            if (extensionValue == null)
                return null;
            try
            {
                Asn1Sequence instance = Asn1Sequence.GetInstance( X509ExtensionUtilities.FromExtensionValue( extensionValue ) );
                IList arrayList = Platform.CreateArrayList();
                foreach (DerObjectIdentifier objectIdentifier in instance)
                    arrayList.Add( objectIdentifier.Id );
                return arrayList;
            }
            catch (Exception ex)
            {
                throw new CertificateParsingException( "error processing extended key usage extension", ex );
            }
        }

        public virtual int GetBasicConstraints()
        {
            if (this.basicConstraints == null || !this.basicConstraints.IsCA())
                return -1;
            return this.basicConstraints.PathLenConstraint == null ? int.MaxValue : this.basicConstraints.PathLenConstraint.IntValue;
        }

        public virtual ICollection GetSubjectAlternativeNames() => this.GetAlternativeNames( "2.5.29.17" );

        public virtual ICollection GetIssuerAlternativeNames() => this.GetAlternativeNames( "2.5.29.18" );

        protected virtual ICollection GetAlternativeNames( string oid )
        {
            Asn1OctetString extensionValue = this.GetExtensionValue( new DerObjectIdentifier( oid ) );
            if (extensionValue == null)
                return null;
            GeneralNames instance = GeneralNames.GetInstance( X509ExtensionUtilities.FromExtensionValue( extensionValue ) );
            IList arrayList1 = Platform.CreateArrayList();
            foreach (GeneralName name in instance.GetNames())
            {
                IList arrayList2 = Platform.CreateArrayList();
                arrayList2.Add( name.TagNo );
                arrayList2.Add( name.Name.ToString() );
                arrayList1.Add( arrayList2 );
            }
            return arrayList1;
        }

        protected override X509Extensions GetX509Extensions() => this.c.Version < 3 ? null : this.c.TbsCertificate.Extensions;

        public virtual AsymmetricKeyParameter GetPublicKey() => PublicKeyFactory.CreateKey( this.c.SubjectPublicKeyInfo );

        public virtual byte[] GetEncoded() => this.c.GetDerEncoded();

        public override bool Equals( object obj )
        {
            if (obj == this)
                return true;
            return obj is X509Certificate x509Certificate && this.c.Equals( x509Certificate.c );
        }

        public override int GetHashCode()
        {
            lock (this)
            {
                if (!this.hashValueSet)
                {
                    this.hashValue = this.c.GetHashCode();
                    this.hashValueSet = true;
                }
            }
            return this.hashValue;
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            string newLine = Platform.NewLine;
            stringBuilder.Append( "  [0]         Version: " ).Append( this.Version ).Append( newLine );
            stringBuilder.Append( "         SerialNumber: " ).Append( SerialNumber ).Append( newLine );
            stringBuilder.Append( "             IssuerDN: " ).Append( IssuerDN ).Append( newLine );
            stringBuilder.Append( "           Start Date: " ).Append( NotBefore ).Append( newLine );
            stringBuilder.Append( "           Final Date: " ).Append( NotAfter ).Append( newLine );
            stringBuilder.Append( "            SubjectDN: " ).Append( SubjectDN ).Append( newLine );
            stringBuilder.Append( "           Public Key: " ).Append( this.GetPublicKey() ).Append( newLine );
            stringBuilder.Append( "  Signature Algorithm: " ).Append( this.SigAlgName ).Append( newLine );
            byte[] signature = this.GetSignature();
            stringBuilder.Append( "            Signature: " ).Append( Hex.ToHexString( signature, 0, 20 ) ).Append( newLine );
            for (int off = 20; off < signature.Length; off += 20)
            {
                int length = System.Math.Min( 20, signature.Length - off );
                stringBuilder.Append( "                       " ).Append( Hex.ToHexString( signature, off, length ) ).Append( newLine );
            }
            X509Extensions extensions = this.c.TbsCertificate.Extensions;
            if (extensions != null)
            {
                IEnumerator enumerator = extensions.ExtensionOids.GetEnumerator();
                if (enumerator.MoveNext())
                    stringBuilder.Append( "       Extensions: \n" );
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
                            if (current.Equals( X509Extensions.BasicConstraints ))
                                stringBuilder.Append( BasicConstraints.GetInstance( asn1Object ) );
                            else if (current.Equals( X509Extensions.KeyUsage ))
                                stringBuilder.Append( KeyUsage.GetInstance( asn1Object ) );
                            else if (current.Equals( MiscObjectIdentifiers.NetscapeCertType ))
                                stringBuilder.Append( new NetscapeCertType( (DerBitString)asn1Object ) );
                            else if (current.Equals( MiscObjectIdentifiers.NetscapeRevocationUrl ))
                                stringBuilder.Append( new NetscapeRevocationUrl( (DerIA5String)asn1Object ) );
                            else if (current.Equals( MiscObjectIdentifiers.VerisignCzagExtension ))
                            {
                                stringBuilder.Append( new VerisignCzagExtension( (DerIA5String)asn1Object ) );
                            }
                            else
                            {
                                stringBuilder.Append( current.Id );
                                stringBuilder.Append( " value = " ).Append( Asn1Dump.DumpAsString( asn1Object ) );
                            }
                        }
                        catch (Exception ex)
                        {
                            stringBuilder.Append( current.Id );
                            stringBuilder.Append( " value = " ).Append( "*****" );
                        }
                    }
                    stringBuilder.Append( newLine );
                }
                while (enumerator.MoveNext());
            }
            return stringBuilder.ToString();
        }

        public virtual void Verify( AsymmetricKeyParameter key ) => this.CheckSignature( new Asn1VerifierFactory( this.c.SignatureAlgorithm, key ) );

        public virtual void Verify( IVerifierFactoryProvider verifierProvider ) => this.CheckSignature( verifierProvider.CreateVerifierFactory( c.SignatureAlgorithm ) );

        protected virtual void CheckSignature( IVerifierFactory verifier )
        {
            if (!IsAlgIDEqual( this.c.SignatureAlgorithm, this.c.TbsCertificate.Signature ))
                throw new CertificateException( "signature algorithm in TBS cert not same as outer cert" );
            Asn1Encodable parameters = this.c.SignatureAlgorithm.Parameters;
            IStreamCalculator calculator = verifier.CreateCalculator();
            byte[] tbsCertificate = this.GetTbsCertificate();
            calculator.Stream.Write( tbsCertificate, 0, tbsCertificate.Length );
            Platform.Dispose( calculator.Stream );
            if (!((IVerifier)calculator.GetResult()).IsVerified( this.GetSignature() ))
                throw new InvalidKeyException( "Public key presented not for certificate signature" );
        }

        private static bool IsAlgIDEqual( AlgorithmIdentifier id1, AlgorithmIdentifier id2 )
        {
            if (!id1.Algorithm.Equals( id2.Algorithm ))
                return false;
            Asn1Encodable parameters1 = id1.Parameters;
            Asn1Encodable parameters2 = id2.Parameters;
            if (parameters1 == null == (parameters2 == null))
                return Equals( parameters1, parameters2 );
            return parameters1 != null ? parameters1.ToAsn1Object() is Asn1Null : parameters2.ToAsn1Object() is Asn1Null;
        }
    }
}
