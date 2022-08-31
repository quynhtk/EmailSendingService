using AutoMapper;
using EmailMaketing.ContentEmails;
using EmailMaketing.Customers;
using EmailMaketing.SenderEmails;
using static EmailMaketing.Web.Pages.SenderEmails.CreateModalModel;

namespace EmailMaketing.Web;

public class EmailMaketingWebAutoMapperProfile : Profile
{
    public EmailMaketingWebAutoMapperProfile()
    {
        //Define your AutoMapper configuration here for the Web project.
        CreateMap<Pages.Customers.CreateModalModel.CreateCustomerViewModal, CreateUpdateCustomer>();
        CreateMap<CustomerDto, Pages.Customers.EditModalModel.EditCustomerViewModal>();
        CreateMap<Pages.Customers.EditModalModel.EditCustomerViewModal, CreateUpdateCustomer>();

        CreateMap<CreateSenderEmailViewModal, CreateUpdateSenderEmailDto>();
        CreateMap<CustomerDto, Pages.Customers.ResetPasswordModel.ResetPasswordCustomerViewModal>();
        CreateMap<SenderEmailDto,Pages.SenderEmails.EditModalModel.EditSenderEmailViewModal>();
        CreateMap<Pages.SenderEmails.EditModalModel.EditSenderEmailViewModal, CreateUpdateSenderEmailDto>();

        CreateMap<Pages.ContentEmails.SendEmailModalModel.CreateContentEmailViewModal, CreateUpdateContentEmailDto>();

        CreateMap<CustomerDto, Pages.Customers.CreateRoleModalModel.EditCustomerRolesViewModal>();

        CreateMap<Pages.Customers.CreateRoleModalModel.EditCustomerRolesViewModal, CreateUpdateCustomer>();
    }
}
