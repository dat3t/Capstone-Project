﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using OneVietnam.BLL;
using OneVietnam.Models;

namespace OneVietnam.Controllers
{
    public class SearchController : Controller
    {
        // GET: Search
        public ActionResult Index()
        {
            return View();
        }
        private PostManager _postManager;
        public PostManager PostManager
        {
            get
            {
                return _postManager ?? HttpContext.GetOwinContext().Get<PostManager>();
            }
            private set { _postManager = value; }
        }
        public async Task<ActionResult> Search(string query)
        {
            var result = await PostManager.FullTextSearch(query);
            var list = (from post in result
                        where post.DeletedFlag == false
                        select new SearchResultItem
                        {
                            Description = post.Description,
                            Title = post.Title,
                            Url = Url.Action("ShowPostDetail", "Post", new { postId = post.Id })
                        }).ToList();
            var searchResult = new SearchResultModel
            {
                Count = result.Count,
                Result = list
            };
            return Json(searchResult, JsonRequestBehavior.AllowGet);
        }
    }
}