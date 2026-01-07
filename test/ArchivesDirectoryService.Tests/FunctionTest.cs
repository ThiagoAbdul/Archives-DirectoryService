using Xunit;
using Amazon.Lambda.Core;
using Amazon.Lambda.TestUtilities;

namespace ArchivesDirectoryService.Tests;

public class FunctionTest
{
    [Fact]
    public void TestToUpperFunction()
    {
        // Invoke the lambda function and confirm the string was upper cased.
        var context = new TestLambdaContext();
        
    }
}