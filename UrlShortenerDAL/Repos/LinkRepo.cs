using System;
using System.Collections.Generic;
using System.Text;
using UrlShortenerDAL.EF;
using UrlShortenerDAL.Models;

namespace UrlShortenerDAL.Repos
{
    public class LinkRepo : BaseRepo<LinkModel>, ILinkRepo
    {
        private static char[] _characters = new char[]
        {
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l',
            'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x',
            'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J',
            'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V',
            'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7',
            '8', '9'
        };

        public LinkRepo(UrlShortenerContext context) : base(context) { }

        public int Add(Random random, LinkModel model)
        {
            string url = generateUrl(random);

            List<LinkModel> links = GetSome(m => m.Url == url);
            for (int i = 0; i < 3; i++)
            {
                if (links.Count == 0) { break; }

                url = generateUrl(random);
                links = GetSome(m => m.Url == url);
            }

            if (links.Count > 0)
            {
                throw new Exception("Generated url already exists. Retry failed.");
            }

            model.Url = url;
            return Add(model); ;
        }

        public List<LinkModel> GetAllForUser(string uid)
        {
            return GetSome(m => m.OwnerId == uid);
        }

        private string generateUrl(Random random, int length = 6)
        {
            char[] chars = new char[length];
            for (int i = 0; i < length; i++)
            {
                chars[i] = _characters[random.Next(_characters.Length)];
            }

            return new string(chars);
        }
    }
}
