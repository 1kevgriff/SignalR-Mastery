using System.Collections.Generic;
using System.Threading.Tasks;

public interface IRandomUserService
{
    Task<IEnumerable<RandomUser>> GetUsers(int max = 10);
}
