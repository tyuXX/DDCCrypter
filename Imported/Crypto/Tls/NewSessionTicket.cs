// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.NewSessionTicket
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System.IO;

namespace Org.BouncyCastle.Crypto.Tls
{
    public class NewSessionTicket
    {
        protected readonly long mTicketLifetimeHint;
        protected readonly byte[] mTicket;

        public NewSessionTicket( long ticketLifetimeHint, byte[] ticket )
        {
            this.mTicketLifetimeHint = ticketLifetimeHint;
            this.mTicket = ticket;
        }

        public virtual long TicketLifetimeHint => this.mTicketLifetimeHint;

        public virtual byte[] Ticket => this.mTicket;

        public virtual void Encode( Stream output )
        {
            TlsUtilities.WriteUint32( this.mTicketLifetimeHint, output );
            TlsUtilities.WriteOpaque16( this.mTicket, output );
        }

        public static NewSessionTicket Parse( Stream input ) => new( TlsUtilities.ReadUint32( input ), TlsUtilities.ReadOpaque16( input ) );
    }
}
