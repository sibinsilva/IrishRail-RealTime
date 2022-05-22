using System.Threading.Tasks;

namespace IrishRail
{
    public interface IGoogleMapsApiService
    {
        Task<GooglePlaceAutoCompleteResult> GetPlaces(string text);
        Task<clsGooglePlaces> GetPlaceDetails(string placeId);
    }
}