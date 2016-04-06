using AutoMapper;
using PanoramioSDK;

namespace PanoramioExplorer.ViewModels
{
    public class ViewModelFactory
    {
        private readonly PanoramioClient client;
        private readonly IMapper mapper;

        public ViewModelFactory(PanoramioClient client)
        {
            this.client = client;

            var configuration = new MapperConfiguration(config => config.CreateMap<Photo, PhotoViewModel>());
            mapper = configuration.CreateMapper();
        }

        public PhotoViewModel CreatePhotoViewModel(Photo source)
        {
            return mapper.Map<PhotoViewModel>(source);
        }

        public PhotoFeedViewModel CreatePhotoFeedViewModel(GeoArea geoArea)
        {
            return new PhotoFeedViewModel(client, geoArea, this);
        }
    }
}
