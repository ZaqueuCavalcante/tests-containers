using API;
using System.Net;
using FluentAssertions;

namespace TESTS;

[TestFixture]
public class Tests : ApiTestBase
{
    [Test]
    public async Task Should_create_a_new_key_value()
    {
        // Arrange
        var input = new ApiKeyValue("project", "tests-containers");

        // Act
        var response = await _client.PostAsync("/data", input.ToStringContent());

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var kv = await response.DeserializeTo<ApiKeyValue>();
        kv.Key.Should().Be(input.Key);
        kv.Value.Should().Be(input.Value);
    }
}
