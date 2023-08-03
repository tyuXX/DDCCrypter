// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Cms.CmsCompressedData
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Cms;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Zlib;
using System.IO;

namespace Org.BouncyCastle.Cms
{
    public class CmsCompressedData
    {
        internal ContentInfo contentInfo;

        public CmsCompressedData( byte[] compressedData )
          : this( CmsUtilities.ReadContentInfo( compressedData ) )
        {
        }

        public CmsCompressedData( Stream compressedDataStream )
          : this( CmsUtilities.ReadContentInfo( compressedDataStream ) )
        {
        }

        public CmsCompressedData( ContentInfo contentInfo ) => this.contentInfo = contentInfo;

        public byte[] GetContent()
        {
            ZInputStream zinputStream = new( ((Asn1OctetString)CompressedData.GetInstance( contentInfo.Content ).EncapContentInfo.Content).GetOctetStream() );
            try
            {
                return CmsUtilities.StreamToByteArray( zinputStream );
            }
            catch (IOException ex)
            {
                throw new CmsException( "exception reading compressed stream.", ex );
            }
            finally
            {
                Platform.Dispose( zinputStream );
            }
        }

        public byte[] GetContent( int limit )
        {
            ZInputStream inStream = new( new MemoryStream( ((Asn1OctetString)CompressedData.GetInstance( contentInfo.Content ).EncapContentInfo.Content).GetOctets(), false ) );
            try
            {
                return CmsUtilities.StreamToByteArray( inStream, limit );
            }
            catch (IOException ex)
            {
                throw new CmsException( "exception reading compressed stream.", ex );
            }
        }

        public ContentInfo ContentInfo => this.contentInfo;

        public byte[] GetEncoded() => this.contentInfo.GetEncoded();
    }
}
