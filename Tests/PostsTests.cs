using System.Net;
using NUnit.Framework;
using RestSharp;
using IfsApiTests.Models;
using IfsApiTests.Helpers;

namespace IfsApiTests.Tests;

[TestFixture]
[Category("Posts")]
public class PostsTests : BaseTest
{
    // ─────────────────────────────────────────────
    // GET /posts
    // ─────────────────────────────────────────────

    [Test]
    [Description("GET /posts should return HTTP 200 OK")]
    public async Task GetAllPosts_ShouldReturn200Ok()
    {
        var request = RequestBuilder.Get("posts");
        var response = await ApiClient.ExecuteAsync(request);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK),
            "Expected 200 OK but got a different status code");
    }

    [Test]
    [Description("GET /posts should return exactly 100 posts")]
    public async Task GetAllPosts_ShouldReturn100Posts()
    {
        var request = RequestBuilder.Get("posts");
        var response = await ApiClient.ExecuteAsync<List<Post>>(request);

        Assert.That(response.Data, Is.Not.Null, "Response data should not be null");
        Assert.That(response.Data!.Count, Is.EqualTo(100),
            $"Expected 100 posts but got {response.Data.Count}");
    }

    [Test]
    [Description("GET /posts each post should have required fields: id, userId, title, body")]
    public async Task GetAllPosts_EachPost_ShouldHaveRequiredFields()
    {
        var request = RequestBuilder.Get("posts");
        var response = await ApiClient.ExecuteAsync<List<Post>>(request);

        Assert.That(response.Data, Is.Not.Null, "Response data should not be null");

        foreach (var post in response.Data!)
        {
            Assert.Multiple(() =>
            {
                Assert.That(post.Id, Is.GreaterThan(0),
                    $"Post ID should be positive, got {post.Id}");
                Assert.That(post.UserId, Is.GreaterThan(0),
                    $"Post UserId should be positive for post {post.Id}");
                Assert.That(post.Title, Is.Not.Null.And.Not.Empty,
                    $"Post title should not be empty for post {post.Id}");
                Assert.That(post.Body, Is.Not.Null.And.Not.Empty,
                    $"Post body should not be empty for post {post.Id}");
            });
        }
    }

    // ─────────────────────────────────────────────
    // GET /posts/{id}
    // ─────────────────────────────────────────────

    [Test]
    [Description("GET /posts/1 should return the correct post with id=1")]
    public async Task GetPostById_WithValidId_ShouldReturnCorrectPost()
    {
        const int postId = 1;
        var request = RequestBuilder.Get($"posts/{postId}");
        var response = await ApiClient.ExecuteAsync<Post>(request);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK),
            "Expected 200 OK for a valid post ID");
        Assert.That(response.Data, Is.Not.Null, "Response data should not be null");
        Assert.That(response.Data!.Id, Is.EqualTo(postId),
            $"Expected post id={postId} but got id={response.Data.Id}");
    }

    [Test]
    [Description("GET /posts/9999 should return 404 for a non-existent post")]
    public async Task GetPostById_WithNonExistentId_ShouldReturn404()
    {
        var request = RequestBuilder.Get("posts/9999");
        var response = await ApiClient.ExecuteAsync(request);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound),
            "Expected 404 Not Found for a non-existent post ID");
    }

    // ─────────────────────────────────────────────
    // POST /posts
    // ─────────────────────────────────────────────

    [Test]
    [Description("POST /posts should return 201 Created")]
    public async Task CreatePost_ShouldReturn201Created()
    {
        var newPost = new Post { UserId = 1, Title = "IFS Test Post", Body = "Automated test body" };
        var request = RequestBuilder.Post("posts", newPost);
        var response = await ApiClient.ExecuteAsync(request);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created),
            "Expected 201 Created when creating a new post");
    }

    [Test]
    [Description("POST /posts response body should reflect the submitted data")]
    public async Task CreatePost_ResponseBody_ShouldContainSubmittedData()
    {
        var newPost = new Post { UserId = 7, Title = "IFS Automation Title", Body = "IFS Automation Body" };
        var request = RequestBuilder.Post("posts", newPost);
        var response = await ApiClient.ExecuteAsync<Post>(request);

        Assert.That(response.Data, Is.Not.Null, "Response data should not be null");
        Assert.Multiple(() =>
        {
            Assert.That(response.Data!.UserId, Is.EqualTo(newPost.UserId),
                "UserId in response does not match the submitted value");
            Assert.That(response.Data.Title, Is.EqualTo(newPost.Title),
                "Title in response does not match the submitted value");
            Assert.That(response.Data.Body, Is.EqualTo(newPost.Body),
                "Body in response does not match the submitted value");
            Assert.That(response.Data.Id, Is.GreaterThan(0),
                "Response should contain a generated ID > 0");
        });
    }

    // ─────────────────────────────────────────────
    // PUT /posts/{id}
    // ─────────────────────────────────────────────

    [Test]
    [Description("PUT /posts/1 should update the post and return updated data")]
    public async Task UpdatePost_WithValidId_ShouldReturnUpdatedPost()
    {
        const int postId = 1;
        var updatedPost = new Post
        {
            Id = postId,
            UserId = 1,
            Title = "Updated Title by IFS Test",
            Body = "Updated body content"
        };

        var request = RequestBuilder.Put($"posts/{postId}", updatedPost);
        var response = await ApiClient.ExecuteAsync<Post>(request);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK),
            "Expected 200 OK when updating a post");
        Assert.That(response.Data, Is.Not.Null, "Response data should not be null");
        Assert.Multiple(() =>
        {
            Assert.That(response.Data!.Id, Is.EqualTo(postId),
                "Returned post ID should match the updated post ID");
            Assert.That(response.Data.Title, Is.EqualTo(updatedPost.Title),
                "Title was not updated correctly");
            Assert.That(response.Data.Body, Is.EqualTo(updatedPost.Body),
                "Body was not updated correctly");
        });
    }

    // ─────────────────────────────────────────────
    // DELETE /posts/{id}
    // ─────────────────────────────────────────────

    [Test]
    [Description("DELETE /posts/1 should return 200 OK")]
    public async Task DeletePost_WithValidId_ShouldReturn200Ok()
    {
        var request = RequestBuilder.Delete("posts/1");
        var response = await ApiClient.ExecuteAsync(request);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK),
            "Expected 200 OK when deleting an existing post");
    }
}
