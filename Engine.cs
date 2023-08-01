using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace DDCCrypter
{
    public static class Engine
    {
        static string[] DivideString( string str, int chunkSize )
        {
            return Enumerable.Range( 0, (int)Math.Ceiling( (double)str.Length / chunkSize ) ).Select( i => str.Substring( i * chunkSize, Math.Min( chunkSize, str.Length - i * chunkSize ) ) ).ToArray();
        }
        public static string ReadFromFile( string file, string passcode )
        {
            List<string> _ = new List<string>() { };
            _.Add( "estring=" + File.ReadAllText( file ) );
            _.Add( "hash=" + passcode );
            _.Add( "do=true" );
            return ArgProcess( _ );
        }
        public static (string, TimeSpan) Process( List<string> sargs )
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            string rtstr = ArgProcess( sargs );
            stopwatch.Stop();
            return (rtstr, stopwatch.Elapsed);
        }
        private static string ArgProcess( List<string> sargs )
        {
            ArgStore args = new ArgStore( true );
            foreach (string str in sargs)
            {
                string[] strs = str.Split( new char[] { '=' }, 2 );
                if (strs.Length == 2)
                {
                    args.Add( new Arg( strs[0], strs[1] ) );
                }
            }
            if (sargs.Contains( "trim" ))
            {
                args.SetArgValue( "estring", args.GetArgValue( "estring" ).Replace( " ", string.Empty ).Replace( "\n", string.Empty ) );
            }
            if (sargs.Contains( "file" ))
            {
                if (bool.Parse( args.GetArgValue( "do" ) ))
                {
                    ArgStore _args = new ArgStore( true );
                    string[] strs = args.GetArgValue( "estring" ).Split( new char[] { '\n' }, 3 );
                    if (strs.Length < 3)
                    {
                        return "error";
                    }
                    _args.Add( args.GetArg( "hash" ) );
                    _args.Add( new Arg( "estring", strs[0] ) );
                    _args.Add( new Arg( "type", strs[1] ) );
                    _args.Add( args.GetArg( "do" ) );
                    try
                    {
                        if (Selector( _args ) == args.GetArgValue( "hash" ))
                        {
                            _args.Remove( _args.GetArg( "estring" ) );
                            _args.Add( new Arg( "estring", strs[2] ) );
                            return Selector( _args );
                        }
                    }
                    catch
                    {
                        return "error";
                    }
                }
                else
                {
                    ArgStore _args = new ArgStore( true );
                    _args.Add( args.GetArg( "type" ) );
                    //TODO - Continue
                }
            }
            return Selector( args );
        }
        private static string Selector( ArgStore args )
        {
            switch ('E' + args.GetArgValue( "type" ))
            {
                case nameof( EMD5 ):
                    {
                        return EMD5( args );
                    }
                case nameof( ESHA256 ):
                    {
                        return ESHA256( args );
                    }
                case nameof( ESHA1 ):
                    {
                        return ESHA1( args );
                    }
                case nameof( ESHA384 ):
                    {
                        return ESHA384( args );
                    }
                case nameof( ESHA512 ):
                    {
                        return ESHA512( args );
                    }
                case nameof( EBASE64 ):
                    {
                        return EBASE64( args );
                    }
                case nameof( EBİNARY ):
                    {
                        return EBİNARY( args );
                    }
                case nameof( EASCIITOUTF8 ):
                    {
                        return EASCIITOUTF8( args );
                    }
                case nameof( EUTF8TOASCII ):
                    {
                        return EUTF8TOASCII( args );
                    }
                case nameof( EMORSECODE ):
                    {
                        return EMORSECODE( args );
                    }
                default:
                    {
                        return "";
                    }
            }
        }
        private static string EBASE64( ArgStore args )
        {
            if (bool.Parse( args.GetArgValue( "do" ) ))
            {
                return Convert.ToBase64String( Encoding.UTF8.GetBytes( args.GetArgValue( "estring" ) ) );
            }
            return Encoding.UTF8.GetString( Convert.FromBase64String( args.GetArgValue( "estring" ) ) );
        }
        private static string EMORSECODE( ArgStore args )
        {
            StringBuilder sb = new StringBuilder();
            if (bool.Parse( args.GetArgValue( "do" ) ))
            {
                foreach (char chr in args.GetArgValue( "estring" ).ToUpper())
                {
                    string _ = chr.ToString();
                    Dict.Morse.TryGetValue( chr, out _ );
                    sb.Append( _ + '/' );
                }
                return sb.ToString().TrimEnd( '/' );
            }
            string[] strs = args.GetArgValue( "estring" ).Split( '/' );
            Dictionary<string, char> __ = Dict.ReverseMorse;
            foreach (string str in strs)
            {
                char _ = ' ';
                __.TryGetValue( str, out _ );
                sb.Append( _ );
            }
            return sb.ToString().ToLower();
        }
        private static string EBİNARY( ArgStore args )
        {
            StringBuilder sb = new StringBuilder();
            if (bool.Parse( args.GetArgValue( "do" ) ))
            {

                foreach (char chr in args.GetArgValue( "estring" ))
                {
                    sb.Append( Convert.ToString( chr, 2 ).PadLeft( 8, '0' ) );
                }
                return sb.ToString();
            }
            string[] strs = DivideString( args.GetArgValue( "estring" ), 8 );
            foreach (string str in strs)
            {
                sb.Append( Convert.ToChar( Convert.ToInt32( str.TrimStart( '0' ), 2 ) ) );
            }
            return sb.ToString();
        }
        private static string EASCIITOUTF8( ArgStore args )
        {
            return Encoding.UTF8.GetString( Encoding.ASCII.GetBytes( args.GetArgValue( "estring" ) ) );
        }
        private static string EUTF8TOASCII( ArgStore args )
        {
            return Encoding.ASCII.GetString( Encoding.UTF8.GetBytes( args.GetArgValue( "estring" ) ) );
        }
        private static string EMD5( ArgStore args )
        {
            if (bool.Parse( args.GetArgValue( "do" ) ))
            {
                byte[] data = Encoding.UTF8.GetBytes( args.GetArgValue( "estring" ) );
                using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
                {
                    byte[] keys = md5.ComputeHash( Encoding.UTF8.GetBytes( args.GetArgValue( "hash" ) ) );
                    using (TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                    {
                        ICryptoTransform transform = tripDes.CreateEncryptor();
                        byte[] results = transform.TransformFinalBlock( data, 0, data.Length );
                        return Convert.ToBase64String( results, 0, results.Length );
                    }
                }
            }
            else
            {
                byte[] data = Convert.FromBase64String( args.GetArgValue( "estring" ) );
                using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
                {
                    byte[] keys = md5.ComputeHash( Encoding.UTF8.GetBytes( args.GetArgValue( "hash" ) ) );
                    using (TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                    {
                        ICryptoTransform transform = tripDes.CreateDecryptor();
                        byte[] results = transform.TransformFinalBlock( data, 0, data.Length );
                        return Encoding.UTF8.GetString( results );
                    }
                }
            }
        }
        private static string ESHA256( ArgStore args )
        {
            if (!bool.Parse( args.GetArgValue( "do" ) )) { return ""; }
            return BitConverter.ToString( new SHA256Managed().ComputeHash( Encoding.UTF8.GetBytes( args.GetArgValue( "estring" ) ) ) ).Replace( "-", string.Empty );
        }
        private static string ESHA1( ArgStore args )
        {
            if (!bool.Parse( args.GetArgValue( "do" ) )) { return ""; }
            return BitConverter.ToString( new SHA1Managed().ComputeHash( Encoding.UTF8.GetBytes( args.GetArgValue( "estring" ) ) ) ).Replace( "-", string.Empty );
        }
        private static string ESHA384( ArgStore args )
        {
            if (!bool.Parse( args.GetArgValue( "do" ) )) { return ""; }
            return BitConverter.ToString( new SHA384Managed().ComputeHash( Encoding.UTF8.GetBytes( args.GetArgValue( "estring" ) ) ) ).Replace( "-", string.Empty );
        }
        private static string ESHA512( ArgStore args )
        {
            if (!bool.Parse( args.GetArgValue( "do" ) )) { return ""; }
            return BitConverter.ToString( new SHA512Managed().ComputeHash( Encoding.UTF8.GetBytes( args.GetArgValue( "estring" ) ) ) ).Replace( "-", string.Empty );
        }
    }
    public static class Dict
    {
        private static Dictionary<string, char> ReverseDict( Dictionary<char, string> dict )
        {
            Dictionary<string, char> _ = new Dictionary<string, char> { };
            foreach (KeyValuePair<char, string> kvp in dict)
            {
                _.Add( kvp.Value, kvp.Key );
            }
            return _;
        }
        public static Dictionary<string, char> ReverseMorse { get => ReverseDict( Morse ); }
        public static Dictionary<char, string> Morse = new Dictionary<char, string> { { 'A', ".-" }, { 'B', "-..." }, { 'C', "-.-." }, { 'D', "-.." }, { 'E', "." }, { 'F', "..-." }, { 'G', "--." }, { 'H', "...." }, { 'I', ".." }, { 'J', ".---" }, { 'K', "-.-" }, { 'L', ".-.." }, { 'M', "--" }, { 'N', "-." }, { 'O', "---" }, { 'P', ".--." }, { 'Q', "--.-" }, { 'R', ".-." }, { 'S', "..." }, { 'T', "-" }, { 'U', "..-" }, { 'V', "...-" }, { 'W', ".--" }, { 'X', "-..-" }, { 'Y', "-.--" }, { 'Z', "--.." }, { '0', "-----" }, { '1', ".----" }, { '2', "..---" }, { '3', "...--" }, { '4', "....-" }, { '5', "....." }, { '6', "-...." }, { '7', "--..." }, { '8', "---.." }, { '9', "----." }, { ' ', " " } };
    }
    public struct Arg
    {
        public Arg( string arg, string value )
        {
            this.arg = arg;
            this.value = value;
        }
        public string arg;
        public string value;
    }
    public struct ArgStore
    {
        public string GetArgValue( string Name )
        {
            foreach (Arg arg in args)
            {
                if (arg.arg == Name)
                {
                    return arg.value;
                }
            }
            return "CFA";
        }
        public Arg GetArg( string Name )
        {
            foreach (Arg arg in args)
            {
                if (arg.arg == Name)
                {
                    return arg;
                }
            }
            return new Arg();
        }
        public void SetArgValue( string Name, string value )
        {
            args[args.IndexOf( GetArg( Name ) )] = new Arg( GetArg( Name ).arg, value );
        }
        public void Add( Arg arg )
        {
            args.Add( arg );
        }
        public void Remove( Arg arg )
        {
            args.Remove( arg );
        }
        public ArgStore( List<Arg> args )
        {
            this.args = args;
        }
        public ArgStore( bool gen = true )
        {
            args = new List<Arg> { };
        }
        public Array GetArray { get { return args.ToArray(); } }
        private List<Arg> args;
    }
}
