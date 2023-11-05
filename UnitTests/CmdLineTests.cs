using System;
using System.Security.Principal;
using ConvertLib;
using NUnit.Framework.Internal;
using Utils;

namespace UnitTests
{
    public class CmdLineTests
    {
        static readonly string[] args = new string[5]
        {
            "-file = TestFile.txt",
            "-prettyprint",
            "-- ugly =formatted",
            "-happy=TrUe",
            "-age=21"
        };

        private CmdLine? cmd;

        [SetUp]
        public void Setup()
        {
            try
            {
                cmd = new(args);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public void Find_Arguments()
        {
            Assert.That(cmd!.FindSwitch("file"), Is.EqualTo(true));
            Assert.That(cmd!.FindSwitch("prettyprint"), Is.EqualTo(true));
            Assert.That(cmd!.FindSwitch("ugly"), Is.EqualTo(true));
        }

        [Test]
        public void Find_String_Values()
        {
            Assert.That(cmd!.GetString("file"), Is.EqualTo("TestFile.txt"));
            Assert.That(cmd!.GetString("file"), Is.Not.EqualTo("testfile.txt"));

            Assert.That(cmd!.GetString("prettyprint"), Is.EqualTo(string.Empty));
            Assert.That(cmd!.GetString("ugly"), Is.EqualTo("formatted"));
        }

        [Test]
        public void Boolean_Values()
        {
            Assert.That(cmd!.GetBool("happy"), Is.True);
            Assert.Throws<FormatException>(() => cmd!.GetBool("age"));
        }

        [Test]
        public void Integer_Values()
        {
            Assert.Throws<FormatException>(() => cmd!.GetNumber("happy"));
            Assert.That(cmd!.GetNumber("age"), Is.EqualTo(21));
        }
    }
}

