using AutoMapper;
using PanoramioSDK;

namespace PanoramioExplorer.ViewModels
{
    public class ViewModelFactory
    {
        private readonly IMapper mapper;

        public ViewModelFactory()
        {
            var configuration = new MapperConfiguration(config => config.CreateMap<Photo, PhotoViewModel>());
            mapper = configuration.CreateMapper();
        }

        public PhotoViewModel CreatePhotoViewModel(Photo source)
        {
            return mapper.Map<PhotoViewModel>(source);
        }
    }
}
