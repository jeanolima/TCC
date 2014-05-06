using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public IList<CurriculoModel> GetResearchsByGroup(int idGroup)
        {
            using(var _context = new TCC_LUCASEntities())
            {
                return (from c in _context.Curriculos
                           join cg in _context.CurriculosGroup on c.Id equals cg.CurriculoId
                           where cg.GroupId.Equals(idGroup)
                        select new CurriculoModel
                        {
                            Id = c.Id,
                            Name = c.Name
                        }).ToList();

            }
        }

        public bool Save(GroupModel model)
        {
            using (var _context = new TCC_LUCASEntities())
            {
                try
                {
                    Group entitie = new Group();
                    if (model.Id != 0)
                        entitie = _context.Group.Where(x => x.Id.Equals(model.Id)).FirstOrDefault();
                    entitie.Name = model.Name;
                    var listAll = new Collection<CurriculosGroup>();

                    foreach (var research in model.Researchs)
                    {
                        if (research.IsSelected)
                        {
                            var CurriculoGroup = _context.CurriculosGroup.Where(x => x.CurriculoId.Equals(research.Id) && x.GroupId.Equals(model.Id)).FirstOrDefault();
                            if (CurriculoGroup != null)
                                listAll.Add(CurriculoGroup);
                            else
                                listAll.Add(new CurriculosGroup
                                {
                                    GroupId = model.Id,
                                    CurriculoId = research.Id
                                });
                        }
                    }

                    var listRemove = entitie.CurriculosGroup.Except(listAll, new CurriculosGroupComparer()).ToList();
                    var listAdd = listAll.Except(entitie.CurriculosGroup, new CurriculosGroupComparer2()).ToList();

                    foreach (var cg in listRemove)
                        _context.CurriculosGroup.Remove(cg);
                    foreach (var cg in listAdd)
                        _context.CurriculosGroup.Add(cg);

                    if (model.Id == 0)
                        _context.Group.Add(entitie);
                    _context.SaveChanges();

                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }
            }
        }
    }
}
