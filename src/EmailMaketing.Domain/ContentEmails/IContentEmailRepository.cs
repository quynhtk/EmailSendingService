using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace EmailMaketing.ContentEmails
{
    public interface IContentEmailRepository: IRepository<ContentEmail, Guid>
    {
        Task<List<ContentEmail>> GetListAsync(
                int skipCount,
                int maxResultCount,
                string sorting,
                string filter
            );
        Task<ContentEmail> FindByIdSenderEmailAsync(Guid id);
    }
}
