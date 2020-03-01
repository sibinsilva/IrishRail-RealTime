using System.Threading.Tasks;

namespace IrishRail
{
    public interface IGoogleMapsApiService
    {
        Task<GooglePlaceAutoCompleteResult> GetPlaces(string text);
        Task<GooglePlaces> GetPlaceDetails(string placeId);
    }
}