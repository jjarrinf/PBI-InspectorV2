﻿using PBIRInspectorWinLibrary.Utils;

#pragma warning disable CS8602 
namespace PBIRInspectorTests
{
    public class CLIArgsUtilsTest
    {
        //[Test]
        //public void TestCLIArgsUtilsSuccess()
        //{
        //    string[] args = "-pbix pbixPath -rules rulesPath -verbose true".Split(" ");
        //    var parsedArgs = ArgsUtils.ParseArgs(args);

        //    Assert.True(parsedArgs.PBIFilePath.Equals("pbixPath", StringComparison.OrdinalIgnoreCase) && parsedArgs.RulesFilePath.Equals("rulesPath", StringComparison.OrdinalIgnoreCase) && parsedArgs.Verbose);
        //}

        //[Test]
        //public void TestCLIArgsUtilsSwappedParamsSuccess()
        //{
        //    string[] args = "-rules rulesPath -pbix pbixPath -verbose true".Split(" ");
        //    var parsedArgs = ArgsUtils.ParseArgs(args);

        //    Assert.True(parsedArgs.PBIFilePath.Equals("pbixPath", StringComparison.OrdinalIgnoreCase) && parsedArgs.RulesFilePath.Equals("rulesPath", StringComparison.OrdinalIgnoreCase) && parsedArgs.Verbose);
        //}

        [Test]
        public void TestCLIArgsUtilsSuccess_PBIPOption()
        {
            string[] args = "-pbip pbipPath -rules rulesPath -verbose true".Split(" ");
            var parsedArgs = ArgsUtils.ParseArgs(args);

            Assert.That(parsedArgs.PBIFilePath.Equals("pbipPath", StringComparison.OrdinalIgnoreCase) && parsedArgs.RulesFilePath.Equals("rulesPath", StringComparison.OrdinalIgnoreCase) && parsedArgs.Verbose);
        }

        [Test]
        public void TestCLIArgsUtilsSuccess_PBIPOption2()
        {
            string[] args = "-pbipreport pbipPath -rules rulesPath -verbose true".Split(" ");
            var parsedArgs = ArgsUtils.ParseArgs(args);

            Assert.That(parsedArgs.PBIFilePath.Equals("pbipPath", StringComparison.OrdinalIgnoreCase) && parsedArgs.RulesFilePath.Equals("rulesPath", StringComparison.OrdinalIgnoreCase) && parsedArgs.Verbose);
        }

        [Test]
        public void TestCLIArgsUtilsSuccess_VerboseOptionMissing()
        {
            string[] args = "-pbipreport pbipPath -rules rulesPath".Split(" ");
            var parsedArgs = ArgsUtils.ParseArgs(args);

            Assert.That(parsedArgs.PBIFilePath.Equals("pbipPath", StringComparison.OrdinalIgnoreCase) && parsedArgs.RulesFilePath.Equals("rulesPath", StringComparison.OrdinalIgnoreCase) && !parsedArgs.Verbose);
        }

        [Test]
        public void TestCLIArgsUtilsSuccess_VerboseOptionFalse()
        {
            string[] args = "-pbipreport pbipPath -rules rulesPath -verbose false".Split(" ");
            var parsedArgs = ArgsUtils.ParseArgs(args);

            Assert.That(parsedArgs.PBIFilePath.Equals("pbipPath", StringComparison.OrdinalIgnoreCase) && parsedArgs.RulesFilePath.Equals("rulesPath", StringComparison.OrdinalIgnoreCase) && !parsedArgs.Verbose);
        }

        [Test]
        public void TestCLIArgsUtilsSuccess_VerboseOptionUnparseable()
        {
            string[] args = "-pbipreport pbipPath -rules rulesPath -verbose XYZ".Split(" ");
            var parsedArgs = ArgsUtils.ParseArgs(args);

            Assert.That(parsedArgs.PBIFilePath.Equals("pbipPath", StringComparison.OrdinalIgnoreCase) && parsedArgs.RulesFilePath.Equals("rulesPath", StringComparison.OrdinalIgnoreCase) && !parsedArgs.Verbose);
        }

        //[Test]
        //public void TestCLIArgsUtilsSuccess_FavourPBIPReportOption()
        //{
        //    string[] args = "-pbix pbixPath -pbipreport pbipPath -rules rulesPath -verbose true".Split(" ");
        //    var parsedArgs = ArgsUtils.ParseArgs(args);

        //    Assert.True(parsedArgs.PBIFilePath.Equals("pbipPath", StringComparison.OrdinalIgnoreCase) && parsedArgs.RulesFilePath.Equals("rulesPath", StringComparison.OrdinalIgnoreCase));
        //}

        [Test]
        public void TestCLIArgsUtilsRules()
        {
            string[] args = "-pbipreport path -rules rulesPath".Split(" ");
            Args? parsedArgs = null;

            parsedArgs = ArgsUtils.ParseArgs(args);

            Assert.That(parsedArgs.RulesFilePath.Equals("rulesPath", StringComparison.OrdinalIgnoreCase));
        }

        [Test]
        public void TestCLIArgsUtilsFormats()
        {
            string[] args = "-pbipreport path -rules rulepath -formats CONSOLE,HTML,PNG,JSON".Split(" ");
            Args? parsedArgs = null;

            parsedArgs = ArgsUtils.ParseArgs(args);

            Assert.That(parsedArgs.CONSOLEOutput && parsedArgs.HTMLOutput && parsedArgs.PNGOutput && parsedArgs.JSONOutput);
        }

        [Test]
        public void TestCLIArgsUtilsDefaults()
        {
            string[] args = "-pbipreport path -rules rulespath".Split(" ");
            Args? parsedArgs = null;

            parsedArgs = ArgsUtils.ParseArgs(args);

            Assert.That(parsedArgs.CONSOLEOutput
                && !parsedArgs.Verbose
                && parsedArgs.DeleteOutputDirOnExit
                && !string.IsNullOrEmpty(parsedArgs.OutputDirPath)
                && !parsedArgs.HTMLOutput
                && !parsedArgs.JSONOutput
                && !parsedArgs.PNGOutput);
        }

        [Test]
        public void TestCLIArgsUtilsThrows()
        {
            string[] args = "-pbipreport pbipreportpath".Split(" ");
            Args? parsedArgs = null;

            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(
            () => parsedArgs = ArgsUtils.ParseArgs(args));
        }

        [Test]
        public void TestCLIArgsUtilsThrows1()
        {
            string[] args = "-pbip pbippath".Split(" ");
            Args? parsedArgs = null;

            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(
            () => parsedArgs = ArgsUtils.ParseArgs(args));
        }

        [Test]
        public void TestCLIArgsUtilsThrows2()
        {
            string[] args = "-rules rulesPath".Split(" ");
            Args? parsedArgs = null;

            ArgumentNullException ex = Assert.Throws<ArgumentNullException>(
            () => parsedArgs = ArgsUtils.ParseArgs(args));
        }

        [Test]
        public void TestCLIArgsUtilsThrows3()
        {
            string[] args = "-other other".Split(" ");
            Args? parsedArgs = null;

            ArgumentException ex = Assert.Throws<ArgumentException>(
            () => parsedArgs = ArgsUtils.ParseArgs(args));
        }

        [Test]
        public void TestCLIArgsUtilsThrows4()
        {
            string[] args = "-rules -pbix pbixPath -other".Split(" ");
            Args? parsedArgs = null;

            ArgumentException ex = Assert.Throws<ArgumentException>(
            () => parsedArgs = ArgsUtils.ParseArgs(args));
        }

        [Test]
        public void TestCLIArgsUtilsThrows5()
        {
            string[] args = "-rules rulesPath -pbip pbipPath -other stuff".Split(" ");
            Args? parsedArgs = null;

            ArgumentException ex = Assert.Throws<ArgumentException>(
            () => parsedArgs = ArgsUtils.ParseArgs(args));
        }

        [Test]
        public void TestCLIArgsUtilsSuccess_FormatsOption()
        {
            string[] args = "-pbip pbipPath -rules rulesPath -formats CONSOLE,HTML,PNG,JSON -verbose true".Split(" ");
            var parsedArgs = ArgsUtils.ParseArgs(args);

            Assert.That(parsedArgs.PBIFilePath.Equals("pbipPath", StringComparison.OrdinalIgnoreCase) && parsedArgs.RulesFilePath.Equals("rulesPath", StringComparison.OrdinalIgnoreCase) && parsedArgs.CONSOLEOutput && parsedArgs.HTMLOutput && parsedArgs.PNGOutput && parsedArgs.JSONOutput && parsedArgs.Verbose);
        }

        [Test]
        public void TestCLIArgsUtilsSuccess_FormatsOptionMissing()
        {
            string[] args = "-pbip pbipPath -rules rulesPath -verbose true".Split(" ");
            var parsedArgs = ArgsUtils.ParseArgs(args);

            Assert.That(parsedArgs.PBIFilePath.Equals("pbipPath", StringComparison.OrdinalIgnoreCase) && parsedArgs.RulesFilePath.Equals("rulesPath", StringComparison.OrdinalIgnoreCase) && parsedArgs.CONSOLEOutput && !parsedArgs.HTMLOutput && !parsedArgs.PNGOutput && !parsedArgs.JSONOutput && parsedArgs.Verbose);
        }

        [Test]
        public void TestCLIArgsUtilsSuccess_FormatsOptionUnparseable()
        {
            string[] args = "-pbip pbipPath -rules rulesPath -formats XYZ -verbose true".Split(" ");
            var parsedArgs = ArgsUtils.ParseArgs(args);

            Assert.That(parsedArgs.PBIFilePath.Equals("pbipPath", StringComparison.OrdinalIgnoreCase) && parsedArgs.RulesFilePath.Equals("rulesPath", StringComparison.OrdinalIgnoreCase) && parsedArgs.CONSOLEOutput && !parsedArgs.HTMLOutput && !parsedArgs.PNGOutput && !parsedArgs.JSONOutput && parsedArgs.Verbose);
        }

        [Test]
        public void TestCLIArgsUtilsSuccess_VerboseOption()
        {
            string[] args = "-pbip pbipPath -rules rulesPath -verbose true".Split(" ");
            var parsedArgs = ArgsUtils.ParseArgs(args);

            Assert.That(parsedArgs.PBIFilePath.Equals("pbipPath", StringComparison.OrdinalIgnoreCase) && parsedArgs.RulesFilePath.Equals("rulesPath", StringComparison.OrdinalIgnoreCase) && parsedArgs.Verbose);
        }

        [Test]
        public void TestCLIArgsUtilsSuccess_VerboseOptionFalse2()
        {
            string[] args = "-pbip pbipPath -rules rulesPath -verbose false".Split(" ");
            var parsedArgs = ArgsUtils.ParseArgs(args);

            Assert.That(parsedArgs.PBIFilePath.Equals("pbipPath", StringComparison.OrdinalIgnoreCase) && parsedArgs.RulesFilePath.Equals("rulesPath", StringComparison.OrdinalIgnoreCase) && !parsedArgs.Verbose);
        }


    }
}

#pragma warning restore CS8602