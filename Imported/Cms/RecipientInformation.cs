// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Cms.RecipientInformation
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using System;
using System.IO;

namespace Org.BouncyCastle.Cms
{
    public abstract class RecipientInformation
    {
        internal RecipientID rid = new RecipientID();
        internal AlgorithmIdentifier keyEncAlg;
        internal CmsSecureReadable secureReadable;
        private byte[] resultMac;

        internal RecipientInformation( AlgorithmIdentifier keyEncAlg, CmsSecureReadable secureReadable )
        {
            this.keyEncAlg = keyEncAlg;
            this.secureReadable = secureReadable;
        }

        internal string GetContentAlgorithmName() => this.secureReadable.Algorithm.Algorithm.Id;

        public RecipientID RecipientID => this.rid;

        public AlgorithmIdentifier KeyEncryptionAlgorithmID => this.keyEncAlg;

        public string KeyEncryptionAlgOid => this.keyEncAlg.Algorithm.Id;

        public Asn1Object KeyEncryptionAlgParams => this.keyEncAlg.Parameters?.ToAsn1Object();

        internal CmsTypedStream GetContentFromSessionKey( KeyParameter sKey )
        {
            CmsReadable readable = this.secureReadable.GetReadable( sKey );
            try
            {
                return new CmsTypedStream( readable.GetInputStream() );
            }
            catch (IOException ex)
            {
                throw new CmsException( "error getting .", ex );
            }
        }

        public byte[] GetContent( ICipherParameters key )
        {
            try
            {
                return CmsUtilities.StreamToByteArray( this.GetContentStream( key ).ContentStream );
            }
            catch (IOException ex)
            {
                throw new Exception( "unable to parse internal stream: " + ex );
            }
        }

        public byte[] GetMac()
        {
            if (this.resultMac == null)
            {
                object cryptoObject = this.secureReadable.CryptoObject;
                if (cryptoObject is IMac)
                    this.resultMac = MacUtilities.DoFinal( (IMac)cryptoObject );
            }
            return Arrays.Clone( this.resultMac );
        }

        public abstract CmsTypedStream GetContentStream( ICipherParameters key );
    }
}
