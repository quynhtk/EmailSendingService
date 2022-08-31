using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace EmailMaketing.Pages;

[Collection(EmailMaketingTestConsts.CollectionDefinitionName)]
public class Index_Tests : EmailMaketingWebTestBase
{
    [Fact]
    public async Task Welcome_Page()
    {
        var response = await GetResponseAsStringAsync("/");
        response.ShouldNotBeNull();
    }
}
