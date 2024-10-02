using DAL.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebForms
{
    public partial class _EditBook : Page
    {
        private readonly IBookRepository _bookRep;
        private readonly ICategoryRepository _catRep;
        private readonly IEnumerable<Category> _categories;

        protected Book editedBook;

        protected string BookNotFoundError;

        public _EditBook(IBookRepository bookRepository, ICategoryRepository categoryRepository)
        {
            _bookRep = bookRepository;
            _catRep = categoryRepository;
            _categories = _catRep.GetAllNoTracking();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BookCategory.DataSource = _categories.Select(q => q.Name);
                BookCategory.DataBind();
            }

            string bookId = Request.Params["BookId"];

            if (bookId == null)
            {
                ErrorLabel.Text = "To edit book BookId paremeter is required.";
                return;
            }

            if (!Guid.TryParse(bookId, out Guid result))
            {
                ErrorLabel.Text = $"Failed to parse {bookId} to id.";
                return;
            }

            editedBook = _bookRep.GetOneWithCategory(result);

            if (editedBook == null)
            {
                ErrorLabel.Text = $"Book with id {result} was not found";
                return;
            }

            if (!IsPostBack)
            {
                BookTitle.Text = editedBook.Title;
                BookAuthor.Text = editedBook.Author;
                BookISBN.Text = editedBook.ISBN;
                BookYear.Text = editedBook.PublicationYear.ToString();
                BookQuantity.Text = editedBook.Quantity.ToString();
                BookCategory.SelectedValue = editedBook.Category.Name;
            }
        }

        protected void Submit_Click(object sender, EventArgs e)
        {
            Page.Validate("BookGroup");

            if (Page.IsValid)
            {
                if (_bookRep.Update(editedBook))
                {
                    SuccessLabel.Visible = true;
                }
            }
        }

        protected void BookValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            editedBook.Title = BookTitle.Text;
            editedBook.Author = BookAuthor.Text;
            editedBook.CategoryId = _categories.ElementAt(BookCategory.SelectedIndex).Id;

            List<ValidationResult> validationResults = new List<ValidationResult>();

            if (int.TryParse(BookYear.Text, out int year))
            {
                editedBook.PublicationYear = year;
            }
            else
            {
                validationResults.Add(new ValidationResult("Year is not a valid number"));
            }

            if (int.TryParse(BookQuantity.Text, out int quantity))
            {
                editedBook.Quantity = quantity;
            }
            else
            {
                validationResults.Add(new ValidationResult("Quantity is not a valid number"));
            }

            if (BookISBN.Text != editedBook.ISBN && !_bookRep.IsISBNAvailable(BookISBN.Text))
            {
                validationResults.Add(new ValidationResult("This ISBN is already present"));
            }
            else
            {
                editedBook.ISBN = BookISBN.Text;
            }

            ValidationContext context = new ValidationContext(editedBook);

            Validator.TryValidateObject(editedBook, context, validationResults, true);

            bool isValid = !validationResults.Any();

            args.IsValid = isValid;

            if (!isValid)
            {
                SuccessLabel.Visible = false;

                foreach (var validationResult in validationResults)
                {
                    CustomValidator validator = new CustomValidator
                    {
                        IsValid = false,
                        ErrorMessage = validationResult.ErrorMessage,
                        ValidationGroup = "BookGroup"
                    };

                    Page.Validators.Add(validator);
                }
            }
        }
    }
}