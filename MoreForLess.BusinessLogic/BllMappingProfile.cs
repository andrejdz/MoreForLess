using System.Linq;
using AutoMapper;
using MoreForLess.BusinessLogic.Models;
using MoreForLess.DataAccess.Entities;

namespace MoreForLess.BusinessLogic
{
    /// <summary>
    ///     Class that maps types.
    /// </summary>
    public class BllMappingProfile : Profile
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="BllMappingProfile"/> class.
        /// </summary>
        public BllMappingProfile()
        {
            this.CreateMap<GoodDomainModel, Good>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.Timestamp, opt => opt.Ignore())
                .ForMember(d => d.CurrencyId, opt => opt.Ignore())
                .ForMember(d => d.Currency, opt => opt.Ignore())
                .ForMember(d => d.ShopId, opt => opt.Ignore())
                .ForMember(d => d.Shop, opt => opt.Ignore())
                .ForMember(d => d.CategoryId, opt => opt.Ignore())
                .ForMember(d => d.Category, opt => opt.Ignore())
                .ForMember(d => d.Comments, opt => opt.Ignore())
                .ForMember(d => d.Scores, opt => opt.Ignore());

            this.CreateMap<Good, GoodDomainModel>()
                .ForMember(d => d.CurrencyName, opt => opt.MapFrom(s => s.Currency.Name))
                .ForMember(d => d.ShopName, opt => opt.MapFrom(s => s.Shop.Name))
                .ForMember(d => d.Average, opt =>
                    opt.MapFrom(s => s.Scores.Count == 0 ? new double?() : s.Scores.Average(src => src.Value)))
                .ForMember(d => d.CategoryIdsOnShop, opt => opt.Ignore());

            this.CreateMap<CategoryDomainModel, StoreCategory>()
                .ForMember(d => d.IdAtStore, opt => opt.MapFrom(s => s.IdAtStore))
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Name))
                .ForMember(d => d.ParentIdAtStore, opt => opt.MapFrom(s => s.ParentIdAtStore))
                .ForAllOtherMembers(opt => opt.Ignore());

            this.CreateMap<StoreCategory, CategoryDomainModel>()
                .ForMember(d => d.IdAtStore, opt => opt.MapFrom(s => s.IdAtStore))
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Name))
                .ForMember(d => d.ParentIdAtStore, opt => opt.MapFrom(s => s.ParentIdAtStore))
                .ForMember(d => d.ChildrenCategories, opt => opt.MapFrom(s => s.Children))
                .ForAllOtherMembers(opt => opt.Ignore());
                
            this.CreateMap<CommentDomainModel, Comment>()
                .ForMember(d => d.Text, opt => opt.MapFrom(s => s.Text))
                .ForMember(d => d.GoodId, opt => opt.MapFrom(s => s.GoodId))
                .ForAllOtherMembers(opt => opt.Ignore());

            this.CreateMap<Comment, CommentDomainModel>()
                .ForMember(d => d.GoodId, opt => opt.MapFrom(s => s.GoodId.Value));

            this.CreateMap<ScoreDomainModel, Score>()
                .ForMember(d => d.Value, opt => opt.MapFrom(s => s.Value))
                .ForMember(d => d.GoodId, opt => opt.MapFrom(s => s.GoodId))
                .ForAllOtherMembers(opt => opt.Ignore());

            this.CreateMap<Score, ScoreDomainModel>()
                .ForMember(d => d.GoodId, opt => opt.MapFrom(s => s.GoodId.Value));
        }
    }
}