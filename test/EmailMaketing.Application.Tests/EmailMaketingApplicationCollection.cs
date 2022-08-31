using EmailMaketing.MongoDB;
using Xunit;

namespace EmailMaketing;

[CollectionDefinition(EmailMaketingTestConsts.CollectionDefinitionName)]
public class EmailMaketingApplicationCollection : EmailMaketingMongoDbCollectionFixtureBase
{

}
