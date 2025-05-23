﻿//MIT License

//Copyright (c) 2022 Greg Dennis

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

using PBIRInspectorLibrary;
using PBIRInspectorLibrary.Exceptions;
using PBIRInspectorLibrary.Output;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace PBIRInspectorTests;

/// <summary>
/// The code in this SuiteRunner is adapted from Greg Dennis's Json Everything test suite (see https://github.com/gregsdennis/json-everything) to ensure that we're not breaking core JsonLogic functionality.
/// </summary>
public class SuiteRunner
{
    #region BasePassSuite
    public static IEnumerable<TestCaseData> BasePassPBIPSuite()
    {
        string PBIPFilePath = @"Files\pbip\Base-rules-passes.Report";
        string RulesFilePath = @"Files\Base-rules.json";

        Console.WriteLine("Running base pass PBIP suite...");
        return Suite(PBIPFilePath, RulesFilePath);
    }

    [TestCaseSource(nameof(BasePassPBIPSuite))]
    public void RunBasePassPBIP(TestResult testResult)
    {
        switch (testResult.RuleId)
        {
            case "REDUCE_PAGES":
                Assert.That(!testResult.Pass, testResult.Message);
                break;
            default:
                Assert.That(testResult.Pass, testResult.Message);
                break;
        }
    }
    #endregion

    #region BaseFailSuite 

    public static IEnumerable<TestCaseData> BaseFailPBIPSuite()
    {
        string PBIPFilePath = @"Files\pbip\Inventory-sample-fails.Report";
        string RulesFilePath = @"Files\Base-rules.json";

        Console.WriteLine("Running base fail PBIP suite...");
        return Suite(PBIPFilePath, RulesFilePath);
    }

    [TestCaseSource(nameof(BaseFailPBIPSuite))]
    public void RunBaseFailPBIP(TestResult testResult)
    {
        RunBaseFail(testResult);
    }

    private void RunBaseFail(TestResult testResult)
    {
        string expected = "[]";
        switch (testResult.RuleId)
        {
            case "REMOVE_UNUSED_CUSTOM_VISUALS":
                expected = "[\"Aquarium1442671919391\"]";
                JsonAssert.AreEquivalent(JsonNode.Parse(expected), testResult.Actual);
                break;
            case "REDUCE_VISUALS_ON_PAGE":
                if (testResult.ParentName == "ReportSectionfb0835fa991786b43a3f")
                {
                    Assert.That(!testResult.Pass, testResult.Message);
                }
                else
                {
                    Assert.That(testResult.Pass, testResult.Message);
                }
                break;
            case "REDUCE_OBJECTS_WITHIN_VISUALS":
                if (testResult.ParentName == "ReportSection4602098ba1ff5a3805a9")
                {
                    Assert.That(!testResult.Pass, testResult.Message);
                }
                else
                {
                    Assert.That(testResult.Pass, testResult.Message);
                }
                break;
            case "REDUCE_TOPN_FILTERS":
                if (testResult.ParentName == "ReportSection3440cc1dc4ec63ca3d06")
                {
                    Assert.That(!testResult.Pass, testResult.Message);
                }
                else
                {
                    Assert.That(testResult.Pass, testResult.Message);
                }
                break;
            case "REDUCE_ADVANCED_FILTERS":
                if (testResult.ParentName == "ReportSectiond7d52b137add50d28b88")
                {
                    Assert.That(!testResult.Pass, testResult.Message);
                }
                else
                {
                    Assert.That(testResult.Pass, testResult.Message);
                }
                break;
            case "REDUCE_PAGES":
                Assert.That(!testResult.Pass, testResult.Message);
                break;
            case "AVOID_SHOW_ITEMS_WITH_NO_DATA":
                //["797168e1f1e7658ceae6","97ad01a2b8fbfca3220c"]
                expected = "[\"797168e1f1e7658ceae6\",\"97ad01a2b8fbfca3220c\"]";
                if (testResult.ParentName == "ReportSection5f326c8a8185db501ad9")
                {
                    JsonAssert.AreEquivalent(JsonNode.Parse(expected), testResult.Actual);
                }
                else
                {
                    Assert.That(testResult.Pass, testResult.Message);
                }
                break;
            case "HIDE_TOOLTIP_DRILLTROUGH_PAGES":
                expected = "[\"Detail\", \"Tooltip page\"]";
                JsonAssert.AreEquivalent(JsonNode.Parse(expected), testResult.Actual);
                break;
            case "ENSURE_THEME_COLOURS":
                if (testResult.ParentName == "ReportSection6c3c3f97279fafdeeb57")
                {
                    expected = "[\"1a67964cf02b6170c3b8\"]";
                    JsonAssert.AreEquivalent(JsonNode.Parse(expected), testResult.Actual);
                }
                else
                {
                    Assert.That(testResult.Pass, testResult.Message);
                }
                break;
            case "ENSURE_PAGES_DO_NOT_SCROLL_VERTICALLY":
                expected = "[\"Scrolling page\"]";
                JsonAssert.AreEquivalent(JsonNode.Parse(expected), testResult.Actual);
                break;
            case "ENSURE_ALTTEXT":
                Assert.That(!testResult.Pass, testResult.Message);
                break;
            default:
                Assert.That(testResult.Pass, testResult.Message);
                break;
        }
    }
    #endregion

    #region ExamplePassSuite
    //TODO: run this suite
    public static IEnumerable<TestCaseData> ExamplePassPBIPSuite()
    {
        string PBIPFilePath = @"Files\pbip\Example-rules-passes.Report";
        string RulesFilePath = @"Files\Examples-rules.json";

        Console.WriteLine("Running example pass PBIP suite...");
        return Suite(PBIPFilePath, RulesFilePath);
    }

    [TestCaseSource(nameof(ExamplePassPBIPSuite))]
    public void RunExamplePassPBIP(TestResult testResult)
    {
        if (testResult.ParentDisplayName == testResult.RuleId)
        {
            Assert.That(testResult.Pass, testResult.Message);
        }

        //TODO: complete test for LOCAL_REPORT_SETTINGS
        if (testResult.RuleId == "DISABLE_SLOW_DATASOURCE_SETTINGS" ||
            //testResult.RuleId == "LOCAL_REPORT_SETTINGS" ||
            testResult.RuleId == "ACTIVE_PAGE" ||
            testResult.RuleId == "UNIQUE_PART_PASS" ||
            testResult.RuleId == "CHECK_FOR_LOCAL_MEASURES" ||
            testResult.RuleId == "CHECK_VERSION")
        {
            Assert.That(testResult.Pass, testResult.Message);
        }
    }
    #endregion

    #region ExampleFailSuite

    public static IEnumerable<TestCaseData> ExampleFailPBIPSuite()
    {
        ///string PBIPFilePath = @"Files\pbip\Inventory-sample-fails.Report";
        string PBIPFilePath = @"Files\pbip\Example-rules-fails.Report";
        string RulesFilePath = @"Files\Examples-rules.json";

        Console.WriteLine("Running base fail PBIP suite...");
        return Suite(PBIPFilePath, RulesFilePath);
    }

    [TestCaseSource(nameof(ExampleFailPBIPSuite))]
    public void RunExampleFailPBIP(TestResult testResult)
    {
        RunExampleFail(testResult);
    }

    private void RunExampleFail(TestResult testResult)
    {
        string expected = "[]";
        switch (testResult.RuleId)
        {
            case "CHARTS_WIDER_THAN_TALL":
                if (testResult.ParentDisplayName == testResult.RuleId)
                {
                    expected = "[\"3f7d302598c1e81e7e78\", \"5094f3ff553da63e610e\"]";
                    JsonAssert.AreEquivalent(JsonNode.Parse(expected), testResult.Actual);
                    Assert.That(!testResult.Pass, testResult.Message);
                }
                else
                {
                    Assert.That(testResult.Pass, testResult.Message);
                }
                break;
            case "MOBILE_CHARTS_WIDER_THAN_TALL":
                //if (testResult.ParentDisplayName == testResult.RuleId)
                //{
                //    expected = "[\"3f7d302598c1e81e7e78\", \"5094f3ff553da63e610e\"]";
                //    JsonAssert.AreEquivalent(JsonNode.Parse(expected), testResult.Actual);
                //    Assert.That(!testResult.Pass, testResult.Message);
                //}
                //else
                //{
                //    Assert.That(testResult.Pass, testResult.Message);
                //}
                break;
            case "DISABLE_SLOW_DATASOURCE_SETTINGS":
                Assert.That(!testResult.Pass, testResult.Message);
                break;
            case "LOCAL_REPORT_SETTINGS":
                Assert.That(!testResult.Pass, testResult.Message);
                break;
            case "ACTIVE_PAGE":
                Assert.That(!testResult.Pass, testResult.Message);
                break;
            case "SHOW_AXES_TITLES":
                if (testResult.ParentDisplayName == testResult.RuleId)
                {
                    expected = "[\"8a0d8392a2400e899bcc\", \"a9243890e8b7ec111322\", \"d65c53d5b679c4cacba0\"]";
                    JsonAssert.AreEquivalent(JsonNode.Parse(expected), testResult.Actual);
                    Assert.That(!testResult.Pass, testResult.Message);
                }
                else
                {
                    Assert.That(testResult.Pass, testResult.Message);
                }
                break;
            case "PERCENTAGE_OF_CHARTS_USING_CUSTOM_COLOURS":
                //if (testResult.ParentName == "ReportSectiond7d52b137add50d28b88")
                //{
                //    Assert.False(testResult.Pass, testResult.Message);
                //}
                //else
                //{
                //    Assert.True(testResult.Pass, testResult.Message);
                //}
                break;
            case "ENSURE_ALT_TEXT_DEFINED_FOR_VISUALS":
                expected = "[\"9032ab70a7e060d08574\",\"eca6ff83ecb390801c3a\"]";
                if (testResult.ParentDisplayName == testResult.RuleId)
                {
                    JsonAssert.AreEquivalent(JsonNode.Parse(expected), testResult.Actual);
                    Assert.That(!testResult.Pass, testResult.Message);
                }

                break;
            case "DISABLE_DROP_SHADOWS_ON_VISUALS":
                expected = "[\"5d4868734a72096e0ada\",\"bdb3c2666ac0e67947aa\"]";
                if (testResult.ParentDisplayName == testResult.RuleId)
                {
                    JsonAssert.AreEquivalent(JsonNode.Parse(expected), testResult.Actual);
                    Assert.That(!testResult.Pass, testResult.Message);
                }
                else
                {
                    Assert.That(testResult.Pass, testResult.Message);
                }
                break;
            case "GIVE_VISIBLE_PAGES_MEANINGFUL_NAMES":
                if (testResult.ParentDisplayName == "Page 1")
                {
                    Assert.That(!testResult.Pass, testResult.Message);
                }
                else
                {
                    Assert.That(!testResult.Pass, testResult.Message);
                }
                break;
            case "DENEB_CHARTS_PROPERTIES":
                //TODO: complete this test
                //Assert.False(testResult.Pass, testResult.Message);
                break;
            case "CHECK_FOR_VISUALS_OVERLAP":
                expected = "[\"11f540db1a90abb52cda\",\"2beb787442a6d0432b4d\",\"93e80741178005eb0ab4\",\"dead16c359819062e164\"]";
                if (testResult.ParentDisplayName == testResult.RuleId)
                {
                    JsonAssert.AreEquivalent(JsonNode.Parse(expected), testResult.Actual);
                    Assert.That(!testResult.Pass, testResult.Message);
                }
                break;
            case "UNIQUE_PART_FAIL":
                Assert.That(!testResult.Pass, testResult.Message);
                break;
            case "CHECK_FOR_LOCAL_MEASURES":
                //TODO: complete this test
                //Assert.False(testResult.Pass, testResult.Message);
                break;
            case "REPORT_THEME_NAME":
                Assert.That(!testResult.Pass, testResult.Message);
                break;
            case "REPORT_THEME_TITLE_FONT":
                Assert.That(!testResult.Pass, testResult.Message);
                break;
            default:
                Assert.That(testResult.Pass, testResult.Message);
                break;
        }
    }
    #endregion

    #region JsonLogicSuite 
    /// <summary>
    /// Test PBIX using the base JsonLogicTest file to make sure we didn't break JsonLogic
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<TestCaseData> JsonLogicSuite()
    {
        string PBIPFilePath = @"Files\pbip\Inventory Sample.pbip";
        var testsPath = Path.Combine(TestContext.CurrentContext.WorkDirectory, @"Files\JsonLogicTests.json");

        return Task.Run(async () =>
        {
            string content = null!;
            try
            {
                using var client = new HttpClient();
                //https://raw.githubusercontent.com/json-everything/json-everything/master/src/JsonLogic.Tests/Files/tests.json
                //https://jsonlogic.com/tests.json
                using var request = new HttpRequestMessage(HttpMethod.Get, "https://raw.githubusercontent.com/json-everything/json-everything/master/src/JsonLogic.Tests/Files/tests.json");
                using var response = await client.SendAsync(request);

                content = await response.Content.ReadAsStringAsync();

                await File.WriteAllTextAsync(testsPath, content);
            }
            catch (Exception e)
            {
                content ??= await File.ReadAllTextAsync(testsPath);

                Console.WriteLine(e);
            }

            var testSuite = JsonSerializer.Deserialize<JsonLogicTestSuite>(content);
            var inspectionRules = new InspectionRules();
            var rules = testSuite!.Tests.Select(t => new PBIRInspectorLibrary.Rule() { Name = t.Logic, PathErrorWhenNoMatch = false, Test = new PBIRInspectorLibrary.Test() { Logic = t.Logic!, Data = t.Data!, Expected = t.Expected! } });
            inspectionRules.Rules = rules.ToList();

            return Suite(PBIPFilePath, inspectionRules);
        }).Result;
    }

    [TestCaseSource(nameof(JsonLogicSuite))]
    public void RunJsonLogicTest(TestResult testResult)
    {
        Assert.That(testResult.Pass, testResult.Message);
    }
    #endregion

    #region BaseSuite 
    public static IEnumerable<TestCaseData> BaseSuite()
    {
        string PBIPFilePath = @"Files\pbip\Inventory sample.Report";
        string RulesFilePath = @"Files\Base-rules.json";

        Console.WriteLine("Running base suite...");
        return Suite(PBIPFilePath, RulesFilePath);
    }

    [TestCaseSource(nameof(BaseSuite))]
    public void RunBase(TestResult testResult)
    {
        Assert.That(testResult.Pass, testResult.Message);
    }
    #endregion

    public static IEnumerable<TestCaseData> Suite(string PBIPFilePath, string RulesFilePath)
    {
        try
        {
            Inspector insp = new Inspector(PBIPFilePath, RulesFilePath);

            var testResults = insp.Inspect();

            return testResults.Select(t => new TestCaseData(t) { TestName = t.RuleName });
        }
        catch (ArgumentNullException e)
        {
            Console.WriteLine(e.Message);
        }
        catch (ArgumentException e)
        {
            Console.WriteLine(e.Message);
        }
        catch (PBIRInspectorException e)
        {
            Console.WriteLine(e.Message);
        }

        return null;
    }

    public static IEnumerable<TestCaseData> Suite(string PBIPFilePath, InspectionRules inspectionRules)
    {
        try
        {
            Inspector insp = new Inspector(PBIPFilePath, inspectionRules);

            var testResults = insp.Inspect();

            return testResults.Select(t => new TestCaseData(t) { TestName = t.RuleName });
        }
        catch (ArgumentNullException e)
        {
            Console.WriteLine(e.Message);
        }
        catch (ArgumentException e)
        {
            Console.WriteLine(e.Message);
        }
        catch (PBIRInspectorException e)
        {
            Console.WriteLine(e.Message);
        }

        return null;
    }
}