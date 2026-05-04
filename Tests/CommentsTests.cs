using System.Net;
using NUnit.Framework;
using IfsApiTests.Models;
using IfsApiTests.Helpers;

namespace IfsApiTests.Tests;

[TestFixture]
[Category("Comments")]
public class CommentsTests : BaseTest
{
    [Test]
    [Description("GET /posts/1/comments should return comments for post with id=1")]
    public async Task GetCommentsByPostId_ShouldReturnComments()
    {
        var request = RequestBuilder.Get("posts/1/comments");
        var response = await ApiClient.ExecuteAsync<List<Comment>>(request);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK),
            "Expected 200 OK for nested comments endpoint");
        Assert.That(response.Data, Is.Not.Null.And.Not.Empty,
            "Comments list should not be null or empty");
    }

    [Test]
    [Description("GET /posts/1/comments all comments should belong to post with id=1")]
    public async Task GetCommentsByPostId_AllComments_ShouldBelongToCorrectPost()
    {
        const int postId = 1;
        var request = RequestBuilder.Get($"posts/{postId}/comments");
        var response = await ApiClient.ExecuteAsync<List<Comment>>(request);

        Assert.That(response.Data, Is.Not.Null, "Response data should not be null");

        foreach (var comment in response.Data!)
        {
            Assert.That(comment.PostId, Is.EqualTo(postId),
                $"Comment {comment.Id} has PostId={comment.PostId}, expected {postId}");
        }
    }

    [Test]
    [Description("GET /comments each comment should have required fields")]
    public async Task GetAllComments_EachComment_ShouldHaveRequiredFields()
    {
        var request = RequestBuilder.Get("comments");
        var response = await ApiClient.ExecuteAsync<List<Comment>>(request);

        Assert.That(response.Data, Is.Not.Null, "Response data should not be null");

        foreach (var comment in response.Data!)
        {
            Assert.Multiple(() =>
            {
                Assert.That(comment.Id, Is.GreaterThan(0));
                Assert.That(comment.PostId, Is.GreaterThan(0));
                Assert.That(comment.Name, Is.Not.Null.And.Not.Empty);
                Assert.That(comment.Email, Is.Not.Null.And.Not.Empty);
                Assert.That(comment.Body, Is.Not.Null.And.Not.Empty);
            });
        }
    }
}
