// (c) 2022 Francesco Del Re <francesco.delre.87@gmail.com>
// This code is licensed under MIT license (see LICENSE.txt for details)
using System.Web.Http;

namespace SharpJDKWrapper.Controller
{
    public class ServiceController : ApiController
    {
        [HttpGet]
        [Route("GetActiveServicesCount")]
        public int GetActiveServicesCount()
        {
            return Wrapper.Wrapper.GetActiveServicesCount();
        }

        [HttpGet]
        [Route("GetActiveServices")]
        public string GetActiveServices()
        {
            return Wrapper.Wrapper.GetActiveServices();
        }

        [HttpGet]
        [Route("GetStatusService")]
        public bool GetStatusService([FromBody] string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                return Wrapper.Wrapper.GetStatusService(id);
            }
            return false;
        }

        [HttpPost]
        [Route("StopService")]
        public void StopService([FromBody] string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                Wrapper.Wrapper.StopProcess(id);
            }
        }
    }
}
