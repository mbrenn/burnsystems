using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BurnSystems.Xml.Html
{
    /// <summary>
    /// Defines an html table
    /// </summary>
    public class HtmlTable : HtmlElement
    {
        /// <summary>
        ///  Stores the current row, being used for add cell
        /// </summary>
        HtmlElement currentRow = null;

        /// <summary>
        /// Initializes a new instance of the HtmlTable class.
        /// </summary>
        /// <param name="contents">Contents being included in the table</param>
        public HtmlTable(params object[] contents)
            : base("table", contents)
        {
        }

        /// <summary>
        /// Add a new row
        /// </summary>
        public void AddRow()
        {
            this.currentRow = new HtmlElement("tr");
            this.Add(this.currentRow);
        }

        /// <summary>
        /// Adds a new cell to the current row
        /// </summary>
        /// <param name="content">Content being added to the cell</param>
        public void AddCellWithContent(string content)
        {
            this.currentRow.Add(
                new HtmlElement("td", HttpUtility.HtmlEncode(content)));
        }

        /// <summary>
        /// Adds a new cell with header text to the current row
        /// </summary>
        /// <param name="content">Content being added to the cell</param>
        public void AddHeaderCellWithContent(string content)
        {
            this.currentRow.Add(
                new HtmlElement("th", HttpUtility.HtmlEncode(content)));
        }
    }
}
