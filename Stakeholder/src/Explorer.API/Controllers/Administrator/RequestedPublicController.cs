//using Explorer.Tours.API.Dtos;
//using Explorer.Tours.API.Public.TourAuthoring.KeypointAddition;
//using Explorer.Tours.API.Public.TourAuthoring.ObjectAddition;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using System.Runtime.CompilerServices;

//namespace Explorer.API.Controllers.Administrator
//{
//    [Authorize(Policy = "administratorPolicy")]
//    [Route("api/administration/publicRequest")]
//    public class RequestedPublicController : BaseApiController
//    {

//        private readonly IObjectService _objectService;
//        private readonly IKeyPointService _keyPointService;

//        public RequestedPublicController(IObjectService objectService, IKeyPointService keyPointService)
//        {
//            _objectService = objectService;
//            _keyPointService = keyPointService;
//        }

//        [HttpGet("getObjects")]
//        public ActionResult<List<ObjectDTO>> GetRequestedPublicObject()
//        {
//            return CreateResponse(_objectService.GetRequestedPublic());
//        }

//        [HttpGet("getKeyPoints")]
//        public ActionResult<List<KeyPointDto>> GetRequestedPublicKeyPoint()
//        {
//            return CreateResponse(_keyPointService.GetRequestedPublic());
//        }

//        [HttpPut("updateObject")]
//        public ActionResult<ObjectDTO> UpdateObject([FromBody] ObjectDTO objectDto)
//        {
//            return CreateResponse(_objectService.Update(objectDto));
//        }

//        [HttpPut("updateKeyPoint")]
//        public ActionResult<KeyPointDto> UpdateKeyPoint([FromBody] KeyPointDto keyPointDto) 
//        {
//            return CreateResponse(_keyPointService.Update(keyPointDto));
//        }
//    }
//}
