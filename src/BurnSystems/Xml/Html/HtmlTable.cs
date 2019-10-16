using System;

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
        private HtmlElement? _currentRow;

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
            _currentRow = new HtmlElement("tr");
            Add(_currentRow);
        }

        /// <summary>
        /// Adds a new cell to the current row
        /// </summary>
        /// <param name="content">Content being added to the cell</param>
        public void AddCellWithContent(string content)
        {
            if ( _currentRow == null ) throw new InvalidOperationException("AddRow not called");
            
            _currentRow.Add(
                new HtmlElement("td", content));
        }

        /// <summary>
        /// Adds a new cell with header text to the current row
        /// </summary>
        /// <param name="content">Content being added to the cell</param>
        public void AddHeaderCellWithContent(string content)
        {
            if ( _currentRow == null ) throw new InvalidOperationException("AddRow not called");

            _currentRow.Add(
                new HtmlElement("th", content));
        }

        /// <summary>
        /// Adds a td-cell with the elements being contained
        /// </summary>
        /// <param name="elements">Elements, that shall be added to the 
        /// container</param>
        public void AddCellWithContent(params object[] elements)
        {
            if ( _currentRow == null ) throw new InvalidOperationException("AddRow not called");

            _currentRow.Add(
                new HtmlElement("td", elements));
        }


        /// <summary>
        /// Adds a th-cell with the elements being contained
        /// </summary>
        /// <param name="elements">Elements, that shall be added to the 
        /// container</param>
        public void AddHeaderCellWithContent(params object[] elements)
        {
            if ( _currentRow == null ) throw new InvalidOperationException("AddRow not called");
            
            _currentRow.Add(
                new HtmlElement("th", elements));
        }
    }
}
