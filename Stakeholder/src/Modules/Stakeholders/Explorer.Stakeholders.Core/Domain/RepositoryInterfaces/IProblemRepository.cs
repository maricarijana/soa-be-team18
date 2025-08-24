using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.Stakeholders.Core.Domain.Problems;

namespace Explorer.Stakeholders.Core.Domain.RepositoryInterfaces
{
    public interface IProblemRepository
    {
        Problem? GetById(long id);
        List<Problem> GetByUserId(long id);
        List<Problem> GetByTourId(long id);
        Problem PostComment(ProblemComment problemComment);
        void Update(Problem problem);
    }
}
