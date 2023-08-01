using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace DDCCrypter
{
    public static class Engine
    {
        public static List<Operation> Ops = new List<Operation>() { };
        public static void OpenForm<T>() where T : Form, new()
        {
            T form = new T();
            form.Show();
        }
        public static bool OrCompare<T>( T tc, params T[] tbc )
        {
            foreach (T tbci in tbc)
            {
                if (tc.Equals( tbci )) { return true; }
            }
            return false;
        }
        public static bool AndCompare<T>( T tc, T[] tbc )
        {
            foreach (T tbci in tbc)
            {
                if (!tc.Equals( tbci )) { return false; }
            }
            return true;
        }
        public static string GenerateHash()
        {
            using (Aes aes = Aes.Create())
            {
                aes.GenerateKey();
                return Convert.ToBase64String( aes.Key );
            }
        }
        private static string[] DivideString( string str, int chunkSize )
        {
            return Enumerable.Range( 0, (int)Math.Ceiling( (double)str.Length / chunkSize ) ).Select( i => str.Substring( i * chunkSize, Math.Min( chunkSize, str.Length - (i * chunkSize) ) ) ).ToArray();
        }
        public static string ReadFromFile( string file, string passcode )
        {
            List<string> _ = new List<string>() { };
            _.Add( "estring=" + File.ReadAllText( file ) );
            _.Add( "hash=" + passcode );
            _.Add( "do=true" );
            return ArgProcess( _ ).Item1;
        }
        public static (string, TimeSpan, bool, ArgStore) Process( List<string> sargs )
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            (string, ArgStore) rtstr = new ValueTuple<string, ArgStore>();
            try
            {
                rtstr = ArgProcess( sargs );
            }
            catch (Exception)
            {
                stopwatch.Stop();
                return (rtstr.Item1, stopwatch.Elapsed, false, rtstr.Item2);
            }
            stopwatch.Stop();
            return (rtstr.Item1, stopwatch.Elapsed, true, rtstr.Item2);
        }
        private static (string, ArgStore) ArgProcess( List<string> sargs )
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
                        return ("error", _args);
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
                            return (Selector( _args ), _args);
                        }
                    }
                    catch
                    {
                        return ("error", _args);
                    }
                }
                else
                {
                    ArgStore _args = new ArgStore( true );
                    _args.Add( args.GetArg( "type" ) );
                    //TODO - Continue
                }
            }
            return (Selector( args ), args);
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
                case nameof( EAES ):
                    {
                        return EAES( args );
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
        private static string EAES( ArgStore args )
        {
            byte[] keyBytes = Convert.FromBase64String( args.GetArgValue( "hash" ) );
            if (bool.Parse( args.GetArgValue( "do" ) ))
            {
                byte[] textBytes = Encoding.UTF8.GetBytes( args.GetArgValue( "estring" ) );
                using (Aes aes = Aes.Create())
                {
                    aes.Key = keyBytes;
                    aes.GenerateIV();
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream( ms, aes.CreateEncryptor(), CryptoStreamMode.Write ))
                        {
                            cs.Write( textBytes, 0, textBytes.Length );
                        }
                        byte[] _ = ms.ToArray();
                        byte[] ivAndData = new byte[aes.IV.Length + _.Length];
                        Array.Copy( aes.IV, ivAndData, aes.IV.Length );
                        Array.Copy( _, 0, ivAndData, aes.IV.Length, _.Length );

                        return Convert.ToBase64String( ivAndData );
                    }
                }
            }
            byte[] encryptedBytes = Convert.FromBase64String( args.GetArgValue( "estring" ) );
            using (Aes aes = Aes.Create())
            {
                byte[] iv = new byte[aes.IV.Length];
                Array.Copy( encryptedBytes, iv, iv.Length );
                byte[] dataToDecrypt = new byte[encryptedBytes.Length - iv.Length];
                Array.Copy( encryptedBytes, iv.Length, dataToDecrypt, 0, dataToDecrypt.Length );
                aes.Key = keyBytes;
                aes.IV = iv;
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream( ms, aes.CreateDecryptor(), CryptoStreamMode.Write ))
                    {
                        cs.Write( dataToDecrypt, 0, dataToDecrypt.Length );
                    }
                    byte[] decryptedBytes = ms.ToArray();
                    return Encoding.UTF8.GetString( decryptedBytes );
                }
            }
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
        public override string ToString()
        {
            return $"{arg}={value}";
        }
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
        public List<Arg> args;
    }
    public struct Operation
    {
        public Guid UUID;
        public (string, TimeSpan, bool) Output;
        public ArgStore Args;
        public Operation( (string, TimeSpan, bool, ArgStore) OpOutput )
        {
            UUID = Guid.NewGuid();
            Output = (OpOutput.Item1, OpOutput.Item2, OpOutput.Item3);
            Args = OpOutput.Item4;
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine( "UUID:" + UUID.ToString() );
            sb.AppendLine( "Output String:" + Output.Item1 );
            sb.AppendLine( "Time:" + Output.Item2.Milliseconds + "ms" );
            sb.AppendLine( "Sucksess:" + Output.Item3 );
            sb.AppendLine( "Args:" );
            foreach (Arg arg in Args.args)
            {
                sb.AppendLine( arg.ToString() );
            }
            return sb.ToString();
        }
    }
}
