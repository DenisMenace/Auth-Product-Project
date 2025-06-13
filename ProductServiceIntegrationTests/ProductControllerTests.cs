using Application.Products.Commands.InsertProduct;
using Domain.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ProductAPI;
using System.Net;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Encodings.Web;

public class ProductControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ProductControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment("Testing");

            builder.ConfigureServices(services =>
            {
                var dbContextTypes = services
                    .Where(d => d.ServiceType.FullName != null &&
                                (d.ServiceType.FullName.Contains("DbContextOptions") ||
                                 d.ServiceType == typeof(ProductServiceDbContext)))
                    .ToList();

                foreach (var descriptor in dbContextTypes)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<ProductServiceDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDb");
                });

                services.AddAuthentication("Test")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", _ => { });

                services.PostConfigureAll<AuthenticationOptions>(options =>
                {
                    options.DefaultAuthenticateScheme = "Test";
                    options.DefaultChallengeScheme = "Test";
                });

                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<ProductServiceDbContext>();
                db.Database.EnsureCreated();
            });
        }).CreateClient();
    }

    [Fact]
    public async Task CreateProduct_ReturnsCreated()
    {
        var request = new InsertProductRequest("Test Product", "Red");
        var response = await _client.PostAsJsonAsync("/products", request);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task GetAllProducts_ReturnsOk()
    {
        var request = new InsertProductRequest("Test A", "Blue");
        await _client.PostAsJsonAsync("/products", request);

        var response = await _client.GetAsync("/products");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var products = await response.Content.ReadFromJsonAsync<List<Product>>();
        Assert.NotNull(products);
        Assert.Contains(products, p => p.Name == "Test A");
    }

    [Fact]
    public async Task GetProductsByColour_ReturnsFilteredResults()
    {
        await _client.PostAsJsonAsync("/products", new InsertProductRequest("A", "Green"));
        await _client.PostAsJsonAsync("/products", new InsertProductRequest("B", "Red"));

        var response = await _client.GetAsync("/products/colour/Green");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var products = await response.Content.ReadFromJsonAsync<List<Product>>();
        Assert.All(products, p => Assert.Equal("Green", p.Colour));
    }
}
public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public TestAuthHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock) : base(options, logger, encoder, clock) { }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var claims = new[] { new Claim(ClaimTypes.Name, "TestUser") };
        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, "Test");

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
