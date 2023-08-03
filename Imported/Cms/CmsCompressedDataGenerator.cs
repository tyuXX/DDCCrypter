// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Cms.CmsCompressedDataGenerator
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Cms;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Zlib;
using System.IO;

namespace Org.BouncyCastle.Cms
{
    public class CmsCompressedDataGenerator
    {
        public const string ZLib = "1.2.840.113549.1.9.16.3.8";

        public CmsCompressedData Generate( CmsProcessable content, string compressionOid )
        {
            AlgorithmIdentifier compressionAlgorithm;
            Asn1OctetString content1;
            try
            {
                MemoryStream output = new();
                ZOutputStream zoutputStream = new( output, -1 );
                content.Write( zoutputStream );
                Platform.Dispose( zoutputStream );
                compressionAlgorithm = new AlgorithmIdentifier( new DerObjectIdentifier( compressionOid ) );
                content1 = new BerOctetString( output.ToArray() );
            }
            catch (IOException ex)
            {
                throw new CmsException( "exception encoding data.", ex );
            }
            ContentInfo encapContentInfo = new( CmsObjectIdentifiers.Data, content1 );
            return new CmsCompressedData( new ContentInfo( CmsObjectIdentifiers.CompressedData, new CompressedData( compressionAlgorithm, encapContentInfo ) ) );
        }
    }
}
