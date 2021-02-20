using AutoMapper;
using Core.Extensions;
using Domain.Entities;
using Domain.Entities.Companies;
using Domain.Entities.Identity;
using Shared.Resources.Companies;
using Shared.Resources.Permission;
using Shared.Resources.PermissionCategory;
using Shared.Resources.Role;
using Shared.Resources.User;
using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Entities.Addresses;
using NetTopologySuite.Geometries;
using Shared.Resources.Addresses;

namespace Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region identity mapping 

            #region User

            CreateMap<User, UserGetData>()
                .ForMember(c => c.DateCreated, opt => opt.MapFrom(m => m.DateCreated.ToCustomFormatString(true)))
                .ForMember(d => d.Detail, opt => opt.MapFrom( s => new UserDetailGetData()
                {
                    Name = s.Detail.Name,
                    SurName = s.Detail.SurName,
                    Phone = s.Detail.Phone,
                    Website = s.Detail.Website,
                    Address = new AddressGetData()
                    {
                        City = s.Detail.Address.City,
                        Street = s.Detail.Address.Street,
                        Suite = s.Detail.Address.Suite,
                        ZipCode = s.Detail.Address.ZipCode,
                        Geo = s.Detail.Address.Geo==null?null: new GeoGetData()
                        {
                            Lng = s.Detail.Address.Geo.X,
                            Lat = s.Detail.Address.Geo.Y
                        }
                    },
                    Company = s.Detail.Company==null?null: new CompanyGetData()
                    {
                        BusinessSegment = s.Detail.Company.BusinessSegment,
                        CatchPhrase = s.Detail.Company.CatchPhrase,
                        Name = s.Detail.Company.Name
                    }
                }));
            CreateMap<UserDetail, UserDetailGetData>();
            




            CreateMap<UserRegisterData, User>()
                .ForMember(c => c.Detail, opt => opt.MapFrom(c => new UserDetail()
                {
                    Name = c.Name,
                    SurName = c.SurName
                }));
            


            CreateMap<UserAddData, User>()
                .ForMember(c => c.Roles, opt => opt.Ignore())
                .ForMember(c => c.DirectivePermissions, opt => opt.Ignore())
                .ForMember(c => c.Detail, opt => opt.MapFrom(c => new UserDetail()
                {
                    Name = c.Name,
                    SurName = c.SurName
                }))
                .AfterMap((userAdd, user) =>
                {

                    var addedRoles = userAdd.Roles.Where(id => user.Roles.All(f => f.RoleId != id)).Select(c => new UserRole() { RoleId = c, DateCreated = DateTime.Now });
                    foreach (var item in addedRoles)
                        user.Roles.Add(item);

                    var addedPermissions = userAdd.DirectivePermissions.Where(id => user.DirectivePermissions.All(f => f.PermissionId != id)).Select(c => new UserPermission() { PermissionId = c, DateCreated = DateTime.Now });
                    foreach (var item in addedPermissions)
                        user.DirectivePermissions.Add(item);
                });
            CreateMap<UserEditData, User>()
                .ForMember(c => c.Roles, opt => opt.Ignore())
                .ForMember(c => c.DirectivePermissions, opt => opt.Ignore())
                .ForMember(c => c.Detail, opt => opt.MapFrom(c => new UserDetail()
                {
                    Name = c.Name,
                    SurName = c.SurName
                }))
                .AfterMap((userAdd, user) =>
                {
                    var removedRoles = user.Roles.Where(f => !userAdd.Roles.Contains(f.RoleId)).ToList();
                    foreach (var item in removedRoles)
                    {
                        user.Roles.Remove(item);
                    }

                    if (userAdd.Roles != null && userAdd.Roles.Any())
                    {
                        var addedRoles = userAdd.Roles.Where(id => user.Roles.All(f => f.RoleId != id)).Select(c => new UserRole() { RoleId = c, DateCreated = DateTime.Now }).ToList();
                        foreach (var item in addedRoles)
                        {
                            user.Roles.Add(item);
                        }
                    }


                    var removedPermissions = user.DirectivePermissions.Where(f => !userAdd.DirectivePermissions.Contains(f.PermissionId)).ToList();
                    foreach (var item in removedPermissions)
                    {
                        user.DirectivePermissions.Remove(item);
                    }

                    if (userAdd.DirectivePermissions != null && userAdd.DirectivePermissions.Any())
                    {
                        var addedPermissions = userAdd.DirectivePermissions.Where(id => user.DirectivePermissions.All(f => f.PermissionId != id)).Select(c => new UserPermission() { PermissionId = c, DateCreated = DateTime.Now }).ToList();
                        foreach (var item in addedPermissions)
                        {
                            user.DirectivePermissions.Add(item);
                        }
                    }
                });


            CreateMap<UserDetailSharedInputData, User>()

                .ForMember(c => c.Detail, opt => opt.MapFrom(c => new UserDetail()
                {
                    Name = c.Name,
                    SurName = c.SurName,
                    Website = c.Website,
                    Phone = c.Phone,
                    CompanyId = c.CompanyId==0?null:c.CompanyId,
                    Address = c.Address == null
                        ? null
                        : new Address()
                        {
                            City = c.Address.City,
                            Street = c.Address.Street,
                            Suite = c.Address.Suite,
                            ZipCode = c.Address.ZipCode,
                            Geo = c.Address.Geo == null || c.Address.Geo.Lat==null|| c.Address.Geo.Lng==null? null : 
                                new Point((double)c.Address.Geo.Lat, (double)c.Address.Geo.Lng) { SRID = 4326 }
                        }
                }));
            
            #endregion

            #region Role
            CreateMap<Role, RoleGetData>()
                .ForMember(c => c.Permissions, opt => opt.MapFrom(m => m.PermissionCategory.Select(c => c.PermissionCategoryPermission)))
                .ForMember(c => c.DateCreated, opt => opt.MapFrom(m => m.DateCreated.ToCustomFormatString(true)));
            CreateMap<RoleData, Role>()
                .ForMember(c => c.PermissionCategory, opt => opt.Ignore())
                .AfterMap((roleAdd, role) =>
                {
                    var removedPermissions = role.PermissionCategory.Where(f => !roleAdd.PermissionCategories.Contains(f.PermissionCategoryPermissionId)).ToList();
                    foreach (var item in removedPermissions)
                    {
                        role.PermissionCategory.Remove(item);
                    }

                    if (roleAdd.PermissionCategories != null && roleAdd.PermissionCategories.Any())
                    {
                        var addedPermissions = roleAdd.PermissionCategories.Where(id => role.PermissionCategory.All(f => f.PermissionCategoryPermissionId != id)).Select(c => new RolePermissionCategory() { PermissionCategoryPermissionId = c, DateCreated = DateTime.Now }).ToList();
                        foreach (var item in addedPermissions)
                        {
                            role.PermissionCategory.Add(item);
                        }
                    }


                });

            #endregion

            #region Permission

            CreateMap<Permission, PermissionGetData>()
                .ForMember(c => c.DateCreated, opt => opt.MapFrom(m => m.DateCreated.ToCustomFormatString(true)));
            #endregion   
            #region Permission Category

            CreateMap<PermissionCategory, PermissionCategoryGetData>()
                .ForMember(c => c.DateCreated, opt => opt.MapFrom(m => m.DateCreated.ToCustomFormatString(true)));
            CreateMap<Permission, PermissionGetData>()
                .ForMember(c => c.DateCreated, opt => opt.MapFrom(m => m.DateCreated.ToCustomFormatString(true)));


            CreateMap<PermissionCategoryPermission, PermissionCategoryRelationGetData>()
                .ForMember(c => c.RelationId, opt => opt.MapFrom(m => m.Id))
                .ForMember(c => c.Category, opt => opt.MapFrom(m => m.Category))
                .ForMember(c => c.Permission, opt => opt.MapFrom(m => m.Permission))
                ;
            #endregion

            #endregion




            #region Company
            CreateMap<Company, CompanyGetData>();
            CreateMap<CompanyData, Company>();
            #endregion
            
			#region Address
			CreateMap<Address, AddressGetData>()
                /*.ForMember(c => c.Geo.Lng, opt => opt.MapFrom(c => c.Geo.X))
                .ForMember(c => c.Geo.Lat, opt => opt.MapFrom(c => c.Geo.Y))*/;

			CreateMap<AddressData, Address>();
			#endregion

        }
    }
}
