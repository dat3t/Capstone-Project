using OneVietnam.DTL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.SignalR;

namespace OneVietnam.BLL
{
    public class UtilityBO
    {        
        public UtilityBO()
        {
        }
        
        public  static List<Tag> GetAddedTags(HttpRequestBase pRequestBase, TagManager pTagManager, string pFormId)
        {
            if(pRequestBase.Form.Count > 0)
            {
                var addedTagValueList = pRequestBase.Form[pFormId];
                if (! String.IsNullOrEmpty(addedTagValueList.ToString()))
                {                    
                    List<Tag> newList = new List<Tag>();
                    var tagsInDb =  pTagManager.GetTagsValueAsync();
                    int numberTags = 0;
                    if(tagsInDb != null)
                    {
                        numberTags = tagsInDb.Count;
                    }
                
                    foreach (var tag in addedTagValueList.Split(','))
                    {
                        if (tagsInDb == null | (tagsInDb != null && !tagsInDb.Contains(tag)))
                        {
                            Tag newTag = new Tag(string.Concat("Tag_", numberTags.ToString()), tag);
                            pTagManager.CreateAsync(newTag);
                            numberTags = numberTags + 1;
                            newList.Add(newTag);
                        }
                        else
                        {
                            var existTag = pTagManager.FindTagByValueAsync(tag);
                            newList.Add(existTag[0]);
                        }
                    }
                    return newList;
                }
                return null;
            }
            return null;
        }        

    }
}