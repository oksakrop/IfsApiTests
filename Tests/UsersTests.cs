using System.Net;
using NUnit.Framework;
using IfsApiTests.Models;
using IfsApiTests.Helpers;

namespace IfsApiTests.Tests;

[TestFixture]
[Category("Users")]
public class UsersTests : BaseTest
{
    [Test]
    [Description("GET /users should return 200 OK")]
    public async Task GetAllUsers_ShouldReturn200Ok()
    {
        var request = RequestBuilder.Get("users");
        var response = await ApiClient.ExecuteAsync(request);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK),
            "Expected 200 OK for /users endpoint");
    }

    [Test]
    [Description("GET /users should return exactly 10 users")]
    public async Task GetAllUsers_ShouldReturn10Users()
    {
        var request = RequestBuilder.Get("users");
        var response = await ApiClient.ExecuteAsync<List<User>>(request);

        Assert.That(response.Data, Is.Not.Null, "Response data should not be null");
        Assert.That(response.Data!.Count, Is.EqualTo(10),
            $"Expected 10 users but got {response.Data.Count}");
    }

    [Test]
    [Description("GET /users each user should have required fields: id, name, username, email")]
    public async Task GetAllUsers_EachUser_ShouldHaveRequiredFields()
    {
        var request = RequestBuilder.Get("users");
        var response = await ApiClient.ExecuteAsync<List<User>>(request);

        Assert.That(response.Data, Is.Not.Null);
        foreach (var user in response.Data!)
        {
            Assert.Multiple(() =>
            {
                Assert.That(user.Id, Is.GreaterThan(0));
                Assert.That(user.Name, Is.Not.Null.And.Not.Empty);
                Assert.That(user.Username, Is.Not.Null.And.Not.Empty);
                Assert.That(user.Email, Is.Not.Null.And.Not.Empty);
            });
        }
    }
}
