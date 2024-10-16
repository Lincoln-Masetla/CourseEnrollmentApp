using Microsoft.AspNetCore.Components;
namespace CourseEnrollmentApp.Web.Tests.Components
{
    public class MockNavigationManager : NavigationManager
    {
        public MockNavigationManager()
        {
            Initialize("http://localhost/", "http://localhost/");
        }

        protected override void NavigateToCore(string uri, bool forceLoad)
        {
            Uri = ToAbsoluteUri(uri).ToString();
        }

        public void PublicNavigateTo(string uri, bool forceLoad = false)
        {
            NavigateToCore(uri, forceLoad);
        }
    }
}