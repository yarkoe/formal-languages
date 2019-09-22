namespace Coding
{
    /// <summary>
    /// Представляет собой таблицу кодов с лексемами, знаками операций и другими символами.
    /// </summary>
    class CodeTable
    {
        public char[] commandChars { get; set; } = { };
        public string[] Terminals { get; set; } = { };
        public string[] Nonterminals { get; set; } = { };
        public string[] Semantics { get; set; } = { };
    }
}
