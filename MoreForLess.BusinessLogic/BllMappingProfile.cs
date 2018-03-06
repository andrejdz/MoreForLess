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
                .ForMember(d => d.Currency, opt => opt.Ignore())
                .ForMember(d => d.Shop, opt => opt.Ignore())
                .ForMember(d => d.Timestamp, opt => opt.Ignore())
                .ForMember(d => d.CurrencyId, opt => opt.Ignore())
                .ForMember(d => d.ShopId, opt => opt.Ignore());


            this.CreateMap<Good, GoodDomainModel>()
                .ForMember(d => d.Categories, opt => opt.Ignore());
        }
    }
}