using EmailMaketing.ContentEmails;
using EmailMaketing.Customers;
using EmailMaketing.EmailSchedules;
using EmailMaketing.RecipientDetails;
using EmailMaketing.SenderEmails;
using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace EmailMaketing.MongoDB;

[ConnectionStringName("Default")]
public class EmailMaketingMongoDbContext : AbpMongoDbContext
{
    /* Add mongo collections here. Example:
     * public IMongoCollection<Question> Questions => Collection<Question>();
     */
    public IMongoCollection<ContentEmail> ContentEmails => Collection<ContentEmail>();
    public IMongoCollection<Customer> Customers => Collection<Customer>();
    public IMongoCollection<RecipientDetail> RecipientDetails => Collection<RecipientDetail>();
    public IMongoCollection<SenderEmail> SenderEmails => Collection<SenderEmail>();
    public IMongoCollection<EmailSchedule> EmailSchedules => Collection<EmailSchedule>();
    protected override void CreateModel(IMongoModelBuilder modelBuilder)
    {
        base.CreateModel(modelBuilder);

        //builder.Entity<YourEntity>(b =>
        //{
        //    //...
        //});
    }
}
