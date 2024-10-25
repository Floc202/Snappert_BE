using SWD392.Snappet.Repository.BusinessModels;

namespace SWD392.Snappet.API.ResponseModel
{
    public class PostResponse
    {
        public string UserName { get; set; }

        public string Content { get; set; }

        public string TagName { get; set; }

        public string PhotoUrl { get; set; }
    }
}
