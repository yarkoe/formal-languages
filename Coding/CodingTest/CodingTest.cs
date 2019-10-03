using NUnit.Framework;

using Coding;
using System.Collections.Generic;

namespace Tests
{
    public class CodingTestClass
    {
        [Test]
        public void ConvolutionTest()
        {
            CodeTable testCodeTable = new CodeTable();

            testCodeTable.Nonterminals = new string[] { "expression" };
            testCodeTable.Terminals = new string[] { ",", "formula", "operation" };
            testCodeTable.Semantics = new string[] { "opcode1", };

            string testGrammatic = "expression : ( formula )*( ',' ) *($opcode1 'operation').";
            List<int> testList = new List<int> { 11, 1, 2, 13, 3, 5, 2, 12, 3, 5, 2, 15, 14, 3, 4};

            Convolution testConvolution = new Convolution();
            List<int> testConvert = testConvolution.Convert(testGrammatic.Replace(" ", ""), testCodeTable);

            Assert.AreEqual(testConvert, testList);
        }
    }
}