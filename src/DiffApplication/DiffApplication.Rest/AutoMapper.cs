using AutoMapper;
using DiffApplication.Domain.Models;
using DiffApplication.Rest.ViewModels;

namespace DiffApplication.Rest
{
    /// <summary>
    /// Automapper configuration. 
    /// </summary>
    public class AutoMapper : Profile
    {
        public AutoMapper() 
        {
            CreateMap<Diff, DiffViewModelPut>().ReverseMap();
            CreateMap<DiffResult, DiffResultViewModelGet>();
            CreateMap<DiffResult, DiffResultWithInfoViewModelGet>();
            CreateMap<DiffResultInfo, DiffResultInfoViewModelGet>();
        }
    }
}
