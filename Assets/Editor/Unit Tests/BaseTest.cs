// Author:			Wolfgang Neumayer
// Creation Date:	05/04/2018

using NUnit.Framework;

public class BaseTest
{
    bool wasRaisingExceptions;

    [SetUp]
    public void Setup()
    {
        this.wasRaisingExceptions = UnityEngine.Assertions.Assert.raiseExceptions;
        UnityEngine.Assertions.Assert.raiseExceptions = false;
    }

    [TearDown]
    public void Teardown()
    {
        UnityEngine.Assertions.Assert.raiseExceptions = this.wasRaisingExceptions;
    }
}