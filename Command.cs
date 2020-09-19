using System;
using System.Collections.Generic;
using System.Linq;

namespace Flexing
{
    enum Token
    {
        Action,
        Target,
        Option,
        Eof
    }

    internal class Command
    {
        private static IDictionary<Token, ISet<string>> _keywords = new Dictionary<Token, ISet<string>>();
        private Token Next = Token.Action;
        public string Action { get; private set; }
        public string Target { get; private set; }
        public string Option { get; private set; }

        public Command()
        {
            _keywords[Token.Action] = new HashSet<string> {"create", "delete", "restart"};
            _keywords[Token.Target] = new HashSet<string> {"service"};
        }

        public Command Build(IEnumerable<string> tokens)
        {
            if (!tokens.Any())
            {
                return this;
            }

            var token = tokens.First().Trim();
            Assign(token);
            return Build(tokens.Skip(1));
        }

        private void Assign(string token)
        {
            if (!IsValidToken(token)) throw new ApplicationException($"Unexpected token {token}");
            switch (Next)
            {
                case Token.Action:
                    Action = token;
                    Next = Token.Target;
                    break;
                case Token.Target:
                    Next = Token.Option;
                    Target = token;
                    break;
                case Token.Option:
                    Option = token;
                    Next = Token.Eof;
                    break;
                case Token.Eof:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private bool IsValidToken(string value)
        {
            return !_keywords.TryGetValue(Next, out var keywords) || keywords.Contains(value.ToLower().Trim());
        }

        public override string ToString()
        {
            return $"Action : {Action} Target : {Target} Option {Option}";
        }
    }
}