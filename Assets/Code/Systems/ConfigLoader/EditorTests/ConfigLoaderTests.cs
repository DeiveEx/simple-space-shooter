using System.IO;
using NUnit.Framework;
using Systems.ConfigLoader;
using UnityEngine;
using UnityEngine.TestTools;

public class ConfigLoaderTests
{
    private class TestConfig
    {
        public int TestValue;
    }

    private JsonLoader _jsonLoader;
    
    private string FilePath => Path.Combine(Application.dataPath, "test.json");
    
    [SetUp]
    public void Setup()
    {
        _jsonLoader = new JsonLoader(Application.dataPath);
        
        File.WriteAllText(FilePath,
            @"
{
    ""TestValue"": 100
}"
            );
    }
    
    [TearDown]
    public void TearDown()
    {
        File.Delete(FilePath);
    }

    [TestCase("test", true)]
    [TestCase("test2", false)]
    public void LoadConfig(string fileName, bool expectedResult)
    {
        LogAssert.ignoreFailingMessages = true;
        
        Assert.That(_jsonLoader.TryLoadConfig<TestConfig>(fileName, out var config), Is.EqualTo(expectedResult));
        
        if(expectedResult)
            Assert.That(config.TestValue, Is.EqualTo(100));
    }
}
