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
            this.CreateMap<URLInfo, GoodDomainModel>()
                .ForMember(d => d.LinkOnProduct, opt => opt.MapFrom(s => s.AbsoluteUri))
                .ForMember(d => d.IdGoodOnShop, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.ShopName, opt => opt.MapFrom(s => s.Platform))
                .ForAllOtherMembers(d => d.Ignore());

            this.CreateMap<ItemInfo, GoodDomainModel>()
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Name))
                .ForMember(d => d.Price, opt => opt.MapFrom(s => s.Price))
                .ForMember(d => d.CurrencyName, opt => opt.MapFrom(s => s.Currency))
                .ForMember(d => d.LinkOnPicture, opt => opt.MapFrom(s => s.ImageURL))
                .ForAllOtherMembers(d => d.Ignore());

            this.CreateMap<GoodDomainModel, Good>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.Currency, opt => opt.Ignore())
                .ForMember(d => d.Shop, opt => opt.Ignore())
                .ForMember(d => d.Timestamp, opt => opt.Ignore())
                .ForMember(d => d.CurrencyId, opt => opt.Ignore())
                .ForMember(d => d.ShopId, opt => opt.Ignore());

            this.CreateMap<Good, GoodDomainModel>();
        }
    }
}