using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.API.Common
{
    public static class GlobalConstants
    {
        /// <summary>
        /// Author's Contrller NLog Messages
        /// </summary>
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
        public const string AuthorUpdateFailed = "Author Update Operation Failed";
        public const string AuthorUpdateSuccessfull = "Author With ID:{id} Successfully Updated";
        public const string TryDeleteAuthor = "Author With ID:{id} Delete Attempted";
        public const string DeleteAuthorBadData = "Author Delete Failed With Bad Data";
        public const string DeleteAuthorByIdNotFound = "Author With ID:{id} Was Not Found";
        public const string DeleteAuthorFailed = "Author Deletion Failed";
        public const string AuthorDeletionSuccessfull = "Author With ID:{id} Successfully Deleted";

        /// <summary>
        /// Book's Controller NLog Messages
        /// </summary>
        public const string TryGetBooks = "Attempted Get All Books";
        public const string GetBooks = "Books Fetched Successfully";
        public const string TryGetBookById = "Attempted Get Book With An ID:{id}";
        public const string BookNotFound = "Book With ID:{id} Not Found";
        public const string BookFoundById = "Book Fetched Successfully With ID:{id}";
        public const string TryCreateBook = "Book Submission Attempted";
        public const string BookBadRequest = "Empty Request Was Submitted";
        public const string BookDataIncomplete = "Book Data Was Incomplete";
        public const string BookCreationFailed = "Book Creation Failed";
        public const string BookCreated = "Book Created Successfully";
        public const string TryBookUpdate = "Book Update Attempted - ID:{id}";
        public const string BookUpdateBadData = "Book Update Failed With Bad Data";
        public const string BookUpdateNotFound = "Book With ID:{id} Was Not Found";
        public const string BookInvalidModelState = "Book Data Was Incomplete";
        public const string BookUpdateFailed = "Book Update Operation Failed";
        public const string BookUpdateSuccessfull = "Book With ID:{id} Successfully Updated";
        public const string TryDeleteBook = "Book With ID:{id} Delete Attempted";
        public const string DeleteBookBadData = "Book Delete Failed With Bad Data";
        public const string DeleteBookByIdNotFound = "Book With ID:{id} Was Not Found";
        public const string DeleteBookFailed = "Book Deletion Failed";
        public const string BookDeletionSuccessfull = "Book With ID:{id} Successfully Deleted";

        /// <summary>
        /// User's Controller NLog Messages
        /// </summary>
        public const string TryUserRegistration = "Registration Attempt From User {username}";
        public const string UserRegistrationFailed = "User {username} Registration Attempt Failed";
        public const string UserRegistrationSuccess = "User {username} Successfully Registered";

        public const string TryUserLogin = "Login Attempt From User {username}";
        public const string UserLoginFailed = "User {username} Login Attempt Failed";
        public const string UserLoginSuccess = "User {username} Successfully Logged";


        /// <summary>
        /// Error 500 Server Not Responding
        /// </summary>
        public const string Error500 = "Something went wrong, please contact the Admin";
    }
}
