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
    public partial class _CreateNew : Page
    {
        private readonly IBookRepository _bookRep;
        private readonly ICategoryRepository _catRep;
        private readonly IEnumerable<Category> _categories;

        private Book _createdBook;

        public _CreateNew(IBookRepository bookRepository, ICategoryRepository categoryRepository)
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
        }

        protected void Submit_Click(object sender, EventArgs e)
        {
            Page.Validate("BookGroup");

            if (Page.IsValid)
            {
                if (_bookRep.Create(_createdBook))
                {
                    SuccessLabel.Visible = true;
                }
            }
        }

        protected void BookValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            Book book = new Book
            {
                Title = BookTitle.Text,
                Author = BookAuthor.Text,
                ISBN = BookISBN.Text,
                CategoryId = _categories.ElementAt(BookCategory.SelectedIndex).Id
            };

            List<ValidationResult> validationResults = new List<ValidationResult>();

            if (int.TryParse(BookYear.Text, out int year))
            {
                book.PublicationYear = year;
            }
            else
            {
                validationResults.Add(new ValidationResult("Year is not a valid number"));
            }

            if (int.TryParse(BookQuantity.Text, out int quantity))
            {
                book.Quantity = quantity;
            }
            else
            {
                validationResults.Add(new ValidationResult("Quantity is not a valid number"));
            }

            if (!_bookRep.IsISBNAvailable(book.ISBN))
            {
                validationResults.Add(new ValidationResult("This ISBN is already present"));
            }

            ValidationContext context = new ValidationContext(book);

            Validator.TryValidateObject(book, context, validationResults, true);

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
            else
            {
                _createdBook = book;
            }
        }
    }
}