using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
   public class CustomStatusResponse
    {

        public static Response GetResponse(int StatusCode)
        {
            if (StatusCode == 200) return new Response()
            {
                Status = 200
            };
            else if (StatusCode == 401) return new Response()
            {
                Status = 401,
                ResponseMsg = "Unauthorized access"
            };
            else if (StatusCode == 403) return new Response()
            {
                Status = 403,
                ResponseMsg = "Sorry, video uploads are only available with a video plan. Upgrade to add videos to your listing or remove the video."
            };
            else if (StatusCode == 320) return new Response()
            {
                Status = 320,
                ResponseMsg = "User with these credential not exist."
            };
            else if (StatusCode == 500) return new Response()
            {
                Status = 500,
                ResponseMsg = "Internal server error"
            };
            else if (StatusCode == 600) return new Response()
            {
                Status = 600
            };
            else return new Response()
            {
                Status = 500,
                ResponseMsg = "Internal server error Error"
            };


        }


    }
}
