namespace PowerBI.KuduServices
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using PowerBI.KuduServices.Models;

    public interface IKuduService
    {
        Task<IEnumerable<KuduWebJobDescription>> GetWebJobsAsync();
    }
}
