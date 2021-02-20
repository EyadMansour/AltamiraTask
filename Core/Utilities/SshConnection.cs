using Renci.SshNet;
using Renci.SshNet.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Core.Utilities
{
    public struct SshResult
    {
        public string Result { get; set; }
        public string FullResult { get; set; }
        public bool IsError { get; set; }

    }
    public class SshConnection : IDisposable
    {
        private SshClient _sshClient;
        private ShellStream _shell;
        private StreamReader _reader;
        private StreamWriter _writer;

        public bool Connect(string host, string user, string password, int port = 22)
        {

            PasswordAuthenticationMethod pauth = new PasswordAuthenticationMethod(user, password);

            ConnectionInfo connectionInfo = new ConnectionInfo(host, 22, user,
                pauth
            );
            this._sshClient = new SshClient(connectionInfo);
            this._sshClient.Connect();
            if (this._sshClient.IsConnected)
            {
                var terminalMode = new Dictionary<TerminalModes, uint>();
                terminalMode.Add(TerminalModes.ECHO, 53);
                var stream = this._sshClient.CreateShellStream("terminal", 0, 0, 0, 0, 4096, terminalMode);

                this._reader = new StreamReader(stream);
                this._writer = new StreamWriter(stream) { AutoFlush = true };
            }

            return true;
        }

        public SshResult Execute(string cmd, int sleep = 200)
        {
            this._writer.WriteLine(cmd);
            Thread.Sleep(sleep);
            var s = this._reader.ReadToEnd();

            return GetResult(s, cmd);
        }

        public SshResult GetResult(string s, string cmd)
        {
            List<Regex> ignoredWordsList = new List<Regex>()
            {
                new Regex(@"^((.+)\s(.+)>)(.*)$" , RegexOptions.IgnoreCase),
                new Regex("^(Last login).*$", RegexOptions.IgnoreCase),
                //new Regex($"^({cmd}).*$", RegexOptions.IgnoreCase),
                new Regex($"^(RMAN>)(.*)$", RegexOptions.IgnoreCase),
                new Regex($"^(SQL>)(.*)$", RegexOptions.IgnoreCase),
            };

            Regex errorRegex = new Regex("^(.*)((RMAN-[1-9])|ORA-[1-9]|SP2-[1-9]|ERROR|command not found\\.)(.*)$");

            bool isError = false;

            var result = new StringBuilder();
            var b = s.Split("\r\n");
            foreach (var item in b)
            {
                var isMatch = ignoredWordsList.Any(regex => regex.IsMatch(item));
                var isCommand = item.Contains(cmd, StringComparison.InvariantCultureIgnoreCase) || item.StartsWith(cmd);
                if (!isMatch && !isCommand)
                    result.Append(item + '\n');


                if (!isError)
                    isError = errorRegex.IsMatch(item);

            }

            return new SshResult()
            {
                FullResult = s,
                Result = result.ToString(),
                IsError = isError
            };
        }

        public void Dispose()
        {
            _shell?.Dispose();
            _shell?.Close();
            _sshClient?.Disconnect();
            _sshClient?.Dispose();
        }
    }
}
