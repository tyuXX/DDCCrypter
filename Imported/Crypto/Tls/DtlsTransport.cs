// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.DtlsTransport
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System;
using System.IO;

namespace Org.BouncyCastle.Crypto.Tls
{
    public class DtlsTransport : DatagramTransport
    {
        private readonly DtlsRecordLayer mRecordLayer;

        internal DtlsTransport( DtlsRecordLayer recordLayer ) => this.mRecordLayer = recordLayer;

        public virtual int GetReceiveLimit() => this.mRecordLayer.GetReceiveLimit();

        public virtual int GetSendLimit() => this.mRecordLayer.GetSendLimit();

        public virtual int Receive( byte[] buf, int off, int len, int waitMillis )
        {
            try
            {
                return this.mRecordLayer.Receive( buf, off, len, waitMillis );
            }
            catch (TlsFatalAlert ex)
            {
                this.mRecordLayer.Fail( ex.AlertDescription );
                throw ex;
            }
            catch (IOException ex)
            {
                this.mRecordLayer.Fail( 80 );
                throw ex;
            }
            catch (Exception ex)
            {
                this.mRecordLayer.Fail( 80 );
                throw new TlsFatalAlert( 80, ex );
            }
        }

        public virtual void Send( byte[] buf, int off, int len )
        {
            try
            {
                this.mRecordLayer.Send( buf, off, len );
            }
            catch (TlsFatalAlert ex)
            {
                this.mRecordLayer.Fail( ex.AlertDescription );
                throw ex;
            }
            catch (IOException ex)
            {
                this.mRecordLayer.Fail( 80 );
                throw ex;
            }
            catch (Exception ex)
            {
                this.mRecordLayer.Fail( 80 );
                throw new TlsFatalAlert( 80, ex );
            }
        }

        public virtual void Close() => this.mRecordLayer.Close();
    }
}
