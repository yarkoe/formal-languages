namespace Coding
{
    /// <summary>
    /// Представляет собой таблицу кодов с лексемами, знаками операций и другими символами.
    /// </summary>
    public class CodeTable
    {
        public string[] OperationChars { get; } = {":", "(", ")", ".", "*", ";", ",", "#", "[", "]"};
        public string[] Nonterminals { get; set; } = { };
        public string[] Terminals { get; set; } = { };
        public string[] Semantics { get; set; } = { };
    }
}
