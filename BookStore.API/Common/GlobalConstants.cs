using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.API.Common
{
    public static class GlobalConstants
    {
        public const string TryGetAuthors = "Attempted Get All Authors";
        public const string GetAuthors = "Authors Fetched Successfully";
        public const string TryGetAuthorById = "Attempted Get Author With An ID:{id}";
        public const string AuthorNotFound = "Author With ID:{id} Not Found";
        public const string AuthorFoundById = "Author Fetched Successfully With ID:{id}";
        public const string TryCreateAuthor = "Author Submission Attempted";
        public const string AuthorBadRequest = "Empty Request Was Submitted";
        public const string AuthorDataIncomplete = "Author Data Was Incomplete";
        public const string AuthorCreationFailed = "Author Creation Failed";
        public const string AuthorCreated = "Author Created Successfully";
        public const string TryAuthorUpdate = "Author Update Attempted - ID:{id}";
        public const string AuthorUpdateBadData = "Author Update Failed With Bad Data";
        public const string AuthorUpdateNotFound = "Author With ID:{id} Was Not Found";
        public const string AuthorInvalidModelState = "Author Data Was Incomplete";
        public const string AuthorUpdateFailed = "Update Operation Failed";
        public const string AuthorUpdateSuccessfull = "Author With ID:{id} Successfully Updated";
        public const string TryDeleteAuthor = "Author With ID:{id} Delete Attempted";
        public const string DeleteAuthorBadData = "Author Delete Failed With Bad Data";
        public const string TryDeleteAuthorById = "Author With ID:{id} Was Not Found";
        public const string DeleteAuthorFailed = "Author Deletion Failed";
        public const string AuthorDeletionSuccessfull = "Author With ID:{id} Successfully Deleted";



        public const string Error500 = "Something went wrong, please contact the Admin";
    }
}
