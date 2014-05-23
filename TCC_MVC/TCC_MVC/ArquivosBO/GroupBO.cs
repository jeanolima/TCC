using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TCC_MVC.Controllers;
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

        public bool Save(LineModel model)
        {
            using (var _context = new TCC_LUCASEntities())
            {
                try
                {
                    Line entitie = new Line();
                    if (model.Id != 0)
                        entitie = _context.Line.Where(x => x.Id.Equals(model.Id)).FirstOrDefault();
                    entitie.Name = model.Name;
                    var listAll = new Collection<CurriculosLine>();

                    foreach (var research in model.Researchs)
                    {
                        if (research.IsSelected)
                        {
                            var CurriculoLine = _context.CurriculosLine.Where(x => x.CurriculoId.Equals(research.Id) && x.LineId.Equals(model.Id)).FirstOrDefault();
                            if (CurriculoLine != null)
                                listAll.Add(CurriculoLine);
                            else
                                listAll.Add(new CurriculosLine
                                {
                                    LineId = model.Id,
                                    CurriculoId = research.Id
                                });
                        }
                    }

                    var listRemove = entitie.CurriculosLine.Except(listAll, new CurriculosLineComparer()).ToList();
                    var listAdd = listAll.Except(entitie.CurriculosLine, new CurriculosLineComparer()).ToList();

                    foreach (var cg in listRemove)
                        _context.CurriculosLine.Remove(cg);
                    foreach (var cg in listAdd)
                        _context.CurriculosLine.Add(cg);

                    var area = _context.Area.Where(x => x.Id.Equals(model.AreaSelected)).FirstOrDefault();
                    entitie.AreaId = area.Id;
                    _context.Line.Attach(entitie);  
                    _context.Entry(entitie).State = EntityState.Modified;


                    if (model.Id == 0)
                        _context.Line.Add(entitie);
                    _context.SaveChanges();

                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }
            }
        }

        public IList<LineModel> GetAllLines()
        {
            var list = new List<LineModel>();

            using (var _context = new TCC_LUCASEntities())
            {
                list = _context.Line.Select(x => new LineModel
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList();
            }
            return list;
        }

        public IList<CurriculoModel> GetResearchsByLine(int idLine)
        {
            using (var _context = new TCC_LUCASEntities())
            {
                return (from c in _context.Curriculos
                        join cg in _context.CurriculosLine on c.Id equals cg.CurriculoId
                        where cg.LineId.Equals(idLine)
                        select new CurriculoModel
                        {
                            Id = c.Id,
                            Name = c.Name
                        }).ToList();
            }
        }
        public Line GetLineById(int id)
        {
            Curriculos research = new Curriculos();
            using (var _context = new TCC_LUCASEntities())
            {
                return _context.Line.Where(x => x.Id.Equals(id)).FirstOrDefault();
            }
        }

        public IList<AreaModel> GetAreas()
        {
            using (var _context = new TCC_LUCASEntities())
            {
                return _context.Area.Select(x => new AreaModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    IsSelected = false
                }).ToList();
            }
        }

        public AreaModel GetAreaById(int id)
        {
            using (var _context = new TCC_LUCASEntities())
            {
                return (from c in _context.Area
                        join cg in _context.Line on c.Id equals cg.AreaId
                        where cg.Id.Equals(id)
                        select new AreaModel
                        {
                            Id = c.Id,
                            Name = c.Name
                        }).FirstOrDefault();
            }
        }
    }
}
