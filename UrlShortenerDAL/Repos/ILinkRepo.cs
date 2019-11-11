using System;
using System.Collections.Generic;
using System.Text;
using UrlShortenerDAL.Models;

namespace UrlShortenerDAL.Repos
{
    public interface ILinkRepo : IRepo<LinkModel>
    {
        int Add(Random random, LinkModel model);
        List<LinkModel> GetAllForUser(string uid);
        LinkModel GetLinkByUrl(string url);
        LinkModel GetLinkByOriginal(string originalUrl);
        LinkModel GetLinkByOriginalForUser(string originalUrl, string uid);
    }
}
