using HotelData.DTOs;
using HotelData.Services;
using Microsoft.AspNetCore.Mvc;

namespace HotelData.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        private readonly ILogger<HotelController> _logger;
        private readonly IHotelService _hotelService;
        public HotelController(ILogger<HotelController> logger,
            IHotelService hotelService)
        {
            _logger = logger;
            _hotelService = hotelService;
        }

        [HttpGet]
        [Route("Get")]
        //api/hotel/get
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IList<HotelDTO>>> Get()
        {
            try
            {
                var hotels = await _hotelService.GetAllHotels();
                return Ok(hotels);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, null, null);
                return StatusCode(500);
            }
        }

        [HttpPost]
        [Route("Create")]
        //api/hotel/create
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Create()
        {
            try
            {
                await _hotelService.GetHotelDataFromSources();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, null, null);
                return StatusCode(500);
            }

            return StatusCode(201);
        }
    }
}
