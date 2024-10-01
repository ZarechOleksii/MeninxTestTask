using DAL.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.UI;

namespace WebForms
{
    public partial class _Default : Page
    {
        private IBookRepository _bookRep;
        private IEnumerable<Book> books;

        public _Default(IBookRepository bookRepository)
        {
            _bookRep = bookRepository;
        }

        private async Task LoadBooksAsync()
        {
            books = await _bookRep.GetAllNoTrackingAsync();
            tableRows.DataSource = books;
            tableRows.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.RegisterAsyncTask(new PageAsyncTask(LoadBooksAsync));
        }
    }
}