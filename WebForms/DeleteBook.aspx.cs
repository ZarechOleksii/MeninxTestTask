using DAL.Interfaces;
using Models;
using System;
using System.Web.UI;

namespace WebForms
{
    public partial class _DeleteBook : Page
    {
        private readonly IBookRepository _bookRep;

        protected Book deletedBook;

        protected string BookNotFoundError;

        public _DeleteBook(IBookRepository bookRepository)
        {
            _bookRep = bookRepository;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string bookId = Request.Params["BookId"];

            if (bookId == null)
            {
                ErrorLabel.Text = "To delete book BookId paremeter is required.";
                return;
            }

            if (!Guid.TryParse(bookId, out Guid result))
            {
                ErrorLabel.Text = $"Failed to parse {bookId} to id.";
                return;
            }

            deletedBook = _bookRep.GetOneWithCategory(result);

            if (deletedBook == null)
            {
                ErrorLabel.Text = $"Book with id {result} was not found";
                return;
            }

            if (!IsPostBack)
            {
                BookTitle.Text = deletedBook.Title;
                BookAuthor.Text = deletedBook.Author;
                BookISBN.Text = deletedBook.ISBN;
                BookYear.Text = deletedBook.PublicationYear.ToString();
                BookQuantity.Text = deletedBook.Quantity.ToString();
            }
        }

        protected void Submit_Click(object sender, EventArgs e)
        {
            if (_bookRep.Delete(deletedBook))
            {
                Response.Redirect("/Default.aspx");
            }
        }
    }
}