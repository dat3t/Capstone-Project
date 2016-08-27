using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Results;

namespace OneVietnam.DTL
{
    public class SuggestedPost:IComparable<SuggestedPost>
    {
        public Post post;
        public int score;
        public int CompareTo(SuggestedPost other)
        {
            if (this.score < other.score) return -1;
            else
            {
                if (this.score == other.score)
                {
                    if ((this.post.CreatedDate < other.post.CreatedDate)) return -1;
                    else return 1;
                }
                else return 1;
            }
        }
    }
}