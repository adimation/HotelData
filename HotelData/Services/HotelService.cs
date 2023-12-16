using HotelData.DTOs;
using System.Reflection;
using Newtonsoft.Json;
using HotelData.Data;
using HotelData.Data.Repository;
using AutoMapper;

namespace HotelData.Services
{
    public class HotelService : IHotelService
    {
        private readonly IHotelRepository _repository;
        private readonly IMapper _mapper;
        public HotelService(IHotelRepository repository, IMapper mapper) 
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IList<HotelDTO>> GetAllHotels()
        {
            var hotels = await _repository.GetAllAsync();

            var hotelsDTOList = _mapper.Map<IList<HotelDTO>>(hotels);

            return hotelsDTOList;
        }

        public async Task GetHotelDataFromSources()
        {
            string pathSource1 = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Assets\source1.json");
            string source1Json = await File.ReadAllTextAsync(pathSource1);
            HotelSourceWrapperDTO source1 = JsonConvert.DeserializeObject<HotelSourceWrapperDTO>(source1Json);

            string pathSource2 = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Assets\source2.json");
            string source2Json = await File.ReadAllTextAsync(pathSource2);
            HotelSourceWrapperDTO source2 = JsonConvert.DeserializeObject<HotelSourceWrapperDTO>(source2Json);

            string pathSource3 = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Assets\source3.json");
            string source3Json = await File.ReadAllTextAsync(pathSource3);
            HotelSourceWrapperDTO source3 = JsonConvert.DeserializeObject<HotelSourceWrapperDTO>(source3Json);

            string pathSource4 = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Assets\source4.json");
            string source4Json = await File.ReadAllTextAsync(pathSource4);
            HotelSourceWrapperDTO source4 = JsonConvert.DeserializeObject<HotelSourceWrapperDTO>(source4Json);

            List<HotelSourceDTO> allHotels = new List<HotelSourceDTO>();
            allHotels.AddRange(source1.Hotels.Select(x => {
                x.SupplierId = source1.SupplierId;
                x.SupplierName = source1.SupplierName;
                return x;
            }));
            allHotels.AddRange(source2.Hotels.Select(x => {
                x.SupplierId = source2.SupplierId;
                x.SupplierName = source2.SupplierName;
                return x;
            }));
            allHotels.AddRange(source3.Hotels.Select(x => {
                x.SupplierId = source3.SupplierId;
                x.SupplierName = source3.SupplierName;
                return x;
            }));
            allHotels.AddRange(source4.Hotels.Select(x => {
                x.SupplierId = source4.SupplierId;
                x.SupplierName = source4.SupplierName;
                return x;
            }));

            IList<HotelSourceDTO> conhsolidatedHotels = await ConsolidateHotels(allHotels);

            var hotels = _mapper.Map<IList<Hotel>>(conhsolidatedHotels);

            await _repository.CreateAllAsync(hotels);
        }

        public async Task<IList<HotelSourceDTO>> ConsolidateHotels(IList<HotelSourceDTO> hotels)
        {
            IList<HotelSourceDTO> result = new List<HotelSourceDTO>();

            result.Add(hotels.First());

            for (int i = 1; i < hotels.Count; i++)
            {
                HotelSourceDTO currentHotel = hotels[i];
                bool add = true;

                foreach(var resultHotel in result)
                {
                    if (AreSimilar(currentHotel, resultHotel))
                    {
                        add = false;
                        break;
                    }
                }

                if (add) result.Add(currentHotel);
            }
            return result;
        }

        public bool AreSimilar(HotelSourceDTO currentHotel, HotelSourceDTO resultHotel)
        {
            if (currentHotel.Address.Country != resultHotel.Address.Country
               || currentHotel.Address.Region != resultHotel.Address.Region
               || currentHotel.Address.City != resultHotel.Address.City) 
                return false;

            if (CalculateDistance(currentHotel.Address.Latitude.Value, currentHotel.Address.Longitude.Value, resultHotel.Address.Latitude.Value, resultHotel.Address.Longitude.Value) > 10.0f)
                return false;

            if (CalculateSimilarityScore(currentHotel.Name, resultHotel.Name) < 0.5f)
                return false;

            return true;
        }

        #region NameComparison
        public static double CalculateSimilarityScore(string str1, string str2)
        {
            // Tokenize the input strings into words
            var tokens1 = Tokenize(str1);
            var tokens2 = Tokenize(str2);

            // Calculate the similarity score for each token
            List<double> scores = new List<double>();
            foreach (string token1 in tokens1)
            {
                double maxScore = 0.0;
                foreach (string token2 in tokens2)
                {
                    double score = CalculateTokenSimilarity(token1, token2);
                    maxScore = Math.Max(maxScore, score);
                }
                scores.Add(maxScore);
            }

            // Calculate the average similarity score
            double averageScore = scores.Average();
            return averageScore;
        }

        private static List<string> Tokenize(string input)
        {
            return input.Split(' ').Where(token => !string.IsNullOrWhiteSpace(token)).ToList();
        }

        private static double CalculateTokenSimilarity(string token1, string token2)
        {
            int maxLen = Math.Max(token1.Length, token2.Length);
            if (maxLen == 0)
                return 1.0; // Both tokens are empty, consider them identical

            int distance = CalculateLevenshteinDistance(token1, token2);
            return 1.0 - (double)distance / maxLen;
        }

        private static int CalculateLevenshteinDistance(string str1, string str2)
        {
            int[,] distance = new int[str1.Length + 1, str2.Length + 1];

            for (int i = 0; i <= str1.Length; i++)
            {
                for (int j = 0; j <= str2.Length; j++)
                {
                    if (i == 0)
                        distance[i, j] = j;
                    else if (j == 0)
                        distance[i, j] = i;
                    else
                        distance[i, j] = Math.Min(
                            Math.Min(distance[i - 1, j] + 1, distance[i, j - 1] + 1),
                            distance[i - 1, j - 1] + (str1[i - 1] == str2[j - 1] ? 0 : 1)
                        );
                }
            }

            return distance[str1.Length, str2.Length];
        }
        #endregion

        #region DistaceCalculation
        public static double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double EarthRadiusKm = 6371.0; // Earth radius in kilometers

            // Convert latitude and longitude from degrees to radians
            lat1 = ToRadians(lat1);
            lon1 = ToRadians(lon1);
            lat2 = ToRadians(lat2);
            lon2 = ToRadians(lon2);

            // Calculate differences
            double dLat = lat2 - lat1;
            double dLon = lon2 - lon1;

            // Haversine formula
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(lat1) * Math.Cos(lat2) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            // Calculate distance
            double distance = EarthRadiusKm * c;
            return distance;
        }

        private static double ToRadians(double degrees)
        {
            return degrees * Math.PI / 180.0;
        }
        #endregion

    }
}
