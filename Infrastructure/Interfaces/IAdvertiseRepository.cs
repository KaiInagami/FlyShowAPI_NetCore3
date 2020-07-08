using Infrastructure.Entities;
using System.Linq;

namespace Infrastructure.Interfaces
{
    public interface IAdvertiseRepository
    {
        IQueryable<Advertise> GetAll();

        int CreateAdvertise(Advertise model);

        bool DeleteAdvertise(int ID);
    }
}
