using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TCC_MVC.Models;

namespace TCC_MVC.ArquivosBO
{
    public class GroupBO 
    {
        public Group GetGroupByName(string name)
        {
            Curriculos research = new Curriculos();
            using (var _context = new TCC_LUCASEntities())
            {
                return _context.Group.Where(x => x.Name.Equals(name)).FirstOrDefault();
            }
        }

        public Group GetGroupById(int id)
        {
            Curriculos research = new Curriculos();
            using (var _context = new TCC_LUCASEntities())
            {
                return _context.Group.Where(x => x.Id.Equals(id)).FirstOrDefault();
            }
        }

        public IList<Group> GetAll()
        {
            using (var _context = new TCC_LUCASEntities())
            {
                return _context.Group.ToList();
            }
        }

        public IList<Curriculos> GetAllWorking()
        {
            using (var _context = new TCC_LUCASEntities())
            {
                return _context.Curriculos.Where(x => x.Working == true).ToList();
            }
        }

        public IList<GroupModel> GetAllGroups()
        {
            var groupList = new List<GroupModel>();
            using (var _context = new TCC_LUCASEntities())
            {
                groupList = _context.Group.Select(x => new GroupModel {
                    Id = x.Id,
                    Name = x.Name
                }).ToList();
            }
            return groupList;
        }

        public IList<Curriculos> GetResearchsByGroup(int idGroup)
        {
            using(var _context = new TCC_LUCASEntities())
            {
                return (from c in _context.Curriculos
                           join cg in _context.CurriculosGroup on c.Id equals cg.CurriculoId
                           where cg.GroupId.Equals(idGroup)
                           select c).ToList();

            }
        }
    }
}
