# IFS API Test Automation Framework

Automated test suite for the [JSONPlaceholder API](https://jsonplaceholder.typicode.com), built with **C# / .NET 8 / NUnit / RestSharp**.

---

## Project Structure

```
IfsApiTests/
├── Config/
│   └── ApiSettings.cs          # Configuration model
├── Clients/
│   └── ApiClient.cs            # RestSharp wrapper with request/response logging
├── Helpers/
│   ├── ConfigurationHelper.cs  # Loads appsettings.json / env vars
│   └── RequestBuilder.cs       # Factory methods for GET/POST/PUT/DELETE requests
├── Models/
│   ├── Post.cs                 # Post response/request model
│   ├── Comment.cs              # Comment model
│   └── User.cs                 # User model
├── Tests/
│   ├── BaseTest.cs             # Base class: setup / teardown per test
│   ├── PostsTests.cs           # All /posts endpoint tests
│   ├── CommentsTests.cs        # All /comments endpoint tests
│   └── UsersTests.cs           # All /users endpoint tests
├── appsettings.json            # Configuration (BaseUrl, Timeout)
└── .github/workflows/ci.yml   # GitHub Actions CI pipeline
```

---

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

---

## Setup & Run

```bash
# 1. Clone the repository
git clone https://github.com/YOUR_USERNAME/IfsApiTests.git
cd IfsApiTests

# 2. Restore NuGet packages
dotnet restore

# 3. Build
dotnet build

# 4. Run all tests
dotnet test

# 5. Run with detailed output
dotnet test --verbosity normal

# 6. Run a specific category
dotnet test --filter "Category=Posts"
dotnet test --filter "Category=Comments"
dotnet test --filter "Category=Users"
```

---

## Configuration

Edit `appsettings.json` to change the base URL or timeout:

```json
{
  "ApiSettings": {
    "BaseUrl": "https://jsonplaceholder.typicode.com",
    "TimeoutSeconds": 30
  }
}
```

You can also override via environment variables:
```bash
ApiSettings__BaseUrl=https://my-staging-api.com dotnet test
```

---

## Test Coverage

| Endpoint | Scenario | Test |
|---|---|---|
| GET /posts | 200 OK | ✅ |
| GET /posts | Returns 100 posts | ✅ |
| GET /posts | Post structure validation | ✅ |
| GET /posts/{id} | Fetch specific post | ✅ |
| GET /posts/{id} | 404 for non-existent | ✅ |
| POST /posts | 201 Created | ✅ |
| POST /posts | Response reflects submitted data | ✅ |
| PUT /posts/{id} | Updates post, returns 200 | ✅ |
| DELETE /posts/{id} | Returns 200 OK | ✅ |
| GET /posts/{id}/comments | Nested resource | ✅ |
| GET /users | Returns 10 users, field validation | ✅ |

---

## CI/CD

GitHub Actions workflow runs automatically on push to `main`/`develop` and on pull requests.
Test results are published as a report in the Actions tab.
