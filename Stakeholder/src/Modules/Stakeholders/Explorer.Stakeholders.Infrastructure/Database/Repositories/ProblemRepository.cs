using Explorer.Stakeholders.Core.Domain.Problems;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using Explorer.Stakeholders.Core.UseCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Infrastructure.Database.Repositories
{
    public class ProblemRepository: IProblemRepository
    {
        private readonly StakeholdersContext _db;

        public ProblemRepository(StakeholdersContext db)
        {
            _db = db;
        }
        public Problem? GetById(long id)
        {
            return _db.Problem.FirstOrDefault(p => p.Id == id);
        }
        public List<Problem> GetByUserId(long id)
        {
            var problems = new List<Problem>();
            foreach(var p in _db.Problem)
            {
                if (p.UserId == id)
                    problems.Add(p);
            }

            return problems;
            //return _db.Problem.Where(t => t.UserId == id).ToList();
        }

        public List<Problem> GetByTourId(long id)
        {
            return _db.Problem.Where(t => t.TourId == id).ToList();
        }

        
        public Problem PostComment(ProblemComment comment)
        {
            var problem = GetById(comment.ProblemId);
            if (problem != null)
            {
                problem.PostComment(comment);
                _db.Problem.Update(problem);
                _db.SaveChanges();
            }
            return problem;
        }

        public void Update(Problem problem)
        {
          
            _db.Entry(problem).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _db.SaveChanges();
        }
    }
}
