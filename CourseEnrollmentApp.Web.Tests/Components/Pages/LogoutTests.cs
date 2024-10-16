using Bunit;
using Moq;
using NUnit.Framework;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using CourseEnrollmentApp.Web.Components.Pages;
using CourseEnrollmentApp.Web.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace CourseEnrollmentApp.Web.Tests.Components.Pages
{
    [TestFixture]
    public class LogoutComponentTests : Bunit.TestContext
    {
        private Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private CustomAuthenticationStateProvider _authStateProvider;
        private DefaultHttpContext _httpContext;
        private Mock<IAuthenticationService> _authenticationServiceMock;
        private MockNavigationManager _navigationManager;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _authenticationServiceMock = new Mock<IAuthenticationService>();

            _httpContext = new DefaultHttpContext();
            _httpContext.RequestServices = new ServiceCollection()
                .AddSingleton(_authenticationServiceMock.Object)
                .BuildServiceProvider();

            _httpContextAccessorMock.Setup(_ => _.HttpContext).Returns(_httpContext);

            _authStateProvider = new CustomAuthenticationStateProvider(_httpContextAccessorMock.Object);
            _navigationManager = new MockNavigationManager();

            Services.AddSingleton<AuthenticationStateProvider>(_authStateProvider);
            Services.AddSingleton<NavigationManager>(_navigationManager);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            Dispose();
        }

        [Test]
        public void OnInitializedAsync_ShouldMarkUserAsLoggedOut_AndNavigateToHomePage()
        {
            // Arrange
            var cut = RenderComponent<Logout>();

            // Act
            cut.WaitForState(() => _authStateProvider.GetAuthenticationStateAsync().IsCompleted);

            // Assert
            Assert.AreEqual("http://localhost/", _navigationManager.Uri);
        }
    }
}
